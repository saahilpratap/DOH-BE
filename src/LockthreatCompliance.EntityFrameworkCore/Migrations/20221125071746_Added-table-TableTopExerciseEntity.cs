using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableTableTopExerciseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableTopExerciseEntitys",
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
                    TableTopExerciseGroupId = table.Column<long>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: false),
                    Submitted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseEntitys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseEntitys_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseEntitys_TableTopExerciseGroups_TableTopExerciseGroupId",
                        column: x => x.TableTopExerciseGroupId,
                        principalTable: "TableTopExerciseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableTopExerciseEntityAttachments",
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
                    TableTopExerciseEntityId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseEntityAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseEntityAttachments_TableTopExerciseEntitys_TableTopExerciseEntityId",
                        column: x => x.TableTopExerciseEntityId,
                        principalTable: "TableTopExerciseEntitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableTopExerciseEntityResponses",
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
                    TableTopExerciseEntityId = table.Column<long>(nullable: false),
                    TableTopExerciseSectionId = table.Column<long>(nullable: false),
                    TableTopExerciseQuestionId = table.Column<long>(nullable: false),
                    QuestionComment = table.Column<string>(nullable: true),
                    CommentMandatory = table.Column<bool>(nullable: false),
                    AnswerType = table.Column<int>(nullable: false),
                    Response = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseEntityResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseEntityResponses_TableTopExerciseEntitys_TableTopExerciseEntityId",
                        column: x => x.TableTopExerciseEntityId,
                        principalTable: "TableTopExerciseEntitys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseEntityResponses_TableTopExerciseQuestions_TableTopExerciseQuestionId",
                        column: x => x.TableTopExerciseQuestionId,
                        principalTable: "TableTopExerciseQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseEntityResponses_TableTopExerciseSections_TableTopExerciseSectionId",
                        column: x => x.TableTopExerciseSectionId,
                        principalTable: "TableTopExerciseSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntityAttachments_TableTopExerciseEntityId",
                table: "TableTopExerciseEntityAttachments",
                column: "TableTopExerciseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntityResponses_TableTopExerciseEntityId",
                table: "TableTopExerciseEntityResponses",
                column: "TableTopExerciseEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntityResponses_TableTopExerciseQuestionId",
                table: "TableTopExerciseEntityResponses",
                column: "TableTopExerciseQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntityResponses_TableTopExerciseSectionId",
                table: "TableTopExerciseEntityResponses",
                column: "TableTopExerciseSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntitys_BusinessEntityId",
                table: "TableTopExerciseEntitys",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseEntitys_TableTopExerciseGroupId",
                table: "TableTopExerciseEntitys",
                column: "TableTopExerciseGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableTopExerciseEntityAttachments");

            migrationBuilder.DropTable(
                name: "TableTopExerciseEntityResponses");

            migrationBuilder.DropTable(
                name: "TableTopExerciseEntitys");
        }
    }
}
