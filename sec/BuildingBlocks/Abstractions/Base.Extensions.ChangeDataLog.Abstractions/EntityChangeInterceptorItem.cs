namespace Base.Extensions.ChangeDataLog;
public class EntityChangeInterceptorItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ContextName { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public string EntityId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Ip { get; set; } = null!;
    public string TransactionId { get; set; } = null!;
    public DateTime DateOfOccurrence { get; set; }
    public string ChangeType { get; set; } = null!;

    public List<PropertyChangeLogItem> PropertyChangeLogItems { get; set; } = [];
    public void AddPropertyChangeItem(string propertyName, string value)
    {
        PropertyChangeLogItems.Add(new PropertyChangeLogItem
        {
            ChangeInterceptorItemId = Id,
            PropertyName = propertyName,
            Value = value
        });
    }
}
