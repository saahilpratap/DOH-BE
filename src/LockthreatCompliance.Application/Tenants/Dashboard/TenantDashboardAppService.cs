using Abp.Auditing;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Timing;
using LockthreatCompliance.Authorization;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Common;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.MultiTenancy.Dto;
using LockthreatCompliance.Tenants.Dashboard.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LockthreatCompliance.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : LockthreatComplianceAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<User, long> _userRepository;
        public TenantDashboardAppService(IRepository<BusinessEntity> businessEntityRepository, IRepository<User, long> userRepository,
            IRepository<Assessment> assessmentRepository, ICommonLookupAppService commonlookupManagerRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository)
        {
            _userRepository = userRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _businessEntityRepository = businessEntityRepository;
            _assessmentRepository = assessmentRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
        }

        public GetMemberActivityOutput GetMemberActivity()
        {
            return new GetMemberActivityOutput
            (
                DashboardRandomDataGenerator.GenerateMemberActivities()
            );
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var output = new GetDashboardDataOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500),
                SalesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod),
                Expenses = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(1000, 9000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(10000, 90000),
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50),
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };

            return output;
        }

        public GetTopStatsOutput GetTopStats()
        {
            return new GetTopStatsOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500)
            };
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return new GetProfitShareOutput
            {
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };
        }

        public GetDailySalesOutput GetDailySales()
        {
            return new GetDailySalesOutput
            {
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50)
            };
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            var salesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod);
            return new GetSalesSummaryOutput(salesSummary)
            {
                Expenses = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(0, 3000)
            };
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(
                DashboardRandomDataGenerator.GenerateRegionalStat()
            );
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }

        public async Task<CommonTopStatForHostAndTenant> GetTopStatByTenant()
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var activeEntities = _businessEntityRepository.GetAll().WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.Id)).ToList();
                var submittedAssessment = _assessmentRepository.GetAll().WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId)).ToList();
                var externalAssessment = _externalAssessmentRepository.GetAll().WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId)).ToList();
                var userDetails = _userRepository.GetAll().Where(x=>x.BusinessEntityId!=null && x.TenantId==AbpSession.TenantId).WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId) ).ToList();
                var commonData = new CommonTopStatForHostAndTenant
                {
                    TotalActiveEntities = activeEntities.Count, 
                    TotalActiveEntitiesChange = activeEntities.Where(c => c.Status == EntityTypeStatus.NotApproved || c.Status == EntityTypeStatus.InActive || c.Status == EntityTypeStatus.Imported).Count(),
                    NewAssessmentsSubmitted = submittedAssessment.Count,
                    NewAssessmentsSubmittedChange = submittedAssessment.Where(e => e.Status == AssessmentStatus.SentToAuthority).Count(),
                    NewExternalAssessments = externalAssessment.Count,
                    NewExternalAssessmentsChange = externalAssessment.Where(e => e.Status == AssessmentStatus.Initialized || e.Status == AssessmentStatus.InReview || e.Status == AssessmentStatus.NeedsClarification || e.Status == AssessmentStatus.SentToAuthority).Count(),
                    NewUsers = userDetails.Count,
                    NewUsersChange = userDetails.Where(u => u.IsActive == false).Count()
                };

                return commonData;
            }
            catch(System.Exception ex)
            {
                throw;
            }
        }
    }
}
