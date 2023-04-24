using Abp.Runtime.Session;
using Abp.Threading.Extensions;
using Abp.Timing.Timezone;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.DataExporting.Excel.EpPlus;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Storage;
using Stripe;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.Internal.AsyncLock;

namespace LockthreatCompliance.AuditProjects.Exporting
{
    public class AuditProjectExcelExporter : EpPlusExcelExporterBase, IAuditProjectExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AuditProjectExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }
        public FileDto ExportAuditProjectToFile(List<ExportAuditProject> exportAuditProject)
        {



            return CreateExcelPackage(
               "AuditProject.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditProject"));
                   sheet.OutLineApplyStyle = true;                
                   AddHeader(
                       sheet,
                       ("AUDIT_YEAR"),
                       ("AUDIT_STATUS"),
                       ("AUDIT_ID"),
                       ("PRIMARY_LICENSE_NUMBER"),
                       ("SECONDARY_LICENSE_NUMBER"),
                       ("ENTITY_GROUP"),
                       ("PRIMARY_HEALTH_CARE_ENTITY"),
                       ("SECONDARY_HEALTH_CARE_ENTITY"),
                       ("FACILITY_TYPE"),
                       ("FACILITY_SUBTYPE"),
                       ("REGION_CITY"),
                       ("AUDIT_TYPE"),
                       ("AUDIT_TITLE"),
                       ("STAGE_1_AUDIT_(START DATE)"),
                       ("STAGE_1_AUDIT_(END DATE)"),
                       ("STAGE_1_NO_OF_DAYS"),
                       ("STAGE_1_LEAD_AUDITOR"),
                       ("STAGE_2_AUDIT_(START DATE)"),
                       ("STAGE_2_AUDIT_(END DATE)"),
                       ("STAGE_2_NO_OF_DAYS"),
                       ("STAGE_2_LEAD_AUDITOR"),
                       ("AUDITEE_NAME"),
                       ("AUDITEE_EMAIL"),
                       ("AUDITEE_CONTACT"),
                       ("AUDIT_CO-ORDINATOR_NAME"),
                       ("AUDIT_CO-ORDINATOR_EMAIL"),
                       ("ENTITY_DIRECTOR_NAME"),
                       ("ENTITY_DIRECTOR_EMAIL"),
                       ("AUDIT_ADMIN_(AUDIT_SERVICE_PROVIDER)"),
                       ("AUDIT_STATUS"),
                       ("CAPA_STATUS"),
                       ("AUDIT_OUTCOME-REPORT"),
                       ("ACTUAL_AUDIT_REPORT_DATE"),
                       ("CAPA_SUBMISSION_DATE"),
                       ("DATE_OF_RELEASING_1ST_REVISED"),
                       ("DATE_OF_RELEASING_2ND_REVISED"),
                       ("COMMENTS"),
                       ("Remarks"),
                       ("payment Details"),
                       ("Evidence Submission Timeline"),
                       ("Overall Status"),
                       ("Evidence shared status"),
                       ("Outcome Report Released Date"),
                       ("Re-audit score 1"),
                       ("Re-audit score 2"),
                       ("Date of releasing Reaudit 1"),
                       ("Date of releasing Reaudit 2"),
                       ("90 Days timeline"),
                       ("Evidence Shared Date 1"),
                       ("Evidence Shared Date 2 ")
                       );

                   AddObjects(
                       sheet, 2, exportAuditProject,
                       _ => _.FiscalYear,
                        _ => _.status,
                       _ => _.Id,
                       _ => _.PrimaryLicenseNumber,
                       _ => _.SecondaryLicenseNumber,
                       _ => _.EntityGroupName,
                       _ => _.PrimaryEntityName,
                       _ => _.SecondaryEntityName,
                       _ => _.FacilityName,
                       _ => _.FacilitySubTypeName,
                       _ => _.City,
                       _ => _.AuditTypeName,
                       _ => _.AuditTitle,
                       _ => _.StartDate,
                       _ => _.EndDate,
                       _ => _.AuditDuration,
                       _ => _.LeadAuditorEmail,
                       _ => _.StageStartDate,
                       _ => _.StageEndDate,
                       _ => _.StageAuditDuration,
                       _ => "",
                       _ => _.AuditeeName,
                       _ => _.AuditeeEmail,
                       _ => _.AuditeeContact,
                       _ => _.AuditCoordinatorName,
                       _ => _.AuditCoordinatorEmail,
                       _ => _.EntityDirectorName,
                       _ => _.EntityDirectorEmail,
                       _ => _.AuditManager,
                       _ => _.AuditStatus,
                       _ => _.CAPAStatus,
                       _ => _.AuditOutcomeReport,
                       _ => _.ActualAuditReport,
                       _ => _.CAPAsubmissiondate,
                       _ => _.DateofReleasing1stRevised,
                       _ => _.DateofReleasing2ndRevised,
                       _ => _.Remarks,
                       _ => _.PaymentDetails,
                       _ => _.EvidenceSubmTimeiClosed,
                       _ => _.OverallStatus,
                       _ => _.EvidenceStatus,
                       _ => _.OutcomeReportReleasedDate,
                       _ => _.ReAuditScoreOne,
                       _ => _.ReAuditScoreTwo,
                       _ => _.DateofReleasingReauditOne,
                       _ => _.DateofReleasingReauditTwo,
                       _ => _.DaysTimeline,
                       _ => _.EvidenceSharedDateOne,
                       _ => _.EvidenceSharedDateTwo                                               
                       );

                   for (var i = 0; i <50 ; i++)
                   {                      
                       sheet.Cells.AutoFitColumns(i);
                   }
                   sheet.Protection.IsProtected = true;
                   sheet.Column(1).Style.Locked = false;
                   sheet.Column(4).Style.Locked = false;
                   sheet.Column(5).Style.Locked = false;
                   sheet.Column(6).Style.Locked = false;
                   sheet.Column(7).Style.Locked = false;
                   sheet.Column(8).Style.Locked = false;
                   sheet.Column(9).Style.Locked = false;
                   sheet.Column(10).Style.Locked = false;
                   sheet.Column(11).Style.Locked = false;
                   sheet.Column(12).Style.Locked = false;
                   sheet.Column(13).Style.Locked = false;
                   sheet.Column(14).Style.Locked = false;
                   sheet.Column(15).Style.Locked = false;
                   sheet.Column(16).Style.Locked = false;
                   sheet.Column(17).Style.Locked = false;
                   sheet.Column(18).Style.Locked = false;
                   sheet.Column(19).Style.Locked = false;
                   sheet.Column(20).Style.Locked = false;
                   sheet.Column(21).Style.Locked = false;
                   sheet.Column(22).Style.Locked = false;
                   sheet.Column(23).Style.Locked = false;
                   sheet.Column(24).Style.Locked = false;
                   sheet.Column(25).Style.Locked = false;
                   sheet.Column(26).Style.Locked = false;
                   sheet.Column(27).Style.Locked = false;
                   sheet.Column(28).Style.Locked = false;
                   sheet.Column(29).Style.Locked = false;
                   sheet.Column(30).Style.Locked = false;
                   sheet.Column(31).Style.Locked = false;
                   sheet.Column(32).Style.Locked = false;
                   sheet.Column(33).Style.Locked = false;
                   sheet.Column(34).Style.Locked = false;
                   sheet.Column(35).Style.Locked = false;
                   sheet.Column(36).Style.Locked = false;
                   sheet.Column(37).Style.Locked = false;
                   sheet.Column(38).Style.Locked = false;
                   sheet.Column(39).Style.Locked = false;
                   sheet.Column(40).Style.Locked = false;
                   sheet.Column(41).Style.Locked = false;
                   sheet.Column(42).Style.Locked = false;
                   sheet.Column(43).Style.Locked = false;
                   sheet.Column(44).Style.Locked = false;
                   sheet.Column(45).Style.Locked = false;
                   sheet.Column(46).Style.Locked = false;
                   sheet.Column(47).Style.Locked = false;
                   sheet.Column(48).Style.Locked = false;
                   sheet.Column(49).Style.Locked = false; 
                   sheet.Column(50).Style.Locked = false;

               });
        }

        public FileDto ExportInvalidAuditProjectToFile(List<ExportAuditProject> exportAuditProject)
        {
            return CreateExcelPackage(
                "InvalidAuditProjectData.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditProject"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        ("AUDIT_YEAR"),
                        ("AUDIT_STATUS"),
                        ("AUDIT_ID"),
                        ("PRIMARY_LICENSE_NUMBER"),
                        ("SECONDARY_LICENSE_NUMBER"),
                        ("ENTITY_GROUP"),
                        ("PRIMARY_HEALTH_CARE_ENTITY"),
                        ("SECONDARY_HEALTH_CARE_ENTITY"),
                        ("FACILITY_TYPE"),
                        ("FACILITY_SUBTYPE"),
                        ("REGION_CITY"),
                        ("AUDIT_TYPE"),
                        ("AUDIT_TITLE"),
                        ("STAGE_1_AUDIT_(START DATE)"),
                        ("STAGE_1_AUDIT_(END DATE)"),
                        ("STAGE_1_NO_OF_DAYS"),
                        ("STAGE_1_LEAD_AUDITOR"),
                        ("STAGE_2_AUDIT_(START DATE)"),
                        ("STAGE_2_AUDIT_(END DATE)"),
                        ("STAGE_2_NO_OF_DAYS"),
                        ("STAGE_2_LEAD_AUDITOR"),
                        ("AUDITEE_NAME"),
                        ("AUDITEE_EMAIL"),
                        ("AUDITEE_CONTACT"),
                        ("AUDIT_CO-ORDINATOR_NAME"),
                        ("AUDIT_CO-ORDINATOR_EMAIL"),
                        ("ENTITY_DIRECTOR_NAME"),
                        ("ENTITY_DIRECTOR_EMAIL"),
                        ("AUDIT_ADMIN_(AUDIT_SERVICE_PROVIDER)"),
                        ("AUDIT_STATUS"),
                        ("CAPA_STATUS"),
                        ("AUDIT_OUTCOME-REPORT"),
                        ("ACTUAL_AUDIT_REPORT_DATE"),
                        ("CAPA_SUBMISSION_DATE"),
                        ("DATE_OF_RELEASING_1ST_REVISED"),
                        ("DATE_OF_RELEASING_2ND_REVISED"),
                        ("COMMENTS"),
                        ("Imported File Row No"),
                        ("Result")
                        );

                    AddObjects(
                        sheet, 2, exportAuditProject,
                        _ => _.FiscalYear,
                         _ => _.status,
                        _ => _.AuditId,
                        _ => _.PrimaryLicenseNumber,
                        _ => _.SecondaryLicenseNumber,
                        _ => _.EntityGroupName,
                        _ => _.PrimaryEntityName,
                        _ => _.SecondaryEntityName,
                        _ => _.FacilityName,
                        _ => _.FacilitySubTypeName,
                        _ => _.City,
                        _ => _.AuditTypeName,
                        _ => _.AuditTitle,
                        _ => _.StartDate,
                        _ => _.EndDate,
                        _ => _.AuditDuration,
                        _ => _.LeadAuditorEmail,
                        _ => _.StageStartDate,
                        _ => _.StageEndDate,
                        _ => _.StageAuditDuration,
                        _ => "",
                        _ => _.AuditeeName,
                        _ => _.AuditeeEmail,
                        _ => _.AuditeeContact,
                        _ => _.AuditCoordinatorName,
                        _ => _.AuditCoordinatorEmail,
                        _ => _.EntityDirectorName,
                        _ => _.EntityDirectorEmail,
                        _ => _.AuditManager,
                         _ => _.AuditStatus,
                         _ => _.CAPAStatus,
                         _ => _.AuditOutcomeReport,
                         _ => _.ActualAuditReport,
                         _ => _.CAPAsubmissiondate,
                         _ => _.DateofReleasing1stRevised,
                        _ => _.DateofReleasing2ndRevised,
                         _ => _.Comments,
                        _ => _.RowName,
                        _=>_.Result
                        );

                    for (var i = 0; i < 39; i++)
                    {
                        sheet.Cells.AutoFitColumns(i);
                    }
                });
        }

        public FileDto ExportInvalidCertificateToFile(List<CertificateImportExcelDto> certificateImportExcelDto)
        {
            return CreateExcelPackage(
               "InvalidCertificateData.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.Workbook.Worksheets.Add(L("InvalidCertificateData"));
                   sheet.OutLineApplyStyle = true;

                   AddHeader(
                       sheet,
                       ("LicenseNumber"),
                       ("EntityName"),
                       ("IssueDate"),
                       ("ExpireDate"),
                       ("Status"),
                       ("RowName"),
                       ("Result")
                       );

                   AddObjects(
                       sheet, 2, certificateImportExcelDto,
                       _ => _.LicenseNumber,
                       _ => _.EntityName,
                       _ => _.IssueDate,
                       _ => _.ExpireDate,
                       _ => _.Status,
                       _ => _.RowName,
                       _ => _.Result
                       );


                   for (var i = 0; i < 5; i++)
                   {
                       sheet.Cells.AutoFitColumns(i);
                   }

               });
        }
    }
}
