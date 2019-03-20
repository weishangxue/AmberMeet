using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AmberMeet.Infrastructure.Search.Extensions
{
    public static class LinqQueryableExtension
    {
        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source,
            string orderByProperty, bool desc)
            where TEntity : class
        {
            var command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                BindingFlags.IgnoreCase);
            var parameter = Expression.Parameter(type, property.Name);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command,
                new[] {type, property.PropertyType},
                source.Expression,
                Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>) source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IOrderedQueryable<TEntity> ThenBy<TEntity>(this IQueryable<TEntity> source,
            string orderByProperty,
            bool desc)
            where TEntity : class
        {
            var command = desc ? "ThenByDescending" : "ThenBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance |
                BindingFlags.IgnoreCase);
            var parameter = Expression.Parameter(type, property.Name);
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command,
                new[] {type, property.PropertyType},
                source.Expression,
                Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>) source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}