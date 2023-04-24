using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableStateActionTemplateIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TemplateId",
                table: "StateActions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_StateActions_TemplateId",
                table: "StateActions",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_StateActions_Templates_TemplateId",
                table: "StateActions",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StateActions_Templates_TemplateId",
                table: "StateActions");

            migrationBuilder.DropIndex(
                name: "IX_StateActions_TemplateId",
                table: "StateActions");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "StateActions");
        }
    }
}
