namespace Base.Extensions.ChangeDataLog.Sql.Options;

public class ChangeDataLogHamsterOptions
{
    public List<string> propertyForReject { get; set; } = new List<string>
    {
        "CreatedByUserId",
        "CreatedDateTime",
        "ModifiedByUserId",
        "ModifiedDateTime"
    };
    public string BusinessIdFieldName { get; set; } = "BusinessId";

}