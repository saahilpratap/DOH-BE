using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_columns_in_PreRegisterBusinessEntity_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackupContactName",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackupContactNumber",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackupDesignation",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackupOfficialEmail",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialEmail",
                table: "PreRegisterBusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactName",
                table: "PreRegisterBusinessEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupContactName",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "BackupContactNumber",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "BackupDesignation",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "BackupOfficialEmail",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "OfficialEmail",
                table: "PreRegisterBusinessEntities");

            migrationBuilder.DropColumn(
                name: "PrimaryContactName",
                table: "PreRegisterBusinessEntities");
        }
    }
}
