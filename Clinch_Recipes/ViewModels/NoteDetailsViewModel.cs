using CodeStash.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CodeStash.ViewModels;

public class NoteDetailsViewModel
{
    public Note Note { get; set; } = new();
    public ApplicationUser Author { get; set; } = new();
    public bool IsOwner { get; set; }
    public bool IsLiked { get; set; }
    public bool IsBookmarked { get; set; }
    public bool IsFollowingAuthor { get; set; }
    public List<NoteComment> Comments { get; set; } = [];
    public List<Note> RelatedNotes { get; set; } = [];
    public List<Note> AuthorOtherNotes { get; set; } = [];
    public AddCommentViewModel NewComment { get; set; } = new();
    public NoteStatsViewModel Stats { get; set; } = new();
}

public class NoteStatsViewModel
{
    public int Views { get; set; }
    public int Likes { get; set; }
    public int Comments { get; set; }
    public int Bookmarks { get; set; }
    public int Shares { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public List<string> ViewerCountries { get; set; } = [];
    public Dictionary<string, int> ViewsPerDay { get; set; } = new();
}

public class AddCommentViewModel
{
    [Required(ErrorMessage = "Comment content is required")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Comment must be between 1 and 1000 characters")]
    public string Content { get; set; } = string.Empty;

    public string NoteId { get; set; } = string.Empty;
    public string? ParentCommentId { get; set; }
}

public class NoteComment
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public ApplicationUser Author { get; set; } = new();
    public string NoteId { get; set; } = string.Empty;
    public string? ParentCommentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsEdited => ModifiedAt.HasValue;
    public List<NoteComment> Replies { get; set; } = [];
    public int LikesCount { get; set; }
    public bool IsLikedByCurrentUser { get; set; }
}
