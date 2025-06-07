using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Username is required")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 30 characters")]
    [RegularExpression(@"^(?!.*([_-])\1)(?!.*(_-|-_))(?![_-])[a-zA-Z0-9][a-zA-Z0-9_-]*[a-zA-Z0-9]$",
    ErrorMessage = "Username can only contain letters, numbers, hyphens, and underscores, cannot start or end with a symbol, and cannot contain consecutive symbols.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
    ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one symbol.")]
    public string Password { get; set; } = string.Empty;


    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }
}
