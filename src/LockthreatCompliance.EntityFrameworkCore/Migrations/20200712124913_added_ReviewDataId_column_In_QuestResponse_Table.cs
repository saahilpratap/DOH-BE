using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_ReviewDataId_column_In_QuestResponse_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewDataId",
                table: "QuestResponses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestResponses_ReviewDataId",
                table: "QuestResponses",
                column: "ReviewDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestResponses_ReviewDatas_ReviewDataId",
                table: "QuestResponses",
                column: "ReviewDataId",
                principalTable: "ReviewDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestResponses_ReviewDatas_ReviewDataId",
                table: "QuestResponses");

            migrationBuilder.DropIndex(
                name: "IX_QuestResponses_ReviewDataId",
                table: "QuestResponses");

            migrationBuilder.DropColumn(
                name: "ReviewDataId",
                table: "QuestResponses");
        }
    }
}
