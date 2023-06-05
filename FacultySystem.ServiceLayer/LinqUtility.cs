using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Journals.ServiceLayer.Utils
{
    public static class LinqUtility
    {
        /// <summary>
        /// Sort Strings which included Numbers
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="sortExpression"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
                throw new ArgumentNullException("source", "source is null.");

            if (string.IsNullOrEmpty(sortExpression))
                throw new ArgumentException("sortExpression is null or empty.", "sortExpression");

            var parts = sortExpression.Split(' ');
            var isDescending = false;
            var propertyName = "";
            var tType = typeof(T);

            if (parts.Length > 0 && parts[0] != "")
            {
                propertyName = parts[0];

                if (parts.Length > 1)
                {
                    isDescending = parts[1].ToLower().Contains("esc");
                }

                PropertyInfo prop = tType.GetProperty(propertyName);

                if (prop == null)
                {
                    throw new ArgumentException(string.Format("No property '{0}' on type '{1}'", propertyName, tType.Name));
                }

                var funcType = typeof(Func<,>)
                    .MakeGenericType(tType, prop.PropertyType);

                var lambdaBuilder = typeof(Expression)
                    .GetMethods()
                    .First(x => x.Name == "Lambda" && x.ContainsGenericParameters && x.GetParameters().Length == 2)
                    .MakeGenericMethod(funcType);

                var parameter = Expression.Parameter(tType);
                var propExpress = Expression.Property(parameter, prop);

                var sortLambda = lambdaBuilder
                    .Invoke(null, new object[] { propExpress, new ParameterExpression[] { parameter } });

                var sorter = typeof(Queryable)
                    .GetMethods()
                    .FirstOrDefault(x => x.Name == (isDescending ? "OrderByDescending" : "OrderBy") && x.GetParameters().Length == 2)
                    .MakeGenericMethod(new[] { tType, prop.PropertyType });

                return (IQueryable<T>)sorter
                    .Invoke(null, new object[] { source, sortLambda });
            }

            return source;
        }


        /// <summary> 
        /// Use the extension method to implement the Between operation in EF 
        /// </summary> 
        /// <typeparam name="TSource">Type of the entity</typeparam> 
        /// <typeparam name="TKey">Type of the return value</typeparam> 
        /// <param name="source">The entity used to apply the method</param> 
        /// <param name="keySelector">The lambda expression used to get the return value</param> 
        /// <param name="low">Low boundary of the return value</param> 
        /// <param name="high">High boundary of the return value</param> 
        /// <returns>return the IQueryable</returns> 
        public static IQueryable<TSource> Between<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, TKey low, TKey high) where TKey : IComparable<TKey>
        {
            // Get a ParameterExpression node of the TSource that is used in the expression tree 
            ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource));

            // Get the body and parameter of the lambda expression 
            Expression body = keySelector.Body;
            ParameterExpression parameter = null;
            if (keySelector.Parameters.Count > 0)
            {
                parameter = keySelector.Parameters[0];
            }

            // Get the Compare method of the type of the return value 
            MethodInfo compareMethod = typeof(TKey).GetMethod("CompareTo", new Type[] { typeof(TKey) });

            // Expression.LessThanOrEqual and Expression.GreaterThanOrEqua method are only used in 
            // the numeric comparision. If we want to compare the non-numeric type, we can't directly  
            // use the two methods.  
            // So we first use the Compare method to compare the objects, and the Compare method  
            // will return a int number. Then we can use the LessThanOrEqual and GreaterThanOrEqua method. 
            // For this reason, we ask all the TKey type implement the IComparable<> interface. 
            Expression upper = Expression.LessThanOrEqual(Expression.Call(body, compareMethod, Expression.Constant(high)), Expression.Constant(0, typeof(int)));
            Expression lower = Expression.GreaterThanOrEqual(Expression.Call(body, compareMethod, Expression.Constant(low)), Expression.Constant(0, typeof(int)));

            Expression andExpression = Expression.And(upper, lower);

            // Get the Where method expression. 
            MethodCallExpression whereCallExpression = Expression.Call(typeof(Queryable), "Where", new Type[] { source.ElementType }, source.Expression, Expression.Lambda<Func<TSource, bool>>(andExpression, new ParameterExpression[] { parameter }));

            return source.Provider.CreateQuery<TSource>(whereCallExpression);
        }

        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other,
                                                                            Func<T, TKey> getKey)
        {
            return from item in items
                   join otherItem in other on getKey(item)
                   equals getKey(otherItem) into tempItems
                   from temp in tempItems.DefaultIfEmpty()
                   where ReferenceEquals(null, temp) || temp.Equals(default(T))
                   select item;

        }
    }
}
