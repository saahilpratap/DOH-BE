using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises.Dto
{
   public class CreateOrEditTableTopExerciseSectionDto : Entity<long>
    {
        public CreateOrEditTableTopExerciseSectionDto()
        {
            TableTopExerciseSectionQuestions = new List<TableTopExerciseSectionQuestionDto>();
            TableTopExerciseSectionAttachement = new List<TableTopExerciseSectionAttachementDto>();
        }
        public int? TenantId { get; set; }

        public virtual string SectionName { get; set; }
        public virtual TimeSpan? CounterLimit { get; set; }

        public virtual List<TableTopExerciseSectionQuestionDto> TableTopExerciseSectionQuestions { get; set; }

       public virtual List<TableTopExerciseSectionAttachementDto> TableTopExerciseSectionAttachement  { get; set; }
    }

    public class TableTopExerciseSectionQuestionDto: Entity<long>
    {
       
        public TableTopExerciseSectionQuestionDto()
        {

        }
        public int? TenantId { get; set; }
        public long TableTopExerciseQuestionId { get; set; }

        public virtual long TableTopExerciseSectionId { get; set; }

    }

    public class TableTopExerciseSectionAttachementDto : Entity<long>
    {
        public TableTopExerciseSectionAttachementDto()
        {

        }
      
        public virtual string FileName { get; set; }
        public virtual string Title { get; set; }
        public virtual string Code { get; set; }
        public virtual string FullPath { get; set; }
        public virtual long TableTopExerciseSectionId { get; set; }
    }

    public class GetAllTableTopExerciseSectionDto: Entity<long>
    {
        public int? TenantId { get; set; }
        public virtual string Code { get; set; }
        public virtual string SectionName { get; set; }

    }

    public class GetTableTopExerciseSectionInput : PagedAndSortedInputDto, IShouldNormalize
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


    public class GetQuestionListDto: Entity<long>
    {
        public virtual string Name { get; set; }
    }

}
