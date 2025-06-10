using System.ComponentModel.DataAnnotations;

namespace CodeStash.Domain.Models;
public class Country
{
    [Key]
    [MaxLength(10)]
    public string Code { get; set; } = string.Empty; // ISO 3166-1 alpha-2 code; E.g., "NG" for Nigeria

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
