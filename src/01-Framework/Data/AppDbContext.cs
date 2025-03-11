using App.Framework.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.Framework.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
}
