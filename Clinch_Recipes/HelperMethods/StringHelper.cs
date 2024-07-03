namespace Clinch_Recipes.HelperMethods;

public static class StringHelper
{
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;

        return value.Length <= maxLength ?
            value :
            string.Concat(value.AsSpan(0, maxLength), "...");
    }
}
