using CodeStash.Domain.Entities;
using CodeStash.Domain.Enums;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace CodeStash.Domain.Models;
public class AuditEntry(EntityEntry entry)
{
    public EntityEntry Entry { get; } = entry;
    public string? UserId { get; set; }
    public required string EntityName { get; set; }
    public string EntityDisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? PrimaryKey { get; set; }
    public Dictionary<string, object?> OldValues { get; } = [];
    public Dictionary<string, object?> NewValues { get; } = [];
    public AuditType AuditType { get; set; }
    public List<string> ChangedColumns { get; } = [];

    public AuditLog ToAuditLog()
    {
        return new AuditLog
        {
            UserId = UserId,
            Type = AuditType.ToString(),
            EntityName = EntityName,
            EntityDisplayName = EntityDisplayName,
            Description = Description,
            TimeStamp = DateTime.UtcNow,
            PrimaryKey = PrimaryKey,
            OldValues = JsonSerializer.Serialize(OldValues),
            NewValues = JsonSerializer.Serialize(NewValues),
            AffectedColumns = JsonSerializer.Serialize(ChangedColumns)
        };
    }
}
