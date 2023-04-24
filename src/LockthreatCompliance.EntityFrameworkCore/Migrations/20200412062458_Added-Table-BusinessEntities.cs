using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableBusinessEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BusinessEntityId",
                table: "AbpUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BusinessEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    EntityId = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: false),
                    CompanyLegalName = table.Column<string>(nullable: true),
                    FacilityTypeId = table.Column<int>(nullable: true),
                    CompanyWebsite = table.Column<string>(nullable: true),
                    NumberOfYearsInBusiness = table.Column<int>(nullable: false),
                    IsGovernmentOwned = table.Column<bool>(nullable: false),
                    IsCompanyLicensed = table.Column<bool>(nullable: false),
                    LicenseNumber = table.Column<string>(nullable: true),
                    CompanyAddress = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: false),
                    CityOrDisctrict = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    IsAuditableEntity = table.Column<bool>(nullable: false),
                    ParentCompanyName = table.Column<string>(nullable: true),
                    ParentCompanyId = table.Column<int>(nullable: true),
                    IsParentReportingEnabled = table.Column<bool>(nullable: false),
                    ParentOrganizationId = table.Column<long>(nullable: true),
                    AdminName = table.Column<string>(nullable: true),
                    AdminSurname = table.Column<string>(nullable: true),
                    AdminPosition = table.Column<string>(nullable: true),
                    AdminMobile = table.Column<string>(nullable: true),
                    EntityType = table.Column<int>(nullable: false),
                    AdminEmail = table.Column<string>(nullable: false),
                    IsSuspended = table.Column<bool>(nullable: false),
                    ComplianceType = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: true),
                    HasAdminGenerated = table.Column<bool>(nullable: false),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessEntities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessEntities_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessEntities_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbpUsers_BusinessEntityId",
                table: "AbpUsers",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_CountryId",
                table: "BusinessEntities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_FacilityTypeId",
                table: "BusinessEntities",
                column: "FacilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntities_OrganizationUnitId",
                table: "BusinessEntities",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbpUsers_BusinessEntities_BusinessEntityId",
                table: "AbpUsers",
                column: "BusinessEntityId",
                principalTable: "BusinessEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbpUsers_BusinessEntities_BusinessEntityId",
                table: "AbpUsers");

            migrationBuilder.DropTable(
                name: "BusinessEntities");

            migrationBuilder.DropIndex(
                name: "IX_AbpUsers_BusinessEntityId",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "BusinessEntityId",
                table: "AbpUsers");
        }
    }
}
