using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinch_Recipes.Controllers;
public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : Controller
{
    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginModel model)
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

        var isValidPassword = await userManager.CheckPasswordAsync(user, model.Password);

        if (!isValidPassword)
        {
            ViewBag.ErrorMessage = "Invalid credentials.";
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(
            user, model.Password, false, false);

        if (!result.Succeeded)
        {
            ViewBag.ErrorMessage = "Error signing in user.";
            return View();
        }

        return RedirectToAction("Index", "Notes");
    }

    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Notes");
    }
}
