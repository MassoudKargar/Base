using Base.Core.Domains.Contracts.Common;

namespace Base.Core.Domains;

public class EntityHelper
{
    public static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId<TEntity, TId>(TId id)
        where TEntity : BaseEntity<TId>
        where TId : struct
    {
        var lambdaParam = Expression.Parameter(typeof(TEntity));
        var lambdaBody = Expression.Equal(
            Expression.PropertyOrField(lambdaParam, nameof(BaseEntity<TId>.Id)),
            Expression.Constant(id, typeof(TId))
        );

        return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
    }
}
