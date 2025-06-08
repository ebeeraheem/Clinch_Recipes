using CodeStash.Application.Utilities;
using CodeStash.Domain.Entities;
using CodeStash.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CodeStash.Controllers;
public partial class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserHelper userHelper) : Controller
{
    [AllowAnonymous]
    public ActionResult Login(string? returnUrl = null)
    {
        if (userHelper.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Try to find a user by email or username
        var user = await signInManager.UserManager.FindByEmailAsync(model.EmailOrUsername) ??
                   await signInManager.UserManager.FindByNameAsync(model.EmailOrUsername);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(
            user, model.Password, model.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        // Add default claims
        var claims = new List<Claim>()
        {
            new("profile_image_url", user.ProfileImageUrl ?? string.Empty)
        };

        await signInManager.UserManager.AddClaimsAsync(user, claims);

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ?
            Redirect(returnUrl)
            : RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public ActionResult Register()
    {
        if (userHelper.IsAuthenticated())
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Username rules (should match client-side)
        if (model.Username.Length < 3 || model.Username.Length > 30)
        {
            ModelState.AddModelError(nameof(model.Username), "Username must be between 3 and 30 characters.");
            return View(model);
        }

        if (!UsernameCharacterValidationRegex().IsMatch(model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "Username can only contain letters, numbers, hyphens, and underscores.");
            return View(model);
        }

        if (!UsernameStartEndValidationRegex().IsMatch(model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "Username cannot begin or end with a non-alphanumeric character.");
            return View(model);
        }

        if (UsernameConsecutiveSpecialCharacterRegex().IsMatch(model.Username))
        {
            ModelState.AddModelError(nameof(model.Username), "Username cannot contain consecutive non-alphanumeric characters.");
            return View(model);
        }

        // Check if username exists
        var existingUser = await signInManager.UserManager.FindByNameAsync(model.Username);
        if (existingUser is not null)
        {
            ModelState.AddModelError(nameof(model.Username), "This username is already taken.");
            return View(model);
        }

        // Check if email exists
        var existingEmailUser = await signInManager.UserManager.FindByEmailAsync(model.Email);
        if (existingEmailUser is not null)
        {
            ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            EmailConfirmed = true // Assuming email confirmation is not required for simplicity
        };

        var result = await signInManager.UserManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            // Add default claims
            var claims = new List<Claim>()
            {
                new("profile_image_url", user.ProfileImageUrl ?? string.Empty)
            };

            await signInManager.UserManager.AddClaimsAsync(user, claims);
            await signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> IsUsernameAvailable(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return Json(new { isValid = false, message = "Username is required." });

        // Username rules (should match client-side)
        if (username.Length < 3 || username.Length > 30)
            return Json(new { isValid = false, message = "Username must be between 3 and 30 characters." });

        if (!UsernameCharacterValidationRegex().IsMatch(username))
            return Json(new { isValid = false, message = "Username can only contain letters, numbers, hyphens, and underscores." });

        if (!UsernameStartEndValidationRegex().IsMatch(username))
            return Json(new { isValid = false, message = "Username cannot begin or end with a non-alphanumeric character." });

        if (UsernameConsecutiveSpecialCharacterRegex().IsMatch(username))
            return Json(new { isValid = false, message = "Username cannot contain consecutive non-alphanumeric characters." });

        // Check if username exists (replace with your user manager or db context)
        var exists = await signInManager.UserManager.FindByNameAsync(username) != null;
        if (exists)
            return Json(new { isValid = false, message = "This username is already taken." });

        return Json(new { isValid = true });
    }

    [GeneratedRegex(@"^[a-zA-Z0-9_-]+$")]
    private static partial Regex UsernameCharacterValidationRegex();
    [GeneratedRegex(@"^[a-zA-Z0-9](?:.*[a-zA-Z0-9])?$")]
    private static partial Regex UsernameStartEndValidationRegex();
    [GeneratedRegex(@"[-_]{2,}")]
    private static partial Regex UsernameConsecutiveSpecialCharacterRegex();
}
