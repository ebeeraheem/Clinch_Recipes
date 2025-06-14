using System.ComponentModel.DataAnnotations;

namespace CodeStash.Application.Models.Dtos;

public class ChangeEmailDto
{
    public string CurrentPassword { get; set; } = string.Empty;

    [EmailAddress]
    public string NewEmail { get; set; } = string.Empty;

    [Compare(nameof(NewEmail), ErrorMessage = "The new email and confirmation email do not match.")]
    public string ConfirmEmail { get; set; } = string.Empty;
}