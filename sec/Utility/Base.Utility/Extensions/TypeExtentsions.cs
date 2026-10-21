namespace Base.Utility.Extensions;
public static class TypeExtensions
{
    /// <summary>
    /// بررسی اینکه آیا نوع ورودی از نوع عمومی خاصی ارث‌بری می‌کند یا خیر
    /// </summary>
    /// <param name="toCheck">نوعی که باید بررسی شود</param>
    /// <param name="generic">نوع عمومی مورد نظر برای بررسی ارث‌بری</param>
    /// <returns>اگر نوع ورودی از نوع عمومی ارث‌بری کند، مقدار true باز می‌گرداند، در غیر این صورت false</returns>
    public static bool IsSubclassOfRawGeneric(this Type toCheck, Type generic)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur)
            {
                return true;
            }
            toCheck = toCheck.BaseType;
        }
        return false;
    }
}
