using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.TableTopExercises.Dto
{
  public  class CreateOrEditTableTopExerciseGroupDto: EntityDto<long?>
    {
        public CreateOrEditTableTopExerciseGroupDto()
        {
            TableTopExerciseGroupSection = new List<TableTopExerciseGroupSectionDto>();
        }
        public int? TenantId { get; set; }

     
        public virtual string TableTopExerciseGroupName { get; set; }

        public virtual string TableTopExerciseDescription { get; set; }

        public List<TableTopExerciseGroupSectionDto> TableTopExerciseGroupSection { get; set; }

    }

    public class TableTopExerciseGroupSectionDto: Entity
    {
        public long TableTopExerciseGroupId { get; set; }

        public virtual long TableTopExerciseSectionId { get; set; }

    }

    public class TableTopExerciseGroupSectionDtoInput : PagedAndSortedInputDto, IShouldNormalize
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


    public class GetAllTableTopExerciseGroupSectionDto : EntityDto<long>
    {
        public virtual string Code { get; set; }
        public virtual string TableTopExerciseGroupName { get; set; }

        public virtual string TableTopExerciseDescription { get; set; }
    }

    public class GetAllSectionListDto : EntityDto<long?>
    {
        public virtual string SectionName { get; set; }
    }

    public class GetAllSectionGroupListDto: EntityDto<long>
    {
        public virtual string TableTopExerciseGroupName { get; set; }
    }

}
