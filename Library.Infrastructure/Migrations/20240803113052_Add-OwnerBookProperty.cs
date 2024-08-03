using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerBookProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerBookEmail",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("2a3a2e4f-94be-4d5e-935a-e24e45bc08e5"),
                column: "OwnerBookEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("4c5d3e7f-9b8d-4a4d-9a6c-3e4a2b7b5d4c"),
                column: "OwnerBookEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("5a2e08e1-2c3a-4a2b-a8de-4e4a2d2b7b6c"),
                column: "OwnerBookEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("6a7b9c1e-9c7b-4e4d-9a8b-2d2b8c4a6c2e"),
                column: "OwnerBookEmail",
                value: null);

            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookID",
                keyValue: new Guid("9a8b7c6d-4a4d-4e4c-9b6c-2d3e4b6b5d4e"),
                column: "OwnerBookEmail",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerBookEmail",
                table: "Books");
        }
    }
}
