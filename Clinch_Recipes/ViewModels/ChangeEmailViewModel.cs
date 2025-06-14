using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class ChangeEmailViewModel
{
    [Required(ErrorMessage = "Current password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Current Password")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "New Email Address")]
    public string NewEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please confirm your new email address")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Confirm New Email Address")]
    [Compare(nameof(NewEmail), ErrorMessage = "The new email and confirmation email do not match")]
    public string ConfirmEmail { get; set; } = string.Empty;
}
