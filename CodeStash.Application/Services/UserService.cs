using CodeStash.Application.Models.Dtos;

namespace CodeStash.Application.Services;
public class UserService(ApplicationDbContext context, ILogger<UserService> logger) : IUserService
{
    public async Task<Result<UserPublicProfileDto>> GetUserPublicProfileAsync(string userName)
    {
        const int pageSize = 50;

        logger.LogInformation("Retrieving user public profile: {UserName}", userName);

        var user = await context.ApplicationUsers
            .Include(u => u.Country)
            .Include(u => u.Notes.Where(n => !n.IsPrivate))
            .ThenInclude(n => n.Tags)
            .Select(u => new UserPublicProfileDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.IsEmailPublic ? u.Email : null,
                Bio = u.Bio,
                Location = u.Country == null ? null : u.Country.Name,
                WebsiteUrl = u.WebsiteUrl,
                GitHubUsername = u.GitHubUsername,
                TwitterHandle = u.TwitterHandle,
                LinkedInProfile = u.LinkedInProfile,
                ProfileImageUrl = u.ProfileImageUrl,
                JoinedAt = u.CreatedAt,
                PublicNotesTotalCount = u.Notes.Count(n => !n.IsPrivate),
                PublicNotes = u.Notes
                    .Where(n => !n.IsPrivate)
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(pageSize)
                    .ToList(),
                IsEmailPublic = u.IsEmailPublic,
                IsLocationPublic = u.IsLocationPublic,
                IsSocialLinksPublic = u.IsSocialLinksPublic
            })
            .FirstOrDefaultAsync(u => u.UserName.Equals(userName));

        if (user is null)
        {
            logger.LogWarning("User not found: {UserName}", userName);
            return Result<UserPublicProfileDto>.Failure(UserErrors.NotFound);
        }

        logger.LogInformation("User public profile retrieved successfully: {UserName}", userName);
        return Result<UserPublicProfileDto>.Success(user);
    }
}
