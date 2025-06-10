namespace CodeStash.Application.Utilities;

public static class ReservedUsernames
{
    private static readonly HashSet<string> _reservedUsernames = new(StringComparer.OrdinalIgnoreCase)
    {
        // System & Administration
        "admin",
        "administrator",
        "root",
        "system",
        "sysadmin",
        "superuser",
        "mod",
        "moderator",
        "owner",
        "master",
        "sudo",
            
        // Support & Help
        "support",
        "help",
        "contact",
        "service",
        "customer",
        "customerservice",
        "helpdesk",
        "assistance",
            
        // Technical & API
        "api",
        "www",
        "mail",
        "email",
        "ftp",
        "cdn",
        "static",
        "assets",
        "resources",
        "files",
        "uploads",
        "download",
        "downloads",
        "media",
        "images",
        "css",
        "js",
        "javascript",
        "login",
        "logout",
        "signin",
        "signout",
        "register",
        "signup",
        "auth",
        "oauth",
        "session",
        "cookie",
        "token",
            
        // Common Subdomains & Pages
        "blog",
        "docs",
        "documentation",
        "wiki",
        "forum",
        "forums",
        "news",
        "about",
        "aboutus",
        "privacy",
        "terms",
        "legal",
        "policy",
        "policies",
        "faq",
        "faqs",
        "home",
        "homepage",
        "index",
        "main",
        "dashboard",
        "profile",
        "profiles",
        "user",
        "users",
        "account",
        "accounts",
        "settings",
        "config",
        "configuration",
        "preferences",
            
        // Development & Testing
        "dev",
        "development",
        "staging",
        "test",
        "testing",
        "beta",
        "alpha",
        "demo",
        "sandbox",
        "localhost",
        "local",
        "debug",
        "trace",
            
        // Business & Marketing
        "sales",
        "marketing",
        "business",
        "enterprise",
        "corporate",
        "company",
        "team",
        "staff",
        "employee",
        "employees",
        "hr",
        "humanresources",
        "careers",
        "jobs",
        "recruitment",
        "billing",
        "invoice",
        "payment",
        "payments",
        "subscription",
        "subscriptions",
        "premium",
        "pro",
        "plus",
        "gold",
        "silver",
        "bronze",
        "vip",
        "elite",
            
        // Generic & Placeholder
        "null",
        "undefined",
        "nil",
        "none",
        "empty",
        "blank",
        "default",
        "placeholder",
        "example",
        "sample",
        "template",
        "guest",
        "anonymous",
        "unknown",
        "public",
        "private",
        "me",
        "you",
        "self",
        "all",
        "everyone",
        "everybody",
        "someone",
        "somebody",
        "nobody",
        "anybody",
            
        // Social & Community
        "follow",
        "following",
        "followers",
        "friend",
        "friends",
        "community",
        "group",
        "groups",
        "channel",
        "channels",
        "chat",
        "message",
        "messages",
        "notification",
        "notifications",
        "alert",
        "alerts",
            
        // Content & Media
        "post",
        "posts",
        "article",
        "articles",
        "content",
        "feed",
        "feeds",
        "timeline",
        "story",
        "stories",
        "comment",
        "comments",
        "review",
        "reviews",
        "rating",
        "ratings",
        "like",
        "likes",
        "share",
        "shares",
        "bookmark",
        "bookmarks",
        "favorite",
        "favorites",
        "favourite",
        "favourites",
            
        // Security & Safety
        "security",
        "safety",
        "abuse",
        "report",
        "reports",
        "spam",
        "scam",
        "phishing",
        "malware",
        "virus",
        "hack",
        "hacker",
        "exploit",
            
        // Common Names & Words
        "john",
        "jane",
        "admin123",
        "test123",
        "password",
        "user123",
        "guest123",
        "temp",
        "temporary",
        "delete",
        "remove",
        "edit",
        "update",
        "create",
        "new",
        "old",
        "archive",
        "backup",
        "restore",
            
        // App-Specific
        "codestash",
        "code_stash",
        "code-stash",
        "codestash1",
        "codestashapp",
        "cod3stash",
        "c0destash",
        "codestashteam",
        "code.stash.team",
        "codestashofficial",
        "officialcodestash",
        "codestashsupport",
            
        // Potentially Offensive (Basic examples - expand as needed)
        "hate",
        "offensive",
        "inappropriate"
        // Add more offensive terms as appropriate for your platform
    };

    /// <summary>
    /// Gets the complete list of reserved usernames
    /// </summary>
    public static IReadOnlyCollection<string> GetAll()
    {
        return _reservedUsernames.ToList().AsReadOnly();
    }

    /// <summary>
    /// Checks if a username is reserved (case-insensitive)
    /// </summary>
    /// <param name="username">The username to check</param>
    /// <returns>True if the username is reserved, false otherwise</returns>
    public static bool IsReserved(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return true;

        return _reservedUsernames.Contains(username.Trim());
    }

    /// <summary>
    /// Validates a username against reserved words and returns validation result
    /// </summary>
    /// <param name="username">The username to validate</param>
    /// <returns>Validation result with success status and error message</returns>
    public static (bool IsValid, string ErrorMessage) ValidateUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return (false, "Username cannot be empty");

        var trimmedUsername = username.Trim();

        if (IsReserved(trimmedUsername))
            return (false, $"The username '{trimmedUsername}' is reserved and cannot be used");

        return (true, string.Empty);
    }

    /// <summary>
    /// Gets count of reserved usernames (useful for database seeding)
    /// </summary>
    public static int Count => _reservedUsernames.Count;

    /// <summary>
    /// Adds additional reserved usernames (useful for app-specific additions)
    /// </summary>
    /// <param name="usernames">Additional usernames to reserve</param>
    public static void AddReserved(params string[] usernames)
    {
        foreach (var username in usernames)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                _reservedUsernames.Add(username.Trim());
            }
        }
    }

    /// <summary>
    /// Checks if username starts with any reserved prefix
    /// </summary>
    /// <param name="username">Username to check</param>
    /// <returns>True if starts with reserved prefix</returns>
    public static bool StartsWithReservedPrefix(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return false;

        var trimmedUsername = username.Trim();
        var reservedPrefixes = new[] { "admin", "mod", "system", "api", "support" };

        return reservedPrefixes.Any(prefix =>
            trimmedUsername.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }
}
