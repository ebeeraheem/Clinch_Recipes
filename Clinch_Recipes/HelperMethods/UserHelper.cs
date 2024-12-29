namespace Clinch_Recipes.HelperMethods;

public class UserHelper(IHttpContextAccessor httpContextAccessor)
{
    public bool IsAuthenticated()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext is null || httpContext.User?.Identity is null)
        {
            return false;
        }

        return httpContext.User.Identity.IsAuthenticated;
    }
}