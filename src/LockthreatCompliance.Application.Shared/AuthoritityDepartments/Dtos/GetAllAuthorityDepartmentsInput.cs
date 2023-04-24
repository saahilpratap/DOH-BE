using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.AuthoritityDepartments.Dtos
{
    public class GetAllAuthorityDepartmentsInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}