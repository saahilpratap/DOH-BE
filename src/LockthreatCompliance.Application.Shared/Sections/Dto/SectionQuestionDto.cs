using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Sections.Dto
{
  public  class SectionQuestionDto : FullAuditedEntityDto<long>
    {
        public int? TenantId { get; set; }

        public string Name { get; set; }

       public List<SectionRelatedQuestionDto> SectionQuestions  { get; set; }

    }
}
