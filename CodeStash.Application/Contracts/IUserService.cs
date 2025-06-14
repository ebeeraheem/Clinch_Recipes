using CodeStash.Application.Models.Dtos;

namespace CodeStash.Application.Contracts;
public interface IUserService
{
    Task<Result<UserProfileDto>> GetUserProfileAsync();
    Task<Result<UserPublicProfileDto>> GetUserPublicProfileAsync(string userName);
}