using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LockthreatCompliance.Web.Models.File
{
    public class UploadAuditQuesResFileOutput
    {
        public string FileName { get; set; }

        public string Code { get; set; }

        public long? AuditProjectId { get; set; }

        public long? QuestionId { get; set; }
    }
}
