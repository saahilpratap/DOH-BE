using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class AddedTableAuthoritativeDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthoritativeDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AuthorityDepartmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthoritativeDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthoritativeDocuments_AuthorityDepartments_AuthorityDepartmentId",
                        column: x => x.AuthorityDepartmentId,
                        principalTable: "AuthorityDepartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthoritativeDocuments_AuthorityDepartmentId",
                table: "AuthoritativeDocuments",
                column: "AuthorityDepartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthoritativeDocuments");
        }
    }
}
