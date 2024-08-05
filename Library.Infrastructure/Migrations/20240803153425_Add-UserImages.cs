using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImage_AspNetUsers_UserID",
                table: "UserImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserImage",
                table: "UserImage");

            migrationBuilder.RenameTable(
                name: "UserImage",
                newName: "UserImages");

            migrationBuilder.RenameIndex(
                name: "IX_UserImage_UserID",
                table: "UserImages",
                newName: "IX_UserImages_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserImages",
                table: "UserImages",
                column: "UserImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImages_AspNetUsers_UserID",
                table: "UserImages",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserImages_AspNetUsers_UserID",
                table: "UserImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserImages",
                table: "UserImages");

            migrationBuilder.RenameTable(
                name: "UserImages",
                newName: "UserImage");

            migrationBuilder.RenameIndex(
                name: "IX_UserImages_UserID",
                table: "UserImage",
                newName: "IX_UserImage_UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserImage",
                table: "UserImage",
                column: "UserImageID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserImage_AspNetUsers_UserID",
                table: "UserImage",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
