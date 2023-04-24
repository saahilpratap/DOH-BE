using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.EmailNotificationTemplates.Dto;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using LockthreatCompliance.FacilityTypes.Dtos;
using Microsoft.EntityFrameworkCore;
using Abp.Notifications;
using System.Collections.Generic;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.WrokFlows;
using Abp.UI;
using LockthreatCompliance.BusinessEntities;

namespace LockthreatCompliance.EmailNotificationTemplates
{
    public class EmailNotificationTemplateAppService : LockthreatComplianceAppServiceBase, IEmailNotificationTemplateAppService
    {
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IRepository<WorkFlowPage, long> _workFlowPageRepository;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;

        public EmailNotificationTemplateAppService(IRepository<EmailNotificationTemplate, long> emailnotificationRepository,
            IRepository<WorkFlowPage, long> workFlowPageRepository, IRepository<EntityApplicationSetting> entityApplicationSettingRepository)
        {
            _workFlowPageRepository = workFlowPageRepository;
            _emailnotificationRepository = emailnotificationRepository;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
        }

        [AbpAllowAnonymous]

        public async Task<List<EmailNotificationTemplateListDto>> GetAll()
        {

            var emailTemplate = new List<EmailNotificationTemplateListDto>();
            try
            {

                var filteredtemplateTypes = _emailnotificationRepository.GetAll().Include(x => x.WorkFlowPage).Include(x => x.AuditStatus);

                emailTemplate = (from obj in filteredtemplateTypes
                                 select new EmailNotificationTemplateListDto()
                                 {
                                     Id = obj.Id,
                                     EmailBody = obj.EmailBody,
                                     Subject = obj.Subject,
                                     WorkFlowPage = (obj.WorkFlowPage != null) ? obj.WorkFlowPage.PageName : null,
                                     To = obj.To,
                                     Cc = obj.Cc,
                                     Bcc = obj.Bcc,
                                     AttachmentJson = obj.AttachmentJson
                                 }).ToList();

                return emailTemplate;

            }
            catch (Exception ex)
            {
                throw;

            }



        }


        public async Task<CreatorEditEmailTemplateDto> GetEmailNotificationTemplateForEdit(EntityDto input)
        {
            var query = new CreatorEditEmailTemplateDto();

            query = await _emailnotificationRepository.GetAll().Include(x => x.WorkFlowPage).Include(x => x.AuditStatus).Where(x => x.Id == input.Id).Select(x => new CreatorEditEmailTemplateDto
            {
                Id = x.Id,
                EmailBody = x.EmailBody,
                To = x.To,
                Cc = x.Cc,
                Bcc = x.Bcc,
                Subject = x.Subject,
                AuditStatusId = x.AuditStatusId,
                AuditStatus = (x.AuditStatus != null) ? x.AuditStatus.Value : null,
                WorkFlowPageId = x.WorkFlowPageId,
                WorkFlowPage = (x.WorkFlowPage != null) ? x.WorkFlowPage.PageName : null,
                ReportType=x.ReportType,
                AttachmentJson = x.AttachmentJson
            }).FirstOrDefaultAsync();
            return query;
        }

        public async Task<CreatorEditEmailTemplateDto> GetStatusWies(int AuditStatusId)
        {

            var query = new CreatorEditEmailTemplateDto();

            query = await _emailnotificationRepository.GetAll().Include(x => x.WorkFlowPage).Include(x => x.AuditStatus).Where(x => x.AuditStatusId == AuditStatusId).Select(x => new CreatorEditEmailTemplateDto
            {
                Id = x.Id,
                EmailBody = x.EmailBody,
                To = x.To,
                Cc = x.Cc,
                Bcc = x.Bcc,
                Subject = x.Subject,
                AuditStatusId = x.AuditStatusId,
                AuditStatus = (x.AuditStatus != null) ? x.AuditStatus.Value : null,
                WorkFlowPageId = x.WorkFlowPageId,
                WorkFlowPage = (x.WorkFlowPage != null) ? x.WorkFlowPage.PageName : null,
                ReportType=x.ReportType,
                AttachmentJson = x.AttachmentJson
            }).FirstOrDefaultAsync();
            return query;
        }
        public async Task<long> CreateOrEdit(CreatorEditEmailTemplateDto input)
        {
                var setting = await _entityApplicationSettingRepository.GetAll().FirstOrDefaultAsync();
                if (setting.Attachmentpath == null || setting.Attachmentpath.Length==0)
                {
                    throw new UserFriendlyException("Please contact Admin to fix this");
                }
                else
                {
                    if (input.Id == null)
                    {
                        var result = await Create(input);
                        return result;
                    }
                    else
                    {
                        var result = await Update(input);
                        return result;
                    }
                }               
           

           
        }

        protected virtual async Task<long> Create(CreatorEditEmailTemplateDto input)
        {
            var getcheck = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == input.WorkFlowPageId).FirstOrDefaultAsync();
            if (getcheck == null)
            {

                var emailTemplate = ObjectMapper.Map<EmailNotificationTemplate>(input);
                if (AbpSession.TenantId != null)
                {
                    emailTemplate.TenantId = (int?)AbpSession.TenantId;
                }
              var id =   await _emailnotificationRepository.InsertAndGetIdAsync(emailTemplate);
                return id;
            }
            else
            {
                var getcheckauditProject = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == input.WorkFlowPageId && x.AuditStatusId == input.AuditStatusId).FirstOrDefaultAsync();

                if (getcheckauditProject == null)
                {
                    var emailTemplate = ObjectMapper.Map<EmailNotificationTemplate>(input);
                    if (AbpSession.TenantId != null)
                    {
                        emailTemplate.TenantId = (int?)AbpSession.TenantId;
                    }
                    var id = await _emailnotificationRepository.InsertAndGetIdAsync(emailTemplate);
                    return id;
                }
                else
                {
                    throw new UserFriendlyException(" Email notification Template already exist");
                }
            }

        }

        protected virtual async Task<long> Update(CreatorEditEmailTemplateDto input)
        {

            var emailTemplate = await _emailnotificationRepository.GetAll().Where(p => p.WorkFlowPageId == input.WorkFlowPageId && p.AuditStatusId == input.AuditStatusId && p.Id == input.Id).FirstOrDefaultAsync();
            if (emailTemplate != null)
            {


                emailTemplate.WorkFlowPageId = input.WorkFlowPageId;
                emailTemplate.To = input.To;
                emailTemplate.Subject = input.Subject;
                emailTemplate.Cc = input.Cc;
                emailTemplate.Bcc = input.Bcc;
                emailTemplate.ReportType = input.ReportType;
                emailTemplate.AuditStatusId = input.AuditStatusId;
                emailTemplate.EmailBody = input.EmailBody;
                emailTemplate.AttachmentJson = input.AttachmentJson;
                if (AbpSession.TenantId != null)
                {
                    emailTemplate.TenantId = (int?)AbpSession.TenantId;
                }
               var id =  _emailnotificationRepository.InsertOrUpdateAndGetId(emailTemplate);
                return id;

            }
            else
            {
                if (input.AuditStatusId == null)
                {
                    var checkpageId = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == input.WorkFlowPageId && x.Id == input.Id).FirstOrDefaultAsync();
                    if (checkpageId != null)
                    {
                        var emailTemplates = await _emailnotificationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                        emailTemplates.WorkFlowPageId = input.WorkFlowPageId;
                        emailTemplates.To = input.To;
                        emailTemplates.Subject = input.Subject;
                        emailTemplates.Cc = input.Cc;
                        emailTemplates.Bcc = input.Bcc;
                        emailTemplates.AuditStatusId = input.AuditStatusId;
                        emailTemplates.EmailBody = input.EmailBody;
                        emailTemplates.AttachmentJson = input.AttachmentJson;
                        emailTemplate.ReportType = input.ReportType;
                        if (AbpSession.TenantId != null)
                        {
                            emailTemplates.TenantId = (int?)AbpSession.TenantId;
                        }
                        var id = _emailnotificationRepository.InsertOrUpdateAndGetId(emailTemplate);
                        return id;
                    }
                    else
                    {
                        var checkworkflowId = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == input.WorkFlowPageId).FirstOrDefaultAsync();
                        if (checkworkflowId == null)
                        {
                            var emailTemplates = await _emailnotificationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                            emailTemplates.WorkFlowPageId = input.WorkFlowPageId;
                            emailTemplates.To = input.To;
                            emailTemplates.Subject = input.Subject;
                            emailTemplates.Cc = input.Cc;
                            emailTemplates.Bcc = input.Bcc;
                            emailTemplates.AuditStatusId = input.AuditStatusId;
                            emailTemplates.EmailBody = input.EmailBody;
                            emailTemplates.AttachmentJson = input.AttachmentJson;
                            emailTemplate.ReportType = input.ReportType;
                            if (AbpSession.TenantId != null)
                            {
                                emailTemplates.TenantId = (int?)AbpSession.TenantId;
                            }
                            var id = _emailnotificationRepository.InsertOrUpdateAndGetId(emailTemplate);
                            return id;
                        }
                        else
                        {
                            throw new UserFriendlyException(" Email notification template already exist");
                        }
                    }
                }
                else
                {
                    var getcheckauditProject = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == input.WorkFlowPageId && x.AuditStatusId == input.AuditStatusId).FirstOrDefaultAsync();

                    if (getcheckauditProject == null)
                    {
                        var emailTemplates = await _emailnotificationRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefaultAsync();

                        emailTemplates.WorkFlowPageId = input.WorkFlowPageId;
                        emailTemplates.To = input.To;
                        emailTemplates.Subject = input.Subject;
                        emailTemplates.Cc = input.Cc;
                        emailTemplates.Bcc = input.Bcc;
                        emailTemplates.AuditStatusId = input.AuditStatusId;
                        emailTemplates.EmailBody = input.EmailBody;
                        emailTemplates.AttachmentJson = input.AttachmentJson;
                        emailTemplate.ReportType = input.ReportType;
                        if (AbpSession.TenantId != null)
                        {
                            emailTemplates.TenantId = (int?)AbpSession.TenantId;
                        }
                        var id = _emailnotificationRepository.InsertOrUpdateAndGetId(emailTemplate);
                        return id;
                    }
                    else
                    {
                        throw new UserFriendlyException(" Email notification template already exist");
                    }
                }
            }
        }

        public async Task Delete(EntityDto input)
        {
            try
            {
                await _emailnotificationRepository.DeleteAsync(input.Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<long> CheckAuditProjectStatus()
        {
            long AuditProjectId = 0;
            try
            {
                var getcheckId = _workFlowPageRepository.FirstOrDefault(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.AuditStatus.Trim().ToString().ToLower());
                if (getcheckId != null)
                {
                    AuditProjectId = getcheckId.Id;
                }
                return AuditProjectId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



    }
}
