using Abp.Domain.Repositories;
using Abp.Organizations;
using Abp.UI;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.CustomExceptions;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Abp.DynamicEntityParameters;

namespace LockthreatCompliance.BusinessEntities
{
    public class EntityApplicationSettingAppService : LockthreatComplianceAppServiceBase, IEntityApplicationSettingAppService
    {
        private readonly IRepository<EntityApplicationSetting> _settingRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        public EntityApplicationSettingAppService(IRepository<EntityApplicationSetting> settingRepository,
            IRepository<DynamicParameter> dynamicParameterRepository, 
            IRepository<OrganizationUnit, long> organizationUnitRepository)
        {
            _settingRepository = settingRepository;
            _dynamicParameterRepository = dynamicParameterRepository;
            _organizationUnitRepository = organizationUnitRepository;
        }

        public async Task AddOrUpdateSettings(EntityApplicationSettingDto input)
        {
            try
            {
                if (input.Id > 0)
                {
                    var setting = await _settingRepository.GetIncluding(e => e.Id == input.Id, "Actors", "FacilityTypeSizeSettings");
                    if (string.IsNullOrWhiteSpace(input.RootUnit))
                    {
                        throw new UserFriendlyException("Owner Cant be Null");
                    }
                    if (string.IsNullOrWhiteSpace(input.FirstUnit))
                    {
                        throw new UserFriendlyException("2nd Party Cant be Null");
                    }
                    if (string.IsNullOrWhiteSpace(input.SecondUnit))
                    {
                        throw new UserFriendlyException("3rd Party Cant be Null");
                    }
                    if (setting.RootUnit != input.RootUnit)
                    {
                        var prevOrg = await _organizationUnitRepository.FirstOrDefaultAsync(o => o.DisplayName == setting.RootUnit);
                        prevOrg.DisplayName = input.RootUnit;
                        await _organizationUnitRepository.UpdateAsync(prevOrg);
                    }
                    if (setting.FirstUnit != input.FirstUnit)
                    {
                        var prevOrg = await _organizationUnitRepository.FirstOrDefaultAsync(o => o.DisplayName == setting.FirstUnit);
                        prevOrg.DisplayName = input.FirstUnit;
                        await _organizationUnitRepository.UpdateAsync(prevOrg);
                    }
                    if (setting.SecondUnit != input.SecondUnit)
                    {
                        var prevOrg = await _organizationUnitRepository.FirstOrDefaultAsync(o => o.DisplayName == setting.SecondUnit);
                        prevOrg.DisplayName = input.SecondUnit;
                        await _organizationUnitRepository.UpdateAsync(prevOrg);
                    }

                    ObjectMapper.Map(input, setting);
                    await UpdateCurrentSettings(input);
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<EntityApplicationSettingDto> AddSettings(EntityApplicationSettingDto input)
        {
            try
            {
                int id = await _settingRepository.InsertAndGetIdAsync(ObjectMapper.Map<EntityApplicationSetting>(input));
                input.Id = id;
                return input;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateCurrentSettings(EntityApplicationSettingDto input)
        {
            var setting = await _settingRepository.GetIncluding(e => e.Id == input.Id, "Actors", "FacilityTypeSizeSettings");

            if (input.InitialiazeSettingIds.Count > 0)
            {
                setting.RemoveInitializedSettings();
                input.InitialiazeSettingIds.ForEach(id =>
                {
                    if (!setting.CheckInitializedSettings(id))
                    {
                        setting.AddInitializedSettings(id);
                    }
                });
            }
            else
            {
                setting.RemoveInitializedSettings();
            }


            if (input.InReviewSettingsIds.Count > 0)
            {
                setting.RemoveInReviewSettings();
                input.InReviewSettingsIds.ForEach(id =>
            {
                if (!setting.CheckInReviewSettings(id))
                {
                    setting.AddInReviewSettings(id);
                }
            });
            }
            else
            {
                setting.RemoveInReviewSettings();
            }


            if (input.NeedsClarificationSettingsIds.Count > 0)
            {
                setting.RemoveNeedsClarificationSettings();
                input.NeedsClarificationSettingsIds.ForEach(id =>
            {
                if (!setting.CheckNeedsClarificationSettings(id))
                {
                    setting.AddNeedsClarificationSettings(id);
                }
            });
            }
            else
            {
                setting.RemoveNeedsClarificationSettings();
            }


            if (input.BEAdminReviewSettingsIds.Count > 0)
            {
                setting.RemoveBEAdminReviewSettings();
                input.BEAdminReviewSettingsIds.ForEach(id =>
            {
                if (!setting.CheckBEAdminReviewSettings(id))
                {
                    setting.AddBEAdminReviewSettings(id);
                }
            });
            }
            else
            {
                setting.RemoveBEAdminReviewSettings();
            }

            if (input.EntityGroupAdminReviewSettingsds.Count > 0)
            {
                setting.RemoveEntityGroupAdminReviewSettings();
                input.EntityGroupAdminReviewSettingsds.ForEach(id =>
            {
                if (!setting.CheckEntityGroupAdminReviewSettings(id))
                {
                    setting.AddEntityGroupAdminReviewSettings(id);
                }
            });
            }
            else
            {
                setting.RemoveEntityGroupAdminReviewSettings();
            }

            if (input.SentToAuthoritySettingsIds.Count > 0)
            {
                setting.RemoveSentToAuthoritySettings();
                input.SentToAuthoritySettingsIds.ForEach(id =>
            {
                if (!setting.CheckSentToAuthoritySettings(id))
                {
                    setting.AddSentToAuthoritySettings(id);
                }

            });
            }
            else
            {
                setting.RemoveSentToAuthoritySettings();
            }

            if (input.ApprovedSettingsIds.Count > 0)
            {
                setting.RemoveApprovedSettings();
                input.ApprovedSettingsIds.ForEach(id =>
            {
                if (!setting.CheckApprovedSettings(id))
                {
                    setting.AddApprovedSettings(id);
                }
            });
            }
            else
            {
                setting.RemoveApprovedSettings();
            }
        }

        public async Task<EntityApplicationSettingDto> GetApplicationSettings()
        {
            try
            {
                //EntityApplicationSettingDto res = ObjectMapper.Map<EntityApplicationSettingDto>(await _settingRepository.FirstOrDefaultAsync(t => t.TenantId == AbpSession.TenantId));

                var data = await _settingRepository.GetIncluding(t => t.TenantId == AbpSession.TenantId, "Actors", "FacilityTypeSizeSettings");
                EntityApplicationSettingDto res = ObjectMapper.Map<EntityApplicationSettingDto>(data);
                if (data != null)
                {
                    if (data.Actors.Count > 0)
                    {
                        res.InitialiazeSettingIds = data.GetInitializedSettings().Select(r => r.SelectedStatusId).ToList();
                        res.InReviewSettingsIds = data.GetInReviewSettings().Select(r => r.SelectedStatusId).ToList();
                        res.BEAdminReviewSettingsIds = data.GetBEAdminReviewSettings().Select(r => r.SelectedStatusId).ToList();
                        res.EntityGroupAdminReviewSettingsds = data.GetEntityGroupAdminReviewSettings().Select(r => r.SelectedStatusId).ToList();
                        res.NeedsClarificationSettingsIds = data.GetNeedsClarificationSettings().Select(r => r.SelectedStatusId).ToList();
                        res.SentToAuthoritySettingsIds = data.GetSentToAuthoritySettings().Select(r => r.SelectedStatusId).ToList();
                        res.ApprovedSettingsIds = data.GetApprovedSettings().Select(r => r.SelectedStatusId).ToList();
                    }
                }
                if (res == null)
                {
                    return new EntityApplicationSettingDto();
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetLoginScreenMessage(int? teantId)
        {
            var query = "";
            try
            {
                if (teantId != null)
                {
                    query = _settingRepository.FirstOrDefault(x => x.TenantId == teantId).LoginScreenDisclaimerMesg;
                }
                return query;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public bool GetWorkFlowTriggerValue()
        {
            var result = false;
            try
            {
                var query = _settingRepository.GetAll().FirstOrDefault();
                if (query != null)
                {
                    result = query.EnableWorkFlowJob;
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<GetDynamicParameterOutputDto> GetSystemDynamicParameterList()
        {
            GetDynamicParameterOutputDto result = new GetDynamicParameterOutputDto();
            var query =await _settingRepository.GetAll().FirstOrDefaultAsync();
            var query1 =await _dynamicParameterRepository.GetAll().Select(x=>x.ParameterName).ToListAsync();

            if (query1 != null)
            {
                result.DynamicParameterList = query1;
            }

            if (query != null && query.SystemDynamicParameterList!=null)
            {
                result.SystemDynamicParameterList = query.SystemDynamicParameterList.Split(',').ToList();
            }
            return result;
        }

        public async Task SetSystemDynamicParameterList(List<string> input)
        {
            var value = input.Aggregate((a, b) => a + "," + b);
            var query = await _settingRepository.GetAll().FirstOrDefaultAsync();
            query.SystemDynamicParameterList = value;
           await _settingRepository.UpdateAsync(query);
        }





    }
}
