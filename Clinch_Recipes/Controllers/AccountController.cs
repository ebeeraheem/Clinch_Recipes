using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Clinch_Recipes.Controllers;
public class AccountController(UserManager<ApplicationUser> userManager) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(string email, string password)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ErrorMessage = "ModelState is invalid.";
            return View(ModelState);
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            ViewBag.ErrorMessage = "User not found.";
            return View();
        }

        var isValidPassword = await userManager.CheckPasswordAsync(user, password);

        if (!isValidPassword)
        {
            ViewBag.ErrorMessage = "Invalid credentials.";
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = true,
            IssuedUtc = DateTime.UtcNow
        };

        await HttpContext.SignInAsync(
            new ClaimsPrincipal(claimsIdentity),
            authProperties);

        return RedirectToAction("Index", "Notes");
    }

    public async Task<ActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Notes");
    }
}
