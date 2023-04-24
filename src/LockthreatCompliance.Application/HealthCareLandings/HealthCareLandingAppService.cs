using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuditDashBoard.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.HealthCareLandings.Dto;
using LockthreatCompliance.Incidents;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.ControlStandards;
using System;
using LockthreatCompliance.Questions;
using Twilio.Rest;
using LockthreatCompliance.Domains;
using Domain = LockthreatCompliance.Domains.Domain;
using Twilio.TwiML.Messaging;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.Common;

namespace LockthreatCompliance.HealthCareLandings
{
    public class HealthCareLandingAppService : LockthreatComplianceAppServiceBase, IHealthCareLandingAppService
    {
        private readonly IRepository<ControlRequirement> _controlRequirementRepository;
        private readonly IRepository<AuthoritativeDocument> _authoritativeDocumentRepository;
        private readonly IRepository<ControlStandard> _controlStandardRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<FindingReport> _findingReportRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<Exceptions.Exception> _exceptionRepository;
        private readonly IRepository<Incident> _incidentRepository;
        private readonly IRepository<BusinessRisk> _businessRiskRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<QuestionGroup,long> _questionGroupRepository;
        private readonly IRepository<Domain> _domainRepository;
        private readonly IRepository<ExternalAssessmentQuestion> _externalAssessmentquestionRepository;
        private readonly IRepository<Question> _questionRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;

        public HealthCareLandingAppService(IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<QuestionGroup, long> questionGroupRepository,
            IRepository<AuthoritativeDocument> authoritativeDocumentRepository,
            IRepository<ExternalAssessmentQuestion> externalAssessmentquestionRepository,
            IRepository<Question> questionRepository,
            IRepository<Domain> domainRepository,
            IRepository<ControlRequirement> controlRequirementRepository, IRepository<ControlStandard> controlStandardRepository,
            IRepository<Exceptions.Exception> exceptionRepository,
         IRepository<Incident> incidentRepository, IRepository<BusinessRisk> businessRiskRepository, IRepository<BusinessEntity> businessEntityRepository,
         IRepository<FindingReport> findingReportRepository, IRepository<Assessment> assessmentRepository, ICommonLookupAppService commonlookupManagerRepository
          )
        {
            _authoritativeDocumentRepository = authoritativeDocumentRepository;
            _externalAssessmentquestionRepository = externalAssessmentquestionRepository;
            _questionRepository = questionRepository;
            _domainRepository = domainRepository;
            _questionGroupRepository = questionGroupRepository;
            _controlRequirementRepository = controlRequirementRepository;
            _controlStandardRepository = controlStandardRepository;
            _assessmentRepository = assessmentRepository;
            _businessEntityRepository = businessEntityRepository;
            _exceptionRepository = exceptionRepository;
            _incidentRepository = incidentRepository;
            _businessRiskRepository = businessRiskRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _findingReportRepository = findingReportRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
        }

        public async Task<ComplianceDashboardDto> GetComplianceDashboard()
        {
            try
            {

                var query = new ComplianceDashboardDto();
                query.ControlRequiremnentCount = _controlRequirementRepository.GetAll().Count();
                query.DomianCount = _authoritativeDocumentRepository.GetAll().Count();
                query.QuestionGroupCount = _questionGroupRepository.GetAll().Count();
                query.SelfAssessmentQuestionCount = _questionRepository.GetAll().Count();
                query.ExternalAsstQuestionCount=_externalAssessmentquestionRepository.GetAll().Count();
                return query;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<HealthcarelandingDto> GetHealthCareLandingDashBoard(int? BusinessEntityId)
        {
            try
            {
                BusinessEntityId = BusinessEntityId == 0 ? null : BusinessEntityId;
                var result = new HealthcarelandingDto();
                var incident = new List<IncidentExceptionBusinessRiskCountDto>();
                var getBusinessrisk = new List<IncidentExceptionBusinessRiskCountDto>();
                var getexceptions = new List<IncidentExceptionBusinessRiskCountDto>();
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                result.FindingCount = _findingReportRepository.GetAll().Where(x => x.Type == FindingReportType.Internal)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                result.ExternalAuditFindingCount = _findingReportRepository.GetAll().Where(x => x.Type == FindingReportType.External)
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                result.InternalAssessmentCount = _assessmentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                result.ExternalAssessmentCount = _externalAssessmentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                var getincident = _incidentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                var getexception = _exceptionRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                var getbusinessRisk = _businessRiskRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                result.IncidentExceptionFindingCount.Add(new IncidentExceptionFindingDto()
                {
                    Name = "Incident",
                    Value = getincident
                });
                result.IncidentExceptionFindingCount.Add(new IncidentExceptionFindingDto()
                {
                    Name = "Exception",
                    Value = getexception
                });
                result.IncidentExceptionFindingCount.Add(new IncidentExceptionFindingDto()
                {
                    Name = "Business Risk",
                    Value = getbusinessRisk
                });
                result.AssementTypeCount = await _externalAssessmentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).GroupBy(x => x.Type).
                                                    Select(x => new AssementTypeCountDto
                                                    {
                                                        Name = x.Key.ToString(),
                                                        Value = x.ToList().Count(),
                                                        Label = x.Key.ToString()
                                                    }).ToListAsync();
                getBusinessrisk = _businessRiskRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToLookup(x => x.IdentificationDate.Value.Year).
                                       Select(x => new IncidentExceptionBusinessRiskCountDto()
                                        {
                                            Name = x.Key.ToString(),
                                           Series = x.ToList().ToLookup(x => x.IdentificationDate.Value.Year).Select(y => new AssementItemsFiscalYearDto
                                            {
                                                Name = "Business Risk",
                                                Value = x.Count()
                                            }).ToList(),

                                        }).ToList();

                incident = _incidentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToLookup(x => x.CreationTime.Year).
                Select(x => new IncidentExceptionBusinessRiskCountDto()
                {
                    Name = x.Key.ToString(),
                    Series = x.ToList().ToLookup(x => x.CreationTime.Year).Select(y => new AssementItemsFiscalYearDto
                    {
                        Name = "Incident",
                        Value = x.Count()
                    }).ToList(),

                }).ToList();


                getexceptions = _exceptionRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToLookup(x => x.CreationTime.Year).
                          Select(x => new IncidentExceptionBusinessRiskCountDto()
                          {
                              Name = x.Key.ToString(),
                              Series = x.ToList().ToLookup(x => x.CreationTime.Year).Select(y => new AssementItemsFiscalYearDto
                              {
                                  Name = "Exception",
                                  Value = x.Count()
                              }).ToList(),
                          }).ToList();

                var keylist = getBusinessrisk.Select(x => x.Name).Concat(incident.Select(x => x.Name)).Concat(getexceptions.Select(x => x.Name)).Distinct().ToList();
                keylist.ForEach(obj =>
                {
                    IncidentExceptionBusinessRiskCountDto query = new IncidentExceptionBusinessRiskCountDto();
                    query.Series = new List<AssementItemsFiscalYearDto>();
                    query.Name = obj;
                    var getcount = (getBusinessrisk.Where(x => x.Name == obj).Select(x => x.Series).
                                  Concat(incident.Where(x => x.Name == obj).Select(x => x.Series)).
                                  Concat(getexceptions.Where(x => x.Name == obj).Select(x => x.Series))).
                                  ToList();

                    getcount.ForEach(o =>
                    {
                        query.Series.AddRange(o);
                    });
                    result.IncidentExceptionBusinessRiskCount.Add(query);
                });

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AssementTypeCountDto>> GetDashboardBusinessIncidentRisk(int? BusinessEntityId)
        {
            var result = new List<AssementTypeCountDto>();
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                BusinessEntityId = BusinessEntityId == 0 ? null : BusinessEntityId;
                var getincident = _incidentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                var getexception = _exceptionRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                var getbusinessRisk = _businessRiskRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .WhereIf(BusinessEntityId != null, x => x.BusinessEntityId == BusinessEntityId).ToList().Count();
                result.Add(new AssementTypeCountDto()
                {
                    Name = "Incident",
                    Value = getincident,

                });
                result.Add(new AssementTypeCountDto()
                {
                    Name = "Exception",
                    Value = getexception
                });
                result.Add(new AssementTypeCountDto()
                {
                    Name = "Business Risk",
                    Value = getbusinessRisk
                });


                return result;

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<ADControlrequirementCountDto>> GetAllQuestionCountByDomain()
        {
            var query = new List<ADControlrequirementCountDto>();

            var controlRequirement = await _controlRequirementRepository.GetAll().Include(r => r.RequirementQuestions)
                .Include(c => c.ControlStandard).Include(c => c.ControlStandard.Domain).ToListAsync();

            query = (from b in controlRequirement.ToLookup(x => x.ControlStandard.DomainId)
                     select new ADControlrequirementCountDto()
                     {
                         Percentage = b.Count(),
                         Colors = "",
                         Name = b.FirstOrDefault().DomainName,
                     }).ToList();
            return query;
        }

        public async Task<List<IRMSummaryDetailDto>> GetAllIRMSummary(IncidentExceptionInputDto input)
        {
            var result = new List<IRMSummaryDetailDto>();
            var incident = new List<IRMSummaryDetailDto>();
            var exception = new List<IRMSummaryDetailDto>();
            var finding = new List<IRMSummaryDetailDto>();
            var businessRisk = new List<IRMSummaryDetailDto>();

            try
            {

                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                exception = _exceptionRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId))
                    .Where(x => x.CreationTime.Date >= DateTime.Parse(input.StartDate.ToString()).Date && x.CreationTime.Date <= DateTime.Parse(input.EndDate.ToString()).Date).
                    ToLookup(x => x.CreationTime.Date).
                          Select(x => new IRMSummaryDetailDto()
                          {
                              Name = "Exception",
                              Series = x.ToList().ToLookup(x => x.CreationTime.Date).Select(y => new IRMSummaryDto
                              {
                                  Value = x.Count(),
                                  Name = x.FirstOrDefault().CreationTime.ToString("dd-MM-yyyy")
                              }).ToList(),
                          }).ToList();

                incident = _incidentRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .Where(x => x.DetectionDateTime.Date >= DateTime.Parse(input.StartDate.ToString()).Date && x.DetectionDateTime.Date <= DateTime.Parse(input.EndDate.ToString()).Date).
                             ToLookup(x => x.DetectionDateTime.Date).
                             Select(x => new IRMSummaryDetailDto()
                             {
                                 Name = "Incident",
                                 Series = x.ToList().ToLookup(x => x.DetectionDateTime.Date).Select(y => new IRMSummaryDto
                                 {
                                     Value = x.Count(),
                                     Name = x.FirstOrDefault().DetectionDateTime.ToString("dd-MM-yyyy")

                                 }).ToList(),
                             }).ToList();

                businessRisk = _businessRiskRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .Where(x => x.CreationTime.Date >= DateTime.Parse(input.StartDate.ToString()).Date && x.CreationTime.Date <= DateTime.Parse(input.EndDate.ToString()).Date).
                             ToLookup(x => x.CreationTime.Date).
                             Select(x => new IRMSummaryDetailDto()
                             {
                                 Name = "Business Risk",
                                 Series = x.ToList().ToLookup(x => x.CreationTime.Date).Select(y => new IRMSummaryDto
                                 {
                                     Value = x.Count(),
                                     Name = x.FirstOrDefault().CreationTime.ToString("dd-MM-yyyy")

                                 }).ToList(),
                             }).ToList();

                    //.WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId)).
                    ////.Where(x => x.IdentificationDate.Date >= DateTime.Parse(input.StartDate.ToString()).Date && x.IdentificationDate.Date <= DateTime.Parse(input.EndDate.ToString()).Date).
                    //       // .ToLookup(x => x.IdentificationDate.Date).
                    //       Select(x => new IRMSummaryDetailDto()
                    //       {
                    //           Name = "Business Risk",
                    //           Series = x.ToList().ToLookup(x => x.IdentificationDate.Date).Select(y => new IRMSummaryDto
                    //           {
                    //               Value = x.Count(),
                    //               Name = x.FirstOrDefault().IdentificationDate.ToString("dd-MM-yyyy")
                    //           }).ToList(),
                    //       }).ToList();

                finding = _findingReportRepository.GetAll()
                    .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId))
                    .Where(x => x.DateFound >= DateTime.Parse(input.StartDate.ToString()).Date && x.DateFound <= DateTime.Parse(input.EndDate.ToString()).Date).
                            ToLookup(x => x.DateFound).Select(x => new IRMSummaryDetailDto()
                            {

                                Name = "Finding",
                                Series = x.ToList().ToLookup(x => x.DateFound).Select(y => new IRMSummaryDto
                                {
                                    Value = x.Count(),
                                    Name = ((DateTime)x.FirstOrDefault().DateFound).ToString("dd-MM-yyyy")
                                }).ToList(),

                            }).ToList();

                var keylist = incident.Select(x => x.Name).Concat(exception.Select(x => x.Name)).Concat(businessRisk.Select(x => x.Name)).Concat(finding.Select(x => x.Name)).Distinct().ToList();
                keylist.ForEach(obj =>
                {
                    IRMSummaryDetailDto query = new IRMSummaryDetailDto();
                    query.Series = new List<IRMSummaryDto>();
                    query.Name = obj;
                    var getcount = (incident.Where(x => x.Name == obj).Select(x => x.Series).
                                  Concat(exception.Where(x => x.Name == obj).Select(x => x.Series)).
                                   Concat(finding.Where(x => x.Name == obj).Select(x => x.Series)).
                                  Concat(businessRisk.Where(x => x.Name == obj).Select(x => x.Series))).
                                  ToList();

                    getcount.ForEach(o =>
                    {
                        query.Series.AddRange(o);
                    });
                    result.Add(query);
                });

                return result;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<EntityControlTypeDashBoardDto> GetAllAssessmentEntityControlType()
        {
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
            var getallAssesmentCount = _assessmentRepository.GetAll().Include(e => e.BusinessEntity)
                .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId));
            var getBasicCount = getallAssesmentCount.Where(x => x.BusinessEntity.ComplianceType == ControlType.Basic).Count();
            var getTransitional = getallAssesmentCount.Where(x => x.BusinessEntity.ComplianceType == ControlType.Transitional).Count();
            var getAdvanced = getallAssesmentCount.Where(x => x.BusinessEntity.ComplianceType == ControlType.Advanced).Count();

            var query = new EntityControlTypeDashBoardDto();
            query.EntityControlTypeCount.Add(new ChartValueDto()
            {
                name = "Basic - " + getBasicCount.ToString(),
                value = getBasicCount
            });
            query.EntityControlTypeCount.Add(new ChartValueDto()
            {
                name = "Transitional - " + getTransitional.ToString(),
                value = getTransitional
            });
            query.EntityControlTypeCount.Add(new ChartValueDto()
            {
                name = "Advanced - " + getAdvanced.ToString(),
                value = getAdvanced
            });

            return query;
        }

        public async Task<OverAllEntityComplianceCountDto> GetAllEntityComplianceCount()
        {
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
            var getallAssesmentCount = _assessmentRepository.GetAll().Include(e => e.BusinessEntity)
                .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId));
            var getSentToAuthCount = getallAssesmentCount.Where(x => x.Status == AssessmentStatus.SentToAuthority).Count();
            var getRemainingCount = getallAssesmentCount.Where(x => x.Status != AssessmentStatus.SentToAuthority).Count();

            var query = new OverAllEntityComplianceCountDto();
            query.OverAllEntityCompCount.Add(new ChartValueDto()
            {
                name = "Sent to Authority - " + getSentToAuthCount.ToString(),
                value = getSentToAuthCount
            });
            query.OverAllEntityCompCount.Add(new ChartValueDto()
            {
                name = "Submission Pending - " + getRemainingCount.ToString(),
                value = getRemainingCount
            });


            return query;
        }

        public async Task<HealthCareEntityComplianceCountDto> GetAllHealthCareEntityComplianceCount()
        {
            var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
            var getallAssesmentCount = _assessmentRepository.GetAll().Include(e => e.BusinessEntity)
                .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId));
            var getSentToAuthCount = getallAssesmentCount.Where(x => x.Status == AssessmentStatus.SentToAuthority && x.BusinessEntity.EntityType == Enums.EntityType.HealthcareEntity).Count();
            var getRemainingCount = getallAssesmentCount.Where(x => x.Status != AssessmentStatus.SentToAuthority && x.BusinessEntity.EntityType == Enums.EntityType.HealthcareEntity).Count();

            var query = new HealthCareEntityComplianceCountDto();
            query.HealthCareEntityCompCount.Add(new ChartValueDto()
            {
                name = "Sent to Authority - " + getSentToAuthCount.ToString(),
                value = getSentToAuthCount
            });
            query.HealthCareEntityCompCount.Add(new ChartValueDto()
            {
                name = "Submission Pending - " + getRemainingCount.ToString(),
                value = getRemainingCount
            });


            return query;
        }



    }
}
