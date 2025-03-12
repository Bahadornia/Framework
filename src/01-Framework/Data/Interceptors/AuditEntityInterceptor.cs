using App.Framework.Contracts.Entities;
using App.Framework.DDD;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.Json;

namespace App.Framework.Data.Interceptors;

public sealed class AuditEntityInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILogger<AuditEntityInterceptor> _logger;

    public AuditEntityInterceptor(IHttpContextAccessor contextAccessor, ILogger<AuditEntityInterceptor> logger)
    {
        _contextAccessor = contextAccessor;
        _logger = logger;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {

        string userId = "1";
        AuditLog auditLog = new();
        IKey? primaryKey;


        //if (_contextAccessor.HttpContext?.User.Identity?.IsAuthenticated == false)
        //{
        //    return base.SavingChangesAsync(eventData, result, cancellationToken);
        //}

        userId = _contextAccessor.HttpContext!.User.FindFirst(ClaimTypes.NameIdentifier)?.Value! ?? "1";

        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        foreach (var entry in dbContext.ChangeTracker.Entries<Entity>())
        {
            if (entry.Entity is AuditLog || entry.Entity is not IAuditable)
            {
                continue;
            }
            primaryKey = entry.Metadata.FindPrimaryKey();

            if (primaryKey is not null)
            {
                auditLog.EntityId = entry.Property(primaryKey.Properties[0].Name).CurrentValue!.ToString();
            }
            else
            {
                auditLog.EntityId = default!;
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
                    auditLog.OldValue = $"{property.Metadata.Name} - {property.OriginalValue}";
                    auditLog.NewValue = $"{property.Metadata.Name} - {property.CurrentValue}";
                }
                auditLog.EntityName = entry.Entity.GetType().Name;
                auditLog.LastModifiedBy = userId;
                auditLog.LastModifiedAt = DateTime.UtcNow;
            }
        }

        dbContext.Set<AuditLog>().Add(auditLog);

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

