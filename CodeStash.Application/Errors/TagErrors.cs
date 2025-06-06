using CodeStash.Domain.Models;

namespace CodeStash.Application.Errors;
public static class TagErrors
{
    public static Error NotFound =>
        new("NotFound", "The requested tag was not found.");

    public static Error InvalidName =>
        new("InvalidName", "The name of the tag is invalid. It must be between 1 and 100 characters long.");

    public static Error UnauthorizedAccess =>
        new("UnauthorizedAccess", "You do not have permission to access this tag.");

    public static Error CreateFailed =>
        new("CreateFailed", "Failed to create the tag. Please try again.");

    public static Error UpdateFailed =>
        new("UpdateFailed", "Failed to update the tag. Please try again.");

    public static Error DeleteFailed =>
        new("DeleteFailed", "Failed to delete the tag. Please try again.");
}
