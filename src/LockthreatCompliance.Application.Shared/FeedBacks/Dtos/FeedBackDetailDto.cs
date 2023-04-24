using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.FeedBacks.Dtos
{
    public class FeedBackDetailDto : EntityDto
    {
        public FeedBackDetailDto()
        {

        }

        public int? TenantId { get; set; }

        public virtual string Code { get; set; }
        public virtual string Title { get; set; }

        public virtual DateTime? ActionDate { get; set; }

        public int LinkValidationDay { get; set; }

        public List<FeedbackQuestionDto> FeedbackDetailQuestions { get; set; }

    }

    public class FeedbackQuestionDto : EntityDto
    {
        public FeedbackQuestionDto()
        {
            QuestionSrNo = 0;
        }
        public int QuestionId { get; set; }
        public virtual string Description { get; set; }
        public int FeedbackDetailId { get; set; }
        public int QuestionSrNo { get; set; }

    }

    public class BusinessEntityFeedbackIds
    {
        public BusinessEntityFeedbackIds()
        {
            BusinessEntityId = new List<int>();
        }
        public List<int> BusinessEntityId { get; set; }
        public int FeedbackDetailId { get; set; }

        public bool EmailSendStatus { get; set; }

    }

    public class FeedbackListDto : EntityDto
    {
        public virtual string Title { get; set; }
    }

    public class DashboardFeedbackDto
    {
        public DashboardFeedbackDto()
        {
            TotalResponse = 0;
            PendingResponse = 0;
            ReceiveResponse = 0;
            FeedBankDetailsInfo = new List<FeedBackDetailDto>();
        }
        public virtual int TotalResponse { get; set; }
        public virtual int PendingResponse { get; set; }
        public virtual int ReceiveResponse { get; set; }
        public virtual List<FeedBackDetailDto> FeedBankDetailsInfo { get; set; }

    }

    public class QuestionChartDto
    {
        public QuestionChartDto()
        {
            QuestionResponses = new List<QuestionResponseDto>();
        }
        public virtual int QuestionId { get; set; }
        public virtual string QuestionName { get; set; }
        public virtual List<QuestionResponseDto> QuestionResponses { get; set; }
    }

    public class QuestionResponseDto
    {
        public virtual string Answer { get; set; }
        public virtual int Count { get; set; }
    }

    public class FeedBackExcelDto
    {
        public virtual string QuestionName { get; set; }
        public virtual string OptionWithCount { get; set; }

    }



}
