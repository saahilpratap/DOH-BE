using Abp.Application.Services.Dto;
using LockthreatCompliance.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FindingReports.Dtos
{
    public class FindingReportDto : EntityDto
    {
        public int? TenantId { get; set; }
       
        public string BusinessEntityName { get; set; }
        public string Code { get; set; }

        public string Title { get; set; }

        public bool IsDeleted { get; set; }
        public string ClassificationName { get; set; }

        public CAPAStatus FindingCAPAStatus { get; set; }
        public int? FindingStatusId { get; set; }

        public FindingReportAction FindingAction { get; set; }

        public FindingReportStatus Status { get; set;}
        public string ReviewComment { get; set; }

        public bool CAPAUpdateRequired { get; set; }

        public DateTime? FindingCloseDate { get; set; }

        public int? ExternalAssessmentId { get; set; }

        public ReviewDataResponseType ExternalAssessmentResponseType { get; set; }

    }

    public class FindingListAuditDto: FindingReportDto
    {
        public int EntityId { get; set; }
        public string Type { get; set; }
        public string status { get; set; }
    }


    public class FindingStatusWiesShowBtnDto
    {
        public FindingStatusWiesShowBtnDto()
        {
            CapaAccepted = false;
            CapaApproved = false;
        }
        public bool CapaAccepted { get; set; }
        public bool CapaApproved { get; set; }
    }

    public class AuditStausWiesShowButton
    {
        public AuditStausWiesShowButton()
        {
            SubmitCapa = false;
            FinicalCapaSubmit = false;
        }
        public bool SubmitCapa  { get; set; }
        public bool FinicalCapaSubmit { get; set; }

       
    }

    public class AllclosedCapaDto
    {
        public long AuditProjectId { get; set; }
        public FindingReportCategory category { get; set; }
        public List<int> FindingIds { get; set; }
    }

}
