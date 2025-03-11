namespace App.Framework.DDD;

public class Entity<T> : Entity, IEntity<T>
{
    public T Id { get; set; }

}
public class Entity
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
    public string OldValue { get; set; } = default!;
    public string NewValue { get; set; } = default!;
}
