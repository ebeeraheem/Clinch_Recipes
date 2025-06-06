using CodeStash.Domain.Entities;
using CodeStash.Domain.Enums;
using CodeStash.Infrastructure.Persistence.Seeder.SeedData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CodeStash.Infrastructure.Persistence.Seeder;
public static class DbInitializer
{
    public static async Task SeedRoles(this IServiceProvider serviceProvider, ILogger logger)
    {
        logger.LogInformation("Seeding roles...");

        var roleManager = serviceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        // Check if roles already exist
        if (await roleManager.Roles.AnyAsync())
        {
            logger.LogInformation("Roles already exist. Skipping seeding.");
            return;
        }

        // Get the list of roles from Roles enum
        var roles = Enum.GetValues(typeof(Roles))
            .Cast<Roles>()
            .Select(role => new IdentityRole(role.ToString()))
            .ToList();

        // Create each role
        foreach (var role in roles)
        {
            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                logger.LogInformation("Role {RoleName} created successfully.", role.Name);
            }
            else
            {
                logger.LogError("Error creating role {RoleName}: {@Errors}",
                    role.Name, result.Errors);
            }
        }

        logger.LogInformation("Roles seeding completed.");
    }

    public static async Task SeedUsers(this IServiceProvider serviceProvider, IConfiguration configuration, ILogger logger)
    {
        logger.LogInformation("Seeding default users...");

        var userManager = serviceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        // Check if users already exist
        if (await userManager.Users.AnyAsync())
        {
            logger.LogInformation("Users already exist. Skipping seeding.");
            return;
        }

        // Get seed user from configuration
        var seedUser = configuration.GetSection("AppUser").Get<SeedUser>();

        if (seedUser is null)
        {
            logger.LogError("Seed user configuration is missing.");
            throw new InvalidOperationException("Seed user configuration is not set.");
        }

        // Create the admin user
        var admin = new ApplicationUser
        {
            FirstName = seedUser.FirstName,
            LastName = seedUser.LastName,
            Email = seedUser.Email,
            UserName = seedUser.Email,
        };

        var result = await userManager.CreateAsync(admin, seedUser.Password);

        if (result.Succeeded)
        {
            // Assign the user to the Admin role
            await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            logger.LogInformation("Default user created and assigned to role {RoleName}.", Roles.Admin.ToString());
        }
        else
        {
            logger.LogError("Error creating default user: {@Errors}", result.Errors);
            throw new InvalidOperationException("Failed to create default user. See logs for details.");
        }
    }

    public static async Task SeedTags(this IServiceProvider serviceProvider, ILogger logger)
    {
        logger.LogInformation("Seeding default tags...");

        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Check if tags already exist
        if (await context.Tags.AnyAsync())
        {
            logger.LogInformation("Tags already exist. Skipping seeding.");
            return;
        }

        var tags = TagsData.GetInitialTags();

        if (tags is null || tags.Count == 0)
        {
            logger.LogError("No tags found to seed.");
            throw new InvalidOperationException("No tags available for seeding.");
        }

        context.Tags.AddRange(tags);
        var result = await context.SaveChangesAsync();

        if (result <= 0)
        {
            logger.LogError("Failed to seed default tags.");
            throw new InvalidOperationException("Failed to seed default tags. See logs for details.");
        }

        logger.LogInformation("Default tags seeded successfully.");
    }
}
