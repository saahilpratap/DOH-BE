using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class linking_for_contact_and_contactType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactTypeId",
                table: "Contacts",
                column: "ContactTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_ContactTypes_ContactTypeId",
                table: "Contacts",
                column: "ContactTypeId",
                principalTable: "ContactTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_ContactTypes_ContactTypeId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_ContactTypeId",
                table: "Contacts");
        }
    }
}
