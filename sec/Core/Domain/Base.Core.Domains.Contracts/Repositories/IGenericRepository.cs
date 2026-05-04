using Base.Core.Domains.Contracts.Common;

using System.Linq.Expressions;

namespace Base.Core.Domains.Contracts.Repositories;

/// <summary>
/// رابط عمومی برای مدیریت عملیات پایه‌ای دیتابیس.
/// </summary>
/// <typeparam name="TEntity">نوع موجودیت که باید با دیتابیس کار کند.</typeparam>
/// <typeparam name="TId">نوع شناسه موجودیت.</typeparam>
public interface IGenericRepository<TEntity, in TId>
    where TEntity : BaseEntity<TId>
    where TId : struct
{

    /// <summary>
    /// موجودیت جدیدی را در دیتابیس درج می‌کند.
    /// </summary>
    Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// موجودیت را به عنوان حذف شده علامت‌گذاری می‌کند.
    /// </summary>
    Task UpdateDeletedAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// تمام موجودیت‌ها را به صورت ناهمزمان بازیابی می‌کند.
    /// </summary>
    Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// تمام موجودیت‌ها را به همراه ویژگی‌های مرتبط مشخص شده بازیابی می‌کند.
    /// </summary>
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// بر اساس شرط ورودی تعداد را محاصبه میکند
    /// </summary>
    /// <param name="predicate">شروط روردی</param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// یک موجودیت را بر اساس شناسه از دیتابیس بازیابی می‌کند.  و چک میکند که حتما یک عدد مجود باشد 
    /// </summary>
    /// <param name="id">شناسه موجودیت مورد نظر.</param>
    /// <returns>موجودیت بازیابی‌شده.</returns>
    Task<TEntity> GetSingleAsync(TId id, CancellationToken cancellationToken);

    /// <summary>
    /// به صورت ناهمزمان یک موجودیت را بر اساس شناسه از دیتابیس بازیابی می‌کند.
    /// </summary>
    Task<TEntity?> GetFindAsync(TId id, CancellationToken cancellationToken);

    /// <summary>
    /// به صورت ناهمزمان یک موجودیت را بر اساس شروط از دیتابیس بازیابی می‌کند.
    /// </summary>
    Task<TEntity?> GetFirstAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// بررسی وجود موجودیت با شرط خاص.
    /// </summary>
    Task<bool> IsExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// موجودیت موجود را به‌روزرسانی می‌کند.
    /// </summary>
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);

    Task SaveChangeAsync(CancellationToken cancellationToken);
}
