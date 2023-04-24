

using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using LockthreatCompliance.Auditing.Dto;
using LockthreatCompliance.Auditing.Exporting;
using LockthreatCompliance.BusinessEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using LockthreatCompliance.Dto;
namespace LockthreatCompliance.Auditing
{
    public class AgreementAcceptanceLogAppService : LockthreatComplianceAppServiceBase, IAgreementAcceptanceLogAppService
    {
        private readonly IRepository<AssessmentAgreementResponse> _assessmentAgreementResponseRepository;

        private readonly IAuditLogListExcelExporter _auditLogListExcelExporter;
        public AgreementAcceptanceLogAppService(IRepository<AssessmentAgreementResponse> assessmentAgreementResponseRepository, IAuditLogListExcelExporter auditLogListExcelExporter)
        {
            _assessmentAgreementResponseRepository = assessmentAgreementResponseRepository;
            _auditLogListExcelExporter = auditLogListExcelExporter;
        }

        public async Task<IReadOnlyList<AgreementAcceptanceDto>> GetAll(GetAgreementAcceptanceInput input)
        {
            var fromDate = "";
            var toDate = "";
            if(input.FromDate !="")
            {
                var fromDateCheck = (DateTime.TryParse(input.FromDate, out DateTime temp));
                if(fromDateCheck == true)
                {
                    fromDate = Convert.ToDateTime(input.FromDate).ToString("yyyy-MM-dd hh:ss tt");
                }
            }
            if (input.ToDate != "")
            {
                var toDateCheck = (DateTime.TryParse(input.ToDate, out DateTime temp));
                if (toDateCheck == true)
                {
                    toDate = Convert.ToDateTime(input.ToDate).ToString("yyyy-MM-dd hh:ss tt");
                }
            }
            var acceptances = (await _assessmentAgreementResponseRepository.GetAll()
                .Include("Assessment")
                .Include("ExternalAssessment")
                .Include("User")
                .Include("BusinessEntity")
                .WhereIf(input.UserId !=0 && input.UserId != null,e=>e.UserId == input.UserId)
                .WhereIf(input.HealthCareEntityName != 0 && input.HealthCareEntityName != null, e => e.BusinessEntityId == input.HealthCareEntityName)
                .WhereIf(fromDate != "", e => e.CreationDate >= Convert.ToDateTime(fromDate) && e.CreationDate <= Convert.ToDateTime(toDate).AddDays(1))
                .Select(e => new AgreementAcceptanceDto
                {
                    AssessmentName = e.Assessment == null ? e.ExternalAssessment.Name : e.Assessment.Name,
                    Signature = e.Signature,
                    Date = e.CreationDate,
                    EntityId = e.BusinessEntity.Id.ToString(),
                    EntityName = e.BusinessEntity.CompanyName,
                    HasAccepted = e.HasAccepted,
                    Username = e.User.Name
                }).ToListAsync()).AsReadOnly();
            return acceptances;
        }

        public async Task<PagedResultDto<AgreementAcceptanceDto>> GetAllAgreementAcceptLog(GetAgreementAcceptanceInput input)
        {
            try
            {
                

                var fromDate = "";
                var toDate = "";
                if (input.FromDate != "")
                {
                    var fromDateCheck = (DateTime.TryParse(input.FromDate, out DateTime temp));
                    if (fromDateCheck == true)
                    {
                        fromDate = Convert.ToDateTime(input.FromDate).ToString("yyyy-MM-dd hh:ss tt");
                    }
                }
                if (input.ToDate != "")
                {
                    var toDateCheck = (DateTime.TryParse(input.ToDate, out DateTime temp));
                    if (toDateCheck == true)
                    {
                        toDate = Convert.ToDateTime(input.ToDate).ToString("yyyy-MM-dd hh:ss tt");
                    }
                }
               var query =  _assessmentAgreementResponseRepository.GetAll()
                    .Include("Assessment")
                    .Include("ExternalAssessment")
                    .Include("User")
                    .Include("BusinessEntity")
                    .WhereIf(input.UserId != 0 && input.UserId != null, e => e.UserId == input.UserId)
                    .WhereIf(input.HealthCareEntityName != 0 && input.HealthCareEntityName != null, e => e.BusinessEntityId == input.HealthCareEntityName)
                    .WhereIf(fromDate != "", e => e.CreationDate >= Convert.ToDateTime(fromDate) && e.CreationDate <= Convert.ToDateTime(toDate).AddDays(1));


                var auditCount = await query.CountAsync();

                var auditItems = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var auditListDtos = auditItems.Select(e => new AgreementAcceptanceDto
                {
                    Id=e.Id,
                    AssessmentName = e.Assessment == null ? e.ExternalAssessment.Name : e.Assessment.Info,
                    Signature = e.Signature,
                    Date = e.CreationDate,
                    EntityId = "ENT-"+e.BusinessEntity.Id.ToString(),
                    EntityName = e.BusinessEntity.CompanyName,
                    HasAccepted = e.HasAccepted,
                    Username = e.User.Name
                });

                return new PagedResultDto<AgreementAcceptanceDto>(
                   auditCount,
                   auditListDtos.OrderByDescending(x => x.Id).ToList()
                   );

                

            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<FileDto> GetAggrementLogToExcel(GetAgreementInput input)
        {


            var fromDate = "";
            var toDate = "";
            if (input.FromDate != "")
            {
                var fromDateCheck = (DateTime.TryParse(input.FromDate, out DateTime temp));
                if (fromDateCheck == true)
                {
                    fromDate = Convert.ToDateTime(input.FromDate).ToString("yyyy-MM-dd hh:ss tt");
                }
            }
            if (input.ToDate != "")
            {
                var toDateCheck = (DateTime.TryParse(input.ToDate, out DateTime temp));
                if (toDateCheck == true)
                {
                    toDate = Convert.ToDateTime(input.ToDate).ToString("yyyy-MM-dd hh:ss tt");
                }
            }
            var acceptanceListDtos = new List<AgreementAcceptanceDto>();

            var acceptances = (_assessmentAgreementResponseRepository.GetAll()
               .Include("Assessment")
               .Include("ExternalAssessment")
               .Include("User")
               .Include("BusinessEntity")
               .WhereIf(input.UserId != 0 && input.UserId != null, e => e.UserId == input.UserId)
               .WhereIf(input.HealthCareEntityName != 0 && input.HealthCareEntityName != null, e => e.BusinessEntityId == input.HealthCareEntityName)
               .WhereIf(fromDate != "", e => e.CreationDate >= Convert.ToDateTime(fromDate) && e.CreationDate <= Convert.ToDateTime(toDate).AddDays(1)));

            var query = (from e in acceptances
                         select new AgreementAcceptanceDto()
                         {
                             AssessmentName = e.Assessment == null ? e.ExternalAssessment.Name : e.Assessment.Name,
                             Signature = e.Signature,
                             Date = e.CreationDate,
                             EntityId = e.BusinessEntity.Id.ToString(),
                             EntityName = e.BusinessEntity.CompanyName,
                             HasAccepted = e.HasAccepted,
                             Username = e.User.Name
                         });

             
             acceptanceListDtos = await query.ToListAsync();

            return _auditLogListExcelExporter.ExportAcceptanceLogToFile(acceptanceListDtos);
        }
    }
}
