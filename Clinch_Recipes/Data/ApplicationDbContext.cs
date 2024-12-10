using Clinch_Recipes.NoteEntity;
using Clinch_Recipes.UserEntity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinch_Recipes.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    public required DbSet<Note> Notes { get; set; }
    public required DbSet<ApplicationUser> ApplicationUsers { get; set; }
}
