using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinch_Recipes.Controllers;
public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : Controller
{
    [HttpGet]
    public ActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage = "ModelState is invalid.";
            return View();
        }

        var user = await userManager.FindByEmailAsync(model.Email);

        if (user is null)
        {
            ViewBag.ErrorMessage = "User not found.";
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(
            user, model.Password, false, false);

        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = "Error signing in user.";
            return View();
        }

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ?
            Redirect(returnUrl)
            : RedirectToAction("Index", "Notes");
    }

    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Notes");
    }
}
