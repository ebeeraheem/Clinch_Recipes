using Clinch_Recipes.ViewModels;
using CodeStash.Application.Utilities;
using CodeStash.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Clinch_Recipes.Controllers;
public class AccountController(
    SignInManager<ApplicationUser> signInManager,
    UserHelper userHelper) : Controller
{
    [AllowAnonymous]
    public ActionResult Login(string? returnUrl = null)
    {
        if (userHelper.IsAuthenticated())
        {
            return RedirectToAction("Index", "Notes");
        }

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Login(LoginModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(
            model.Email, model.Password, model.RememberMe, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        return !string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl) ?
            Redirect(returnUrl)
            : RedirectToAction("Index", "Notes");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Notes");
    }
}
