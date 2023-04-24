using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableTableTopExerciseGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableTopExerciseGroups",
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
                    TableTopExerciseGroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TableTopExerciseGroupSections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableTopExerciseGroupId = table.Column<long>(nullable: false),
                    TableTopExerciseSectionId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableTopExerciseGroupSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseGroupSections_TableTopExerciseGroups_TableTopExerciseGroupId",
                        column: x => x.TableTopExerciseGroupId,
                        principalTable: "TableTopExerciseGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableTopExerciseGroupSections_TableTopExerciseSections_TableTopExerciseSectionId",
                        column: x => x.TableTopExerciseSectionId,
                        principalTable: "TableTopExerciseSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseGroupSections_TableTopExerciseGroupId",
                table: "TableTopExerciseGroupSections",
                column: "TableTopExerciseGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_TableTopExerciseGroupSections_TableTopExerciseSectionId",
                table: "TableTopExerciseGroupSections",
                column: "TableTopExerciseSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableTopExerciseGroupSections");

            migrationBuilder.DropTable(
                name: "TableTopExerciseGroups");
        }
    }
}
