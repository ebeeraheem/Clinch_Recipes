using CodeStash.Application.Contracts;
using CodeStash.Application.Models.QueryParams;
using CodeStash.Application.Models.RequestModels;
using CodeStash.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeStash.Controllers;

[Authorize]
#pragma warning disable S6934 // A Route attribute should be added to the controller when a route template is specified at the action level
public class NotesController(INoteService noteService, ITagService tagService) : Controller
#pragma warning restore S6934 // A Route attribute should be added to the controller when a route template is specified at the action level
{
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
        var myNotesData = await noteService.GetMyNotesAndStatsAsync(queryParams); // PERF: Consider fetching only necessary data for grid

        // Only return the notes grid, not the full page
        return PartialView("_MyNotesGrid", myNotesData.PagedResult.Items);
    }

    [AllowAnonymous]
    [Route("Note/{slug}")]
    public async Task<IActionResult> Details(string slug)
    {
        if (string.IsNullOrEmpty(slug))
        {
            return NotFound();
        }

        var result = await noteService.GetNoteBySlugAsync(slug);

        if (result.IsFailure)
        {
            return NotFound();
        }

        return View(result.Value);
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

    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }

        var result = await noteService.GetNoteByIdAsync(id);

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error.Message;
            return NotFound();
        }

        var note = result.Value;

        var viewModel = new EditNoteViewModel
        {
            Id = note.Id,
            Title = note.Title,
            Content = note.Content,
            Description = note.Description,
            IsPrivate = note.IsPrivate,
            TagsInput = string.Join(", ", note.Tags.Select(t => t.Name)),
            ViewCount = note.ViewCount,
            CreatedAt = note.CreatedAt,
            ModifiedAt = note.ModifiedAt,
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EditNoteViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var tags = viewModel.TagsInput
            ?.Split([','], StringSplitOptions.RemoveEmptyEntries)
            .Select(t => t.Trim())
            .ToList();

        var request = new UpdateNoteRequest()
        {
            Title = viewModel.Title,
            Description = viewModel.Description,
            Content = viewModel.Content,
            IsPrivate = viewModel.IsPrivate,
            Tags = tags ?? [],
        };

        var result = await noteService.UpdateNoteAsync(viewModel.Id, request);

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error.Message;
            return View(viewModel);
        }

        TempData["SuccessMessage"] = $"Note '{viewModel.Title}' updated successfully!";
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

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> LikeNote(string noteId)
    //{
    //    if (string.IsNullOrEmpty(noteId))
    //    {
    //        return Json(new { success = false, message = "Invalid note ID" });
    //    }

    //    // TODO: Implement actual like logic
    //    return Json(new { success = true, isLiked = true, likeCount = 42 });
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> BookmarkNote(string noteId)
    //{
    //    if (string.IsNullOrEmpty(noteId))
    //    {
    //        return Json(new { success = false, message = "Invalid note ID" });
    //    }

    //    // TODO: Implement actual bookmark logic
    //    return Json(new { success = true, isBookmarked = true });
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> AddComment(AddCommentViewModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return Json(new { success = false, message = "Invalid comment data" });
    //    }

    //    // TODO: Implement actual comment creation logic
    //    var newComment = new NoteComment
    //    {
    //        Id = Guid.NewGuid().ToString(),
    //        Content = model.Content,
    //        AuthorId = "ebeeraheem",
    //        Author = new ApplicationUser { UserName = "ebeeraheem", Id = "ebeeraheem" },
    //        NoteId = model.NoteId,
    //        ParentCommentId = model.ParentCommentId,
    //        CreatedAt = DateTime.UtcNow,
    //        LikesCount = 0,
    //        IsLikedByCurrentUser = false,
    //        Replies = new List<NoteComment>()
    //    };

    //    return Json(new
    //    {
    //        success = true,
    //        comment = new
    //        {
    //            id = newComment.Id,
    //            content = newComment.Content,
    //            authorUsername = newComment.Author.UserName,
    //            authorId = newComment.AuthorId,
    //            createdAt = newComment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
    //            isEdited = false,
    //            likesCount = 0,
    //            isLikedByCurrentUser = false,
    //            parentCommentId = newComment.ParentCommentId
    //        }
    //    });
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> LikeComment(string commentId)
    //{
    //    if (string.IsNullOrEmpty(commentId))
    //    {
    //        return Json(new { success = false, message = "Invalid comment ID" });
    //    }

    //    // TODO: Implement actual comment like logic
    //    return Json(new { success = true, isLiked = true, likeCount = 5 });
    //}
}
