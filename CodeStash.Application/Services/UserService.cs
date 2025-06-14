using CodeStash.Application.Models.Dtos;
using Microsoft.AspNetCore.Identity;

namespace CodeStash.Application.Services;
public class UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, UserHelper userHelper, ILogger<UserService> logger) : IUserService
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
        const int topCount = 3; // Number of recent/popular notes to retrieve
        var userId = userHelper.GetUserId();

        logger.LogInformation("Retrieving user profile for user ID: {UserId}", userId);

        var user = await context.ApplicationUsers
            .AsSplitQuery()
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
    public async Task<Result<UserSettingsDto>> GetUserSettingsAsync()
    {
        var userId = userHelper.GetUserId();
        logger.LogInformation("Retrieving user settings for user ID: {UserId}", userId);

        var user = await context.ApplicationUsers
            .Select(u => new UserSettingsDto
            {
                Id = u.Id,
                IsEmailPublic = u.IsEmailPublic,
                IsLocationPublic = u.IsLocationPublic,
                IsSocialLinksPublic = u.IsSocialLinksPublic,
            })
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            logger.LogWarning("User settings not found for user ID: {UserId}", userId);
            return Result<UserSettingsDto>.Failure(UserErrors.NotFound);
        }

        logger.LogInformation("User settings retrieved successfully for user ID: {UserId}", userId);
        return Result<UserSettingsDto>.Success(user);
    }
    public async Task<Result> UpdateUserSettingsAsync(UserSettingsDto settings)
    {
        var userId = userHelper.GetUserId();
        logger.LogInformation("Updating user settings for user ID: {UserId}", userId);

        var user = await context.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            logger.LogWarning("User not found for updating settings: {UserId}", userId);
            return Result.Failure(UserErrors.NotFound);
        }

        // Update settings
        user.IsEmailPublic = settings.IsEmailPublic;
        user.IsLocationPublic = settings.IsLocationPublic;
        user.IsSocialLinksPublic = settings.IsSocialLinksPublic;

        context.ApplicationUsers.Update(user);

        var result = await context.SaveChangesAsync();

        if (result <= 0)
        {
            logger.LogError("Failed to update user settings for user ID: {UserId}", userId);
            return Result.Failure(UserErrors.SettingsUpdateFailed);
        }

        logger.LogInformation("User settings updated successfully for user ID: {UserId}", userId);
        return Result.Success();
    }
    public async Task<Result> ChangePasswordAsync(ChangePasswordDto request)
    {
        var userId = userHelper.GetUserId();
        logger.LogInformation("Changing password for user ID: {UserId}", userId);

        var user = await context.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user is null)
        {
            logger.LogWarning("User not found for changing password: {UserId}", userId);
            return Result.Failure(UserErrors.NotFound);
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to change password for user ID: {UserId}. Errors: {@Errors}",
                userId, result.Errors);
            return Result.Failure(UserErrors.PasswordChangeFailed);
        }

        logger.LogInformation("Password changed successfully for user ID: {UserId}", userId);
        return Result.Success();
    }
}
