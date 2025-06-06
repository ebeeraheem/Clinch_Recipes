using System.ComponentModel.DataAnnotations;

namespace CodeStash.Domain.Entities;
public class AuditLog
{
    public int Id { get; set; }

    [MaxLength(450)]
    public string? UserId { get; set; }

    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    [MaxLength(450)]
    public string EntityName { get; set; } = string.Empty;

    [MaxLength(450)]
    public string EntityDisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TimeStamp { get; set; }

    [MaxLength(8000)]
    public string? OldValues { get; set; }

    [MaxLength(8000)]
    public string? NewValues { get; set; }

    [MaxLength(2000)]
    public string? AffectedColumns { get; set; }

    [MaxLength(450)]
    public string? PrimaryKey { get; set; }
}
