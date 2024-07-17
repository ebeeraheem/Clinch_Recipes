using Clinch_Recipes.NoteEntity;
using Microsoft.EntityFrameworkCore;

namespace Clinch_Recipes.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
        base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>().HasData(
            new Note
            {
                Id = 1,
                Title = "Take Out the Trash",
                Content = "Remember to take out the trash before 8 PM.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 2,
                Title = "Shopping List",
                Content = "Buy milk, eggs, bread, and butter.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 3,
                Title = "Dinner with Friend",
                Content = "Dinner with Sarah at 7 PM.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 4,
                Title = "Doctor's Appointment",
                Content = "Doctor's appointment on Monday at 10 AM.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 5,
                Title = "Gym Workout",
                Content = "Leg day workout at the gym.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 6,
                Title = "Call Mom",
                Content = "Call mom to check in and say hi.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 7,
                Title = "Project Deadline",
                Content = "Finish the project report by Friday.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 8,
                Title = "Book Club Meeting",
                Content = "Book club meeting on Thursday at 6 PM.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 9,
                Title = "Grocery Shopping",
                Content = "Get fresh vegetables, fruits, and chicken.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            },
            new Note
            {
                Id = 10,
                Title = "Car Service",
                Content = "Car service appointment on Saturday at 9 AM.",
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            }
        );
    }
}
