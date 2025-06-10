namespace CodeStash.Application.Errors;
public static class LocationErrors
{
    public static Error CountryNotFound(string countryCode) =>
        new("CountryNotFound", $"Country with code '{countryCode}' not found.");
}
