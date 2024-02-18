using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcessQuestService.ProcessQuestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class changetablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PassedUsers_Rooms_room_id",
                table: "PassedUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PassedUsers",
                table: "PassedUsers");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "room");

            migrationBuilder.RenameTable(
                name: "PassedUsers",
                newName: "passing_user");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "room",
                newName: "is_deleted");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "passing_user",
                newName: "is_deleted");

            migrationBuilder.RenameIndex(
                name: "IX_PassedUsers_room_id",
                table: "passing_user",
                newName: "IX_passing_user_room_id");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "room",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_deleted",
                table: "passing_user",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_room",
                table: "room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_passing_user",
                table: "passing_user",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_room_Id",
                table: "room",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_passing_user_Id",
                table: "passing_user",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_passing_user_room_room_id",
                table: "passing_user",
                column: "room_id",
                principalTable: "room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_passing_user_room_room_id",
                table: "passing_user");

            migrationBuilder.DropPrimaryKey(
                name: "PK_room",
                table: "room");

            migrationBuilder.DropIndex(
                name: "IX_room_Id",
                table: "room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_passing_user",
                table: "passing_user");

            migrationBuilder.DropIndex(
                name: "IX_passing_user_Id",
                table: "passing_user");

            migrationBuilder.RenameTable(
                name: "room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "passing_user",
                newName: "PassedUsers");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "Rooms",
                newName: "IsDeleted");

            migrationBuilder.RenameColumn(
                name: "is_deleted",
                table: "PassedUsers",
                newName: "IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_passing_user_room_id",
                table: "PassedUsers",
                newName: "IX_PassedUsers_room_id");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Rooms",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "PassedUsers",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PassedUsers",
                table: "PassedUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PassedUsers_Rooms_room_id",
                table: "PassedUsers",
                column: "room_id",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
