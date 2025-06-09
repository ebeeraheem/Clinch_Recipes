using CodeStash.Domain.Entities;
using CodeStash.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeStash.Controllers;
#pragma warning disable S6934 // A Route attribute should be added to the controller when a route template is specified at the action level
public class UsersController : Controller
#pragma warning restore S6934 // A Route attribute should be added to the controller when a route template is specified at the action level
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var currentUserId = "ebeeraheem"; // Get from User.Identity in real implementation
        var viewModel = GetDummyUserProfile(currentUserId);
        return View(viewModel);
    }

    [Route("User/{username}")] // TODO: Change url to [HttpGet("{username}")]; i.e /users/{username}
    public async Task<IActionResult> PublicProfile(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            return NotFound();
        }

        var currentUserId = "ebeeraheem"; // Get from User.Identity in real implementation
        var viewModel = GetDummyPublicProfile(username, currentUserId);

        if (viewModel == null)
        {
            return NotFound();
        }

        return View(viewModel);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var currentUserId = "ebeeraheem";
        var userProfile = GetDummyUserProfile(currentUserId);

        var viewModel = new EditProfileViewModel
        {
            Username = userProfile.Username,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            Bio = userProfile.Bio,
            Location = userProfile.Location,
            Website = userProfile.Website,
            GitHubUsername = userProfile.GitHubUsername,
            TwitterHandle = userProfile.TwitterHandle,
            LinkedInProfile = userProfile.LinkedInProfile,
            IsEmailPublic = userProfile.IsEmailPublic,
            FavoriteLanguagesInput = string.Join(", ", userProfile.FavoriteLanguages),
            AvailableLanguages = GetDummyLanguages()
        };

        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(EditProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableLanguages = GetDummyLanguages();
            return View(model);
        }

        // TODO: Implement actual profile update logic
        TempData["SuccessMessage"] = "Profile updated successfully!";
        return RedirectToAction(nameof(Profile));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Settings()
    {
        var viewModel = GetDummyUserSettings();
        return View(viewModel);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Settings(UserSettingsViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: Implement actual settings update logic
        TempData["SuccessMessage"] = "Settings updated successfully!";
        return View(model);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // TODO: Implement actual password change logic
        // For demo, simulate password validation
        if (model.CurrentPassword != "password123")
        {
            ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Password changed successfully!";
        return RedirectToAction(nameof(Profile));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FollowUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Invalid user ID" });
        }

        // TODO: Implement actual follow logic
        return Json(new { success = true, message = "User followed successfully", isFollowing = true });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UnfollowUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Invalid user ID" });
        }

        // TODO: Implement actual unfollow logic
        return Json(new { success = true, message = "User unfollowed successfully", isFollowing = false });
    }

    // Dummy data methods
    private UserProfileViewModel GetDummyUserProfile(string userId)
    {
        return new UserProfileViewModel
        {
            Id = userId,
            Username = "ebeeraheem",
            Email = "ebeeraheem@example.com",
            FirstName = "Ebee",
            LastName = "Raheem",
            Bio = "Full-stack developer passionate about clean code and elegant solutions. Love working with modern web technologies and sharing knowledge with the community.",
            Location = "Nigeria",
            Website = "https://ebeeraheem.dev",
            GitHubUsername = "ebeeraheem",
            TwitterHandle = "ebeeraheem",
            LinkedInProfile = "https://linkedin.com/in/ebeeraheem",
            JoinedAt = DateTime.Parse("2024-01-15"),
            LastActiveAt = DateTime.Parse("2025-06-08 14:30:00"),
            ProfileImageUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop&crop=face",
            IsEmailPublic = true,
            FavoriteLanguages = new List<string> { "C#", "JavaScript", "TypeScript", "Python", "SQL" },
            Stats = GetDummyUserStats(),
            RecentNotes = GetDummyUserRecentNotes(),
            PopularNotes = GetDummyUserPopularNotes()
        };
    }

    private PublicUserProfileViewModel GetDummyPublicProfile(string username, string currentUserId)
    {
        if (username == "ebeeraheem")
        {
            return new PublicUserProfileViewModel
            {
                Id = "ebeeraheem",
                Username = "ebeeraheem",
                FirstName = "Ebee",
                LastName = "Raheem",
                Bio = "Full-stack developer passionate about clean code and elegant solutions. Love working with modern web technologies and sharing knowledge with the community.",
                Location = "Nigeria",
                Website = "https://ebeeraheem.dev",
                GitHubUsername = "ebeeraheem",
                TwitterHandle = "ebeeraheem",
                LinkedInProfile = "https://linkedin.com/in/ebeeraheem",
                JoinedAt = DateTime.Parse("2024-01-15"),
                LastActiveAt = DateTime.Parse("2025-06-08 14:30:00"),
                ProfileImageUrl = "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop&crop=face",
                IsEmailPublic = true,
                Email = "ebeeraheem@example.com",
                FavoriteLanguages = new List<string> { "C#", "JavaScript", "TypeScript", "Python", "SQL" },
                Stats = GetDummyUserStats(),
                PublicNotes = GetDummyUserPublicNotes(),
                IsCurrentUser = currentUserId == "ebeeraheem",
                IsFollowing = false
            };
        }

        // Return another dummy user for testing
        return new PublicUserProfileViewModel
        {
            Id = "johndoe",
            Username = "johndoe",
            FirstName = "John",
            LastName = "Doe",
            Bio = "Frontend developer specializing in React and Vue.js. Always learning something new!",
            Location = "San Francisco, CA",
            Website = "https://johndoe.dev",
            GitHubUsername = "johndoe",
            TwitterHandle = "johndoe",
            JoinedAt = DateTime.Parse("2023-08-20"),
            LastActiveAt = DateTime.Parse("2025-06-07 10:15:00"),
            ProfileImageUrl = "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&h=150&fit=crop&crop=face",
            IsEmailPublic = true,
            Email = "john@example.com",
            FavoriteLanguages = new List<string> { "JavaScript", "TypeScript", "CSS", "HTML" },
            Stats = new UserStatsViewModel
            {
                TotalNotes = 42,
                PublicNotes = 38,
                PrivateNotes = 4,
                TotalViews = 8420,
                Followers = 156,
                Following = 89,
                NotesThisMonth = 5,
                MostUsedLanguage = "JavaScript"
            },
            PublicNotes = GetDummyUserPublicNotes().Take(6).ToList(),
            IsCurrentUser = false,
            IsFollowing = true
        };
    }

    private UserStatsViewModel GetDummyUserStats()
    {
        return new UserStatsViewModel
        {
            TotalNotes = 127,
            PublicNotes = 98,
            PrivateNotes = 29,
            TotalViews = 24680,
            TotalLikes = 1845,
            Followers = 342,
            Following = 156,
            LanguageDistribution = new Dictionary<string, int>
            {
                { "C#", 35 },
                { "JavaScript", 28 },
                { "TypeScript", 22 },
                { "Python", 18 },
                { "SQL", 15 },
                { "CSS", 9 }
            },
            NotesPerMonth = new Dictionary<string, int>
            {
                { "2025-01", 8 },
                { "2025-02", 12 },
                { "2025-03", 15 },
                { "2025-04", 18 },
                { "2025-05", 22 },
                { "2025-06", 6 }
            },
            NotesThisWeek = 3,
            NotesThisMonth = 6,
            NotesThisYear = 81,
            AverageViewsPerNote = 194.3,
            MostUsedLanguage = "C#",
            FirstNoteDate = DateTime.Parse("2024-01-20"),
            LastNoteDate = DateTime.Parse("2025-06-06")
        };
    }

    private UserSettingsViewModel GetDummyUserSettings()
    {
        return new UserSettingsViewModel
        {
            IsProfilePublic = true,
            ShowEmail = false,
            ShowLocation = true,
            ShowSocialLinks = true,
            ShowStats = true,
            EmailOnNewFollower = true,
            EmailOnNoteComment = true,
            EmailOnNoteLike = false,
            WeeklyDigest = true,
            MarketingEmails = false,
            Theme = "dark",
            NotesPerPage = 12,
            DefaultNoteVisibility = "public",
            EditorFontSize = 14,
            ShowLineNumbers = true,
            AvailableThemes = new List<string> { "dark", "light", "auto" }
        };
    }

    private List<Note> GetDummyUserRecentNotes()
    {
        return new List<Note>
        {
            new Note
            {
                Id = "recent1",
                Title = "ASP.NET Core Dependency Injection Best Practices",
                Slug = "aspnet-core-di-best-practices",
                ViewCount = 234,
                CreatedAt = DateTime.Parse("2025-06-06 09:30:00"),
                Tags = new List<Tag> { new Tag { Name = "C#" }, new Tag { Name = "ASP.NET Core" } }
            },
            new Note
            {
                Id = "recent2",
                Title = "JavaScript Array Methods Performance",
                Slug = "js-array-methods-performance",
                ViewCount = 156,
                CreatedAt = DateTime.Parse("2025-06-04 14:20:00"),
                Tags = new List<Tag> { new Tag { Name = "JavaScript" }, new Tag { Name = "Performance" } }
            },
            new Note
            {
                Id = "recent3",
                Title = "SQL Query Optimization Techniques",
                Slug = "sql-query-optimization",
                ViewCount = 189,
                CreatedAt = DateTime.Parse("2025-06-02 11:45:00"),
                Tags = new List<Tag> { new Tag { Name = "SQL" }, new Tag { Name = "Database" } }
            }
        };
    }

    private List<Note> GetDummyUserPopularNotes()
    {
        return new List<Note>
        {
            new Note
            {
                Id = "popular1",
                Title = "React Custom Hooks Collection",
                Slug = "react-custom-hooks-collection",
                ViewCount = 1842,
                CreatedAt = DateTime.Parse("2025-03-15 16:00:00"),
                Tags = new List<Tag> { new Tag { Name = "React" }, new Tag { Name = "JavaScript" } }
            },
            new Note
            {
                Id = "popular2",
                Title = "Entity Framework Core Performance Tips",
                Slug = "ef-core-performance-tips",
                ViewCount = 1256,
                CreatedAt = DateTime.Parse("2025-02-28 10:30:00"),
                Tags = new List<Tag> { new Tag { Name = "C#" }, new Tag { Name = "Entity Framework" } }
            },
            new Note
            {
                Id = "popular3",
                Title = "CSS Grid Layout Masterclass",
                Slug = "css-grid-layout-masterclass",
                ViewCount = 987,
                CreatedAt = DateTime.Parse("2025-04-12 13:15:00"),
                Tags = new List<Tag> { new Tag { Name = "CSS" }, new Tag { Name = "Layout" } }
            }
        };
    }

    private List<Note> GetDummyUserPublicNotes()
    {
        return new List<Note>
        {
            new Note
            {
                Id = "public1",
                Title = "TypeScript Utility Types Guide",
                Slug = "typescript-utility-types-guide",
                ViewCount = 567,
                CreatedAt = DateTime.Parse("2025-05-28 14:00:00"),
                Tags = new List<Tag> { new Tag { Name = "TypeScript" }, new Tag { Name = "Guide" } }
            },
            new Note
            {
                Id = "public2",
                Title = "Node.js Express Middleware Patterns",
                Slug = "nodejs-express-middleware-patterns",
                ViewCount = 423,
                CreatedAt = DateTime.Parse("2025-05-20 11:30:00"),
                Tags = new List<Tag> { new Tag { Name = "Node.js" }, new Tag { Name = "Express" } }
            },
            new Note
            {
                Id = "public3",
                Title = "Python Data Analysis with Pandas",
                Slug = "python-pandas-data-analysis",
                ViewCount = 645,
                CreatedAt = DateTime.Parse("2025-05-15 09:45:00"),
                Tags = new List<Tag> { new Tag { Name = "Python" }, new Tag { Name = "Data Analysis" } }
            },
            new Note
            {
                Id = "public4",
                Title = "Docker Compose Development Setup",
                Slug = "docker-compose-dev-setup",
                ViewCount = 789,
                CreatedAt = DateTime.Parse("2025-05-10 16:20:00"),
                Tags = new List<Tag> { new Tag { Name = "Docker" }, new Tag { Name = "DevOps" } }
            },
            new Note
            {
                Id = "public5",
                Title = "Vue.js Composition API Examples",
                Slug = "vuejs-composition-api-examples",
                ViewCount = 334,
                CreatedAt = DateTime.Parse("2025-05-05 12:10:00"),
                Tags = new List<Tag> { new Tag { Name = "Vue.js" }, new Tag { Name = "JavaScript" } }
            },
            new Note
            {
                Id = "public6",
                Title = "Git Workflow Best Practices",
                Slug = "git-workflow-best-practices",
                ViewCount = 892,
                CreatedAt = DateTime.Parse("2025-04-30 08:30:00"),
                Tags = new List<Tag> { new Tag { Name = "Git" }, new Tag { Name = "Workflow" } }
            }
        };
    }

    private List<string> GetDummyLanguages()
    {
        return new List<string>
        {
            "C#", "JavaScript", "TypeScript", "Python", "Java", "PHP", "Go", "Rust",
            "Swift", "Kotlin", "CSS", "HTML", "SQL", "Bash", "PowerShell", "Docker"
        };
    }
}
