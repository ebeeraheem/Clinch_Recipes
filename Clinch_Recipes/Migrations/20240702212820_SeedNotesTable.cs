using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinch_Recipes.Migrations
{
    /// <inheritdoc />
    public partial class SeedNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "CreatedDate", "LastUpdatedDate", "Title" },
                values: new object[,]
                {
                    { 1, "Remember to take out the trash before 8 PM.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(402), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(418), "Take Out the Trash" },
                    { 2, "Buy milk, eggs, bread, and butter.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(421), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(422), "Shopping List" },
                    { 3, "Dinner with Sarah at 7 PM.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(424), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(425), "Dinner with Friend" },
                    { 4, "Doctor's appointment on Monday at 10 AM.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(427), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(428), "Doctor's Appointment" },
                    { 5, "Leg day workout at the gym.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(430), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(431), "Gym Workout" },
                    { 6, "Call mom to check in and say hi.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(432), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(433), "Call Mom" },
                    { 7, "Finish the project report by Friday.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(435), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(436), "Project Deadline" },
                    { 8, "Book club meeting on Thursday at 6 PM.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(438), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(439), "Book Club Meeting" },
                    { 9, "Get fresh vegetables, fruits, and chicken.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(441), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(441), "Grocery Shopping" },
                    { 10, "Car service appointment on Saturday at 9 AM.", new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(443), new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(444), "Car Service" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
