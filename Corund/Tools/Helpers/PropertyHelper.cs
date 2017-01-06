using System;
using System.Linq.Expressions;

namespace Corund.Tools.Helpers
{
    /// <summary>
    /// Various methods for handling properties.
    /// </summary>
    public static class PropertyHelper
    {
        #region Methods

        /// <summary>
        /// Creates a property descriptor that can get and set values based on getter expression.
        /// </summary>
        public static PropertyDescriptor<TObject, TProperty> GetDescriptor<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            return new PropertyDescriptor<TObject, TProperty>(
                CompileGetter(expression),
                CompileSetter(expression),
                GetPropertyName(expression)
            );
        }

        /// <summary>
        /// Returns the expression name as string.
        /// </summary>
        public static string GetPropertyName<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            var body = expression.Body.ToString();
            var argName = expression.Parameters[0].Name;
            var headerLength = argName.Length + " => .".Length;
            return body.Substring(headerLength);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates a getter function from expression.
        /// </summary>
        private static Func<TObject, TProperty> CompileGetter<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            return expression.Compile();
        }

        /// <summary>
        /// Creates a setter function from expression.
        /// </summary>
        private static Action<TObject, TProperty> CompileSetter<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            var setExpr = GetAssignableExpression(expression.Body);
            var valueParam = Expression.Parameter(typeof(TProperty), "value");
            var setter = Expression.Lambda<Action<TObject, TProperty>>(
                Expression.Assign(setExpr, valueParam),
                expression.Parameters[0],
                valueParam
            );

            return setter.Compile();
        }

        /// <summary>
        /// Gets an assignable expression from the body.
        /// </summary>
        private static Expression GetAssignableExpression(Expression body)
        {
            var member = body as MemberExpression;
            if (member != null)
                return member;

            var idx = body as BinaryExpression;
            if (idx != null && idx.NodeType == ExpressionType.ArrayIndex)
                return Expression.ArrayAccess(idx.Left, idx.Right);

            return null;
        }

        #endregion
    }
}
