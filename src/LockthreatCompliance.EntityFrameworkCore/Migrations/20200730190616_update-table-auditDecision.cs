using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class updatetableauditDecision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditDecForms_EntityGroups_EntityGroupId",
                table: "AuditDecForms");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditDecForms_FacilityTypes_FacilityTypeId",
                table: "AuditDecForms");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityTypeId",
                table: "AuditDecForms",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "EntityGroupId",
                table: "AuditDecForms",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDecForms_EntityGroups_EntityGroupId",
                table: "AuditDecForms",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDecForms_FacilityTypes_FacilityTypeId",
                table: "AuditDecForms",
                column: "FacilityTypeId",
                principalTable: "FacilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditDecForms_EntityGroups_EntityGroupId",
                table: "AuditDecForms");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditDecForms_FacilityTypes_FacilityTypeId",
                table: "AuditDecForms");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityTypeId",
                table: "AuditDecForms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EntityGroupId",
                table: "AuditDecForms",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDecForms_EntityGroups_EntityGroupId",
                table: "AuditDecForms",
                column: "EntityGroupId",
                principalTable: "EntityGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditDecForms_FacilityTypes_FacilityTypeId",
                table: "AuditDecForms",
                column: "FacilityTypeId",
                principalTable: "FacilityTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
