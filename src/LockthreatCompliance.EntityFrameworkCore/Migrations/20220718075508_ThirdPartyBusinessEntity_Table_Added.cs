using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class ThirdPartyBusinessEntity_Table_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessEntityThirdPartys",
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
                    LICENSE_NO = table.Column<string>(nullable: true),
                    STATUS = table.Column<string>(nullable: true),
                    FACILITY_CATEGORY = table.Column<string>(nullable: true),
                    HAS_INSURANCE = table.Column<string>(nullable: true),
                    IS_PUBLIC_FACILITY = table.Column<string>(nullable: true),
                    FACILITY_GROUP = table.Column<string>(nullable: true),
                    DED_LICENSE = table.Column<string>(nullable: true),
                    DED_TEMPORARY_NUMBER = table.Column<string>(nullable: true),
                    FACILITY_NAME = table.Column<string>(nullable: true),
                    FACILITY_TYPE = table.Column<string>(nullable: true),
                    FACILITY_SUBTYPE = table.Column<string>(nullable: true),
                    ADDRESS = table.Column<string>(nullable: true),
                    CITY = table.Column<string>(nullable: true),
                    TELEPHONE = table.Column<string>(nullable: true),
                    EMAIL = table.Column<string>(nullable: true),
                    OWNERNAME_EN = table.Column<string>(nullable: true),
                    OWNER_TELEPHONE = table.Column<string>(nullable: true),
                    OWNER_EMAIL = table.Column<string>(nullable: true),
                    OWNER_EID = table.Column<string>(nullable: true),
                    DIRECTOR_INCHARGE_ENGLISH_NAME = table.Column<string>(nullable: true),
                    DIRECTOR_INCHARGE_TELEPHONE = table.Column<string>(nullable: true),
                    DIRECTOR_INCHARGE_EMAIL = table.Column<string>(nullable: true),
                    PRO_NAME = table.Column<string>(nullable: true),
                    PRO_TELEPHONE = table.Column<string>(nullable: true),
                    PRO_EMAIL = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessEntityThirdPartys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessEntityThirdPartys");
        }
    }
}
