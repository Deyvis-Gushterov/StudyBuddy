using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyGroupBlogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyGroupId",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_StudyGroupId",
                table: "Blogs",
                column: "StudyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_StudyGroups_StudyGroupId",
                table: "Blogs",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_StudyGroups_StudyGroupId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_StudyGroupId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "Blogs");
        }
    }
}
