using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.AuthoritityDepartments.Dtos
{
    public class GetAuthorityDepartmentForEditOutput
    {
		public CreateOrEditAuthorityDepartmentDto AuthorityDepartment { get; set; }


    }
}