using System.Data;
using Base.Extensions.Serializers.EPPlus.Extensions;
using Base.Extensions.Serializers.Abstractions;
using Base.Extensions.Translations.Abstractions;

namespace Base.Extensions.Serializers.EPPlus.Services;

public class EPPlusExcelSerializer(ITranslator translator) : IExcelSerializer
{
    public byte[] ListToExcelByteArray<T>(List<T> list, string sheetName = "Result") => list.ToExcelByteArray(translator, sheetName);

    public DataTable ExcelToDataTable(byte[] bytes) => bytes.ToDataTableFromExcel();

    public List<T> ExcelToList<T>(byte[] bytes) => bytes.ToDataTableFromExcel().ToList<T>(translator);
}
