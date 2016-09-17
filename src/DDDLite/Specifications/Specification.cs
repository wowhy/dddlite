namespace DDDLite.Specifications
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents the base class for specifications.
    /// </summary>
    /// <typeparam name="T">The type of the object to which the specification is applied.</typeparam>
    public abstract class Specification<T>
    {
        #region Public Methods
        /// <summary>
        /// Evaluates a LINQ expression to its corresponding specification.
        /// </summary>
        /// <param name="expression">The LINQ expression to be evaluated.</param>
        /// <returns>The specification which represents the same semantics as the given LINQ expression.</returns>
        public static Specification<T> Eval(Expression<Func<T, bool>> expression)
        {
            return new ExpressionSpecification<T>(expression);
        }

        public static Specification<T> Any()
        {
            return new AnySpecification<T>();
        }

        public static Specification<T> And(Specification<T> left, Specification<T> right)
        {
            return new AndSpecification<T>(left, right);
        }

        public static Specification<T> Or(Specification<T> left, Specification<T> right)
        {
            return new OrSpecification<T>(left, right);
        }

        public static Specification<T> Not(Specification<T> specification)
        {
            return new NotSpecification<T>(specification);
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="System.Boolean"/> value which indicates whether the specification
        /// is satisfied by the given object.
        /// </summary>
        /// <param name="obj">The object to which the specification is applied.</param>
        /// <returns>True if the specification is satisfied, otherwise false.</returns>
        public virtual bool IsSatisfiedBy(T obj)
        {
            return this.Expression.Compile()(obj);
        }
        /// <summary>
        /// Combines the current specification instance with another specification instance
        /// and returns the combined specification which represents that both the current and
        /// the given specification must be satisfied by the given object.
        /// </summary>
        /// <param name="other">The specification instance with which the current specification
        /// is combined.</param>
        /// <returns>The combined specification instance.</returns>
        public Specification<T> And(Specification<T> other)
        {
            return new AndSpecification<T>(this, other);
        }
        /// <summary>
        /// Combines the current specification instance with another specification instance
        /// and returns the combined specification which represents that either the current or
        /// the given specification should be satisfied by the given object.
        /// </summary>
        /// <param name="other">The specification instance with which the current specification
        /// is combined.</param>
        /// <returns>The combined specification instance.</returns>
        public Specification<T> Or(Specification<T> other)
        {
            return new OrSpecification<T>(this, other);
        }
        /// <summary>
        /// Combines the current specification instance with another specification instance
        /// and returns the combined specification which represents that the current specification
        /// should be satisfied by the given object but the specified specification should not.
        /// </summary>
        /// <param name="other">The specification instance with which the current specification
        /// is combined.</param>
        /// <returns>The combined specification instance.</returns>
        public Specification<T> AndNot(Specification<T> other)
        {
            return new AndNotSpecification<T>(this, other);
        }
        /// <summary>
        /// Reverses the current specification instance and returns a specification which represents
        /// the semantics opposite to the current specification.
        /// </summary>
        /// <returns>The reversed specification instance.</returns>
        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
        /// <summary>
        /// Gets the LINQ expression which represents the current specification.
        /// </summary>
        /// <returns>The LINQ expression.</returns>
        public abstract Expression<Func<T, bool>> Expression { get; }

        /// <summary>
        /// Implicitly converts a given <see cref="Specification{T}"/> instance into a <see cref="Expression{TDelegate}"/> instance.
        /// </summary>
        /// <param name="specification"></param>
        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification) => specification.Expression;

        /// <summary>
        /// Implicitly converts a given <see cref="Expression{TDelegate}"/> instance into a <see cref="Specification{T}"/> instance.
        /// </summary>
        /// <param name="expression"></param>
        public static implicit operator Specification<T>(Expression<Func<T, bool>> expression) => new ExpressionSpecification<T>(expression);

    }
}
