using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LockthreatCompliance.FindingReports
{
    public class FindingReportLog : FullAuditedEntity
    {
        public int? TenantId { get; set; }
        public int FindingId { get; set; }
        public string UpdatedFieldName { get; set; }
        public bool CreateOrEditFlag { get; set; }

        public string Old_Title { get; set; }
        public string Old_Category { get; set; }
        public DateTime? Old_DateFound { get; set; }
        public string Old_FindingDetails { get; set; }
        public string Old_Reference { get; set; }
        public DateTime? Old_ActionResponseDate { get; set; }
        public DateTime? Old_DateClosed { get; set; }
        public string Old_CorrectiveActionResponse { get; set; }
        public string Old_ActualRootCause { get; set; }
        public string Old_Status { get; set; }
        public string Old_AuditorRemarks { get; set; }
        public bool Old_CAPAUpdateRequired { get; set; }
        public string Old_Attachment { get; set; }

        public string New_Title { get; set; }
        public string New_Category { get; set; }
        public DateTime? New_DateFound { get; set; }
        public string New_FindingDetails { get; set; }
        public string New_Reference { get; set; }
        public DateTime? New_ActionResponseDate { get; set; }
        public DateTime? New_DateClosed { get; set; }
        public string New_CorrectiveActionResponse { get; set; }
        public string New_ActualRootCause { get; set; }
        public string New_Status { get; set; }
        public string New_AuditorRemarks { get; set; }
        public bool New_CAPAUpdateRequired { get; set; }
        public string New_Attachment { get; set; }
        public string UserName { get; set; }
    }

}
