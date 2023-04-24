using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LockthreatCompliance.Migrations
{
    public partial class added_FindingReportLog_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FindingReportLogs",
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
                    FindingId = table.Column<int>(nullable: false),
                    UpdatedFieldName = table.Column<string>(nullable: true),
                    CreateOrEditFlag = table.Column<bool>(nullable: false),
                    Old_Title = table.Column<string>(nullable: true),
                    Old_Category = table.Column<string>(nullable: true),
                    Old_DateFound = table.Column<DateTime>(nullable: true),
                    Old_FindingDetails = table.Column<string>(nullable: true),
                    Old_Reference = table.Column<string>(nullable: true),
                    Old_ActionResponseDate = table.Column<DateTime>(nullable: true),
                    Old_DateClosed = table.Column<DateTime>(nullable: true),
                    Old_CorrectiveActionResponse = table.Column<string>(nullable: true),
                    Old_ActualRootCause = table.Column<string>(nullable: true),
                    Old_Status = table.Column<string>(nullable: true),
                    Old_AuditorRemarks = table.Column<string>(nullable: true),
                    Old_CAPAUpdateRequired = table.Column<bool>(nullable: false),
                    Old_Attachment = table.Column<string>(nullable: true),
                    New_Title = table.Column<string>(nullable: true),
                    New_Category = table.Column<string>(nullable: true),
                    New_DateFound = table.Column<DateTime>(nullable: true),
                    New_FindingDetails = table.Column<string>(nullable: true),
                    New_Reference = table.Column<string>(nullable: true),
                    New_ActionResponseDate = table.Column<DateTime>(nullable: true),
                    New_DateClosed = table.Column<DateTime>(nullable: true),
                    New_CorrectiveActionResponse = table.Column<string>(nullable: true),
                    New_ActualRootCause = table.Column<string>(nullable: true),
                    New_Status = table.Column<string>(nullable: true),
                    New_AuditorRemarks = table.Column<string>(nullable: true),
                    New_CAPAUpdateRequired = table.Column<bool>(nullable: false),
                    New_Attachment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FindingReportLogs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FindingReportLogs");
        }
    }
}
