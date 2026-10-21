public static class StringValidatorExtensions
{
    /// <summary>
    /// صحت سنجی کد ملی
    /// </summary>
    /// <param name="nationalCode">کد ملی</param>
    /// <returns>درست یا غلط</returns>
    public static bool IsNationalCode(this string nationalCode)
    {
        if (string.IsNullOrWhiteSpace(nationalCode) || !nationalCode.IsLengthBetween(8, 10))
            return false;

        nationalCode = nationalCode.PadLeft(10, '0');

        if (!nationalCode.IsNumeric())
            return false;

        if (!IsFormat1Validate(nationalCode))
            return false;

        if (!IsFormat2Validate(nationalCode))
            return false;
        return true;

        static bool IsFormat1Validate(string nationalCode)
        {
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (!allDigitEqual.Contains(nationalCode))
                return true;
            return false;
        }

        static bool IsFormat2Validate(string nationalCode)
        {
            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = num0 + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
            var c = b % 11;

            var result = c < 2 && a == c || c >= 2 && 11 - c == a;
            if (result)
                return true;
            return false;
        }
    }

    /// <summary>
    /// صحت سنجی شناسه ملی شرکت‌ها
    /// </summary>
    /// <param name="nationalId"></param>
    /// <returns></returns>
    public static bool IsLegalNationalIdValid(this string nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId) || !nationalId.IsLengthEqual(11))
            return false;

        if (!nationalId.IsNumeric())
            return false;

        if (!IsFormat1Validate(nationalId))
            return false;

        if (!IsFormat2Validate(nationalId))
            return false;
        return true;

        static bool IsFormat1Validate(string nationalId)
        {
            var allDigitEqual = new[] { "00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666", "77777777777", "88888888888", "99999999999" };
            if (!allDigitEqual.Contains(nationalId))
                return true;
            return false;
        }

        static bool IsFormat2Validate(string nationalId)
        {
            var chArray = nationalId.ToCharArray();
            var controlCode = Convert.ToInt32(nationalId[10].ToString());
            var factor = Convert.ToInt32(nationalId[9].ToString()) + 2;
            var sum = 0;
            sum += (factor + Convert.ToInt32(chArray[0].ToString())) * 29;
            sum += (factor + Convert.ToInt32(chArray[1].ToString())) * 27;
            sum += (factor + Convert.ToInt32(chArray[2].ToString())) * 23;
            sum += (factor + Convert.ToInt32(chArray[3].ToString())) * 19;
            sum += (factor + Convert.ToInt32(chArray[4].ToString())) * 17;
            sum += (factor + Convert.ToInt32(chArray[5].ToString())) * 29;
            sum += (factor + Convert.ToInt32(chArray[6].ToString())) * 27;
            sum += (factor + Convert.ToInt32(chArray[7].ToString())) * 23;
            sum += (factor + Convert.ToInt32(chArray[8].ToString())) * 19;
            sum += (factor + Convert.ToInt32(chArray[9].ToString())) * 17;
            var remaining = sum % 11;
            if (remaining == 10)
                remaining = 0;
            return remaining == controlCode;
        }
    }

    /// <summary>
    /// بررسی اینکه آیا رشته عددی است یا خیر
    /// </summary>
    /// <param name="nationalCode">رشته ورودی برای بررسی عدد بودن</param>
    /// <returns>اگر رشته عددی باشد، مقدار true باز می‌گرداند، در غیر این صورت false</returns>
    public static bool IsNumeric(this string nationalCode)
    {
        var regex = new Regex(@"\d+");
        if (regex.IsMatch(nationalCode))
            return true;
        return false;
    }

    /// <summary>
    /// بررسی طول رشته بین دو مقدار حداقل و حداکثر
    /// </summary>
    /// <param name="input">رشته ورودی برای بررسی طول</param>
    /// <param name="minLength">طول حداقل مجاز</param>
    /// <param name="maxLength">طول حداکثر مجاز</param>
    /// <returns>اگر طول رشته در محدوده حداقل و حداکثر باشد، مقدار true باز می‌گرداند</returns>
    public static bool IsLengthBetween(this string input, int minLength, int maxLength)
    {
        if (input.Length <= maxLength && input.Length >= minLength)
            return true;
        return false;
    }

    /// <summary>
    /// بررسی اینکه طول رشته کمتر از مقدار داده‌شده است یا خیر
    /// </summary>
    /// <param name="input">رشته ورودی برای بررسی طول</param>
    /// <param name="length">مقدار طول</param>
    /// <returns>اگر طول رشته کمتر از مقدار داده‌شده باشد، مقدار true باز می‌گرداند</returns>
    public static bool IsLengthLessThan(this string input, int length)
    {
        return input.Length < length;
    }

    /// <summary>
    /// بررسی اینکه طول رشته کمتر یا مساوی مقدار داده‌شده است یا خیر
    /// </summary>
    /// <param name="input">رشته ورودی برای بررسی طول</param>
    /// <param name="length">مقدار طول</param>
    /// <returns>اگر طول رشته کمتر یا مساوی مقدار داده‌شده باشد، مقدار true باز می‌گرداند</returns>
    public static bool IsLengthLessThanOrEqual(this string input, int length)
    {
        return input.Length <= length;
    }

    /// <summary>
    /// بررسی اینکه طول رشته بزرگتر از مقدار داده‌شده است یا خیر
    /// </summary>
    /// <param name="input">رشته ورودی برای بررسی طول</param>
    /// <param name="length">مقدار طول</param>
    /// <returns>اگر طول رشته بزرگتر از مقدار داده‌شده باشد، مقدار true باز می‌گرداند</returns>
    public static bool IsLengthGreaterThan(this string input, int length)
    {
        return input.Length > length;
    }

    /// <summary>
    /// بررسی اینکه طول رشته بزرگتر یا مساوی مقدار داده‌شده است یا خیر
    /// </summary>
    /// <param name="input">رشته ورودی برای بررسی طول</param>
    /// <param name="length">مقدار طول</param>
    /// <returns>اگر طول رشته بزرگتر یا مساوی مقدار داده‌شده باشد، مقدار true باز می‌گرداند</returns>
    public static bool IsLengthGreaterThanOrEqual(this string input, int length)
    {
        return input.Length >= length;
    }

    /// <summary>
    /// بررسی اینکه طول رشته دقیقا برابر با مقدار داده‌شده است یا خیر
    /// </summary>
    /// <param name="input">رشته ورودی برای بررسی طول</param>
    /// <param name="length">مقدار طول</param>
    /// <returns>اگر طول رشته برابر با مقدار داده‌شده باشد، مقدار true باز می‌گرداند</returns>
    public static bool IsLengthEqual(this string input, int length)
    {
        return input.Length == length;
    }
}
