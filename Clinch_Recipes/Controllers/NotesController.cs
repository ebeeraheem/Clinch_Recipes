using CodeStash.Application.Contracts;
using CodeStash.Application.Models;
using CodeStash.Application.Models.QueryParams;
using CodeStash.Application.Models.RequestModels;
using CodeStash.Domain.Entities;
using CodeStash.ViewModels;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CodeStash.Controllers;
#pragma warning disable S6934 // A Route attribute should be added to the controller when a route template is specified at the action level
public class NotesController(INoteService noteService, ITagService tagService) : Controller
#pragma warning restore S6934 // A Route attribute should be added to the controller when a route template is specified at the action level
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

    public async Task<IActionResult> MyNotes([FromQuery] MyNotesQueryParams queryParams)
    {
        var myNotesData = await noteService.GetMyNotesAndStatsAsync(queryParams);

        var viewModel = new MyNotesViewModel
        {
            PagedResult = myNotesData.PagedResult,
            Stats = myNotesData.Extras,
            Filter = queryParams,
        };

        return View(viewModel);
    }

    public async Task<IActionResult> MyNotesGrid([FromQuery] MyNotesQueryParams queryParams)
    {
        var myNotesData = await noteService.GetMyNotesAndStatsAsync(queryParams);

        // Only return the notes grid, not the full page
        return PartialView("_MyNotesGrid", myNotesData.PagedResult.Items);
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

    [Route("Note/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var currentUserId = "ebeeraheem"; // Get from User.Identity in real implementation
        var viewModel = GetDummyNoteDetails(slug, currentUserId);

        if (viewModel == null)
        {
            return NotFound();
        }

        // Increment view count (in real implementation)
        // await _noteService.IncrementViewCountAsync(viewModel.Note.Id, currentUserId);

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeNote(string noteId)
    {
        if (string.IsNullOrEmpty(noteId))
        {
            return Json(new { success = false, message = "Invalid note ID" });
        }

        // TODO: Implement actual like logic
        return Json(new { success = true, isLiked = true, likeCount = 42 });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BookmarkNote(string noteId)
    {
        if (string.IsNullOrEmpty(noteId))
        {
            return Json(new { success = false, message = "Invalid note ID" });
        }

        // TODO: Implement actual bookmark logic
        return Json(new { success = true, isBookmarked = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(AddCommentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Invalid comment data" });
        }

        // TODO: Implement actual comment creation logic
        var newComment = new NoteComment
        {
            Id = Guid.NewGuid().ToString(),
            Content = model.Content,
            AuthorId = "ebeeraheem",
            Author = new ApplicationUser { UserName = "ebeeraheem", Id = "ebeeraheem" },
            NoteId = model.NoteId,
            ParentCommentId = model.ParentCommentId,
            CreatedAt = DateTime.UtcNow,
            LikesCount = 0,
            IsLikedByCurrentUser = false,
            Replies = new List<NoteComment>()
        };

        return Json(new
        {
            success = true,
            comment = new
            {
                id = newComment.Id,
                content = newComment.Content,
                authorUsername = newComment.Author.UserName,
                authorId = newComment.AuthorId,
                createdAt = newComment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                isEdited = false,
                likesCount = 0,
                isLikedByCurrentUser = false,
                parentCommentId = newComment.ParentCommentId
            }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LikeComment(string commentId)
    {
        if (string.IsNullOrEmpty(commentId))
        {
            return Json(new { success = false, message = "Invalid comment ID" });
        }

        // TODO: Implement actual comment like logic
        return Json(new { success = true, isLiked = true, likeCount = 5 });
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = new CreateNoteViewModel
        {
            PopularTags = await tagService.GetPopularTagsAsync(),
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateNoteViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.PopularTags = await tagService.GetPopularTagsAsync();
            return View(viewModel);
        }

        var tags = viewModel.TagsInput
            ?.Split([','], StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .ToList();

        var request = new CreateNoteRequest
        {
            Title = viewModel.Title,
            Content = viewModel.Content,
            Description = viewModel.Description,
            IsPrivate = viewModel.IsPrivate,
            Tags = tags ?? [],
        };

        var result = await noteService.CreateNoteAsync(request);

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error.Message;
            viewModel.PopularTags = await tagService.GetPopularTagsAsync();
            return View(viewModel);
        }

        TempData["SuccessMessage"] = $"Note '{viewModel.Title}' created successfully!";
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
            Content = note.Content,
            IsPrivate = note.IsPrivate,
            TagsInput = string.Join(", ", note.Tags.Select(t => t.Name)),
            ViewCount = note.ViewCount,
            CreatedAt = note.CreatedAt,
            ModifiedAt = note.ModifiedAt,
            PopularTags = GetDummySuggestedTags()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditNoteViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.PopularTags = GetDummySuggestedTags();
            return View(model);
        }

        // TODO: Implement actual note update logic
        TempData["SuccessMessage"] = $"Note '{model.Title}' updated successfully!";
        return RedirectToAction(nameof(MyNotes));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePrivacy([FromRoute(Name = "id")] string noteId)
    {
        if (string.IsNullOrEmpty(noteId))
        {
            return Json(new { success = false, message = "Invalid note ID" });
        }

        var result = await noteService.ToggleNotePrivacyAsync(noteId);

        if (result.IsFailure)
        {
            return Json(new { success = false, message = result.Error.Message });
        }

        return Json(new { success = true, message = "Note privacy updated" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute(Name = "id")] string noteId)
    {
        if (string.IsNullOrEmpty(noteId))
        {
            return Json(new { success = false, message = "Invalid note ID" });
        }

        var result = await noteService.DeleteNoteAsync(noteId);

        if (result.IsFailure)
        {
            return Json(new { success = false, message = result.Error.Message });
        }

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

    private NoteDetailsViewModel? GetDummyNoteDetails(string slug, string currentUserId)
    {
        // Simulate different notes based on slug
        if (slug != "javascript-array-methods-cheat-sheet")
        {
            return new NoteDetailsViewModel
            {
                Note = new Note
                {
                    Id = "note_1",
                    Title = "JavaScript Array Methods Cheat Sheet",
                    Slug = "javascript-array-methods-cheat-sheet",
                    Content = @"<h2>Essential JavaScript Array Methods</h2>

<p>Here's a comprehensive guide to the most useful JavaScript array methods that every developer should know.</p>

<h3>Transformation Methods</h3>

<pre><code class=""language-javascript"">// Map - Transform each element
const numbers = [1, 2, 3, 4, 5];
const doubled = numbers.map(n => n * 2);
console.log(doubled); // [2, 4, 6, 8, 10]

// Filter - Select elements based on condition
const evens = numbers.filter(n => n % 2 === 0);
console.log(evens); // [2, 4]

// Reduce - Accumulate values
const sum = numbers.reduce((acc, n) => acc + n, 0);
console.log(sum); // 15</code></pre>

<h3>Search Methods</h3>

<pre><code class=""language-javascript"">// Find - Get first matching element
const users = [
  { id: 1, name: 'John', active: true },
  { id: 2, name: 'Jane', active: false },
  { id: 3, name: 'Bob', active: true }
];

const activeUser = users.find(user => user.active);
console.log(activeUser); // { id: 1, name: 'John', active: true }

// FindIndex - Get index of first matching element
const activeIndex = users.findIndex(user => user.active);
console.log(activeIndex); // 0

// Includes - Check if element exists
const fruits = ['apple', 'banana', 'orange'];
console.log(fruits.includes('banana')); // true</code></pre>

<h3>Validation Methods</h3>

<pre><code class=""language-javascript"">// Some - Check if at least one element passes test
const hasEven = numbers.some(n => n % 2 === 0);
console.log(hasEven); // true

// Every - Check if all elements pass test
const allPositive = numbers.every(n => n > 0);
console.log(allPositive); // true</code></pre>

<h3>Modification Methods</h3>

<pre><code class=""language-javascript"">// Sort - Sort elements (mutates original array)
const unsorted = [3, 1, 4, 1, 5, 9];
const sorted = [...unsorted].sort((a, b) => a - b);
console.log(sorted); // [1, 1, 3, 4, 5, 9]

// Reverse - Reverse array order (mutates original)
const reversed = [...numbers].reverse();
console.log(reversed); // [5, 4, 3, 2, 1]</code></pre>

<h3>Utility Methods</h3>

<pre><code class=""language-javascript"">// Join - Convert array to string
const words = ['Hello', 'World', 'from', 'JavaScript'];
const sentence = words.join(' ');
console.log(sentence); // ""Hello World from JavaScript""

// Slice - Extract portion of array (non-mutating)
const middle = numbers.slice(1, 4);
console.log(middle); // [2, 3, 4]

// Concat - Combine arrays (non-mutating)
const moreNumbers = [6, 7, 8];
const combined = numbers.concat(moreNumbers);
console.log(combined); // [1, 2, 3, 4, 5, 6, 7, 8]</code></pre>

<blockquote>
<p><strong>Pro Tip:</strong> Always consider whether you need to mutate the original array or create a new one. Methods like <code>map</code>, <code>filter</code>, and <code>slice</code> return new arrays, while <code>push</code>, <code>pop</code>, and <code>sort</code> modify the original.</p>
</blockquote>",
                    IsPrivate = false,
                    ViewCount = 1847,
                    CreatedAt = DateTime.Parse("2025-05-15 14:30:00"),
                    ModifiedAt = DateTime.Parse("2025-06-05 10:20:00"),
                    AuthorId = "ebeeraheem",
                    //Language = "JavaScript",
                    Description = "A comprehensive reference guide for the most commonly used JavaScript array methods with practical examples.",
                    Tags = new List<Tag>
                {
                    new Tag { Name = "JavaScript" },
                    new Tag { Name = "Arrays" },
                    new Tag { Name = "Reference" },
                    new Tag { Name = "Beginner" }
                }
                },
                Author = new ApplicationUser
                {
                    Id = "ebeeraheem",
                    UserName = "ebeeraheem",
                    Email = "ebeeraheem@example.com"
                },
                IsOwner = currentUserId == "ebeeraheem",
                IsLiked = false,
                IsBookmarked = true,
                IsFollowingAuthor = false,
                Comments = GetDummyComments(),
                RelatedNotes = GetDummyRelatedNotes(),
                AuthorOtherNotes = GetDummyAuthorNotes(),
                NewComment = new AddCommentViewModel { NoteId = "note_1" },
                Stats = new NoteStatsViewModel
                {
                    Views = 1847,
                    Likes = 234,
                    Comments = 18,
                    Bookmarks = 89,
                    Shares = 45,
                    CreatedAt = DateTime.Parse("2025-05-15 14:30:00"),
                    ModifiedAt = DateTime.Parse("2025-06-05 10:20:00"),
                    ViewerCountries = new List<string> { "US", "UK", "Canada", "Germany", "India" },
                    ViewsPerDay = new Dictionary<string, int>
                {
                    { "2025-06-03", 45 },
                    { "2025-06-04", 67 },
                    { "2025-06-05", 89 },
                    { "2025-06-06", 123 },
                    { "2025-06-07", 156 },
                    { "2025-06-08", 134 },
                    { "2025-06-09", 23 }
                }
                }
            };
        }

        return null; // Note not found
    }

    private List<NoteComment> GetDummyComments()
    {
        return new List<NoteComment>
    {
        new NoteComment
        {
            Id = "comment_1",
            Content = "This is exactly what I was looking for! The examples are clear and concise. Thanks for sharing!",
            AuthorId = "johndoe",
            Author = new ApplicationUser { UserName = "johndoe", Id = "johndoe" },
            NoteId = "note_1",
            CreatedAt = DateTime.Parse("2025-06-07 15:45:00"),
            LikesCount = 12,
            IsLikedByCurrentUser = false,
            Replies = new List<NoteComment>
            {
                new NoteComment
                {
                    Id = "comment_2",
                    Content = "Glad it helped! I plan to add more advanced array methods in a follow-up post.",
                    AuthorId = "ebeeraheem",
                    Author = new ApplicationUser { UserName = "ebeeraheem", Id = "ebeeraheem" },
                    NoteId = "note_1",
                    ParentCommentId = "comment_1",
                    CreatedAt = DateTime.Parse("2025-06-07 16:20:00"),
                    LikesCount = 5,
                    IsLikedByCurrentUser = true,
                    Replies = new List<NoteComment>()
                }
            }
        },
        new NoteComment
        {
            Id = "comment_3",
            Content = "Could you add examples for flatMap() and reduceRight()? Those are often overlooked but very useful.",
            AuthorId = "sarahdev",
            Author = new ApplicationUser { UserName = "sarahdev", Id = "sarahdev" },
            NoteId = "note_1",
            CreatedAt = DateTime.Parse("2025-06-08 09:30:00"),
            LikesCount = 8,
            IsLikedByCurrentUser = false,
            Replies = new List<NoteComment>()
        },
        new NoteComment
        {
            Id = "comment_4",
            Content = "Great reference! I bookmarked this for my junior developers. The pro tip about mutating vs non-mutating methods is gold 🏆",
            AuthorId = "techleader",
            Author = new ApplicationUser { UserName = "techleader", Id = "techleader" },
            NoteId = "note_1",
            CreatedAt = DateTime.Parse("2025-06-08 14:15:00"),
            ModifiedAt = DateTime.Parse("2025-06-08 14:17:00"),
            LikesCount = 15,
            IsLikedByCurrentUser = true,
            Replies = new List<NoteComment>()
        }
    };
    }

    private List<Note> GetDummyRelatedNotes()
    {
        return new List<Note>
    {
        new Note
        {
            Id = "related_1",
            Title = "JavaScript Object Methods You Should Know",
            Slug = "javascript-object-methods-guide",
            ViewCount = 892,
            CreatedAt = DateTime.Parse("2025-05-20 11:00:00"),
            AuthorId = "johndoe",
            Author = new ApplicationUser { UserName = "johndoe" },
            Tags = new List<Tag> { new Tag { Name = "JavaScript" }, new Tag { Name = "Objects" } }
        },
        new Note
        {
            Id = "related_2",
            Title = "Modern JavaScript ES6+ Features",
            Slug = "modern-javascript-es6-features",
            ViewCount = 1234,
            CreatedAt = DateTime.Parse("2025-05-25 16:30:00"),
            AuthorId = "sarahdev",
            Author = new ApplicationUser { UserName = "sarahdev" },
            Tags = new List<Tag> { new Tag { Name = "JavaScript" }, new Tag { Name = "ES6" } }
        },
        new Note
        {
            Id = "related_3",
            Title = "Functional Programming in JavaScript",
            Slug = "functional-programming-javascript",
            ViewCount = 756,
            CreatedAt = DateTime.Parse("2025-06-01 13:20:00"),
            AuthorId = "funcpro",
            Author = new ApplicationUser { UserName = "funcpro" },
            Tags = new List<Tag> { new Tag { Name = "JavaScript" }, new Tag { Name = "Functional Programming" } }
        }
    };
    }

    private List<Note> GetDummyAuthorNotes()
    {
        return new List<Note>
    {
        new Note
        {
            Id = "author_1",
            Title = "React Custom Hooks Best Practices",
            Slug = "react-custom-hooks-best-practices",
            ViewCount = 2156,
            CreatedAt = DateTime.Parse("2025-04-10 09:15:00"),
            AuthorId = "ebeeraheem",
            Tags = new List<Tag> { new Tag { Name = "React" }, new Tag { Name = "Hooks" } }
        },
        new Note
        {
            Id = "author_2",
            Title = "CSS Grid Layout Mastery",
            Slug = "css-grid-layout-mastery",
            ViewCount = 1689,
            CreatedAt = DateTime.Parse("2025-03-28 14:45:00"),
            AuthorId = "ebeeraheem",
            Tags = new List<Tag> { new Tag { Name = "CSS" }, new Tag { Name = "Grid" } }
        },
        new Note
        {
            Id = "author_3",
            Title = "TypeScript Type Guards Explained",
            Slug = "typescript-type-guards-explained",
            ViewCount = 943,
            CreatedAt = DateTime.Parse("2025-05-02 11:30:00"),
            AuthorId = "ebeeraheem",
            Tags = new List<Tag> { new Tag { Name = "TypeScript" }, new Tag { Name = "Types" } }
        }
    };
    }
}
