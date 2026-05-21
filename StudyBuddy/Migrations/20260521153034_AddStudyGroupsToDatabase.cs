using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudyBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddStudyGroupsToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroup_AspNetUsers_CreatorId",
                table: "StudyGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroupMember_AspNetUsers_UserId",
                table: "StudyGroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroupMember_StudyGroup_StudyGroupId",
                table: "StudyGroupMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyGroupMember",
                table: "StudyGroupMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyGroup",
                table: "StudyGroup");

            migrationBuilder.RenameTable(
                name: "StudyGroupMember",
                newName: "StudyGroupMembers");

            migrationBuilder.RenameTable(
                name: "StudyGroup",
                newName: "StudyGroups");

            migrationBuilder.RenameIndex(
                name: "IX_StudyGroupMember_UserId",
                table: "StudyGroupMembers",
                newName: "IX_StudyGroupMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_StudyGroup_CreatorId",
                table: "StudyGroups",
                newName: "IX_StudyGroups_CreatorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyGroupMembers",
                table: "StudyGroupMembers",
                columns: new[] { "StudyGroupId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyGroups",
                table: "StudyGroups",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroupMembers_AspNetUsers_UserId",
                table: "StudyGroupMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroupMembers_StudyGroups_StudyGroupId",
                table: "StudyGroupMembers",
                column: "StudyGroupId",
                principalTable: "StudyGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroups_AspNetUsers_CreatorId",
                table: "StudyGroups",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroupMembers_AspNetUsers_UserId",
                table: "StudyGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroupMembers_StudyGroups_StudyGroupId",
                table: "StudyGroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyGroups_AspNetUsers_CreatorId",
                table: "StudyGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyGroups",
                table: "StudyGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudyGroupMembers",
                table: "StudyGroupMembers");

            migrationBuilder.RenameTable(
                name: "StudyGroups",
                newName: "StudyGroup");

            migrationBuilder.RenameTable(
                name: "StudyGroupMembers",
                newName: "StudyGroupMember");

            migrationBuilder.RenameIndex(
                name: "IX_StudyGroups_CreatorId",
                table: "StudyGroup",
                newName: "IX_StudyGroup_CreatorId");

            migrationBuilder.RenameIndex(
                name: "IX_StudyGroupMembers_UserId",
                table: "StudyGroupMember",
                newName: "IX_StudyGroupMember_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyGroup",
                table: "StudyGroup",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudyGroupMember",
                table: "StudyGroupMember",
                columns: new[] { "StudyGroupId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroup_AspNetUsers_CreatorId",
                table: "StudyGroup",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroupMember_AspNetUsers_UserId",
                table: "StudyGroupMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyGroupMember_StudyGroup_StudyGroupId",
                table: "StudyGroupMember",
                column: "StudyGroupId",
                principalTable: "StudyGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
