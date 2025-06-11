using CodeStash.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeStash.Domain.Entities;

[Index(nameof(Title))]
[Index(nameof(Slug), IsUnique = true)]
public class Note : IAuditableEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Slug { get; set; } = string.Empty;

    [MaxLength(24000)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; } // TODO: Add description back into the UI

    public bool IsPrivate { get; set; } = false;
    public int ViewCount { get; set; }

    [ForeignKey(nameof(Author))]
    public string AuthorId { get; set; } = string.Empty;

    // Navigation properties
    public List<Tag> Tags { get; set; } = [];
    public ApplicationUser Author { get; set; } = null!;

    // Audit related properties
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
