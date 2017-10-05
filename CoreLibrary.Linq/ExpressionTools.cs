using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CoreLibrary.Linq
{
    public static class ExpressionTools
    {
        public static Expression Equals<TEntity, TValueType>(ParameterExpression paramExpression, string propertyName, TValueType value)
        {
            Expression propertyExpression = Expression.Property(paramExpression, propertyName);
            Expression constantExpression = Expression.Constant(value, typeof(TValueType));
            return Expression.Equal(propertyExpression, constantExpression);
        }

        public static Expression Equals<TEntity, TValueType>(ParameterExpression paramExpression, Expression propertyChain, TValueType value)
        {
            return Expression.Equal(propertyChain, Expression.Constant(value, typeof(TValueType)));
        }

        public static Expression<Func<TEntity, bool>> EqualsLambda<TEntity, TValueType>(ParameterExpression paramExpression, string propertyName, TValueType value)
        {
            return Expression.Lambda<Func<TEntity, bool>>(Equals<TEntity, TValueType>(paramExpression, propertyName, value), new[] { paramExpression });
        }

        public static Expression NestedProperty(ParameterExpression paramExpression, string propertyDotChain)
        {
            string[] nestedProperties = propertyDotChain.Split('.');
            Expression propertyExpression = Expression.Property(paramExpression, nestedProperties[0]);
            for (int i = 1; i < nestedProperties.Length; i++)
            {
                propertyExpression = Expression.Property(propertyExpression, nestedProperties[i]);
            }

            return propertyExpression;
        }

        /// <summary>
        /// Note that the implementer must ensure to pass in expressions in this
        /// function that have the same parameter expression!
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression And<TEntity>(params Expression[] expressions)
        {
            if (expressions.Length < 2)
                return expressions[0];

            Expression andExpression = expressions[0];
            for (int i = 1; i < expressions.Length; i++)
            {
                andExpression = Expression.AndAlso(andExpression, expressions[i]);
            }

            return andExpression;
        }

        /// <summary>
        /// Unfortunately, for now, the implementer is responsible for creating
        /// the parameter expression, and also to make sure that each expression
        /// passed here uses the same instance of that parameter expression :|.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> AndLambda<TEntity>(ParameterExpression parameterExpression, params Expression[] expressions)
        {
            return Expression.Lambda<Func<TEntity, bool>>(And<TEntity>(expressions), new[] { parameterExpression });
        }

        /// <summary>
        /// Note that the implementer must ensure to pass in expressions in this
        /// function that have the same parameter expression!
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression Or<TEntity>(params Expression[] expressions)
        {
            if (expressions.Length < 2)
                return expressions[0];

            Expression andExpression = expressions[0];

            for (int i = 1; i < expressions.Length; i++)
            {
                andExpression = Expression.OrElse(andExpression, expressions[i]);
            }

            return andExpression;
        }

        /// <summary>
        /// Unfortunately, for now, the implementer is responsible for creating
        /// the parameter expression, and also to make sure that each expression
        /// passed here uses the same instance of that parameter expression :|.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="expressions"></param>
        /// <returns></returns>
        public static Expression<Func<TEntity, bool>> OrLambda<TEntity>(ParameterExpression parameterExpression, params Expression[] expressions)
        {
            return Expression.Lambda<Func<TEntity, bool>>(Or<TEntity>(expressions), new[] { parameterExpression });
        }

        private static Expression StringExpression<TEntity>(ParameterExpression paramExpression, string propertyName, string stringFunctionName, string value)
        {
            Expression constantExpression = Expression.Constant(value, typeof(string));
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(propertyName);

            Expression memberExpression = Expression.MakeMemberAccess(paramExpression, propertyInfo);
            MethodInfo methodInfo = typeof(string).GetMethod(stringFunctionName, new Type[] { typeof(string) });

            return Expression.Call(memberExpression, methodInfo, constantExpression);
        }

        public static Expression StartsWith<TEntity>(ParameterExpression paramExpression, string propertyName, string value)
        {
            return StringExpression<TEntity>(paramExpression, propertyName, "StartsWith", value);
        }

        public static Expression EndsWith<TEntity>(ParameterExpression paramExpression, string propertyName, string value)
        {
            return StringExpression<TEntity>(paramExpression, propertyName, "EndsWith", value);
        }

        public static Expression Contains<TEntity>(ParameterExpression paramExpression, string propertyName, string value)
        {
            return StringExpression<TEntity>(paramExpression, propertyName, "Contains", value);
        }

        public static Expression<Func<TEntity, TReturn>> PropertyLambda<TEntity, TReturn>(ParameterExpression paramExpression, string propertyName)
        {
            return Expression.Lambda<Func<TEntity, TReturn>>(Expression.Property(paramExpression, propertyName), paramExpression);
        }
    }
}
