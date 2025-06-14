using CodeStash.Application.Models.Dtos;

namespace CodeStash.Application.Contracts;
public interface IUserService
{
    Task<Result<UserProfileDto>> GetUserProfileAsync();
    Task<Result<UserPublicProfileDto>> GetUserPublicProfileAsync(string userName);
    Task<Result<UserSettingsDto>> GetUserSettingsAsync();
    Task<Result> UpdateUserSettingsAsync(UserSettingsDto settings);
}