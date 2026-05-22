using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyGroupNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyGroupId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_StudyGroupId",
                table: "Notes",
                column: "StudyGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_StudyGroups_StudyGroupId",
                table: "Notes",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_StudyGroups_StudyGroupId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_StudyGroupId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "StudyGroupId",
                table: "Notes");
        }
    }
}
