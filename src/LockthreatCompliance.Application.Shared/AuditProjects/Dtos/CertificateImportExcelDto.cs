using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class CertificateImportExcelDto
    {
        public string LicenseNumber { get; set; }
        public string EntityName { get; set; }
        public string IssueDate { get; set; }
        public string ExpireDate { get; set; }
        public string Status { get; set; }
        public bool IsLicenseNumber { get; set; }
        public bool IsIssueDate { get; set; }
        public bool IsExpireDate { get; set; }
        public bool IsStatus { get; set; }
        public string RowName { get; set; }
        public string Result { get; set; }
        public bool IdBeImported { get; set; }
    }
}
