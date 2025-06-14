using CodeStash.Application.Models.Dtos;

namespace CodeStash.Application.Services;
public class UserService(ApplicationDbContext context, UserHelper userHelper, ILogger<UserService> logger) : IUserService
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

    public async Task<Result<UserProfileDto>> GetUserProfileAsync()
    {
        const int topCount = 3;
        var userId = userHelper.GetUserId();

        logger.LogInformation("Retrieving user profile for user ID: {UserId}", userId);

        var user = await context.ApplicationUsers
            .Include(u => u.Country)
            .Include(u => u.Notes)
            .ThenInclude(n => n.Tags)
            .Select(u => new UserProfileDto
            {
                Id = u.Id,
                UserName = u.UserName ?? string.Empty,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Bio = u.Bio,
                Location = u.Country == null ? null : u.Country.Name,
                WebsiteUrl = u.WebsiteUrl,
                GitHubUsername = u.GitHubUsername,
                TwitterHandle = u.TwitterHandle,
                LinkedInProfile = u.LinkedInProfile,
                ProfileImageUrl = u.ProfileImageUrl,
                JoinedAt = u.CreatedAt,
                RecentNotes = u.Notes
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(topCount)
                    .ToList(),
                PopularNotes = u.Notes
                    .OrderByDescending(n => n.ViewCount)
                    .Take(topCount)
                    .ToList(),
                TotalNotes = u.Notes.Count,
                AverageViewsPerNote = u.Notes.Any() ? u.Notes.Average(n => n.ViewCount) : 0,
                TotalViews = u.Notes.Sum(n => n.ViewCount),
                NotesThisWeek = u.Notes.Count(n => n.CreatedAt >= DateTime.UtcNow.AddDays(-7)),
                NotesThisMonth = u.Notes.Count(n => n.CreatedAt >= DateTime.UtcNow.AddMonths(-1)),
                NotesThisYear = u.Notes.Count(n => n.CreatedAt >= DateTime.UtcNow.AddYears(-1)),
                NotesPerMonth = new()
            })
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            logger.LogWarning("User profile not found for user ID: {UserId}", userId);
            return Result<UserProfileDto>.Failure(UserErrors.NotFound);
        }

        //// Calculate notes per month
        //user.NotesPerMonth = await context.Notes
        //    .Where(n => n.AuthorId == userId)
        //    .GroupBy(n => new { n.CreatedAt.Year, n.CreatedAt.Month })
        //    .Select(g => new { Month = $"{g.Key.Year}-{g.Key.Month:D2}", Count = g.Count() })
        //    .ToDictionaryAsync(g => g.Month, g => g.Count); //

        logger.LogInformation("User profile retrieved successfully for user ID: {UserId}", userId);
        return Result<UserProfileDto>.Success(user);
    }
}
