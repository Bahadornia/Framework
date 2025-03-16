using App.Framework.DDD;

namespace App.Framework.Contracts.Entities;

public sealed class AuditLog: Entity
{
    public int Id { get; set; }
    public string? EntityName { get; set; }
    public string? EntityId { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? IPAddress { get; set; }
    public string? Agent { get; set; }
    public string? Action { get; set; }
}
