using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_columns_in_BusinessEntity_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackupContactName",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackupContactNumber",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackupDesignation",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BackupOfficialEmail",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialEmail",
                table: "BusinessEntities",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryContactName",
                table: "BusinessEntities",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackupContactName",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "BackupContactNumber",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "BackupDesignation",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "BackupOfficialEmail",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "OfficialEmail",
                table: "BusinessEntities");

            migrationBuilder.DropColumn(
                name: "PrimaryContactName",
                table: "BusinessEntities");
        }
    }
}
