using System.ComponentModel.DataAnnotations;

namespace Clinch_Recipes.NoteEntity;

public class Note
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdatedDate { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
