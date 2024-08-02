using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImageID);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvatarImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorID);
                    table.ForeignKey(
                        name: "FK_Authors_Images_AvatarImageID",
                        column: x => x.AvatarImageID,
                        principalTable: "Images",
                        principalColumn: "ImageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicationDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookID);
                    table.ForeignKey(
                        name: "FK_Books_Images_CoverImageID",
                        column: x => x.CoverImageID,
                        principalTable: "Images",
                        principalColumn: "ImageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookAuthor",
                columns: table => new
                {
                    BookID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookAuthor", x => new { x.BookID, x.AuthorID });
                    table.ForeignKey(
                        name: "FK_BookAuthor_Authors_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Authors",
                        principalColumn: "AuthorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookAuthor_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookFiles",
                columns: table => new
                {
                    BookFileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BookID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookFiles", x => x.BookFileID);
                    table.ForeignKey(
                        name: "FK_BookFiles_Books_BookID",
                        column: x => x.BookID,
                        principalTable: "Books",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Images",
                columns: new[] { "ImageID", "ImagePath" },
                values: new object[,]
                {
                    { new Guid("26a64d72-91e0-44fa-8e1a-7f55570d1168"), "images/jane_smith_avatar.jpg" },
                    { new Guid("6a7c7d5f-4c8e-4a4d-878b-2d2b7e8f5b7e"), "images/introduction_to_algorithms_cover.jpg" },
                    { new Guid("6b35adff-bdb4-4b69-b935-08d5dfd2d64c"), "images/john_doe_avatar.jpg" },
                    { new Guid("7b8c7d5e-5c6d-4c4c-878a-3f3d7e6e4c3d"), "images/machine_learning_basics_cover.jpg" },
                    { new Guid("7f3c63f2-7cc7-4c7b-85b7-44b1c70f6262"), "images/emily_jones_avatar.jpg" },
                    { new Guid("8a7b7d5e-6c5d-4c4d-878b-3e3b7e5e4b6d"), "images/data_science_for_beginners_cover.jpg" },
                    { new Guid("8ab07e4c-5af6-4f59-8c3f-233b23c1ff13"), "images/david_wilson_avatar.jpg" },
                    { new Guid("b6bb833a-3eec-4df1-9784-fd9d621d5f5c"), "images/learning_csharp_cover.jpg" },
                    { new Guid("bd44fabc-e840-4e3b-81da-2a3cb3b6e4d8"), "images/sarah_johnson_avatar.jpg" },
                    { new Guid("c9e4c891-909e-43a2-9a75-8973b907b4d7"), "images/linda_taylor_avatar.jpg" },
                    { new Guid("d16d704b-9a29-4a0b-9c70-bcd9b2c26f37"), "images/michael_brown_avatar.jpg" },
                    { new Guid("fb1e6b58-4b34-4c4c-878b-0f4b83f3d8b7"), "images/advanced_aspnet_core_cover.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorID", "AvatarImageID", "DateOfBirth", "FirstName", "LastName" },
                values: new object[,]
                {
                    { new Guid("2b19d6db-81a3-4042-9132-9b67ea7a9bbd"), new Guid("d16d704b-9a29-4a0b-9c70-bcd9b2c26f37"), new DateTime(1975, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Michael", "Brown" },
                    { new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new Guid("6b35adff-bdb4-4b69-b935-08d5dfd2d64c"), new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John", "Doe" },
                    { new Guid("9b23c7c4-8b16-4b1e-9a22-2df6e79dbb09"), new Guid("26a64d72-91e0-44fa-8e1a-7f55570d1168"), new DateTime(1990, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jane", "Smith" },
                    { new Guid("ad2c7bc9-3f9f-40b5-a84f-60b1d843b1e5"), new Guid("7f3c63f2-7cc7-4c7b-85b7-44b1c70f6262"), new DateTime(1985, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emily", "Jones" },
                    { new Guid("b13c1f29-1df1-4d95-9829-5b7a0e7e44ed"), new Guid("c9e4c891-909e-43a2-9a75-8973b907b4d7"), new DateTime(1995, 7, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Linda", "Taylor" },
                    { new Guid("d2fa31d7-0e4b-4712-9a5c-9d45a732b3b8"), new Guid("bd44fabc-e840-4e3b-81da-2a3cb3b6e4d8"), new DateTime(1988, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sarah", "Johnson" },
                    { new Guid("d3cb3f0e-689d-4c4d-86a0-3fba53a1dbd4"), new Guid("8ab07e4c-5af6-4f59-8c3f-233b23c1ff13"), new DateTime(1983, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "David", "Wilson" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookID", "CoverImageID", "Description", "Genre", "PublicationDate", "Title" },
                values: new object[,]
                {
                    { new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5"), new Guid("b6bb833a-3eec-4df1-9784-fd9d621d5f5c"), "A comprehensive guide to C# programming.", "Programming", "2020-05-15", "Learning C#" },
                    { new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c"), new Guid("7b8c7d5e-5c6d-4c4c-878a-3f3d7e6e4c3d"), "Understanding the fundamentals of machine learning.", "Artificial Intelligence", "2022-01-15", "Machine Learning Basics" },
                    { new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c"), new Guid("fb1e6b58-4b34-4c4c-878b-0f4b83f3d8b7"), "Deep dive into ASP.NET Core framework.", "Programming", "2021-08-20", "Advanced ASP.NET Core" },
                    { new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e"), new Guid("6a7c7d5f-4c8e-4a4d-878b-2d2b7e8f5b7e"), "An extensive resource on algorithms and data structures.", "Computer Science", "2019-03-10", "Introduction to Algorithms" },
                    { new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e"), new Guid("8a7b7d5e-6c5d-4c4d-878b-3e3b7e5e4b6d"), "An introduction to the field of data science.", "Data Science", "2018-11-05", "Data Science for Beginners" }
                });

            migrationBuilder.InsertData(
                table: "BookAuthor",
                columns: new[] { "AuthorID", "BookID" },
                values: new object[,]
                {
                    { new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5") },
                    { new Guid("9b23c7c4-8b16-4b1e-9a22-2df6e79dbb09"), new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5") },
                    { new Guid("d3cb3f0e-689d-4c4d-86a0-3fba53a1dbd4"), new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c") },
                    { new Guid("2b19d6db-81a3-4042-9132-9b67ea7a9bbd"), new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c") },
                    { new Guid("ad2c7bc9-3f9f-40b5-a84f-60b1d843b1e5"), new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c") },
                    { new Guid("d2fa31d7-0e4b-4712-9a5c-9d45a732b3b8"), new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e") },
                    { new Guid("b13c1f29-1df1-4d95-9829-5b7a0e7e44ed"), new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e") }
                });

            migrationBuilder.InsertData(
                table: "BookFiles",
                columns: new[] { "BookFileID", "BookID", "FilePath", "FileType" },
                values: new object[,]
                {
                    { new Guid("a7c6b5d8-9d7e-4e4d-9a6c-d3e2d1b4a5e7"), new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c"), "files/machine_learning_basics.pdf", "pdf" },
                    { new Guid("b5a6d7c8-8c7d-4e4d-9a6c-d1e4e2a3b5c7"), new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e"), "files/introduction_to_algorithms.docx", "docx" },
                    { new Guid("b6d5a7c8-8e7d-4e4d-9a6c-d4e2e1b3a5c7"), new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c"), "files/machine_learning_basics.docx", "docx" },
                    { new Guid("b7a6c5d8-9e7d-4e4d-9a6c-d3e2d4b1a5c7"), new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e"), "files/data_science_for_beginners.docx", "docx" },
                    { new Guid("c7b6a5d8-9c7d-4e4d-9a6c-d2e4d1b3a5e7"), new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e"), "files/introduction_to_algorithms.pdf", "pdf" },
                    { new Guid("d4e5b6c7-8a9d-4e4d-9b6c-e2d1e4a3b5c7"), new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c"), "files/advanced_aspnet_core.docx", "docx" },
                    { new Guid("d7a6b5c8-8e7d-4e4d-9a6c-d2e1d3b4a5c7"), new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e"), "files/data_science_for_beginners.pdf", "pdf" },
                    { new Guid("e6b4a7c8-9b7d-4e4d-9a6c-d4e1e2d3b5a2"), new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c"), "files/advanced_aspnet_core.pdf", "pdf" },
                    { new Guid("f5d4e6b7-9a8c-4b7b-9e2d-d2e1d5f6c7a3"), new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5"), "files/learning_csharp.docx", "docx" },
                    { new Guid("f74e6a4b-b8d1-4b8a-9a4d-e4e5e2e1d6b3"), new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5"), "files/learning_csharp.pdf", "pdf" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AvatarImageID",
                table: "Authors",
                column: "AvatarImageID");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_AuthorID",
                table: "BookAuthor",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_BookFiles_BookID",
                table: "BookFiles",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CoverImageID",
                table: "Books",
                column: "CoverImageID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropTable(
                name: "BookFiles");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Images");
        }
    }
}
