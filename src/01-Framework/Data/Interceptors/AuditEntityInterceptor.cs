using App.Framework.DDD;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace App.Framework.Data.Interceptors;

public sealed class AuditEntityInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuditEntityInterceptor(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        string userId = null;

        if (_contextAccessor.HttpContext?.User.Identity?.IsAuthenticated == false)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
        
          userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        
        var dbContext = eventData.Context;
        if (dbContext is not null)
        {

            foreach (var entry in dbContext.ChangeTracker.Entries<Entity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = userId;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnEntities())
                {
                    foreach(var property in entry.Properties)
                    {
                        entry.Entity.OldValue = (string)property.OriginalValue!;
                        entry.Entity.NewValue = (string)property.CurrentValue!;
                    }
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
}

public static class Extensions
{
    public static bool HasChangedOwnEntities(this EntityEntry entry)
    {
        return entry.References.Any(r =>
        r.TargetEntry != null &&
        r.TargetEntry.Metadata.IsOwned() &&
        (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}

