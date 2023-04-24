using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updated_entityGroup_member : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityGroupMember_EntityGroups_EntityGroupId",
                table: "EntityGroupMember");

            migrationBuilder.AlterColumn<int>(
                name: "EntityGroupId",
                table: "EntityGroupMember",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EntityGroupMember_EntityGroups_EntityGroupId",
                table: "EntityGroupMember",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EntityGroupMember_EntityGroups_EntityGroupId",
                table: "EntityGroupMember");

            migrationBuilder.AlterColumn<int>(
                name: "EntityGroupId",
                table: "EntityGroupMember",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_EntityGroupMember_EntityGroups_EntityGroupId",
                table: "EntityGroupMember",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
