namespace App.Framework.DDD;

public class Entity<T> : Entity, IEntity<T>, IAuditable
{
    public T Id { get; set; }

}
public class Entity
{
}
