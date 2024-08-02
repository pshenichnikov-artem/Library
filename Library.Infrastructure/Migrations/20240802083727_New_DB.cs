using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class New_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookAuthor");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublicationDate",
                table: "Books",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5"),
                columns: new[] { "Author", "PublicationDate" },
                values: new object[] { null, new DateTime(2020, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c"),
                columns: new[] { "Author", "PublicationDate" },
                values: new object[] { null, new DateTime(2022, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c"),
                columns: new[] { "Author", "PublicationDate" },
                values: new object[] { null, new DateTime(2021, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e"),
                columns: new[] { "Author", "PublicationDate" },
                values: new object[] { null, new DateTime(2019, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e"),
                columns: new[] { "Author", "PublicationDate" },
                values: new object[] { null, new DateTime(2018, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "PublicationDate",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AvatarImageID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5"),
                column: "PublicationDate",
                value: "2020-05-15");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c"),
                column: "PublicationDate",
                value: "2022-01-15");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c"),
                column: "PublicationDate",
                value: "2021-08-20");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e"),
                column: "PublicationDate",
                value: "2019-03-10");

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e"),
                column: "PublicationDate",
                value: "2018-11-05");

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

            migrationBuilder.CreateIndex(
                name: "IX_Authors_AvatarImageID",
                table: "Authors",
                column: "AvatarImageID");

            migrationBuilder.CreateIndex(
                name: "IX_BookAuthor_AuthorID",
                table: "BookAuthor",
                column: "AuthorID");
        }
    }
}
