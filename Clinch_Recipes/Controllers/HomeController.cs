using CodeStash.Domain.Entities;
using CodeStash.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CodeStash.Controllers;
//public class HomeController(ILogger<HomeController> logger) : Controller
//{
//    private readonly ILogger<HomeController> _logger = logger;

//    public IActionResult Index()
//    {
//        return View();
//    }

//    public IActionResult Privacy()
//    {
//        return View();
//    }

//    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//    public IActionResult Error()
//    {
//        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//    }
//}

public class HomeController : Controller
{
    // You'll need to inject your services here
    // private readonly INoteService _noteService;

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            NewNotes = GetDummyNewNotes(),
            PopularNotes = GetDummyPopularNotes(),
            SearchFilter = new SearchFilterViewModel
            {
                AvailableLanguages = GetDummyLanguages(),
                AvailableTags = GetDummyTags()
            }
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes(string query = "", string language = "",
        string tag = "", string sortBy = "newest", int page = 1, int pageSize = 30)
    {
        // This will be your actual implementation
        var notes = GetDummyNotes(page, pageSize);

        var response = new NotesApiResponse
        {
            Notes = notes,
            HasMore = page < 10, // Simulate more pages for larger dataset
            Page = page
        };

        return Json(response);
    }

    // Updated dummy data methods (replace with actual service calls)
    private List<Note> GetDummyNewNotes()
    {
        return new List<Note>
        {
            new Note
            {
                Id = "1",
                Title = "React useEffect Hook Examples",
                Slug = "react-useeffect-examples",
                Content = "// React useEffect examples with cleanup and dependencies...", // Won't be displayed
                ViewCount = 245,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Author = new ApplicationUser { UserName = "johndoe" },
                Tags = new List<Tag> {
                    new Tag { Name = "React" },
                    new Tag { Name = "JavaScript" },
                    new Tag { Name = "Hooks" }
                }
            },
            new Note
            {
                Id = "2",
                Title = "Python List Comprehensions Cheat Sheet",
                Slug = "python-list-comprehensions",
                Content = "# Python list comprehension examples...", // Won't be displayed
                ViewCount = 189,
                CreatedAt = DateTime.UtcNow.AddHours(-12),
                Author = new ApplicationUser { UserName = "pythonista" },
                Tags = new List<Tag> {
                    new Tag { Name = "Python" },
                    new Tag { Name = "Lists" },
                    new Tag { Name = "Basics" }
                }
            },
            new Note
            {
                Id = "3",
                Title = "Advanced SQL JOIN Techniques",
                Slug = "advanced-sql-joins",
                Content = "-- Advanced SQL JOIN examples...", // Won't be displayed
                ViewCount = 156,
                CreatedAt = DateTime.UtcNow.AddHours(-8),
                Author = new ApplicationUser { UserName = "sqlmaster" },
                Tags = new List<Tag> {
                    new Tag { Name = "SQL" },
                    new Tag { Name = "Database" },
                    new Tag { Name = "Advanced" }
                }
            },
            new Note
            {
                Id = "4",
                Title = "CSS Grid Layout Complete Guide",
                Slug = "css-grid-complete-guide",
                Content = "/* CSS Grid examples and techniques... */", // Won't be displayed
                ViewCount = 298,
                CreatedAt = DateTime.UtcNow.AddHours(-6),
                Author = new ApplicationUser { UserName = "cssmaster" },
                Tags = new List<Tag> {
                    new Tag { Name = "CSS" },
                    new Tag { Name = "Grid" },
                    new Tag { Name = "Layout" }
                }
            }
        };
    }

    private List<Note> GetDummyPopularNotes()
    {
        return new List<Note>
        {
            new Note
            {
                Id = "5",
                Title = "JavaScript Array Methods Masterclass",
                Slug = "js-array-methods-masterclass",
                Content = "// JavaScript array methods...", // Won't be displayed
                ViewCount = 1543,
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                Author = new ApplicationUser { UserName = "jsdev" },
                Tags = new List<Tag> {
                    new Tag { Name = "JavaScript" },
                    new Tag { Name = "Arrays" },
                    new Tag { Name = "ES6" }
                }
            },
            new Note
            {
                Id = "6",
                Title = "Docker Compose Best Practices",
                Slug = "docker-compose-best-practices",
                Content = "# Docker compose configurations...", // Won't be displayed
                ViewCount = 892,
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                Author = new ApplicationUser { UserName = "devops_guru" },
                Tags = new List<Tag> {
                    new Tag { Name = "Docker" },
                    new Tag { Name = "DevOps" },
                    new Tag { Name = "Containers" }
                }
            },
            new Note
            {
                Id = "7",
                Title = "Git Commands Every Developer Should Know",
                Slug = "essential-git-commands",
                Content = "# Essential git commands...", // Won't be displayed
                ViewCount = 2156,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                Author = new ApplicationUser { UserName = "gitmaster" },
                Tags = new List<Tag> {
                    new Tag { Name = "Git" },
                    new Tag { Name = "Version Control" },
                    new Tag { Name = "CLI" }
                }
            },
            new Note
            {
                Id = "8",
                Title = "Node.js Express Middleware Patterns",
                Slug = "express-middleware-patterns",
                Content = "// Express middleware examples...", // Won't be displayed
                ViewCount = 734,
                CreatedAt = DateTime.UtcNow.AddDays(-2),
                Author = new ApplicationUser { UserName = "nodejs_dev" },
                Tags = new List<Tag> {
                    new Tag { Name = "Node.js" },
                    new Tag { Name = "Express" },
                    new Tag { Name = "Middleware" }
                }
            }
        };
    }

    private List<Note> GetDummyNotes(int page, int pageSize)
    {
        var languages = new[] { "JavaScript", "Python", "C#", "Java", "CSS", "SQL", "TypeScript", "PHP", "Go", "Rust" };
        var topics = new[] { "Authentication", "API Design", "Database", "Frontend", "Backend", "DevOps", "Testing", "Security", "Performance", "Architecture" };
        var authors = new[] { "codeMaster", "devGuru", "techLead", "fullStackDev", "apiExpert", "dbAdmin", "frontendPro", "backendNinja", "devOpsWiz", "securityPro" };

        var allNotes = new List<Note>();
        var random = new Random();

        for (int i = 1; i <= pageSize; i++)
        {
            var noteIndex = (page - 1) * pageSize + i;
            var language = languages[random.Next(languages.Length)];
            var topic = topics[random.Next(topics.Length)];
            var author = authors[random.Next(authors.Length)];

            allNotes.Add(new Note
            {
                Id = $"note_{noteIndex}",
                Title = $"{language} {topic} - Best Practices #{noteIndex}",
                Slug = $"{language.ToLower()}-{topic.ToLower()}-{noteIndex}",
                Content = $"// {language} code snippet for {topic}...", // Won't be displayed
                ViewCount = random.Next(5, 2000),
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 90)),
                Author = new ApplicationUser { UserName = author },
                Tags = new List<Tag> {
                    new Tag { Name = language },
                    new Tag { Name = topic },
                    new Tag { Name = random.Next(2) == 0 ? "Beginner" : "Advanced" }
                }
            });
        }
        return allNotes;
    }

    private List<string> GetDummyLanguages()
    {
        return new List<string> { "JavaScript", "Python", "C#", "Java", "CSS", "SQL", "TypeScript", "PHP", "Go", "Rust", "Swift", "Kotlin" };
    }

    private List<Tag> GetDummyTags()
    {
        return new List<Tag>
        {
            new Tag { Name = "React" },
            new Tag { Name = "Vue" },
            new Tag { Name = "Angular" },
            new Tag { Name = "Node.js" },
            new Tag { Name = "Database" },
            new Tag { Name = "API" },
            new Tag { Name = "CSS" },
            new Tag { Name = "HTML" },
            new Tag { Name = "DevOps" },
            new Tag { Name = "Testing" },
            new Tag { Name = "Security" },
            new Tag { Name = "Performance" }
        };
    }
}