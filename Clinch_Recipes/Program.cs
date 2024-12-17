using Clinch_Recipes.Data;
using Clinch_Recipes.HelperMethods;
using Clinch_Recipes.NoteEntity;
using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
var audience = builder.Configuration.GetValue<string>("Jwt:Audience");
var key = builder.Configuration.GetValue<string>("Jwt:Key");

ArgumentNullException.ThrowIfNull(issuer);
ArgumentNullException.ThrowIfNull(audience);
ArgumentNullException.ThrowIfNull(key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = true;
        options.SlidingExpiration = true;
        options.Cookie.IsEssential = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(45);
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<TokenHelper>();

builder.Services.AddMemoryCache();

var app = builder.Build();

await app.Services.SeedRoles();
await app.Services.SeedDefaultUsers();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCookiePolicy();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notes}/{action=Index}/{id?}");

await app.RunAsync();
