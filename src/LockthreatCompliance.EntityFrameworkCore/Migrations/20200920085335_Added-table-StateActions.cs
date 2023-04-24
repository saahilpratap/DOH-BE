using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedtableStateActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StateActions",
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
                    StateId = table.Column<long>(nullable: true),
                    ActionType = table.Column<int>(nullable: false),
                    ActionCategory = table.Column<int>(nullable: false),
                    ActionTime = table.Column<int>(nullable: false),
                    ActionTimeType = table.Column<int>(nullable: false),
                    ActionTemplate = table.Column<string>(nullable: true),
                    ActionRecipientsId = table.Column<long>(nullable: true),
                    ActionCCRecipientsId = table.Column<long>(nullable: true),
                    ActionBCCRecipientsId = table.Column<long>(nullable: true),
                    ActionSMSId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateActions_AbpUsers_ActionBCCRecipientsId",
                        column: x => x.ActionBCCRecipientsId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateActions_AbpUsers_ActionCCRecipientsId",
                        column: x => x.ActionCCRecipientsId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateActions_AbpUsers_ActionRecipientsId",
                        column: x => x.ActionRecipientsId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateActions_AbpUsers_ActionSMSId",
                        column: x => x.ActionSMSId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateActions_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_ActionBCCRecipientsId",
                table: "StateActions",
                column: "ActionBCCRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_ActionCCRecipientsId",
                table: "StateActions",
                column: "ActionCCRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_ActionRecipientsId",
                table: "StateActions",
                column: "ActionRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_ActionSMSId",
                table: "StateActions",
                column: "ActionSMSId");

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_StateId",
                table: "StateActions",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StateActions");
        }
    }
}
