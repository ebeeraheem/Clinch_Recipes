using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class ResetPasswordViewModel
{
    [Required]
    public string Id { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Length(8, 100, ErrorMessage = "Password must be between 8 and 100 characters long")]
    [Display(Name = "New Password")]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Display(Name = "Confirm New Password")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}