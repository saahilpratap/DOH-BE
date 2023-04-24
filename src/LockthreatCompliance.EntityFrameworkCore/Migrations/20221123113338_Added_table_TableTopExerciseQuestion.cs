using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class Added_table_TableTopExerciseQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableTopExerciseQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Mandatory = table.Column<bool>(nullable: false),
                    CommentMandatory = table.Column<bool>(nullable: false),
                    AnswerType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableTopExerciseQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableTopExerciseQuestionId = table.Column<long>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    Score = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseQuestionOptions_TableTopExerciseQuestions_TableTopExerciseQuestionId",
                        column: x => x.TableTopExerciseQuestionId,
                        principalTable: "TableTopExerciseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseQuestionOptions_TableTopExerciseQuestionId",
                table: "TableTopExerciseQuestionOptions",
                column: "TableTopExerciseQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableTopExerciseQuestionOptions");

            migrationBuilder.DropTable(
                name: "TableTopExerciseQuestions");
        }
    }
}
