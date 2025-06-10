using CodeStash.Domain.Contracts;
using CodeStash.Domain.Entities;
using CodeStash.Domain.Enums;
using CodeStash.Domain.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Security.Claims;

namespace CodeStash.Infrastructure.Persistence;
public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IHttpContextAccessor httpContextAccessor) : IdentityDbContext(options)
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    public DbSet<Note> Notes { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;

    public DbSet<Country> Countries { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply configuration to all entities that implement IAuditableEntity  
        foreach (var entityType in builder.Model.GetEntityTypes()
            .Select(entityType => entityType.ClrType)
            .Where(entityType => typeof(IAuditableEntity).IsAssignableFrom(entityType)))
        {
            builder.Entity(entityType)
                .Property(nameof(IAuditableEntity.CreatedBy))
                .HasMaxLength(150);

            builder.Entity(entityType)
                .Property(nameof(IAuditableEntity.ModifiedBy))
                .HasMaxLength(150);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Apply audit trail before saving
        OnBeforeSaveChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void OnBeforeSaveChanges()
    {
        var userId = httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? httpContextAccessor?.HttpContext?.Connection.RemoteIpAddress?.ToString()
            ?? "System";

        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();

        var auditableEntries = ChangeTracker.Entries()
                .Where(entry => entry.State is not (EntityState.Detached or EntityState.Unchanged)
                    && entry.Entity is not AuditLog);

        foreach (var entry in auditableEntries)
        {
            // Create a new audit entry based on the current entity
            var auditEntry = new AuditEntry(entry)
            {
                UserId = userId,
                EntityName = entry.Entity.GetType().Name,
                EntityDisplayName = entry.Entity.GetType().Name.Humanize(LetterCasing.Title)
            };

            // Populate audit fields in auditable properties
            if (entry.Entity is IAuditableEntity auditableEntity)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditableEntity.CreatedAt = DateTime.UtcNow;
                        if (string.IsNullOrEmpty(auditableEntity.CreatedBy))
                        {
                            auditableEntity.CreatedBy = userId;
                        }
                        break;
                    case EntityState.Modified:
                        auditableEntity.ModifiedAt = DateTime.UtcNow;
                        auditableEntity.ModifiedBy = userId;
                        break;
                }
            }

            // Record primary key
            var primaryKeyProperty = entry.Properties
                .FirstOrDefault(p => p.Metadata.IsPrimaryKey());

            if (primaryKeyProperty is not null)
            {
                auditEntry.PrimaryKey = primaryKeyProperty.CurrentValue?.ToString();
            }

            // Collect changed properties
            var relevantProperties = entry.Properties
                .Where(p => !ShouldIgnoreProperty(p.Metadata.Name))
                .ToList();

            foreach (var property in relevantProperties)
            {
                var propertyName = property.Metadata.Name;
                var friendlyName = property.Metadata.Name.Humanize(LetterCasing.Title);

                // Is this a sensitive property that should be redacted?
                var isSensitive = IsSensitiveProperty(propertyName);

                // Redact sensitive properties
                var formattedCurrentValue = isSensitive ? "[REDACTED]" : property.CurrentValue;
                var formattedOriginalValue = isSensitive ? "[REDACTED]" : property.OriginalValue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[friendlyName] = formattedCurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[friendlyName] = formattedOriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && !Equals(property.CurrentValue, property.OriginalValue))
                        {
                            auditEntry.ChangedColumns.Add(friendlyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[friendlyName] = formattedOriginalValue;
                            auditEntry.NewValues[friendlyName] = formattedCurrentValue;
                        }
                        break;
                }
            }

            // Generate human-readable description of what happened
            auditEntry.Description = GenerateDescription(entry, auditEntry);

            // Add the audit entry to the list if it represents a meaningful change
            if (auditEntry.AuditType is not AuditType.None)
            {
                auditEntries.Add(auditEntry);
            }
        }

        // Add all audit logs to the context
        AuditLogs.AddRange(auditEntries.Select(a => a.ToAuditLog()));


    }

    // Technical fields that should be excluded from audit logs
    private static readonly HashSet<string> TechnicalFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "ConcurrencyStamp",
        "SecurityStamp",
        "RowVersion",
        "Version",
        "Timestamp",
        "CreatedAt",
        "CreatedBy",
        "ModifiedAt",
        "ModifiedBy"
    };

    // Sensitive fields that should be redacted
    private static readonly HashSet<string> SensitiveFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "Password",
        "PasswordHash",
        "Secret",
        "ApiKey",
        "Token",
        "Salt"
    };

    private static bool ShouldIgnoreProperty(string propertyName)
    {
        // Ignore technical implementation details
        return TechnicalFields.Contains(propertyName);
    }

    private static bool IsSensitiveProperty(string propertyName)
    {
        // Check if this is a sensitive field that should be redacted
        return SensitiveFields.Any(sensitivePattern =>
            propertyName.Contains(sensitivePattern, StringComparison.OrdinalIgnoreCase));
    }

    private static string GenerateDescription(EntityEntry entry, AuditEntry auditEntry)
    {
        var entityDisplayName = auditEntry.EntityDisplayName;

        switch (entry.State)
        {
            case EntityState.Added:
                return $"Created new {entityDisplayName}";

            case EntityState.Deleted:
                return $"Deleted {entityDisplayName}";

            case EntityState.Modified:
                // For modifications, include what changed
                if (auditEntry.ChangedColumns.Count == 0)
                    return $"Updated {entityDisplayName} (no changes)";

                if (auditEntry.ChangedColumns.Count == 1)
                    return $"Updated {entityDisplayName}: Changed {auditEntry.ChangedColumns[0]}";

                if (auditEntry.ChangedColumns.Count <= 3)
                    return $"Updated {entityDisplayName}: Changed {string.Join(", ", auditEntry.ChangedColumns)}";

                return $"Updated {entityDisplayName}: Changed {auditEntry.ChangedColumns.Count} properties";

            case EntityState.Unchanged:
                return $"No changes made to {entityDisplayName}";

            case EntityState.Detached:
                return $"{entityDisplayName} is detached from tracking.";

            default:
                return $"Unknown operation on {entityDisplayName}";
        }
    }
}
