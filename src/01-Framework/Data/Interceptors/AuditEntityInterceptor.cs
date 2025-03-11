using App.Framework.Contracts.Entities;
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
        AuditLog auditLog = new();

        if (_contextAccessor.HttpContext?.User.Identity?.IsAuthenticated == false)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        var dbContext = eventData.Context as AppDbContext;
        if (dbContext is not null)
        {

            foreach (var entry in dbContext.ChangeTracker.Entries<Entity>())
            {
                if (entry.Entity is AuditLog || entry.Entity is not IAuditable)
                {
                    continue;
                }

                if (entry.State == EntityState.Added)
                {
                    auditLog.CreatedAt = DateTime.UtcNow;
                    auditLog.CreatedBy = userId;
                    auditLog.EntityName = entry.Entity.GetType().Name;
                }

                if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnEntities())
                {
                    foreach (var property in entry.Properties)
                    {
                        auditLog.OldValue = $"{property.Metadata.Name} - {property.CurrentValue}";
                        auditLog.NewValue = $"{property.Metadata.Name} - {property.OriginalValue}";
                    }
                    auditLog.LastModifiedBy = userId;
                    auditLog.LastModifiedAt = DateTime.UtcNow;
                }
            }

            dbContext.AuditLogs.Add(auditLog);
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

