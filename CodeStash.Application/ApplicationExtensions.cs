using CodeStash.Application.Services;
using CodeStash.Infrastructure.Persistence.Seeder;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Slugify;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

namespace CodeStash.Application;
public static class ApplicationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration,
        ConfigureHostBuilder host)
    {
        // Register framework services
        services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

        // Register custom services
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<INoteService, NoteService>();

        // Register other services
        services.AddScoped<IPagedResultService, PagedResultService>();
        services.AddScoped<ISlugHelper, SlugHelper>();
        services.AddScoped<UserHelper>();

        // Configure rate limiting
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // global rate limiter
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var key = context.User.Identity?.Name
                                ?? context.Connection.RemoteIpAddress?.ToString()
                                ?? "unknown";
                return RateLimitPartition.GetFixedWindowLimiter(key, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 500,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 20
                });
            });
        });

        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        host.UseSerilog();

        // Configure Hangfire
        services.AddHangfire(x => x.UseSqlServerStorage(
            configuration.GetConnectionString("DefaultConnection")));
        services.AddHangfireServer();

        // Configure cookie authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
            .AddCookie(options =>
            {
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
            });

        services.AddAuthorization();

        return services;
    }

    public static WebApplication UseApplicationServices(this WebApplication app)
    {
        // Seed the database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var configuration = services.GetRequiredService<IConfiguration>();

            try
            {
                var logger = loggerFactory.CreateLogger("DatabaseSeeder");

                services.SeedRoles(logger).GetAwaiter().GetResult();
                services.SeedUsers(configuration, logger).GetAwaiter().GetResult();
                services.SeedTags(logger).GetAwaiter().GetResult();
                services.SeedCountries(logger).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("DatabaseSeeder");
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();
        app.UseRateLimiter();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHangfireDashboard();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        return app;
    }
}
