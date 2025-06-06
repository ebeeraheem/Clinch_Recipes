using CodeStash.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeStash.Domain.Entities;

[Index(nameof(Name))]
public class Tag : IAuditableEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public List<Note> Notes { get; set; } = [];

    // Audit related properties
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
