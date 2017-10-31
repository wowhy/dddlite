namespace DDDLite.WebApi.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using DDDLite.Specifications;
    using DDDLite.WebApi.Exception;

    public class FilterParser<TAggregateRoot>
        where TAggregateRoot : class
    {
        internal static readonly Type type = typeof(TAggregateRoot);
        internal static readonly ParameterExpression param = Expression.Parameter(type, "k");
        internal static readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains");

        private readonly string filter;

        public FilterParser(string filter)
        {
            this.filter = filter;
        }

        public Specification<TAggregateRoot> Parse()
        {
            try
            {
                if(string.IsNullOrWhiteSpace(filter)) 
                {
                    return Specification<TAggregateRoot>.Any();
                }

                var parser = new ExpressionParser(filter);
                var expr = parser.Parse();

                return Specification<TAggregateRoot>.Eval(Expression.Lambda<Func<TAggregateRoot, bool>>(expr, param));
            }
            catch (Exception ex)
            {
                if (ex is FilterParseException)
                    throw;
                throw new FilterParseException(ex);
            }
        }

        #region Implements
        internal enum TokenType
        {
            None,

            Identifier,

            Number,
            Null,
            True,
            False,
            String,

            LParen,
            RParen,

            EQ,
            NE,
            GT,
            GE,
            LT,
            LE,
            CONTAINS,

            AND,
            OR,
            NOT,
        }

        internal class ExpressionParser
        {
            private readonly string rawExpr;

            private int index = 0;
            private TokenType tokenType;
            private string tokenText;

            public ExpressionParser(string expr)
            {
                this.rawExpr = expr.Trim();
            }

            /*

            <expr> ::= <term> OR <term>
                | <term>

            <term> ::= <factor> AND <factor>
                | <factor>

            <factor> ::= LPAREN <expr> RPAREN
                | NOT LPAREN <expr> RPAREN
                | NOT ID
                | NOT TRUE
                | NOT FALSE
                | <primary>

            <primary> ::= <atom> <operator> <atom>

            <atom> ::= ID | NUMBER | STRING | TRUE | FALSE | NULL

            <operator> ::= LT | LE | GT | GE | EQ | NE

            */

            public Expression Parse()
            {
                index = 0;
                tokenType = TokenType.None;
                tokenText = null;

                Next();

                return this.Expr();
            }

            private Expression Expr()
            {
                var left = Term();
                while (true)
                {
                    if (tokenType != TokenType.OR)
                        break;

                    Next();
                    var right = Term();
                    left = Expression.OrElse(left, right);
                }

                return left;
            }

            /*
                <term> ::= <factor> AND <factor>
                    | <factor>
             */
            private Expression Term()
            {
                var left = Factor();
                while (true)
                {
                    if (tokenType != TokenType.AND)
                        break;

                    Next();
                    var right = Factor();
                    left = Expression.AndAlso(left, right);
                }
                return left;
            }

            /*
                <factor> ::= LPAREN <expr> RPAREN
                    | NOT LPAREN <expr> RPAREN
                    | NOT ID
                    | NOT TRUE
                    | NOT FALSE
                    | <primary>
             */
            private Expression Factor()
            {
                if (tokenType == TokenType.LParen)
                {
                    Match("(");
                    var expr = Expr();
                    Match(")");
                    return expr;
                }

                if (tokenType == TokenType.NOT)
                {
                    var bodyExpr = default(Expression);
                    Match("not");
                    if (tokenType == TokenType.LParen)
                    {
                        Match("(");
                        bodyExpr = Expr();
                        Match(")");
                    }
                    else
                    {
                        switch (tokenType)
                        {
                            case TokenType.Identifier:
                                bodyExpr = GetProperty(tokenText);
                                break;

                            case TokenType.True:
                                bodyExpr = Expression.Constant(true);
                                break;

                            case TokenType.False:
                                bodyExpr = Expression.Constant(false);
                                break;

                            default:
                                throw new FilterParseException(index);
                        }

                        Next();
                    }

                    return Expression.Not(bodyExpr);
                }

                return Primary();
            }

            /*
                <primary> ::= <atom> <operator> <atom>
             */
            private Expression Primary()
            {
                var left = Atom();
                Next();
                var op = Operator();
                Next();
                var right = Atom();

                // 修正类型                
                if (left is MemberExpression && right is ConstantExpression)
                {
                    FixedExpressionType((MemberExpression)left, ref right);
                }

                if (right is MemberExpression && left is ConstantExpression)
                {
                    FixedExpressionType((MemberExpression)right, ref left);
                }

                Next();
                return op(left, right);
            }

            private Expression Atom()
            {
                if (tokenType == TokenType.Identifier)
                {
                    return GetProperty(tokenText);
                }
                else if (tokenType == TokenType.Null)
                {
                    return Expression.Constant(null);
                }
                else if (tokenType == TokenType.Number)
                {
                    return Expression.Constant(decimal.Parse(tokenText));
                }
                else if (tokenType == TokenType.String)
                {
                    return Expression.Constant(tokenText.Trim('\''));
                }
                else if (tokenType == TokenType.True)
                {
                    return Expression.Constant(true);
                }
                else if (tokenType == TokenType.False)
                {
                    return Expression.Constant(false);
                }

                throw new FilterParseException(index);
            }

            private Func<Expression, Expression, Expression> Operator()
            {
                var func = default(Func<Expression, Expression, Expression>);
                switch (tokenType)
                {
                    case TokenType.EQ:
                        func = Expression.Equal;
                        break;

                    case TokenType.NE:
                        func = Expression.NotEqual;
                        break;

                    case TokenType.GT:
                        func = Expression.GreaterThan;
                        break;

                    case TokenType.GE:
                        func = Expression.GreaterThanOrEqual;
                        break;

                    case TokenType.LT:
                        func = Expression.LessThan;
                        break;

                    case TokenType.LE:
                        func = Expression.LessThanOrEqual;
                        break;

                    case TokenType.CONTAINS:
                        func = new Func<Expression, Expression, Expression>((left, right) =>
                        {
                            return Expression.Call(left, containsMethod, right);
                        });
                        break;

                    default:
                        throw new FilterParseException(index);
                }

                return func;
            }

            private void Next()
            {
                while (index < rawExpr.Length && char.IsWhiteSpace(rawExpr[index]))
                    index++;

                if (index >= rawExpr.Length)
                {
                    tokenType = TokenType.None;
                    tokenText = null;
                    return;
                }

                var ch = rawExpr[index];
                switch (ch)
                {
                    case '(':
                        ReadToken("(", TokenType.LParen);
                        break;

                    case ')':
                        ReadToken(")", TokenType.RParen);
                        break;

                    case '\'':
                        ReadString();
                        break;

                    case '-':
                    case char c when c >= '0' && c <= '9':
                        ReadNumber();
                        break;

                    case char c when (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z'):
                        var originIndex = index;
                        while (index < rawExpr.Length)
                        {
                            var p = rawExpr[index];
                            if (char.IsLetterOrDigit(p)
                                || p == '_'
                                || p == '.')
                                index++;
                            else
                                break;
                        }

                        tokenText = rawExpr.Substring(originIndex, index - originIndex);

                        switch (tokenText)
                        {
                            case "true":
                                tokenType = TokenType.True;
                                break;

                            case "false":
                                tokenType = TokenType.False;
                                break;

                            case "null":
                                tokenType = TokenType.Null;
                                break;

                            case "gt":
                                tokenType = TokenType.GT;
                                break;

                            case "ge":
                                tokenType = TokenType.GE;
                                break;

                            case "lt":
                                tokenType = TokenType.LT;
                                break;

                            case "le":
                                tokenType = TokenType.LE;
                                break;

                            case "eq":
                                tokenType = TokenType.EQ;
                                break;

                            case "ne":
                                tokenType = TokenType.NE;
                                break;

                            case "contains":
                                tokenType = TokenType.CONTAINS;
                                break;

                            case "or":
                                tokenType = TokenType.OR;
                                break;

                            case "and":
                                tokenType = TokenType.AND;
                                break;

                            case "not":
                                tokenType = TokenType.NOT;
                                break;

                            default:
                                tokenType = TokenType.Identifier;
                                break;
                        }
                        break;

                    default:
                        throw new FilterParseException(index);
                }
            }

            private void Match(string text)
            {
                if (tokenText != text)
                {
                    throw new FilterParseException(index);
                }

                Next();
            }

            private bool ReadToken(string target, TokenType type)
            {
                if (rawExpr.Substring(index, target.Length) == target)
                {
                    tokenType = type;
                    tokenText = target;
                    index += target.Length;
                    return true;
                }

                return false;
            }

            private void ReadNumber()
            {
                var originIndex = index;
                index++;
                while (rawExpr.Length > index && (char.IsDigit(rawExpr[index]) || rawExpr[index] == '.'))
                    index++;
                tokenType = TokenType.Number;
                tokenText = rawExpr.Substring(originIndex, index - originIndex);
            }

            private void ReadString()
            {
                var originIndex = index;
                while (rawExpr.Length > index)
                {
                    index++;
                    if (rawExpr[index] == '\'' && rawExpr[index - 1] != '\\')
                    {
                        break;
                    }
                }

                index++;

                tokenType = TokenType.String;
                tokenText = rawExpr.Substring(originIndex, index - originIndex);
            }

            private Expression GetProperty(string name)
            {
                var split = name.Split('.');
                var prop = Expression.PropertyOrField(param, split[0]);

                for (var i = 1; i < split.Length; i++)
                {
                    prop = Expression.PropertyOrField(prop, split[i]);
                }

                return prop;
            }

            private void FixedExpressionType(MemberExpression memberExpr, ref Expression toFixed)
            {
                var constExpr = toFixed as ConstantExpression;
                var targetType = memberExpr.Type;
                var innerType = Nullable.GetUnderlyingType(targetType);
                if (innerType != null)
                {
                    targetType = innerType;
                }

                if (constExpr.Value?.GetType() != targetType)
                {
                    var val = default(object);
                    if (targetType == typeof(Guid))
                    {
                        val = Guid.Parse((string)constExpr.Value);
                    }
                    else if (targetType == typeof(DateTime))
                    {
                        val = DateTime.Parse(constExpr.Value.ToString());
                    }
                    else if (targetType.IsEnum)
                    {
                        if (constExpr.Value is decimal)
                        {
                            val = Enum.ToObject(targetType, Convert.ToInt32(constExpr.Value));
                        }
                        else if (constExpr.Value is string)
                        {
                            val = Enum.Parse(targetType, (string)constExpr.Value);
                        }
                    }
                    else
                    {
                        val = Convert.ChangeType(constExpr.Value, targetType);
                    }

                    toFixed = Expression.Constant(val);
                }
            }
        }
        #endregion
    }
}