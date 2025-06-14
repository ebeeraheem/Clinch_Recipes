using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class ForgotPasswordViewModel
{
    [Required, EmailAddress, Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;
}
