namespace CodeStash.Application.Services;
public class LocationService(ApplicationDbContext context, ILogger<LocationService> logger) : ILocationService
{
    public async Task<List<Country>> GetAllCountriesAsync()
    {
        return await context.Countries.ToListAsync();
    }

    public async Task<Result<Country>> GetCountryByCodeAsync(string countryCode)
    {
        logger.LogInformation("Fetching country with code: {CountryCode}", countryCode);

        var country = await context.Countries
            .FirstOrDefaultAsync(c => c.Code.Equals(countryCode));

        if (country is null)
        {
            logger.LogWarning("Country with code {CountryCode} not found", countryCode);
            return Result<Country>.Failure(LocationErrors.CountryNotFound(countryCode));
        }

        logger.LogInformation("Country found: {CountryName}", country.Name);
        return Result<Country>.Success(country);
    }
}
