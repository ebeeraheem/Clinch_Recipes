using CodeStash.Application.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeStash.Controllers;

[AllowAnonymous]
[Route("Locations")]
public class LocationsController(ILocationService locationService) : Controller
{
    [HttpGet("Countries")]
    public async Task<IActionResult> Countries()
    {
        var countries = await locationService.GetAllCountriesAsync();
        return Json(countries);
    }

    [HttpGet("Countries/{countryCode}")]
    public async Task<IActionResult> Countries(string countryCode)
    {
        var result = await locationService.GetCountryByCodeAsync(countryCode);

        return result.IsSuccess
            ? Json(result.Value)
            : NotFound(result.Error.Message);
    }
}
