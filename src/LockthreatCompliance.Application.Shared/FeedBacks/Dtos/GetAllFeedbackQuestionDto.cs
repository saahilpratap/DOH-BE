using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using LockthreatCompliance.Questions;

namespace LockthreatCompliance.FeedBacks.Dtos
{
   public class GetAllFeedbackQuestionDto : EntityDto
    {
        public int TenantId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public virtual bool Mandatory { get; set; }

    }

    public class GetFeedbackQuestionResponseList: EntityDto
    {
        public int FeedBackEntityId { get; set; }
      
        public virtual string Question { get; set; }
        public int QuestionId { get; set; }
    
        public AnswerType AnswerType { get; set; }
        public virtual string Response { get; set; }

        public List<ResponseOptions> ResponseOptions { get; set; }
        public bool Mandatory { get; set; }
    }

    public class GetFeedBackDto
    {
        public GetFeedBackDto() {
            GetFeedbackQuestionResponseList = new List<GetFeedbackQuestionResponseList>();
        }
        public List<GetFeedbackQuestionResponseList> GetFeedbackQuestionResponseList { get; set; }
        public bool flag { get; set; }
    }

    public class ResponseOptions
    {
        public string QuestionOption { get; set; }
    }


    public class FeedBackQuestionGroupWiseDto
    {
        public FeedBackQuestionGroupWiseDto() {
            QuestionResponseGroupDtos = new List<QuestionResponseGroupDto>();
        }
        public string Title { get; set; }
        public List<QuestionResponseGroupDto> QuestionResponseGroupDtos { get; set; }

    }

    public class QuestionResponseGroupDto {
        public QuestionResponseGroupDto()
        {
            EntityWithCounts = new List<EntityWithCount>();
        }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public List<EntityWithCount> EntityWithCounts { get; set; }
    }

    public class EntityWithCount
    {
        public string EntityName { get; set; }
        public string Count { get; set; }
    }


}
