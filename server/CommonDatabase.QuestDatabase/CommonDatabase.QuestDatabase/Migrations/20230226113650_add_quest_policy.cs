using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommonDatabase.QuestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class addquestpolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PolicyId",
                table: "quest",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "quest_policy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PolicyType = table.Column<byte>(type: "smallint", nullable: false),
                    MemberType = table.Column<byte>(type: "smallint", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quest_policy", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_quest_PolicyId",
                table: "quest",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_quest_policy_Id",
                table: "quest_policy",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_quest_quest_policy_PolicyId",
                table: "quest",
                column: "PolicyId",
                principalTable: "quest_policy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_quest_quest_policy_PolicyId",
                table: "quest");

            migrationBuilder.DropTable(
                name: "quest_policy");

            migrationBuilder.DropIndex(
                name: "IX_quest_PolicyId",
                table: "quest");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "quest");
        }
    }
}
