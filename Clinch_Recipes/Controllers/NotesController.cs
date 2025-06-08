using CodeStash.Application.Models;
using CodeStash.Domain.Entities;
using CodeStash.ViewModels;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CodeStash.Controllers;
public class NotesController : Controller
{
    //private const int PageSize = 40;

    //public async Task<IActionResult> Index()
    //{
    //    const int pageNumber = 1;

    //    var pagedResult = await GetCachedOrFreshNotesAsync(pageNumber);

    //    if (pagedResult is null)
    //    {
    //        return View(new PagedResult<Note>()
    //        {
    //            CurrentPage = pageNumber,
    //            PageSize = PageSize,
    //            Items = [],
    //            TotalCount = 0
    //        });
    //    }

    //    return View(pagedResult);
    //}

    [HttpGet]
    public async Task<IActionResult> MyNotes(string search = "", string language = "",
        string tag = "", string sort = "newest", bool privateOnly = false, int page = 1)
    {
        var currentUserId = "ebeeraheem"; // Get from User.Identity in real implementation

        var viewModel = new MyNotesViewModel
        {
            Notes = GetDummyUserNotes(currentUserId, search, language, tag, sort, privateOnly, page),
            Filter = new NotesFilterViewModel
            {
                SearchQuery = search,
                Language = language,
                Tag = tag,
                SortBy = sort,
                IsPrivateFilter = privateOnly,
                AvailableLanguages = GetDummyLanguages(),
                AvailableTags = GetDummyUserTags(currentUserId)
            },
            Pagination = new PaginationViewModel
            {
                CurrentPage = page,
                TotalPages = 8, // Simulate pagination
                PageSize = 12,
                TotalItems = 94 // Simulate total
            },
            Stats = GetDummyUserStats(currentUserId)
        };

        return View(viewModel);
    }

    //private async Task<PagedResult<Note>?> GetCachedOrFreshNotesAsync(int pageNumber)
    //{
    //    var cacheKey = $"notes_page_{pageNumber}";

    //    // Check if the cache contains the notes
    //    if (memoryCache.TryGetValue(cacheKey, out PagedResult<Note>? cachedResult))
    //    {
    //        return cachedResult;
    //    }

    //    // Get the notes from the database
    //    var query = noteRepository.GetAllNotesAsync();

    //    var ordered = query.OrderByDescending(n => n.CreatedDate);

    //    var pagedResult = await pagedResultService
    //        .GetPagedResultAsync(ordered, pageNumber, PageSize);

    //    // Cache the notes if they exist
    //    if (pagedResult.Items.Count != 0)
    //    {
    //        var cacheOptions = new MemoryCacheEntryOptions()
    //            .SetPriority(CacheItemPriority.NeverRemove)
    //            .SetSize(2048);

    //        memoryCache.Set(cacheKey, pagedResult, cacheOptions);
    //    }

    //    return pagedResult;
    //}

    //public async Task<IActionResult> GetNotes(int pageNumber = 2)
    //{
    //    var pagedResult = await GetCachedOrFreshNotesAsync(pageNumber);

    //    if (pagedResult is null)
    //    {
    //        return Json(new { success = false });
    //    }

    //    return Json(pagedResult);
    //}

    //public async Task<IActionResult> Search(string term)
    //{
    //    if (string.IsNullOrWhiteSpace(term))
    //    {
    //        return Json(Array.Empty<Note>());
    //    }

    //    var notes = await noteRepository.GetAllNotesAsync()
    //        .Where(n => n.Title.Contains(term))
    //        .ToListAsync();

    //    return Json(notes);
    //}

    //[Authorize]
    //public async Task<IActionResult> Upsert(Guid? id)
    //{
    //    var note = new Note();

    //    if (id is null) return View(note);

    //    // Get the note with the specified id
    //    note = await noteRepository.GetNoteByIdAsync((Guid)id);

    //    return note is null ? NotFound() : View(note);
    //}

    //[Authorize]
    //[HttpPost]
    //public async Task<IActionResult> Upsert(Note note)
    //{
    //    if (!ModelState.IsValid) return View(note);

    //    note.LastUpdatedDate = DateTime.UtcNow;
    //    note.Content = Markdown.ToHtml(note.Content);

    //    // Remove cache for updated note
    //    var noteCacheKey = $"note_{note.Id}";
    //    memoryCache.Remove(noteCacheKey);

    //    // Invalidate cache for all notes
    //    if (memoryCache is MemoryCache concreteCache)
    //        concreteCache.Clear();

    //    if (note.Id == Guid.Empty)
    //    {
    //        note.CreatedDate = DateTime.UtcNow;
    //        await noteRepository.AddNoteAsync(note);

    //        return RedirectToAction(nameof(Index));
    //    }

    //    await noteRepository.UpdateNoteAsync(note);

    //    return RedirectToAction(nameof(Details), new { id = note.Id });
    //}

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateNoteViewModel
        {
            AvailableLanguages = GetDummyLanguages(),
            SuggestedTags = GetDummySuggestedTags()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableLanguages = GetDummyLanguages();
            model.SuggestedTags = GetDummySuggestedTags();
            return View(model);
        }

        // Generate slug if not provided
        if (string.IsNullOrWhiteSpace(model.Slug))
        {
            model.Slug = GenerateSlug(model.Title);
        }

        // TODO: Implement actual note creation logic
        // For now, simulate creation
        TempData["SuccessMessage"] = $"Note '{model.Title}' created successfully!";
        return RedirectToAction(nameof(MyNotes));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        // TODO: Get actual note from database
        var note = GetDummyNoteById(id);
        if (note == null)
        {
            return NotFound();
        }

        var viewModel = new EditNoteViewModel
        {
            Id = note.Id,
            Title = note.Title,
            Slug = note.Slug,
            Content = note.Content,
            IsPrivate = note.IsPrivate,
            TagsInput = string.Join(", ", note.Tags.Select(t => t.Name)),
            ViewCount = note.ViewCount,
            CreatedAt = note.CreatedAt,
            ModifiedAt = note.ModifiedAt,
            AvailableLanguages = GetDummyLanguages(),
            SuggestedTags = GetDummySuggestedTags()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableLanguages = GetDummyLanguages();
            model.SuggestedTags = GetDummySuggestedTags();
            return View(model);
        }

        // TODO: Implement actual note update logic
        TempData["SuccessMessage"] = $"Note '{model.Title}' updated successfully!";
        return RedirectToAction(nameof(MyNotes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePrivacy(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Json(new { success = false, message = "Invalid note ID" });
        }

        // TODO: Implement actual privacy toggle logic
        return Json(new { success = true, message = "Note privacy updated" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return Json(new { success = false, message = "Invalid note ID" });
        }

        // TODO: Implement actual note deletion logic
        return Json(new { success = true, message = "Note deleted successfully" });
    }

    //[Authorize]
    //[HttpPost]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    var result = await noteRepository.DeleteNoteAsync(id);

    //    if (result <= 0)
    //    {
    //        return Json(new { success = false });
    //    }

    //    if (memoryCache is MemoryCache concreteCache)
    //    {
    //        concreteCache.Clear();
    //    }

    //    return Json(new { success = true });
    //}

    //public IActionResult Details(Guid id)
    //{
    //    return View();
    //}

    //public async Task<IActionResult> GetNoteFromServer(Guid id)
    //{
    //    var cacheKey = $"note_{id}";

    //    if (!memoryCache.TryGetValue(cacheKey, out Note? note))
    //    {
    //        note = await noteRepository.GetNoteByIdAsync(id);

    //        var cacheOptions = new MemoryCacheEntryOptions()
    //            .SetSlidingExpiration(TimeSpan.FromMinutes(30))
    //            .SetPriority(CacheItemPriority.Normal);

    //        memoryCache.Set(cacheKey, note, cacheOptions);
    //    }

    //    if (note is null)
    //    {
    //        return NotFound();
    //    }

    //    return View(nameof(Details), note);
    //}

    // Dummy data methods (replace with actual service calls)
    private List<Note> GetDummyUserNotes(string userId, string search, string language,
        string tag, string sort, bool privateOnly, int page)
    {
        var languages = new[] { "JavaScript", "Python", "C#", "Java", "CSS", "SQL", "TypeScript", "PHP" };
        var topics = new[] { "Authentication", "API", "Database", "Frontend", "Backend", "Testing", "Utils", "Helpers" };
        var random = new Random();

        var notes = new List<Note>();
        var startIndex = (page - 1) * 12;

        for (var i = 1; i <= 12; i++)
        {
            var noteIndex = startIndex + i;
            var lang = languages[random.Next(languages.Length)];
            var topic = topics[random.Next(topics.Length)];
            var isPrivate = random.Next(4) == 0; // 25% chance of being private

            notes.Add(new Note
            {
                Id = $"note_{noteIndex}",
                Title = $"{lang} {topic} - Solution #{noteIndex}",
                Slug = $"{lang.ToLower()}-{topic.ToLower()}-{noteIndex}",
                Content = GetSampleCode(lang, topic),
                IsPrivate = isPrivate,
                ViewCount = random.Next(0, 500),
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 365)),
                ModifiedAt = random.Next(2) == 0 ? DateTime.UtcNow.AddDays(-random.Next(1, 30)) : null,
                AuthorId = userId,
                Author = new ApplicationUser { UserName = "ebeeraheem" },
                Tags = new List<Tag>
                {
                    new Tag { Name = lang },
                    new Tag { Name = topic },
                    new Tag { Name = random.Next(2) == 0 ? "Beginner" : "Advanced" }
                }
            });
        }

        return notes;
    }

    private NotesStatsViewModel GetDummyUserStats(string userId)
    {
        return new NotesStatsViewModel
        {
            TotalNotes = 94,
            PublicNotes = 71,
            PrivateNotes = 23,
            TotalViews = 12847,
            NotesThisMonth = 8
        };
    }

    private Note? GetDummyNoteById(string id)
    {
        return new Note
        {
            Id = id,
            Title = "JavaScript Array Methods Cheat Sheet",
            Slug = "javascript-array-methods-cheat-sheet",
            Content = @"// Essential JavaScript Array Methods

// Map - Transform each element
const numbers = [1, 2, 3, 4, 5];
const doubled = numbers.map(n => n * 2);
console.log(doubled); // [2, 4, 6, 8, 10]

// Filter - Select elements based on condition
const evens = numbers.filter(n => n % 2 === 0);
console.log(evens); // [2, 4]

// Reduce - Accumulate values
const sum = numbers.reduce((acc, n) => acc + n, 0);
console.log(sum); // 15

// Find - Get first matching element
const found = numbers.find(n => n > 3);
console.log(found); // 4

// Some & Every - Boolean checks
const hasEven = numbers.some(n => n % 2 === 0);
const allPositive = numbers.every(n => n > 0);",
            IsPrivate = false,
            ViewCount = 245,
            CreatedAt = DateTime.UtcNow.AddDays(-15),
            ModifiedAt = DateTime.UtcNow.AddDays(-3),
            AuthorId = "ebeeraheem",
            Author = new ApplicationUser { UserName = "ebeeraheem" },
            Tags = new List<Tag>
            {
                new Tag { Name = "JavaScript" },
                new Tag { Name = "Arrays" },
                new Tag { Name = "Cheat Sheet" }
            }
        };
    }

    private string GetSampleCode(string language, string topic)
    {
        return language switch
        {
            "JavaScript" => $"// {language} {topic} implementation\nconst {topic.ToLower()} = () => {{\n  console.log('Implementing {topic}');\n}};",
            "Python" => $"# {language} {topic} implementation\ndef {topic.ToLower()}():\n    print('Implementing {topic}')",
            "C#" => $"// {language} {topic} implementation\npublic class {topic}\n{{\n    public void Execute()\n    {{\n        Console.WriteLine(\"Implementing {topic}\");\n    }}\n}}",
            _ => $"// {language} {topic} implementation\n// Your code here..."
        };
    }

    private List<string> GetDummyLanguages()
    {
        return new List<string> { "JavaScript", "Python", "C#", "Java", "CSS", "SQL", "TypeScript", "PHP", "Go", "Rust" };
    }

    private List<Tag> GetDummyUserTags(string userId)
    {
        return new List<Tag>
        {
            new Tag { Name = "JavaScript" },
            new Tag { Name = "Python" },
            new Tag { Name = "C#" },
            new Tag { Name = "React" },
            new Tag { Name = "API" },
            new Tag { Name = "Database" },
            new Tag { Name = "Utils" },
            new Tag { Name = "Authentication" }
        };
    }

    private List<Tag> GetDummySuggestedTags()
    {
        return new List<Tag>
        {
            new Tag { Name = "JavaScript" },
            new Tag { Name = "Python" },
            new Tag { Name = "React" },
            new Tag { Name = "Vue" },
            new Tag { Name = "API" },
            new Tag { Name = "Database" },
            new Tag { Name = "CSS" },
            new Tag { Name = "HTML" },
            new Tag { Name = "Node.js" },
            new Tag { Name = "Express" }
        };
    }

    private string GenerateSlug(string title)
    {
        return title.ToLower()
            .Replace(" ", "-")
            .Replace("'", "")
            .Replace("\"", "")
            .Replace(".", "")
            .Replace(",", "")
            .Replace("!", "")
            .Replace("?", "");
    }
}
