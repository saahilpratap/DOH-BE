using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_EntityApplicationSetting_tble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EntityApplicationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    EnableNewUserApproval = table.Column<bool>(nullable: false),
                    EnablePreRegVerification = table.Column<bool>(nullable: false),
                    PreRegVerificationList = table.Column<string>(maxLength: 999, nullable: true),
                    SkipReviewerApproval = table.Column<bool>(nullable: false),
                    EnableEntityGroupAdminApproval = table.Column<bool>(nullable: false),
                    LoginScreenDisclaimerMesg = table.Column<string>(maxLength: 999, nullable: true),
                    RootUnit = table.Column<string>(nullable: true),
                    FirstUnit = table.Column<string>(nullable: true),
                    SecondUnit = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityApplicationSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EntityApplicationSettings");
        }
    }
}
