using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.QuestResponses
{
 public class QuestResponseAttachment : FullAuditedEntity
    {

        public int QuestResponseId  { get; set; }
        public QuestResponse QuestResponse { get; set; }
        public string FileName { get; set; }      
        public string Code { get; set; }

    }
}
