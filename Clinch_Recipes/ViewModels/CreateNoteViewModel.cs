using CodeStash.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class CreateNoteViewModel
{
    [Required(ErrorMessage = "Title is required")]
    [Length(3, 200, ErrorMessage = "Title must be between 3 and 200 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required")]
    [Length(10, 8000, ErrorMessage = "Content must be between 10 and 8000 characters")]
    [Display(Name = "Code Content")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Tags (comma-separated)")]
    public string? TagsInput { get; set; }

    [Display(Name = "Make this note private")]
    public bool IsPrivate { get; set; } = false;

    [Display(Name = "Description (optional)")]
    [Length(0, 500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string? Description { get; set; }

    public List<Tag> PopularTags { get; set; } = [];
}
