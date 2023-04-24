using Abp.DynamicEntityParameters;
using LockthreatCompliance.DynamicEntityParameters.Dto;
using LockthreatCompliance.EntityGroups.Dtos;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FindingReports.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class ExportAuditProject
    {
        public long? Id { get; set; }
        public int? TenantId { get; set; }
        public virtual string Code { get; set; }
        public string PrimaryLicenseNumber { get; set; }
        public string PrimaryEntityName { get; set; }

        public string AuditeeName { get; set; }
        public string EntityDirectorName { get; set; }
        public string EntityDirectorEmail { get; set; }

        public string AuditeeContact { get; set; }

        public string AuditeeEmail { get; set; }

        public string FacilityName { get; set; }

        public string FacilitySubTypeName { get; set; }

        public string AuditTitle { get; set; }

        public string FiscalYear { get; set; }


        public string AuditTypeName { get; set; }

        public string AuditCoordinatorName { get; set; }
        public string AuditCoordinatorEmail { get; set; }
        public string EntityGroupName { get; set; }
        public string LeadAuditorEmail { get; set; }


        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string StageStartDate { get; set; }

        public string StageEndDate { get; set; }

        public string StageAuditDuration { get; set; }

        public string AuditDuration { get; set; }

        public string ActualAuditReport { get; set; }
        public string CAPAsubmissiondate { get; set; }
        public string DateofReleasing1stRevised { get; set; }
        public string DateofReleasing2ndRevised { get; set; }
        public string AuditStatus { get; set; }
        public string CAPAStatus { get; set; }
        public string AuditOutcomeReport { get; set; }
        public string Comments { get; set; }

        public string City { get; set; }

        public EntityType EntityType { get; set; }

        public Boolean IdBeImported { get; set; }

        public Boolean LicenseBeImported { get; set; }

        public Boolean StartDateBeImported { get; set; }

        public Boolean EndDateBeImported { get; set; }

        public string RowName { get; set; }

        public bool IsLeadEmailCheck { get; set; }

        public string Exception { get; set; }

        public long LeadAuditorId { get; set; }

        public string AuditId { get; set; }

        public string AuditManager { get; set; }

        public string Result { get; set; }

        public bool IsAuditDurationCheck { get; set; }

        public bool IsStageAuditDurationCheck { get; set; }

        public bool IsStageStartDateCheck { get; set; }

        public bool IsStageEndDateCheck { get; set; }

        public string status { get; set; }

        public string SecondaryLicenseNumber { get; set; }
        public string SecondaryEntityName { get; set; }

        public string Remarks { get; set; }
        public string PaymentDetails { get; set; }
        public string OutcomeReportReleasedDate { get; set; }
        public string ReAuditScoreOne { get; set; }
        public string ReAuditScoreTwo { get; set; }
        public string  DateofReleasingReauditOne { get; set; }
        public string DateofReleasingReauditTwo { get; set; }
        public string OverallStatus { get; set; }
        public string DaysTimeline { get; set; }
        public string EvidenceSubmTimeiClosed { get; set; } 
        public string EvidenceStatus { get; set; }
        public string EvidenceSharedDateOne { get; set; }
        public string EvidenceSharedDateTwo { get; set; }

    }
}
