﻿// <auto-generated />
using System;
using Clinch_Recipes.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Clinch_Recipes.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240702212820_SeedNotesTable")]
    partial class SeedNotesTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Clinch_Recipes.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastUpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Notes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Content = "Remember to take out the trash before 8 PM.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(402),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(418),
                            Title = "Take Out the Trash"
                        },
                        new
                        {
                            Id = 2,
                            Content = "Buy milk, eggs, bread, and butter.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(421),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(422),
                            Title = "Shopping List"
                        },
                        new
                        {
                            Id = 3,
                            Content = "Dinner with Sarah at 7 PM.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(424),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(425),
                            Title = "Dinner with Friend"
                        },
                        new
                        {
                            Id = 4,
                            Content = "Doctor's appointment on Monday at 10 AM.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(427),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(428),
                            Title = "Doctor's Appointment"
                        },
                        new
                        {
                            Id = 5,
                            Content = "Leg day workout at the gym.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(430),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(431),
                            Title = "Gym Workout"
                        },
                        new
                        {
                            Id = 6,
                            Content = "Call mom to check in and say hi.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(432),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(433),
                            Title = "Call Mom"
                        },
                        new
                        {
                            Id = 7,
                            Content = "Finish the project report by Friday.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(435),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(436),
                            Title = "Project Deadline"
                        },
                        new
                        {
                            Id = 8,
                            Content = "Book club meeting on Thursday at 6 PM.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(438),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(439),
                            Title = "Book Club Meeting"
                        },
                        new
                        {
                            Id = 9,
                            Content = "Get fresh vegetables, fruits, and chicken.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(441),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(441),
                            Title = "Grocery Shopping"
                        },
                        new
                        {
                            Id = 10,
                            Content = "Car service appointment on Saturday at 9 AM.",
                            CreatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(443),
                            LastUpdatedDate = new DateTime(2024, 7, 2, 22, 28, 18, 960, DateTimeKind.Local).AddTicks(444),
                            Title = "Car Service"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}