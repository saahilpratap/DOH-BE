using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Questions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises.Dto
{
   public class CreateOrEditTableTopExerciseQuestionDto: EntityDto<long?>
    {
        public  CreateOrEditTableTopExerciseQuestionDto()
         {
            TableTopExerciseQuestionOption = new List<TableTopExerciseQuestionOptionDto>();
         }

        public int? TenantId { get; set; }
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Mandatory { get; set; }

        public virtual bool CommentRequired { get; set; }
        public virtual bool CommentMandatory { get; set; }

        public AnswerType AnswerType { get; set; }

       public List<TableTopExerciseQuestionOptionDto> TableTopExerciseQuestionOption { get; set; }

    }

   public class TableTopExerciseQuestionOptionDto : EntityDto
    {
        public  TableTopExerciseQuestionOptionDto()
          {

          }
        public long TableTopExerciseQuestionId { get; set; }
        public string Value { get; set; }
        public double Score { get; set; }
    }

  public class GetTableTopExerciseQuestionInput: PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id,Name,Description";
            }

            Filter = Filter?.Trim();
        }
    }

   public class GetTableTopExerciseQuestionDto  : EntityDto<long>
    {
        public int TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual bool Mandatory { get; set; }
        public virtual bool CommentMandatory { get; set; }

    }

    public class CreateTTXEntityRequestDto
    {
        public CreateTTXEntityRequestDto()
        {
            BusinessEntityId = new List<int>();
        }
        public List<int> BusinessEntityId { get; set; }
        public long TableTopExerciseGroupId { get; set; }
        public bool EmailSendStatus { get; set; }

    }



    public  class GetAllGroupListDto: EntityDto<long>
    {
        public virtual string TableTopExerciseGroupName { get; set; }
    }
     


}
