using System.ComponentModel.DataAnnotations;

namespace Base.Infrastructure.Messaging.Cap.Entity;

public partial class CapLock
{
    [Key]
    [StringLength(128)]
    public string Key { get; set; } = null!;

    [StringLength(256)]
    public string Instance { get; set; } = null!;

    public DateTime LastLockTime { get; set; }
}
