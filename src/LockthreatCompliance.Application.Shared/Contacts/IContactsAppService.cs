using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Contacts.Dtos;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Contacts
{
    public interface IContactsAppService : IApplicationService 
    {
        Task<List<ContactDto>> GetAllContact(List<int> BusinessEntityId);
        Task<List<ContactDto>> GetContactByBusinessEntity(int Id);
        Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input);

        Task<GetContactForViewDto> GetContactForView(int id);

		Task<GetContactForEditOutput> GetContactForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditContactDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input);

		
    }
}