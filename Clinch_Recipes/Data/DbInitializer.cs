using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Identity;

namespace Clinch_Recipes.Data;

public static class DbInitializer
{
    public static async Task SeedRoles(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        List<string> roles = ["Admin", "User"];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = role });
            }
        }
    }

    public static async Task SeedDefaultUsers(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        var firstName = config.GetValue<string>("AppUser:FirstName");
        var lastName = config.GetValue<string>("AppUser:LastName");
        var password = config.GetValue<string>("AppUser:Password");
        var email = config.GetValue<string>("AppUser:Email");
        var phone = config.GetValue<string>("AppUser:Phone");
        var role = config.GetValue<string>("AppUser:Role");

        ArgumentNullException.ThrowIfNull(firstName);
        ArgumentNullException.ThrowIfNull(lastName);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(email);
        ArgumentNullException.ThrowIfNull(phone);
        ArgumentNullException.ThrowIfNull(role);

        var admin = new ApplicationUser()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            PhoneNumber = phone,
            Role = role
        };

        if (!userManager.Users.Any(a => a.Email!.Equals(admin.Email)))
        {
            var result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, admin.Role);
            }
        }
    }
}
