using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeStash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IncreaseCountryCodeLengthAndRemoveFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Countries_CountryCode",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CountryCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Countries",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "AspNetUsers",
                type: "nvarchar(2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CountryCode",
                table: "AspNetUsers",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Countries_CountryCode",
                table: "AspNetUsers",
                column: "CountryCode",
                principalTable: "Countries",
                principalColumn: "Code");
        }
    }
}
