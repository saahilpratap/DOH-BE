using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Abp.Domain.Entities.Auditing;
using LockthreatCompliance.Extensions;

namespace LockthreatCompliance.RemediationPlans
{
    [Table("RemediationDocuments")]
    public class RemediationDocument : FullAuditedEntity
    {
        public RemediationDocument()
        {

        }
        public RemediationDocument(string fileName, string title = null)
        {
            Code = Guid.NewGuid().ToString() + "." + fileName.ReverseChars().GetUntil('.').ReverseChars();
            FileName = fileName;
            Title = title;
        }

        public string FileName { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public int? RemediationId { get; set; }
        public Remediation Remediation { get; set; }

    }
}
