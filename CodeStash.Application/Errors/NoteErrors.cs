using CodeStash.Domain.Models;

namespace CodeStash.Application.Errors;
public static class NoteErrors
{
    public static Error NotFound =>
        new("NotFound", "The requested note was not found.");
    public static Error UnauthorizedAccess =>
        new("UnauthorizedAccess", "You do not have permission to access this note.");
    public static Error ForbiddenAccess =>
        new("ForbiddenAccess", "You do not have permission to perform this action on the note.");
    public static Error SlugAlreadyExists =>
        new("SlugAlreadyExists", "A note with this slug already exists. Please choose a different slug.");
    public static Error InvalidTags =>
        new("InvalidTags", "One or more tags associated with this note were not found.");
    public static Error CreateFailed =>
        new("CreateFailed", "Failed to create the note. Please try again.");
    public static Error UpdateFailed =>
        new("UpdateFailed", "Failed to update the note. Please try again.");
    public static Error DeleteFailed =>
        new("DeleteFailed", "Failed to delete the note. Please try again.");
}
