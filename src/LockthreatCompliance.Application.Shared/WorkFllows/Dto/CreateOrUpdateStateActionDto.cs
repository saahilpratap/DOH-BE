using Abp.Domain.Entities;
using LockthreatCompliance.WorkFlow;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.WorkFllows.Dto
{
  public  class CreateOrUpdateStateActionDto :Entity<long>
    {
        public long? StateId { get; set; }      
        public ActionType ActionType { get; set; }
        public ActionCategory ActionCategory { get; set; }
        public int ActionTime { get; set; }
        public DateTime? SetTime { get; set; }
        public ActionTimeType ActionTimeType { get; set; }
        public long TemplateId { get; set; }
        public long? ActionRecipientsId { get; set; }      
        public long? ActionCCRecipientsId { get; set; }      
        public long? ActionBCCRecipientsId { get; set; }     
        public long? ActionSMSId { get; set; }
      
    }

    public class IdAndNameDto {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class NameAndDataTypeDto
    {
        public string DataType { get; set; }
        public string Name { get; set; }
    }
    public class IdAndNameStringDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
