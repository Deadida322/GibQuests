using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommonDatabase.QuestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class addorderquestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "question",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "question");
        }
    }
}
