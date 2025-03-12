using App.Framework.DDD;

namespace App.Framework.Contracts.Entities;

public sealed class AuditLog: Entity
{
    public int Id { get; set; }
    public string EntityName { get; set; } = default!;
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? OldValue { get; set; } = default!;
    public string? NewValue { get; set; } = default!;


}
