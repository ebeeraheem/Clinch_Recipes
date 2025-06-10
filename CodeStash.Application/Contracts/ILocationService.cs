namespace CodeStash.Application.Contracts;

public interface ILocationService
{
    Task<List<Country>> GetAllCountriesAsync();
    Task<Result<Country>> GetCountryByCodeAsync(string countryCode);
}