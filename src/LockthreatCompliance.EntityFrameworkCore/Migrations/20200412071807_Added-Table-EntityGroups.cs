using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableEntityGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityGroups",
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
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    PrimaryEntityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityGroups_AbpOrganizationUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalTable: "AbpOrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityGroupMember",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessEntityId = table.Column<int>(nullable: false),
                    EntityGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityGroupMember", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityGroupMember_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntityGroupMember_EntityGroups_EntityGroupId",
                        column: x => x.EntityGroupId,
                        principalTable: "EntityGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EntityGroupMember_BusinessEntityId",
                table: "EntityGroupMember",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityGroupMember_EntityGroupId",
                table: "EntityGroupMember",
                column: "EntityGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityGroups_OrganizationUnitId",
                table: "EntityGroups",
                column: "OrganizationUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityGroupMember");

            migrationBuilder.DropTable(
                name: "EntityGroups");
        }
    }
}
