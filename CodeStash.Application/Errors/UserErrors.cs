namespace CodeStash.Application.Errors;
public static class UserErrors
{
    public static Error NotFound =>
        new("NotFound", "The requested user was not found.");
    public static Error SettingsUpdateFailed =>
        new("SettingsUpdateFailed", "Failed to update user settings. Please try again later.");
}
