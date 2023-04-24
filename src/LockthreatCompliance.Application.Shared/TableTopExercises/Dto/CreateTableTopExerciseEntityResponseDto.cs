using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FeedBacks.Dtos;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises.Dto
{
    public class CreateTableTopExerciseEntityResponseDto : EntityDto<long?>
    {
        public CreateTableTopExerciseEntityResponseDto()
        {

        }
        public long TableTopExerciseEntityId { get; set; }
        public virtual long TableTopExerciseSectionId { get; set; }
        public long TableTopExerciseQuestionId { get; set; }
        public virtual string QuestionComment { get; set; }
        public virtual bool CommentRequired { get; set; }
        public virtual bool CommentMandatory { get; set; }
        public AnswerType AnswerType { get; set; }
        public virtual string Response { get; set; }
    }

    public class GetTTEEntityReponsesDto
    {
        public GetTTEEntityReponsesDto()
        {
            TableTopExerciseEntityResponses = new List<TableTopExerciseEntityResponseDto>();
            TableTopExerciseEntityAttachments = new List<TableTopExerciseEntityAttachmentDto>();
            SectionAttachmentQuestions = new List<SectionAttachmentQuestion>();
            Submitted = false;
        }
        public virtual bool Submitted { get; set; }

        public string GroupName { get; set; }
        public string GroupDescription { get; set; }

        public virtual List<TableTopExerciseEntityResponseDto> TableTopExerciseEntityResponses { get; set; }
        public virtual List<TableTopExerciseEntityAttachmentDto> TableTopExerciseEntityAttachments { get; set; }
        public virtual List<SectionAttachmentQuestion> SectionAttachmentQuestions { get; set; }

    }

    public class TableTopExerciseEntityResponseDto : EntityDto<long?>
    {
        public TableTopExerciseEntityResponseDto()
        {
            ResponseOptions = new List<CodeNameDto>();
            MultiResponse = new List<CodeNameDto>();
            TableTopExerciseSectionAttachements = new List<TableTopExerciseSectionAttachementDto>();
        }
        public long TableTopExerciseEntityId { get; set; }
        public virtual long TableTopExerciseSectionId { get; set; }
        public long? TableTopExerciseQuestionId { get; set; }
        public virtual string QuestionComment { get; set; }
        public virtual bool QuestionMandatory { get; set; }
        public virtual bool CommentRequired { get; set; }
        public virtual bool CommentMandatory { get; set; }
        public AnswerType AnswerType { get; set; }
        public virtual string Response { get; set; }
        public virtual string QuestionName { get; set; }
        public virtual string SectionName { get; set; }
        public virtual TimeSpan? CounterLimit { get; set; }
        public List<CodeNameDto> ResponseOptions { get; set; }
        public List<CodeNameDto> MultiResponse { get; set; }
        public List<TableTopExerciseSectionAttachementDto> TableTopExerciseSectionAttachements { get; set; }

    }

    public class CodeNameDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }

    }

    public class TableTopExerciseEntityAttachmentDto : EntityDto<long?>
    {
        public long TableTopExerciseEntityId { get; set; }
        public virtual string FileName { get; set; }
        public virtual string Title { get; set; }
        public virtual string Code { get; set; }

    }

    public class GetResponseDto
    {
        public GetResponseDto()
        {
            SectionAttachmentQuestions = new List<SectionAttachmentQuestion>();
            TableTopExerciseEntityAttachments = new List<TableTopExerciseEntityAttachmentDto>();
        }
        public virtual List<TableTopExerciseEntityAttachmentDto> TableTopExerciseEntityAttachments { get; set; }
        public virtual List<SectionAttachmentQuestion> SectionAttachmentQuestions { get; set; }

    }

    public class SectionAttachmentQuestion
    {
        public SectionAttachmentQuestion()
        {
            TableTopExerciseEntityResponses = new List<TableTopExerciseEntityResponseDto>();
            TableTopExerciseSectionAttachements = new List<TableTopExerciseSectionAttachementDto>();
        }
        public virtual string SectionName { get; set; }
        public virtual TimeSpan? CounterLimit { get; set; }

        public virtual List<TableTopExerciseEntityResponseDto> TableTopExerciseEntityResponses { get; set; }
        public List<TableTopExerciseSectionAttachementDto> TableTopExerciseSectionAttachements { get; set; }

    }


    public class TabletopExerciseEntityList : EntityDto<long>
    {
        public virtual string TableTopExerciseGroupName { get; set; }

        public virtual string Code { get; set; }

        public virtual string EntityName { get; set; }

        public virtual string LicenseNumber { get; set; }

        public virtual bool Submitted { get; set; }



    }

    public class TableTopExerciseEntityDtoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }

            Filter = Filter?.Trim();
        }
    }

}
