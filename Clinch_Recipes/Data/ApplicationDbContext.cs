using Microsoft.EntityFrameworkCore;

namespace Clinch_Recipes.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }
}
