using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddMigtation_tableWorkFlowPages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "WorkFlowPageId",
                table: "State",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WorkFlowPage",
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
                    PageName = table.Column<string>(nullable: false),
                    PageDescription = table.Column<string>(nullable: true),
                    IsPageActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowPage", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_State_WorkFlowPageId",
                table: "State",
                column: "WorkFlowPageId");

            migrationBuilder.AddForeignKey(
                name: "FK_State_WorkFlowPage_WorkFlowPageId",
                table: "State",
                column: "WorkFlowPageId",
                principalTable: "WorkFlowPage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_State_WorkFlowPage_WorkFlowPageId",
                table: "State");

            migrationBuilder.DropTable(
                name: "WorkFlowPage");

            migrationBuilder.DropIndex(
                name: "IX_State_WorkFlowPageId",
                table: "State");

            migrationBuilder.DropColumn(
                name: "WorkFlowPageId",
                table: "State");
        }
    }
}
