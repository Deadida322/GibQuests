using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonDatabase.QuestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class addquestpolicyfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quest_quest_policy_PolicyId",
                table: "quest");

            migrationBuilder.DropIndex(
                name: "IX_quest_PolicyId",
                table: "quest");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "quest");

            migrationBuilder.AddColumn<int>(
                name: "quest_id",
                table: "quest_policy",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_quest_policy_quest_id",
                table: "quest_policy",
                column: "quest_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_quest_policy_quest_quest_id",
                table: "quest_policy",
                column: "quest_id",
                principalTable: "quest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quest_policy_quest_quest_id",
                table: "quest_policy");

            migrationBuilder.DropIndex(
                name: "IX_quest_policy_quest_id",
                table: "quest_policy");

            migrationBuilder.DropColumn(
                name: "quest_id",
                table: "quest_policy");

            migrationBuilder.AddColumn<int>(
                name: "PolicyId",
                table: "quest",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_quest_PolicyId",
                table: "quest",
                column: "PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_quest_quest_policy_PolicyId",
                table: "quest",
                column: "PolicyId",
                principalTable: "quest_policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
