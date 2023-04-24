using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class addedtableTableTopExerciseSection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableTopExerciseSections",
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
                    SectionName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableTopExerciseSectionAttachements",
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
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    TableTopExerciseSectionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseSectionAttachements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseSectionAttachements_TableTopExerciseSections_TableTopExerciseSectionId",
                        column: x => x.TableTopExerciseSectionId,
                        principalTable: "TableTopExerciseSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableTopExerciseSectionQuestions",
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
                    TableTopExerciseQuestionId = table.Column<long>(nullable: false),
                    TableTopExerciseSectionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseSectionQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseSectionQuestions_TableTopExerciseQuestions_TableTopExerciseQuestionId",
                        column: x => x.TableTopExerciseQuestionId,
                        principalTable: "TableTopExerciseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseSectionQuestions_TableTopExerciseSections_TableTopExerciseSectionId",
                        column: x => x.TableTopExerciseSectionId,
                        principalTable: "TableTopExerciseSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseSectionAttachements_TableTopExerciseSectionId",
                table: "TableTopExerciseSectionAttachements",
                column: "TableTopExerciseSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseSectionQuestions_TableTopExerciseQuestionId",
                table: "TableTopExerciseSectionQuestions",
                column: "TableTopExerciseQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseSectionQuestions_TableTopExerciseSectionId",
                table: "TableTopExerciseSectionQuestions",
                column: "TableTopExerciseSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableTopExerciseSectionAttachements");

            migrationBuilder.DropTable(
                name: "TableTopExerciseSectionQuestions");

            migrationBuilder.DropTable(
                name: "TableTopExerciseSections");
        }
    }
}
