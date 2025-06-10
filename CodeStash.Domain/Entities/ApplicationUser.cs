using CodeStash.Domain.Contracts;
using CodeStash.Domain.Models;
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
    public string? ProfileImageUrl { get; set; }
    public Country? Country { get; set; }

    // Public profile settings
    public bool IsEmailPublic { get; set; } = false;
    public bool IsLocationPublic { get; set; } = false;
    public bool IsSocialLinksPublic { get; set; } = true;

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
