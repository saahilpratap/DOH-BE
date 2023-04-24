using Abp.Application.Services.Dto;
using System;

namespace LockthreatCompliance.AuthoritativeDocuments.Dtos
{
    public class GetAllAuthoritativeDocumentsForExcelInput
    {
		public string Filter { get; set; }

		public string CodeFilter { get; set; }

		public string NameFilter { get; set; }



    }
}