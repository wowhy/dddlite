namespace DDDLite.WebApi.Internal.Parser
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
        internal static readonly Dictionary<string, PropertyInfo> props = type.GetProperties().ToDictionary(k => k.Name.ToLower());

        public FilterParser()
        {
        }

        public Specification<TAggregateRoot> Parse(string filter)
        {
            try
            {
                var param = Expression.Parameter(type, "k");
                var parser = new ExpressionParser(filter, param);
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
            private readonly Expression param;

            private int index = 0;
            private TokenType tokenType;
            private string tokenText;

            public ExpressionParser(string expr, Expression param)
            {
                this.rawExpr = expr.Trim();
                this.param = param;
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
                                bodyExpr = Expression.PropertyOrField(param, tokenText);
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
                    var constExpr = right as ConstantExpression;
                    if (constExpr.Value?.GetType() == typeof(decimal))
                    {
                        var targetType = (left as MemberExpression).Type;
                        right = Expression.Constant(Convert.ChangeType(constExpr.Value, targetType));
                    }
                }

                if (right is MemberExpression && left is ConstantExpression)
                {
                    var constExpr = left as ConstantExpression;
                    if (constExpr.Value?.GetType() == typeof(decimal))
                    {
                        var targetType = (right as MemberExpression).Type;
                        left = Expression.Constant(Convert.ChangeType(constExpr.Value, targetType));
                    }
                }

                Next();
                return op(left, right);
            }

            private Expression Atom()
            {
                if (tokenType == TokenType.Identifier)
                {
                    return Expression.PropertyOrField(param, tokenText);
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

            private Func<Expression, Expression, BinaryExpression> Operator()
            {
                var func = default(Func<Expression, Expression, BinaryExpression>);
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

                    // TODO: 实现CONTAINS
                    // case TokenType.CONTAINS:
                    //     // func = Expression.;
                    //     break;

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
                        while (index < rawExpr.Length && !char.IsWhiteSpace(rawExpr[index]))
                            index++;

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
        }
        #endregion
    }
}