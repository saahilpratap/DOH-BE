using Abp.Domain.Entities;
using LockthreatCompliance.BusinessEntities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LockthreatCompliance.Extensions;
using LockthreatCompliance.Questions;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Exceptions;
using System;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.AuditProjects;

namespace LockthreatCompliance.Storage
{
    public class DocumentPath : Entity
    {
        public DocumentPath()
        {

        }
        public DocumentPath(string fileName, int? reviewId = null, int? reviewQuestionId = null, string title = null)
        {
            ReviewDataId = reviewId;
            ReviewQuestionId = reviewQuestionId;
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
        }

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? ReviewDataId { get; set; }

        public ReviewData ReviewData { get; set; }

        public int? ReviewQuestionId { get; set; }

        public ReviewQuestion ReviewQuestion { get; set; }

        public int? FindingReportId { get; set; }

        public FindingReport FindingReport { get; set; }

        public int? BusinessRiskId { get; set; }

        public BusinessRisk BusinessRisk { get; set; }

        public int? IncidentId { get; set; }

        public Incident Incident { get; set; }

        public int? ExceptionId { get; set; }

        public Exceptions.Exception Exception { get; set; }

        public int? AWPId { get; set; }

        public ExternalAssessmentAuditWorkPaper AWP { get; set; }


    }
}
