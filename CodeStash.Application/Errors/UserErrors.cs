namespace CodeStash.Application.Errors;
public static class UserErrors
{
    public static Error NotFound =>
        new("NotFound", "The requested user was not found.");
}
