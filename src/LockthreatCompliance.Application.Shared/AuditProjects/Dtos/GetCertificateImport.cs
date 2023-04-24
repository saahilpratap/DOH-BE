using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuditProjects.Dtos
{
   public class GetCertificateImport : EntityDto
    {
        public string Code { get; set; }
        public string LicenseNumber { get; set; }
        public string EntityName { get; set; }
        public string IssueDate { get; set; }
        public string ExpireDate { get; set; }
        public string Status { get; set; }
        public string Qrcode { get; set; }
        public string FileName { get; set; }
    }
}
