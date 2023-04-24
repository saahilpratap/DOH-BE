

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuthoritityDepartments.Exporting;
using LockthreatCompliance.AuthoritityDepartments.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.AutoMapper;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.WrokFlows;

namespace LockthreatCompliance.AuthoritityDepartments
{
	[AbpAuthorize(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments)]
    public class AuthorityDepartmentsAppService : LockthreatComplianceAppServiceBase, IAuthorityDepartmentsAppService
    {
		 private readonly IRepository<AuthorityDepartment> _authorityDepartmentRepository;
		 private readonly IAuthorityDepartmentsExcelExporter _authorityDepartmentsExcelExporter;
        private readonly IRepository<WorkFlowPage,long> _workFlowPageRepository;
        private readonly IRepository<Authorityworkflowactor> _authorityworkflowactorRepository;



          public AuthorityDepartmentsAppService(IRepository<AuthorityDepartment> authorityDepartmentRepository,
              IRepository<WorkFlowPage, long> workFlowPageRepository,
              IAuthorityDepartmentsExcelExporter authorityDepartmentsExcelExporter, IRepository<Authorityworkflowactor> authorityworkflowactorRepository) 
		  {
            _workFlowPageRepository = workFlowPageRepository;
            _authorityworkflowactorRepository = authorityworkflowactorRepository;
            _authorityDepartmentRepository = authorityDepartmentRepository;
			_authorityDepartmentsExcelExporter = authorityDepartmentsExcelExporter;
			
		  }
       
        [AbpAllowAnonymous]
        public async Task<List<AuthorityDepartmentDto>> GetAllAuthorityDepartments()
        {
            try
            {
                var query = await _authorityDepartmentRepository.GetAll().ToListAsync();             
                return ObjectMapper.Map<List<AuthorityDepartmentDto>>(query); ;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAllowAnonymous]
        public async Task<List<WorkFlowPageDto>> GetAllPages()
        {
            var query = new List<WorkFlowPageDto>();

            query = await _workFlowPageRepository.GetAll().Select(x => new WorkFlowPageDto()
            {
                Id=x.Id,
                PageName=x.PageName
            }).ToListAsync();

            return query;

        }


         public async Task<PagedResultDto<GetAuthorityDepartmentForViewDto>> GetAll(GetAllAuthorityDepartmentsInput input)
         {
			
			var filteredAuthorityDepartments = _authorityDepartmentRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter.Trim().ToLower()))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name.Trim().ToLower() == input.NameFilter.Trim().ToLower());

			var pagedAndFilteredAuthorityDepartments = filteredAuthorityDepartments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var authorityDepartments = from o in pagedAndFilteredAuthorityDepartments
                         select new GetAuthorityDepartmentForViewDto() {
							AuthorityDepartment = new AuthorityDepartmentDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredAuthorityDepartments.CountAsync();

            return new PagedResultDto<GetAuthorityDepartmentForViewDto>(
                totalCount,
                await authorityDepartments.ToListAsync()
            );
         }
		 
		 public async Task<GetAuthorityDepartmentForViewDto> GetAuthorityDepartmentForView(int id)
         {
            var authorityDepartment = await _authorityDepartmentRepository.GetAsync(id);

            var output = new GetAuthorityDepartmentForViewDto { AuthorityDepartment = ObjectMapper.Map<AuthorityDepartmentDto>(authorityDepartment) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Edit)]
		 public async Task<GetAuthorityDepartmentForEditOutput> GetAuthorityDepartmentForEdit(EntityDto input)
         {
            var authorityDepartment = await _authorityDepartmentRepository.GetIncluding(x => x.Id == input.Id, "Actors");

            var output = new GetAuthorityDepartmentForEditOutput { AuthorityDepartment = ObjectMapper.Map<CreateOrEditAuthorityDepartmentDto>(authorityDepartment) };

            output.AuthorityDepartment.AuthoritativeDocumentId = await _authorityworkflowactorRepository.GetAll().Where(x => x.AuthorityDepartmentId == input.Id).Select(x=>x.AuthoritativeDocumentId).Distinct().FirstOrDefaultAsync();
           output.AuthorityDepartment.WorkFlowNameId= await _authorityworkflowactorRepository.GetAll().Where(x => x.AuthorityDepartmentId == input.Id).Select(x => x.WorkFlowNameId).Distinct().FirstOrDefaultAsync();
            output.AuthorityDepartment.ReviewerIds = authorityDepartment.GetReviewers().Select(r => r.UserId.Value).ToList();
            output.AuthorityDepartment.NotifierIds = authorityDepartment.GetNotifiers().Select(r => r.NotifierUserId.Value).ToList();
            output.AuthorityDepartment.ApproverIds = authorityDepartment.GetApprovers().Select(r => r.UserId.Value).ToList();
            output.AuthorityDepartment.AuthorityIds = authorityDepartment.GetAuthoritys().Select(r => r.UserId.Value).ToList();
            output.AuthorityDepartment.PrimaryApproverId = authorityDepartment.GetApprovers().Where(p => p.IsPrimaryUser).Select(r =>r.UserId.Value).FirstOrDefault();
            output.AuthorityDepartment.PrimaryReviewerId = authorityDepartment.GetReviewers().Where(p => p.IsPrimaryUser).Select(r => r.UserId.Value).FirstOrDefault();		
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditAuthorityDepartmentDto input)
         {
            if(input.Id == 0){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Create)]
		 protected virtual async Task Create(CreateOrEditAuthorityDepartmentDto input)
         {

            if (AbpSession.TenantId != null)
            {
                input.TenantId = (int?)AbpSession.TenantId;
            }

            var authorityDepartment = ObjectMapper.Map<AuthorityDepartment>(input);

		
            await _authorityDepartmentRepository.InsertAsync(authorityDepartment);
         }

		 [AbpAuthorize(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Edit)]
		 protected virtual async Task Update(CreateOrEditAuthorityDepartmentDto input)
         {

            await _authorityworkflowactorRepository.HardDeleteAsync(x => x.AuthorityDepartmentId == input.Id);

            var authorityDepartment = await _authorityDepartmentRepository.GetIncluding(e => e.Id == input.Id, "Actors");

            ObjectMapper.Map(input, authorityDepartment);
            List<Authorityworkflowactor> authorityworkflowactors = new List<Authorityworkflowactor>();

            if (input.ApproverIds.Count() > 0)
            {
                input.ApproverIds.ForEach(approverId =>
                {
                    authorityworkflowactors.Add(new Authorityworkflowactor(BusinessEntityWorkflowActorType.Approver,input.WorkFlowNameId, authorityDepartment.Id, approverId, input.AuthoritativeDocumentId, null, input.PrimaryApproverId == approverId ? true : false, input.IsActive));
                });
            }
            if (input.NotifierIds.Count> 0)
            {
                input.NotifierIds.ForEach(notifierId =>
                {
                    authorityworkflowactors.Add(new Authorityworkflowactor(BusinessEntityWorkflowActorType.Notifier, input.WorkFlowNameId, authorityDepartment.Id, null, input.AuthoritativeDocumentId, notifierId, false, input.IsActive));
                });
            }

            if (input.ReviewerIds.Count > 0)
            {
                input.ReviewerIds.ForEach(reviewerId =>
                {
                    authorityworkflowactors.Add(new Authorityworkflowactor(BusinessEntityWorkflowActorType.Reviewer, input.WorkFlowNameId, authorityDepartment.Id, reviewerId, input.AuthoritativeDocumentId, null, input.PrimaryApproverId == reviewerId ? true : false, input.IsActive));
                });
            }

            if (input.AuthorityIds.Count > 0)
            {
                input.AuthorityIds.ForEach(authorityId =>
                {
                    authorityworkflowactors.Add(new Authorityworkflowactor(BusinessEntityWorkflowActorType.Authority, input.WorkFlowNameId, authorityDepartment.Id, authorityId, input.AuthoritativeDocumentId, null, false, input.IsActive));
                });
            }

            authorityDepartment.Actors = authorityworkflowactors;

           
         }

		 [AbpAuthorize(AppPermissions.Pages_OrganizationSetup_AuthorityDepartments_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _authorityDepartmentRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetAuthorityDepartmentsToExcel(GetAllAuthorityDepartmentsForExcelInput input)
         {
			
			var filteredAuthorityDepartments = _authorityDepartmentRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredAuthorityDepartments
                         select new GetAuthorityDepartmentForViewDto() { 
							AuthorityDepartment = new AuthorityDepartmentDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var authorityDepartmentListDtos = await query.ToListAsync();

            return _authorityDepartmentsExcelExporter.ExportToFile(authorityDepartmentListDtos);
         }


    }
}