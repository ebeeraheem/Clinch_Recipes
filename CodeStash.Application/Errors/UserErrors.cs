namespace CodeStash.Application.Errors;
public static class UserErrors
{
    public static Error NotFound =>
        new("NotFound", "The requested user was not found.");
    public static Error SettingsUpdateFailed =>
        new("SettingsUpdateFailed", "Failed to update user settings. Please try again later.");
    public static Error PasswordChangeFailed =>
        new("PasswordChangeFailed", "Failed to change password. Please ensure your current password is correct and try again.");
    public static Error EmailChangeFailed =>
        new("EmailChangeFailed", "Failed to change email. Please ensure your current password is correct and try again.");
    public static Error EmailAlreadyInUse =>
        new("EmailAlreadyInUse", "The email address is already in use. Please use a different email address.");
    public static Error InvalidCredentials =>
        new("InvalidCredentials", "The provided credentials are invalid. Please check your details and try again.");
}
