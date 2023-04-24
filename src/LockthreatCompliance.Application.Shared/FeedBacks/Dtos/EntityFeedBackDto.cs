using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FeedBacks.Dtos
{
   public class EntityFeedBackDto : EntityDto
    {
        public EntityFeedBackDto()
        {
            FeedBackEntityResponses = new List<FeedBackEntityResponseDto>();
        }
        public int? TenantId { get; set; }

       
        public virtual string Code { get; set; }

        public int FeedbackDetailId { get; set; }

        public virtual String FeedbackName { get; set; }
        public int? BusinessEntityId { get; set; }

        public virtual string BusinessEntityName { get; set; }
       
        public virtual List<FeedBackEntityResponseDto> FeedBackEntityResponses { get; set; }
    }

    public class FeedBackEntityResponseDto: EntityDto
    {
        public int FeedBackEntityId { get; set; }
   
        public int QuestionId { get; set; }
    
        public virtual string Response { get; set; }
    }

   
}
