using Clinch_Recipes.Data;
using Clinch_Recipes.HelperMethods;
using Clinch_Recipes.HelperMethods.Pagination;
using Clinch_Recipes.NoteEntity;
using Clinch_Recipes.UserEntity;
using Delta;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
    connectionString,
    options =>
    {
        options.EnableRetryOnFailure(3);
    }));

builder.Services.AddSqlServer<ApplicationDbContext>(connectionString);

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
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8;
})
   .AddEntityFrameworkStores<ApplicationDbContext>()
   .AddDefaultTokenProviders();

builder.Services.AddScoped<INoteRepository, NoteRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<UserHelper>();
builder.Services.AddScoped<IPagedResultService, PagedResultService>();

builder.Services.AddMemoryCache();

var app = builder.Build();

await app.Services.SeedRoles();
await app.Services.SeedDefaultUsers();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseDelta<ApplicationDbContext>();

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
