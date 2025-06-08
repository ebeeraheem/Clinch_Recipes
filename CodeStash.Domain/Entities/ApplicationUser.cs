using CodeStash.Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CodeStash.Domain.Entities;
public class ApplicationUser : IdentityUser, IAuditableEntity
{
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Bio { get; set; }
    public bool IsEmailPublic { get; set; } = false;

    [MaxLength(100)]
    public string? Location { get; set; } // City, Country
    public string? ProfileImageUrl { get; set; }

    // Social links
    public string? WebsiteUrl { get; set; }

    [MaxLength(100)]
    public string? GitHubUsername { get; set; }

    [MaxLength(100)]
    public string? TwitterHandle { get; set; }

    [MaxLength(100)]
    public string? LinkedInProfile { get; set; }

    // Audit related properties
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
