namespace Clinch_Recipes.Models;

public class NoteViewModel
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTime CreatedDate { get; init; }
    public DateTime LastUpdatedDate { get; init; }
}