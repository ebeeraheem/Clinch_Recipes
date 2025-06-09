using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeStash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLocationToCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "AspNetUsers",
                newName: "Country");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "AspNetUsers",
                newName: "Location");
        }
    }
}
