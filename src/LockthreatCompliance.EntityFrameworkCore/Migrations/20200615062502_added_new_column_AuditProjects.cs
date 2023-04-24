using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_new_column_AuditProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditMamangerId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "AuditeeTeam",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "AuditorTeam",
                table: "AuditProjects");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AuditProjects",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditProjects_CountryId",
                table: "AuditProjects",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditProjects_Countries_CountryId",
                table: "AuditProjects",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditProjects_Countries_CountryId",
                table: "AuditProjects");

            migrationBuilder.DropIndex(
                name: "IX_AuditProjects_CountryId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "AuditProjects");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AuditProjects");

            migrationBuilder.AddColumn<long>(
                name: "AuditMamangerId",
                table: "AuditProjects",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditeeTeam",
                table: "AuditProjects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditorTeam",
                table: "AuditProjects",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
