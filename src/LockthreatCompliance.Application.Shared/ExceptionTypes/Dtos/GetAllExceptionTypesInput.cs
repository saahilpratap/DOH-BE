using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.ExceptionTypes.Dtos
{
    public class GetAllExceptionTypesInput : PagedAndSortedResultRequestDto
    {
		public string Filter { get; set; }

		public string NameFilter { get; set; }



    }
}