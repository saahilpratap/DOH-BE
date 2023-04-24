using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using LockthreatCompliance.Dto;
using System;

namespace LockthreatCompliance.AuthoritativeDocuments.Dtos
{
    public class GetAllAuthoritativeDocumentsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filter { get; set; }

        public string AuthDocuID { get; set; }

        public string AuthDocuName { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id,Name";
            }

            Filter = Filter?.Trim();
        }

    }
}