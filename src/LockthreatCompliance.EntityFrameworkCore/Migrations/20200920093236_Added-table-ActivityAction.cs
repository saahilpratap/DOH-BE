using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableActivityAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityActions",
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
                    ActivitiesId = table.Column<long>(nullable: true),
                    ActionType = table.Column<int>(nullable: false),
                    ActionCategory = table.Column<int>(nullable: false),
                    ActionTimeType = table.Column<int>(nullable: false),
                    ActionTime = table.Column<int>(nullable: true),
                    ActionTemplate = table.Column<string>(nullable: true),
                    ActionRecipientsId = table.Column<long>(nullable: true),
                    ActionCCRecipientsId = table.Column<long>(nullable: true),
                    ActionBCCRecipientsId = table.Column<long>(nullable: true),
                    ActionSMSId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityActions_AbpUsers_ActionBCCRecipientsId",
                        column: x => x.ActionBCCRecipientsId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityActions_AbpUsers_ActionCCRecipientsId",
                        column: x => x.ActionCCRecipientsId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityActions_AbpUsers_ActionRecipientsId",
                        column: x => x.ActionRecipientsId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityActions_AbpUsers_ActionSMSId",
                        column: x => x.ActionSMSId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityActions_Activities_ActivitiesId",
                        column: x => x.ActivitiesId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActionBCCRecipientsId",
                table: "ActivityActions",
                column: "ActionBCCRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActionCCRecipientsId",
                table: "ActivityActions",
                column: "ActionCCRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActionRecipientsId",
                table: "ActivityActions",
                column: "ActionRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActionSMSId",
                table: "ActivityActions",
                column: "ActionSMSId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityActions_ActivitiesId",
                table: "ActivityActions",
                column: "ActivitiesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityActions");
        }
    }
}
