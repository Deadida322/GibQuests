using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommonDatabase.QuestDatabase.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "quest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Img = table.Column<string>(type: "text", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    StageType = table.Column<byte>(type: "smallint", nullable: false),
                    QuestId = table.Column<int>(type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stages_quest_QuestId",
                        column: x => x.QuestId,
                        principalTable: "quest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "map_stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_map_stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_map_stage_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "qrcode_stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qrcode_stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_qrcode_stage_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "test_stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_test_stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_test_stage_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "text_stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_text_stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_text_stage_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "video_stage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_video_stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_video_stage_Stages_Id",
                        column: x => x.Id,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "coordinates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<decimal>(type: "numeric", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric", nullable: false),
                    mapstageid = table.Column<int>(name: "map_stage_id", type: "integer", nullable: false),
                    isdeleted = table.Column<bool>(name: "is_deleted", type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_coordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_coordinates_map_stage_map_stage_id",
                        column: x => x.mapstageid,
                        principalTable: "map_stage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_coordinates_Id",
                table: "coordinates",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_coordinates_map_stage_id",
                table: "coordinates",
                column: "map_stage_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_map_stage_Id",
                table: "map_stage",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_qrcode_stage_Id",
                table: "qrcode_stage",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_quest_Id",
                table: "quest",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_QuestId",
                table: "Stages",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_test_stage_Id",
                table: "test_stage",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_text_stage_Id",
                table: "text_stage",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_video_stage_Id",
                table: "video_stage",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "coordinates");

            migrationBuilder.DropTable(
                name: "qrcode_stage");

            migrationBuilder.DropTable(
                name: "test_stage");

            migrationBuilder.DropTable(
                name: "text_stage");

            migrationBuilder.DropTable(
                name: "video_stage");

            migrationBuilder.DropTable(
                name: "map_stage");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "quest");
        }
    }
}
