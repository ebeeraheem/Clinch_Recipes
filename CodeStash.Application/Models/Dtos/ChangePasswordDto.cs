using System.ComponentModel.DataAnnotations;

namespace CodeStash.Application.Models.Dtos;

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;

    [Compare(nameof(NewPassword), ErrorMessage = "The new password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}