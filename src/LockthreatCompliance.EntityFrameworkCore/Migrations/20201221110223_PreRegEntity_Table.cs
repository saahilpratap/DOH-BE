using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class PreRegEntity_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreRegEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    CompanyName = table.Column<string>(nullable: true),
                    AdminEmail = table.Column<string>(nullable: true),
                    AdminMobile = table.Column<string>(nullable: true),
                    VerificationCode = table.Column<string>(nullable: true),
                    IsVerificationDone = table.Column<bool>(nullable: false),
                    IsRequestApproved = table.Column<bool>(nullable: false),
                    EntityType = table.Column<int>(nullable: false),
                    ControlType = table.Column<int>(nullable: false),
                    ThirdPartyId = table.Column<int>(nullable: true),
                    LicenseNumber = table.Column<string>(nullable: true),
                    Facility_EN = table.Column<string>(nullable: true),
                    IsPublic = table.Column<bool>(nullable: false),
                    DistrictId = table.Column<int>(nullable: true),
                    FacilityTypeId = table.Column<int>(nullable: true),
                    FacilitySubTypeId = table.Column<int>(nullable: true),
                    HFLName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Facility_Email = table.Column<string>(nullable: true),
                    Owner_EN = table.Column<string>(nullable: true),
                    Owner_Email = table.Column<string>(nullable: true),
                    Owner_Mobile = table.Column<string>(nullable: true),
                    Director_Incharge_EN = table.Column<string>(nullable: true),
                    Director_Incharge_Email = table.Column<string>(nullable: true),
                    Director_Incharge_Mobile = table.Column<string>(nullable: true),
                    Pro_EN = table.Column<string>(nullable: true),
                    Pro_Email = table.Column<string>(nullable: true),
                    Pro_Mobile = table.Column<string>(nullable: true),
                    PrimaryContactName = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    ContactNumber = table.Column<string>(nullable: true),
                    OfficialEmail = table.Column<string>(nullable: true),
                    BackupContactName = table.Column<string>(nullable: true),
                    BackupDesignation = table.Column<string>(nullable: true),
                    BackupContactNumber = table.Column<string>(nullable: true),
                    BackupOfficialEmail = table.Column<string>(nullable: true),
                    AdminName = table.Column<string>(nullable: true),
                    AdminSurname = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: true),
                    CityOrDisctrict = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreRegEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreRegEntities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegEntities_AbpDynamicParameterValues_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegEntities_FacilitySubTypes_FacilitySubTypeId",
                        column: x => x.FacilitySubTypeId,
                        principalTable: "FacilitySubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegEntities_FacilityTypes_FacilityTypeId",
                        column: x => x.FacilityTypeId,
                        principalTable: "FacilityTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreRegEntities_AbpDynamicParameterValues_ThirdPartyId",
                        column: x => x.ThirdPartyId,
                        principalTable: "AbpDynamicParameterValues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreRegEntities_CountryId",
                table: "PreRegEntities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegEntities_DistrictId",
                table: "PreRegEntities",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegEntities_FacilitySubTypeId",
                table: "PreRegEntities",
                column: "FacilitySubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegEntities_FacilityTypeId",
                table: "PreRegEntities",
                column: "FacilityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PreRegEntities_ThirdPartyId",
                table: "PreRegEntities",
                column: "ThirdPartyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreRegEntities");
        }
    }
}
