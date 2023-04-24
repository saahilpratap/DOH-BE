using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_BusinessEntityAdminChangeRequest_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessEntityAdminChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    OldAdminId = table.Column<long>(nullable: true),
                    NewAdminId = table.Column<long>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    BusinessEntityId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessEntityAdminChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BusinessEntityAdminChangeRequests_BusinessEntities_BusinessEntityId",
                        column: x => x.BusinessEntityId,
                        principalTable: "BusinessEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessEntityAdminChangeRequests_AbpUsers_NewAdminId",
                        column: x => x.NewAdminId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BusinessEntityAdminChangeRequests_AbpUsers_OldAdminId",
                        column: x => x.OldAdminId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityAdminChangeRequests_BusinessEntityId",
                table: "BusinessEntityAdminChangeRequests",
                column: "BusinessEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityAdminChangeRequests_NewAdminId",
                table: "BusinessEntityAdminChangeRequests",
                column: "NewAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessEntityAdminChangeRequests_OldAdminId",
                table: "BusinessEntityAdminChangeRequests",
                column: "OldAdminId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessEntityAdminChangeRequests");
        }
    }
}
