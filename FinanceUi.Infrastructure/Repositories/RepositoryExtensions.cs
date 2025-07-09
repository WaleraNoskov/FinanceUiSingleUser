using System.Linq.Expressions;

namespace FinanceUi.Infrastructure.Repositories;

public static class RepositoryExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool descending)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.PropertyOrField(param, propertyName);
        var keySelector = Expression.Lambda(body, param);

        string methodName = descending ? "OrderByDescending" : "OrderBy";

        var method = typeof(Queryable).GetMethods()
            .First(m => m.Name == methodName && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(T), body.Type);

        return (IQueryable<T>)method.Invoke(null, new object[] { source, keySelector });
    }
}