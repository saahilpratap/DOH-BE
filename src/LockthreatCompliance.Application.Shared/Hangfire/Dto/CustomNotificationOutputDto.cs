using LockthreatCompliance.BusinessEntities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Hangfire.Dto
{
   public class CustomNotificationOutputDto
    {
        public CustomNotificationOutputDto() {
            ToEmailId = new List<string>();
        }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<string> ToEmailId { get; set; }

        public string FileJson { get; set; }
    }

    public class AuditProjectBusinessEntityDto {
       
        public long AuditProjectId { get; set; }
        public int ExternalAssessmentId { get; set; }
        public BusinessEntityDto BusinessEntities { get; set; }
    }

    public class CustomNotificationOutputNewDto : CustomNotificationOutputDto
    { 
        public CustomNotificationOutputNewDto()
        {
            CcEmailId = new List<string>();
            BccEmailId = new List<string>();
        }
        public List<string> CcEmailId { get; set; }
        public List<string> BccEmailId { get; set; }
        public string Type { get; set; }
    }

    public class HHMMDDDto {

        public HHMMDDDto() {
            Day = 0;
            Min = 0;
            Hr = 0;
        }
        public int Day { get; set; }
        public int Min { get; set; }
        public int Hr { get; set; }
    }

}
