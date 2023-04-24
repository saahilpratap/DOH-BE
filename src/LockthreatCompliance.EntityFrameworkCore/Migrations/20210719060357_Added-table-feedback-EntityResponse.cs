using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtablefeedbackEntityResponse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedBackEntitys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    FeedbackDetailId = table.Column<int>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackEntitys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedBackEntitys_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FeedBackEntitys_FeedbackDetails_FeedbackDetailId",
                        column: x => x.FeedbackDetailId,
                        principalTable: "FeedbackDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedBackEntityResponses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    FeedBackEntityId = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: false),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBackEntityResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedBackEntityResponses_FeedBackEntitys_FeedBackEntityId",
                        column: x => x.FeedBackEntityId,
                        principalTable: "FeedBackEntitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeedBackEntityResponses_FeedBackQuestioner_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "FeedBackQuestioner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackEntityResponses_FeedBackEntityId",
                table: "FeedBackEntityResponses",
                column: "FeedBackEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackEntityResponses_QuestionId",
                table: "FeedBackEntityResponses",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackEntitys_BusinessEntityId",
                table: "FeedBackEntitys",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedBackEntitys_FeedbackDetailId",
                table: "FeedBackEntitys",
                column: "FeedbackDetailId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedBackEntityResponses");

            migrationBuilder.DropTable(
                name: "FeedBackEntitys");
        }
    }
}
