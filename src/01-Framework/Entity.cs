﻿namespace App.Framework;

public class Entity<T> : IEntity<T>
    where T : class
{
    public T Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? LastModified { get; set; }
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}
