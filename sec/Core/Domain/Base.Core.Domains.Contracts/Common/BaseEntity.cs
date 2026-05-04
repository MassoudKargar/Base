namespace Base.Core.Domains.Contracts.Common;

/// <summary>
/// کلاس پایه برای تمام موجودیت‌ها در سیستم که ویژگی‌ها و متدهای مشترک برای مدیریت موجودیت‌ها 
/// شامل شناسه، تاریخ ایجاد/ویرایش و وضعیت حذف را فراهم می‌کند.
/// </summary>
/// <typeparam name="TId">نوع شناسه منحصر به فرد موجودیت.</typeparam>
public class BaseEntity<TId> where TId : struct
{
    /// <summary>
    /// شناسه منحصر به فرد موجودیت را می‌گیرد یا تنظیم می‌کند.
    /// این شناسه برای ذخیره‌سازی در پایگاه داده و ساده‌سازی عملیات استفاده می‌شود.
    /// </summary>
    public TId Id { get; set; }

    /// <summary>
    /// تاریخ و زمان ایجاد موجودیت را می‌گیرد یا تنظیم می‌کند.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// تاریخ و زمان آخرین ویرایش موجودیت را می‌گیرد یا تنظیم می‌کند.
    /// این فیلد اختیاری است و در صورتی که موجودیت هیچ‌گاه ویرایش نشده باشد، می‌تواند مقدار null داشته باشد.
    /// </summary>
    public DateTime? ModificationDate { get; set; }

    public long? CreatedBy { get; set; }
    public long? ModifiedBy { get; set; }

    /// <summary>
    /// وضعیت حذف موجودیت را می‌گیرد یا تنظیم می‌کند.
    /// اگر موجودیت حذف شده باشد، مقدار true وگرنه false خواهد بود.
    /// </summary>
    public bool Deleted { get; set; }

    /// <summary>
    /// سازنده محافظت‌شده برای اطمینان از این که موجودیت‌ها با ویژگی‌های اولیه‌سازی شده ساخته شوند.
    /// </summary>
    protected BaseEntity() { }

    /// <summary>
    /// مقایسه می‌کند که آیا دو موجودیت برابر هستند یا خیر.
    /// </summary>
    public bool Equals(BaseEntity<TId>? other) => this == other;

    /// <summary>
    /// مقایسه می‌کند که آیا موجودیت جاری با موجودیت دیگر برابر است یا خیر.
    /// </summary>
    public override bool Equals(object? obj) =>
         obj is BaseEntity<TId> otherObject && Id.Equals(otherObject.Id);

    /// <summary>
    /// دریافت کد هش موجودیت.
    /// </summary>
    public override int GetHashCode() => Id.GetHashCode();

    /// <summary>
    /// عملگر مقایسه برابری برای موجودیت‌ها.
    /// </summary>
    public static bool operator ==(BaseEntity<TId>? left, BaseEntity<TId>? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;
        return left.Equals(right);
    }

    /// <summary>
    /// عملگر مقایسه نابرابری برای موجودیت‌ها.
    /// </summary>
    public static bool operator !=(BaseEntity<TId> left, BaseEntity<TId> right)
        => !(right == left);

}
