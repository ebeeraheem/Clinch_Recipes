using CodeStash.Application.Models.Dtos;

namespace CodeStash.Application.Contracts;
public interface IUserService
{
    Task<Result> ChangePasswordAsync(ChangePasswordDto request);
    Task<Result<UserProfileDto>> GetUserProfileAsync();
    Task<Result<UserPublicProfileDto>> GetUserPublicProfileAsync(string userName);
    Task<Result<UserSettingsDto>> GetUserSettingsAsync();
    Task<Result> ForgotPasswordAsync(string email);
    Task<Result> ResetPasswordAsync(ResetPasswordDto request);
    Task<Result> UpdateUserSettingsAsync(UserSettingsDto settings);
}