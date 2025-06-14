using CodeStash.Domain.Entities;
using CodeStash.Infrastructure.EmailService;
using CodeStash.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeStash.Infrastructure;
public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
        })
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>()
           .AddDefaultTokenProviders();

        services.AddHttpContextAccessor();

        var emailServiceConfig = configuration
            .GetSection("EmailService")
            .Get<EmailServiceOptions>()
            ?? throw new InvalidOperationException("EmailService configuration is missing.");

        services.AddHttpClient("EmailService", client =>
        {
            client.BaseAddress = new Uri(emailServiceConfig.BaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        services.Configure<EmailServiceOptions>(configuration.GetSection("EmailService"));

        services.AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}
