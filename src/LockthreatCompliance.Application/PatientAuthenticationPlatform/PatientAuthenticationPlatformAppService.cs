using LockthreatCompliance.AuthoritativeDocuments;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Domains.Exporting;
using LockthreatCompliance.Domains.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.CustomExceptions;
using Abp.UI;
using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.PatientAuthenticationPlatform.Dtos;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.Storage;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.Common;
using LockthreatCompliance.Authorization.Users;
using System.Text.RegularExpressions;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.TableTopExercises.Dto;
using LockthreatCompliance.TableTopExercises;
using LockthreatCompliance.Editions.Dto;
using LockthreatCompliance.FacilityTypes;
using Abp.Collections.Extensions;
using Castle.DynamicProxy.Contributors;
using Castle.DynamicProxy.Internal;
using static LockthreatCompliance.Authorization.Roles.StaticRoleNames.Tenants;
using Stripe;
using NPOI.SS.Formula.Functions;
using Twilio.TwiML.Messaging;
using System.Linq.Expressions;
using IdentityServer4.Models;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.Url;
using Castle.Core;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.AuditProjects.Dtos;

namespace LockthreatCompliance.PatientAuthenticationPlatform
{
    public class PatientAuthenticationPlatformAppService : LockthreatComplianceAppServiceBase, IPatientAuthenticationPlatformAppService
    {
        private readonly IRepository<PatientAuthenticationPlatform, long> _patientAuthenticationPlatformRepository;
        private readonly IRepository<PatientAuthenticationPlatformContactInformation, long> _patientAuthenticationPlatformContactInformationRepository;
        private readonly IRepository<PatientAuthenticationPlatformLog, long> _patientAuthenticationPlatformLogRepository;
        private readonly IRepository<PatientAuthenticationPlatformAttachment> _patientAuthenticationPlatformAttachmentRepository;
        private readonly IRepository<PatientAuthenticationPlatformSelectedEntity, long> _papSelectedEntityRepository;
        private readonly IRepository<BusinessEntityUser> _businessEntityUserPlatformRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<BusinessEntities.BusinessEntity> _businessEntityRepository;
        private readonly ICustomDynamicAppService _customDynamicAppService;
        private readonly IRepository<Template, long> _templateserviceRepository;
        private readonly RoleManager _roleManager;

        private readonly IRepository<FacilityType> _facilityTypeRepository;

        private readonly IRepository<WorkFlowPage, long> _workflowpageAppserviceRepository;
        private readonly IUserEmailer _userEmailerRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<User, long> _userRepository;

        private readonly IRepository<PatientAuthenticationPlatformGlobalAttachment, long> _patientAuthenticationPlatformGlobalAttachmentRepository;

        public PatientAuthenticationPlatformAppService(IRepository<FacilityType> facilityTypeRepository,  IRepository<PatientAuthenticationPlatformGlobalAttachment, long> patientAuthenticationPlatformGlobalAttachmentRepository, IRepository<PatientAuthenticationPlatform, long> patientAuthenticationPlatformRepository, IRepository<PatientAuthenticationPlatformContactInformation, long> patientAuthenticationPlatformContactInformationRepository, IRepository<PatientAuthenticationPlatformLog, long> patientAuthenticationPlatformLogRepository, IRepository<PatientAuthenticationPlatformAttachment> patientAuthenticationPlatformAttachmentRepository, IRepository<PatientAuthenticationPlatformSelectedEntity, long> papSelectedEntityRepository, IRepository<BusinessEntityUser> businessEntityUserPlatformRepository, IRepository<EntityGroupMember> entityGroupMemberRepository,
            IRepository<BusinessEntities.BusinessEntity> businessEntityRepository, ICustomDynamicAppService customDynamicAppService, IRepository<Template, long> templateserviceRepository, RoleManager roleManager, IRepository<WorkFlowPage, long> workflowpageAppserviceRepository, IUserEmailer userEmailerRepository, ICommonLookupAppService commonlookupManagerRepository, IRepository<User, long> userRepository)
        {
            _facilityTypeRepository = facilityTypeRepository;
            _patientAuthenticationPlatformGlobalAttachmentRepository = patientAuthenticationPlatformGlobalAttachmentRepository;
            _patientAuthenticationPlatformRepository = patientAuthenticationPlatformRepository;
            _patientAuthenticationPlatformContactInformationRepository = patientAuthenticationPlatformContactInformationRepository;
            _patientAuthenticationPlatformLogRepository = patientAuthenticationPlatformLogRepository;
            _patientAuthenticationPlatformAttachmentRepository = patientAuthenticationPlatformAttachmentRepository;
            _papSelectedEntityRepository = papSelectedEntityRepository;
            _businessEntityUserPlatformRepository = businessEntityUserPlatformRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _businessEntityRepository = businessEntityRepository;
            _customDynamicAppService = customDynamicAppService;
            _templateserviceRepository = templateserviceRepository;
            _roleManager = roleManager;
            _workflowpageAppserviceRepository = workflowpageAppserviceRepository;
            _userEmailerRepository = userEmailerRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResultDto<PatientAuthenticationPlatformListDto>> GetAll(GetAllPatientAuthenticationPlatformsInput input)
        {
            try
            {
                int statusId = input.StatusId == null ? 0 : int.Parse("" + input.StatusId);
                int totalCount = 0;
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                var pagedAndFilteredObjs = new List<PatientAuthenticationPlatformListDto>();
                var checkFilter = new List<long>();

                if (!getcheckUser.Isadmin)
                {

                    checkFilter = await _papSelectedEntityRepository.GetAll().AsNoTracking().Where(e => getcheckUser.BusinessEntityId.Contains((int)e.BusinessEntityId)).Select(x => (long)x.PAPId).ToListAsync();
                    if (checkFilter.Count() > 0)
                    {

                        var filteredObjs = _patientAuthenticationPlatformRepository.GetAll().AsNoTracking().Include(x => x.PatientAuthenticationPlatformSelectedEntitys)
                                            .Include(x => x.Status)
                                          .WhereIf(!getcheckUser.Isadmin, e => checkFilter.Contains(e.Id))
                                          .WhereIf(statusId > 0, e => e.StatusId == input.StatusId)
                                          .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PatientAuthenticationPlatformSelectedEntitys.Select(x => x.BusinessEntity.LicenseNumber).Contains(input.Filter.Trim().ToLower()));




                        pagedAndFilteredObjs = await filteredObjs
                           .OrderBy(input.Sorting ?? "id desc")
                           .PageBy(input)
                           .Select(e => new PatientAuthenticationPlatformListDto
                           {
                               Code = e.Code,
                               Id = e.Id,
                               GroupName = e.GroupName,
                               Status = e.Status.Value.Trim().ToLower(),
                               FacilityLicenseNumber = String.Join(",", e.PatientAuthenticationPlatformSelectedEntitys.Select(yy => yy.BusinessEntity.LicenseNumber)),
                               AdditionalInformation = e.AdditionalInformation,
                               CreateationTime=e.CreationTime,
                           }).ToListAsync();


                        totalCount = await filteredObjs.CountAsync();
                    }
                    return new PagedResultDto<PatientAuthenticationPlatformListDto>(
                       totalCount,
                        pagedAndFilteredObjs
                   );

                }

                else
                {
                    if (getcheckUser.Isadmin)
                    {

                        var filteredObjs = _patientAuthenticationPlatformRepository.GetAll().Include(x => x.PatientAuthenticationPlatformSelectedEntitys).Include(x => x.Status)

                                         .WhereIf(statusId > 0, e => e.StatusId == input.StatusId)
                                         .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.PatientAuthenticationPlatformSelectedEntitys.Select(x => x.BusinessEntity.LicenseNumber).Contains(input.Filter.Trim().ToLower()));



                        pagedAndFilteredObjs = await filteredObjs
                           .OrderBy(input.Sorting ?? "id desc")
                           .PageBy(input)
                           .Select(e => new PatientAuthenticationPlatformListDto
                           {
                               Code = e.Code,
                               Id = e.Id,
                               GroupName = e.GroupName,
                               Status = e.Status.Value.Trim().ToLower(),
                               FacilityLicenseNumber = String.Join(",", e.PatientAuthenticationPlatformSelectedEntitys.Select(yy => yy.BusinessEntity.LicenseNumber)),
                               AdditionalInformation = e.AdditionalInformation,
                               CreateationTime = e.CreationTime,
                           }).ToListAsync();


                        totalCount = await filteredObjs.CountAsync();
                    }
                }

                return new PagedResultDto<PatientAuthenticationPlatformListDto>(
                     totalCount,
                      pagedAndFilteredObjs
                 );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BusinessEntitiListDto>> CreateOrEdit(CreateOrEditPatientAuthenticationPlatformDto input, int flag)
        {

            var duplicateEntityList = new List<BusinessEntitiListDto>();

            var statusList = await _customDynamicAppService.GetDynamicEntityDatabyName("PAP Status");

            if (statusList.Count() > 0 && statusList != null)
            {
                var onlySave = false;
                if (flag == 0)
                    input.StatusId = statusList.Where(x => x.Name.Trim().ToLower() == "Submitted".Trim().ToLower()).FirstOrDefault().Id;
                if (flag == 1)
                    input.StatusId = statusList.Where(x => x.Name.Trim().ToLower() == "Under Process".Trim().ToLower()).FirstOrDefault().Id;
                if (flag == 2)
                    input.StatusId = statusList.Where(x => x.Name.Trim().ToLower() == "Completed".Trim().ToLower()).FirstOrDefault().Id;

                if (flag == 4)
                {
                    onlySave = true;
                }

                if (input.Id == null)
                {
                    duplicateEntityList= await Create(input);
                }

                else
                {
                    await Update(input, onlySave);
                }
            }
            else
            {
                throw new UserFriendlyException("Please insert pap status");
            }

            return duplicateEntityList;

        }

        protected virtual async Task<List<BusinessEntitiListDto>> Create(CreateOrEditPatientAuthenticationPlatformDto input)
        {
            
                var entityList = new List<BusinessEntitiListDto>();

                var BusinessentityId = new List<int>();

                var entityListId = new HashSet<int>();

                BusinessentityId = input.PatientAuthenticationPlatformSelectedEntityDtos.Select(x => x.BusinessEntityId).ToList();

                var checkentity = await _papSelectedEntityRepository.GetAll().Where(x => BusinessentityId.Contains(x.BusinessEntityId)).ToListAsync();

                checkentity.ForEach(obj =>
                {
                     var availableEntity = _patientAuthenticationPlatformRepository.GetAll().Where(x => x.Id == obj.PAPId && x.Connecting == input.Connecting).FirstOrDefault();

                     if(availableEntity!=null)
                    {
                        entityListId.Add(obj.BusinessEntityId);
                    }   
                     
                });



                if(entityListId.Count() > 0)
                {
                    var check = BusinessentityId.All(entityListId.Contains) && BusinessentityId.Count == entityListId.Count;

                    if (check)
                    {

                      throw new UserFriendlyException("PAP enrollment request for the selected facility(s) already exists.");

                    }
                    else
                    {
                        entityList = _businessEntityRepository.GetAll().Where(x => entityListId.Contains(x.Id)).Select(x => new BusinessEntitiListDto()
                        {
                            Id = x.Id,
                            Name = x.LicenseNumber + "-" + x.CompanyLegalName

                        }).ToList();

                        return entityList;
                    }
                }
                    
                   

                if (entityListId.Count() == 0)
                {

                    var obj = ObjectMapper.Map<PatientAuthenticationPlatform>(input);
                    obj.TenantId = (int)AbpSession.TenantId;
                    obj.PatientAuthenticationPlatformContactInformations.ForEach(x =>
                    {
                        x.TenantId = (int)AbpSession.TenantId;
                    });
                    obj.PatientAuthenticationPlatformSelectedEntitys = ObjectMapper.Map<List<PatientAuthenticationPlatformSelectedEntity>>(input.PatientAuthenticationPlatformSelectedEntityDtos);
                    var papId = await _patientAuthenticationPlatformRepository.InsertAndGetIdAsync(obj);

                    CreateOrEditPatientAuthenticationPlatformLogDto objLog = new CreateOrEditPatientAuthenticationPlatformLogDto()
                    {
                        Id = 0,
                        PAPId = papId,
                        StatusId = input.StatusId,
                        LogUserId = AbpSession.UserId,
                        TenantId = AbpSession.TenantId,
                        Action = "Submitted"
                    };
                    var logId = await _patientAuthenticationPlatformLogRepository.InsertAndGetIdAsync(ObjectMapper.Map<PatientAuthenticationPlatformLog>(objLog));

                    string auditbody = null;
                    string PapsubjectSubject = "";
                    string PapsubjectBody = "";

                    HashSet<string> toemails = new HashSet<string>();
                    HashSet<string> ccemail = new HashSet<string>();
                    HashSet<string> bccEmail = new HashSet<string>();

                    var currentUser = _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).FirstOrDefault();

                    var businessEntitiesIds = input.PatientAuthenticationPlatformSelectedEntityDtos.Select(x => x.BusinessEntityId).ToList();

                    var businessEntities = await _businessEntityRepository.GetAll().Where(x => businessEntitiesIds.Contains(x.Id)).ToListAsync();

                    var getpagename = await _workflowpageAppserviceRepository.GetAll().Where(x => x.PageName.Trim().ToLower() == "Submitted".Trim().ToLower()).FirstOrDefaultAsync();
                    if (getpagename != null)
                    {
                        var getTemplate = await _templateserviceRepository.GetAll().Where(x => x.WorkFlowPageId == getpagename.Id).FirstOrDefaultAsync();

                        if (getTemplate != null)
                        {


                        List<string> templateSubject = new List<string>();
                        var PaptemplateSubject = getTemplate.TemplateSubject;
                        PapsubjectSubject = getTemplate.TemplateSubject.ToString();
                        while (PaptemplateSubject.Contains("{"))
                        {
                            templateSubject.Add("{" + PaptemplateSubject.Split('{', '}')[1] + "}");
                            PaptemplateSubject = PaptemplateSubject.Replace("{" + PaptemplateSubject.Split('{', '}')[1] + "}", "");
                        };
                        PapsubjectSubject = ReplaceValueFunction(papId, templateSubject, PapsubjectSubject);


                        List<string> templatebody = new List<string>();
                        var Paptemplatesbody = getTemplate.TemplateBody;
                        PapsubjectBody = getTemplate.TemplateBody.ToString();

                        while (Paptemplatesbody.Contains("{"))
                        {
                            templatebody.Add("{" + Paptemplatesbody.Split('{', '}')[1] + "}");
                            Paptemplatesbody = Paptemplatesbody.Replace("{" + Paptemplatesbody.Split('{', '}')[1] + "}", "");
                        };
                        PapsubjectBody = ReplaceBodyFucntion(papId, templatebody, PapsubjectBody);


                        List<string> templatevariables = new List<string>();
                            List<string> toLists = getTemplate.TemplateTo.Split(',').ToList();

                            toLists.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");

                                    if (currentUser != null)
                                    {
                                        toemails.Add(currentUser.EmailAddress);
                                    }
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        toemails.Add(email);

                                    }
                                    if (currentUser != null)
                                    {
                                        toemails.Add(currentUser.EmailAddress);
                                    }
                                }
                            });

                            var ccList = getTemplate.TemplateCc;
                            List<string> templateCc = new List<string>();
                            List<string> ccLists = getTemplate.TemplateCc.Split(',').ToList();
                            ccLists.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        ccemail.Add(email);
                                    }
                                }
                            });

                            var bccList = getTemplate.TemplateBcc;
                            List<string> templatebCc = new List<string>();
                            List<string> bccLists = getTemplate.TemplateCc.Split(',').ToList();

                            bccLists.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templatebCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        bccEmail.Add(email);
                                    }
                                }
                            });

                            var queryto = await GetToCCBCCMail(templatevariables, businessEntities, papId);
                            queryto.ForEach(obj =>
                            {
                                toemails.Add(obj);
                            });

                            var querycc = await GetToCCBCCMail(templateCc, businessEntities, papId);
                            querycc.ForEach(obj =>
                            {
                                ccemail.Add(obj);
                            });

                            var querybcc = await GetToCCBCCMail(templatebCc, businessEntities, papId);
                            querybcc.ForEach(obj =>
                            {
                                bccEmail.Add(obj);
                            });

                        }
                        await _userEmailerRepository.SendMailForPAPStatus(toemails, ccemail, bccEmail, PapsubjectSubject, (int)AbpSession.TenantId, PapsubjectBody, getpagename.Id);
                    }


                }

               

                return entityList;
            
            

        }


        private string ReplaceBodyFucntion(long PapId, List<string> input, string output)
        {
            var PAPbody = output;          
           

            input.ForEach(x =>
            {
                var getpapEntityList = _papSelectedEntityRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.PAPId == PapId)               
                    .Select(x => new 
                    { LicenseNo =x.BusinessEntity.LicenseNumber, FacilityName=x.BusinessEntity.CompanyLegalName }).ToList();

                switch (x)
                {
                                    
                    case "{EntityList}":
                        {

                            if (getpapEntityList.Count > 0)
                            {
                                var sb = "";
                                sb = sb + "<div style='float:center !important'>";

                                sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                                sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";
                               
                                foreach (var item in getpapEntityList)
                                {
                                    sb = sb + "<tr style='border:solid 1px black'>";
                                    sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.LicenseNo + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.FacilityName + "</td>";                                
                                    sb = sb + "</tr>";
                                }
                                sb = sb + "</table></div>";

                                PAPbody = PAPbody.Replace("{EntityList}", sb);
                            }
                            break;
                        }
                   
                }

            });
            return PAPbody;
        }

        public async Task<bool> CreatePAPwithSkip(CreateOrEditPatientAuthenticationPlatformDto input, int flag)
        {
            bool result = false;

            var statusList = await _customDynamicAppService.GetDynamicEntityDatabyName("PAP Status");
            if (statusList.Count() > 0 && statusList != null)
            {
                
                if (flag == 0)
                    input.StatusId = statusList.Where(x => x.Name.Trim().ToLower() == "Submitted".Trim().ToLower()).FirstOrDefault().Id;
                if (flag == 1)
                    input.StatusId = statusList.Where(x => x.Name.Trim().ToLower() == "Under Process".Trim().ToLower()).FirstOrDefault().Id;
                if (flag == 2)
                    input.StatusId = statusList.Where(x => x.Name.Trim().ToLower() == "Completed".Trim().ToLower()).FirstOrDefault().Id;

                var entityListId = new HashSet<int>();

                var BusinessentityId = new List<int>();

                BusinessentityId = input.PatientAuthenticationPlatformSelectedEntityDtos.Select(x => x.BusinessEntityId).ToList();

                var checkentity = await _papSelectedEntityRepository.GetAll().Where(x => BusinessentityId.Contains(x.BusinessEntityId)).ToListAsync();

                checkentity.ForEach(obj =>
                {

                    var availableEntity = _patientAuthenticationPlatformRepository.GetAll().Where(x => x.Id == obj.PAPId && x.Connecting == input.Connecting).FirstOrDefault();

                    if (availableEntity != null)
                    {
                        entityListId.Add(obj.BusinessEntityId);
                    }
                });

               
                input.PatientAuthenticationPlatformSelectedEntityDtos.RemoveAll(x => entityListId.Contains(x.BusinessEntityId));



                var obj = ObjectMapper.Map<PatientAuthenticationPlatform>(input);
                obj.TenantId = (int)AbpSession.TenantId;
                obj.PatientAuthenticationPlatformContactInformations.ForEach(x =>
                {
                    x.TenantId = (int)AbpSession.TenantId;
                });



                obj.PatientAuthenticationPlatformSelectedEntitys = ObjectMapper.Map<List<PatientAuthenticationPlatformSelectedEntity>>(input.PatientAuthenticationPlatformSelectedEntityDtos);
                var papId = await _patientAuthenticationPlatformRepository.InsertAndGetIdAsync(obj);
                result = true;
                CreateOrEditPatientAuthenticationPlatformLogDto objLog = new CreateOrEditPatientAuthenticationPlatformLogDto()
                {
                    Id = 0,
                    PAPId = papId,
                    StatusId = input.StatusId,
                    LogUserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    Action = "Submitted"
                };
                var logId = await _patientAuthenticationPlatformLogRepository.InsertAndGetIdAsync(ObjectMapper.Map<PatientAuthenticationPlatformLog>(objLog));

                string auditbody = null;
                string PapsubjectSubject = "";
                string PapsubjectBody = "";

                HashSet<string> toemails = new HashSet<string>();
                HashSet<string> ccemail = new HashSet<string>();
                HashSet<string> bccEmail = new HashSet<string>();

                var currentUser = _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).FirstOrDefault();

                var businessEntitiesIds = input.PatientAuthenticationPlatformSelectedEntityDtos.Select(x => x.BusinessEntityId).ToList();

                var businessEntities = await _businessEntityRepository.GetAll().Where(x => businessEntitiesIds.Contains(x.Id)).ToListAsync();

                var getpagename = await _workflowpageAppserviceRepository.GetAll().Where(x => x.PageName.Trim().ToLower() == "Submitted".Trim().ToLower()).FirstOrDefaultAsync();
                if (getpagename != null)
                {
                    var getTemplate = await _templateserviceRepository.GetAll().Where(x => x.WorkFlowPageId == getpagename.Id).FirstOrDefaultAsync();
                     
                    if (getTemplate != null)
                    {
                       
                        List<string> templateSubject = new List<string>();
                        var PaptemplateSubject = getTemplate.TemplateSubject;
                        PapsubjectSubject = getTemplate.TemplateSubject.ToString();
                    

                        while (PaptemplateSubject.Contains("{"))
                        {
                            templateSubject.Add("{" + PaptemplateSubject.Split('{', '}')[1] + "}");
                            PaptemplateSubject = PaptemplateSubject.Replace("{" + PaptemplateSubject.Split('{', '}')[1] + "}", "");
                        };

                        PapsubjectSubject = ReplaceValueFunction(papId,templateSubject, PapsubjectSubject);



                        List<string> templatebody = new List<string>();
                        var Paptemplatesbody = getTemplate.TemplateBody;
                        PapsubjectBody = getTemplate.TemplateBody.ToString();

                        while (Paptemplatesbody.Contains("{"))
                        {
                            templatebody.Add("{" + Paptemplatesbody.Split('{', '}')[1] + "}");
                            Paptemplatesbody = Paptemplatesbody.Replace("{" + Paptemplatesbody.Split('{', '}')[1] + "}", "");
                        };
                        PapsubjectBody = ReplaceBodyFucntion(papId, templatebody, PapsubjectBody);


                        List<string> templatevariables = new List<string>();
                        List<string> toLists = getTemplate.TemplateTo.Split(',').ToList();

                        toLists.ForEach(emailid =>
                        {
                            if (emailid.Contains("{"))
                            {
                                templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");

                                if (currentUser != null)
                                {
                                    toemails.Add(currentUser.EmailAddress);
                                }
                            }
                            else
                            {
                                string email = emailid.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    toemails.Add(email);

                                }
                                if (currentUser != null)
                                {
                                    toemails.Add(currentUser.EmailAddress);
                                }
                            }
                        });

                        var ccList = getTemplate.TemplateCc;
                        List<string> templateCc = new List<string>();
                        List<string> ccLists = getTemplate.TemplateCc.Split(',').ToList();
                        ccLists.ForEach(emailid =>
                        {
                            if (emailid.Contains("{"))
                            {
                                templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                            }
                            else
                            {
                                string email = emailid.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    ccemail.Add(email);
                                }
                            }
                        });

                        var bccList = getTemplate.TemplateBcc;
                        List<string> templatebCc = new List<string>();
                        List<string> bccLists = getTemplate.TemplateCc.Split(',').ToList();

                        bccLists.ForEach(emailid =>
                        {
                            if (emailid.Contains("{"))
                            {
                                templatebCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                            }
                            else
                            {
                                string email = emailid.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    bccEmail.Add(email);
                                }
                            }
                        });

                        var queryto = await GetToCCBCCMail(templatevariables, businessEntities, papId);
                        queryto.ForEach(obj =>
                        {
                            toemails.Add(obj);
                        });

                        var querycc = await GetToCCBCCMail(templateCc, businessEntities, papId);
                        querycc.ForEach(obj =>
                        {
                            ccemail.Add(obj);
                        });

                        var querybcc = await GetToCCBCCMail(templatebCc, businessEntities, papId);
                        querybcc.ForEach(obj =>
                        {
                            bccEmail.Add(obj);
                        });

                    }
                    await _userEmailerRepository.SendMailForPAPStatus(toemails, ccemail, bccEmail, PapsubjectSubject, (int)AbpSession.TenantId, PapsubjectBody, getpagename.Id);
                }
               

            }
            return result;

        }

        private string ReplaceValueFunction(long PAPID, List<string> input, string output)
        {
            var PAPEmailsubject = output;
           
          

            input.ForEach(x =>
            {
                switch (x)
                {
                    case "{PAP_Id}":
                        {
                            PAPEmailsubject = (PAPID != 0) ? PAPEmailsubject.Replace("{PAP_Id}", "PAP-"+PAPID) : PAPEmailsubject.Replace("{PAP_Id}", "");
                            break;
                        }
                    
                }

            });

            return PAPEmailsubject;
        }
        protected virtual async Task Update(CreateOrEditPatientAuthenticationPlatformDto input, bool onlySave)
        {
            try
            {
                var statusList = await _customDynamicAppService.GetDynamicEntityDatabyName("PAP Status");

                var contactList = await _patientAuthenticationPlatformContactInformationRepository.GetAll().Where(x => x.PAPId == input.Id).ToListAsync();
                contactList.ForEach(cl => {
                    _patientAuthenticationPlatformContactInformationRepository.HardDeleteAsync(cl);
                });

                var temp = await _patientAuthenticationPlatformRepository.GetAll().
                    Include(x => x.PatientAuthenticationPlatformContactInformations).Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                temp.Comment1 = input.Comment1;

                temp.PatientAuthenticationPlatformContactInformations = ObjectMapper.Map<List<PatientAuthenticationPlatformContactInformation>>(input.PatientAuthenticationPlatformContactInformationDtos);
                temp.PatientAuthenticationPlatformContactInformations.ForEach(x => {
                    x.Id = 0;
                    x.TenantId = (int)AbpSession.TenantId;
                });

                if (onlySave != true)
                    temp.StatusId = input.StatusId;

                await _patientAuthenticationPlatformRepository.UpdateAsync(temp);

                CreateOrEditPatientAuthenticationPlatformLogDto objLog = new CreateOrEditPatientAuthenticationPlatformLogDto()
                {
                    Id = 0,
                    PAPId = (int)input.Id,
                    StatusId = input.StatusId,
                    LogUserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    Action = "Update"
                };

                var logId = await _patientAuthenticationPlatformLogRepository.InsertAndGetIdAsync(ObjectMapper.Map<PatientAuthenticationPlatformLog>(objLog));

                if (onlySave != true)
                {
                    string auditbody = null;
                    string PapsubjectSubject = "";
                    string PapsubjectBody = "";

                    HashSet<string> toemails = new HashSet<string>();
                    HashSet<string> ccemail = new HashSet<string>();
                    HashSet<string> bccEmail = new HashSet<string>();

                    var currentUser = _userRepository.GetAll().Where(x => x.Id == AbpSession.UserId).FirstOrDefault();

                    var businessEntitiesIds = input.PatientAuthenticationPlatformSelectedEntityDtos.Select(x => x.BusinessEntityId).ToList();

                    //if (currentUser.BusinessEntityId != null)
                    //{
                    //    businessEntitiesIds.Add((int)currentUser.BusinessEntityId);
                    //}

                    var businessEntities = await _businessEntityRepository.GetAll().Where(x => businessEntitiesIds.Contains(x.Id)).ToListAsync();

                    var statusOBj = statusList.FirstOrDefault(x => x.Id == input.StatusId);
                    var getpagename = new WorkFlowPage();
                    if (statusOBj.Name.Trim().ToLower() == "Under Process".Trim().ToLower())
                    {
                        getpagename = await _workflowpageAppserviceRepository.GetAll().Where(x => x.PageName.Trim().ToLower() == "Under Process".Trim().ToLower()).FirstOrDefaultAsync();
                    }
                    else if (statusOBj.Name.Trim().ToLower() == "completed".Trim().ToLower())
                    {
                        getpagename = await _workflowpageAppserviceRepository.GetAll().Where(x => x.PageName.Trim().ToLower() == "completed".Trim().ToLower()).FirstOrDefaultAsync();
                    }

                    if (getpagename != null)
                    {
                        var getTemplate = await _templateserviceRepository.GetAll().Where(x => x.WorkFlowPageId == getpagename.Id).FirstOrDefaultAsync();

                        if (getTemplate != null)
                        {

                            List<string> templateSubject = new List<string>();
                            var PaptemplateSubject = getTemplate.TemplateSubject;
                            PapsubjectSubject = getTemplate.TemplateSubject.ToString();
                            while (PaptemplateSubject.Contains("{"))
                            {
                                templateSubject.Add("{" + PaptemplateSubject.Split('{', '}')[1] + "}");
                                PaptemplateSubject = PaptemplateSubject.Replace("{" + PaptemplateSubject.Split('{', '}')[1] + "}", "");
                            };
                            PapsubjectSubject = ReplaceValueFunction((long)input.Id, templateSubject, PapsubjectSubject);



                            List<string> templatebody = new List<string>();
                            var Paptemplatesbody = getTemplate.TemplateBody;
                            PapsubjectBody = getTemplate.TemplateBody.ToString();

                            while (Paptemplatesbody.Contains("{"))
                            {
                                templatebody.Add("{" + Paptemplatesbody.Split('{', '}')[1] + "}");
                                Paptemplatesbody = Paptemplatesbody.Replace("{" + Paptemplatesbody.Split('{', '}')[1] + "}", "");
                            };
                            PapsubjectBody = ReplaceBodyFucntion((long)input.Id, templatebody, PapsubjectBody);



                            List<string> templatevariables = new List<string>();
                            List<string> toLists = getTemplate.TemplateTo.Split(',').ToList();

                            toLists.ForEach(async emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");

                                    //if (currentUser != null)
                                    //{
                                    //    toemails.Add(currentUser.EmailAddress);
                                    //}
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        toemails.Add(email);
                                    }

                                    //if (currentUser != null)
                                    //{
                                    //    toemails.Add(currentUser.EmailAddress);
                                    //}
                                }
                            });

                            var ccList = getTemplate.TemplateCc;
                            List<string> templateCc = new List<string>();
                            List<string> ccLists = getTemplate.TemplateCc.Split(',').ToList();
                            ccLists.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        ccemail.Add(email);
                                    }
                                }
                            });

                            var bccList = getTemplate.TemplateBcc;
                            List<string> templatebCc = new List<string>();
                            List<string> bccLists = getTemplate.TemplateCc.Split(',').ToList();

                            bccLists.ForEach(emailid =>
                            {
                                if (emailid.Contains("{"))
                                {
                                    templatebCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                }
                                else
                                {
                                    string email = emailid.Trim();
                                    bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                    if (isEmail == true)
                                    {
                                        bccEmail.Add(email);
                                    }
                                }
                            });

                            var queryto = await GetToCCBCCMail(templatevariables, businessEntities,(long)input.Id);
                            queryto.ForEach(obj =>
                            {
                                toemails.Add(obj);
                            });

                            var querycc = await GetToCCBCCMail(templateCc, businessEntities, (long)input.Id);
                            querycc.ForEach(obj =>
                            {
                                ccemail.Add(obj);
                            });

                            var querybcc = await GetToCCBCCMail(templatebCc, businessEntities, (long)input.Id);
                            querybcc.ForEach(obj =>
                            {
                                bccEmail.Add(obj);
                            });

                        }
                        await _userEmailerRepository.SendMailForPAPStatus(toemails, ccemail, bccEmail, PapsubjectSubject, (int)AbpSession.TenantId, PapsubjectBody, getpagename.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        private async Task<List<string>> GetToCCBCCMail(List<string> templatevariables, List<BusinessEntities.BusinessEntity> businessEntityslist,long PAPId)
        {
            var result = new List<string>();

            templatevariables.ForEach(x =>
           {
               switch (x)
               {
                   case "{Business_Entity_Admin_Email}":
                       {
                           businessEntityslist.ForEach(yy =>
                           {
                               if (yy.AdminEmail != null)
                               {
                                   result.Add(yy.AdminEmail);
                               }
                           });
                           break;
                       }
                   case "{Primary_Contact_Email}":
                       {
                           businessEntityslist.ForEach(yy =>
                           {
                               if (yy.OfficialEmail != null)
                               {
                                   var splitEmail = yy.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                   foreach (var i in splitEmail)
                                   {
                                       string email = i.Trim();
                                       bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                       if (isEmail == true)
                                       {
                                           result.Add(i);
                                       }
                                   }
                               }
                           });
                           break;
                       }
                   case "{Secondary_Contact_Email}":
                       {
                           businessEntityslist.ForEach(yy =>
                           {
                               if (yy.BackupOfficialEmail != null)
                               {
                                   var splitEmail = yy.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                   foreach (var i in splitEmail)
                                   {
                                       string email = i.Trim();
                                       bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                       if (isEmail == true)
                                       {
                                           result.Add(i);
                                       }
                                   }
                               }
                           });
                           break;
                       }
                   case "{Director_Incharge_Email}":
                       {
                           businessEntityslist.ForEach(yy =>
                           {
                               if (yy.Director_Incharge_Email != null)
                               {
                                   var splitEmail = yy.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                   foreach (var i in splitEmail)
                                   {
                                       string email = i.Trim();
                                       bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                       if (isEmail == true)
                                       {
                                           result.Add(i);
                                       }
                                   }
                               }
                           });
                           break;
                       }
                   case "{Group_Admin}":
                       {
                           businessEntityslist.ForEach(yy =>
                           {
                               var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == yy.Id).FirstOrDefault();
                               if (getGroup != null)
                               {
                                   var getuser = _userRepository.FirstOrDefault(x => x.Id == getGroup.EntityGroup.UserId);

                                   var splitEmail = getuser.EmailAddress.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                   foreach (var i in splitEmail)
                                   {
                                       string email = i.Trim();
                                       bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                       if (isEmail == true)
                                       {
                                           result.Add(i);
                                       }
                                   }
                               }

                           });

                           break;
                       }

                   case "{Contact_Information}":
                       {
                           var getContactinformationList = _patientAuthenticationPlatformContactInformationRepository.GetAll().Where(x => x.PAPId == PAPId).Select(x=>x.EmailAddress).ToList();

                           getContactinformationList.ForEach(item =>
                           {
                               var splitEmail = item.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                               foreach (var i in splitEmail)
                               {
                                   string email = i.Trim();
                                   bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                   if (isEmail == true)
                                   {
                                       result.Add(i);
                                   }
                               }
                           });
                           break;
                       }                 
               }
           });

            return result;
        }


        
        


        public async Task Delete(int input)
        {
            var obj = _patientAuthenticationPlatformRepository.GetAll().Where(x => x.Id == input).FirstOrDefault();
            if (obj != null)
            {
                CreateOrEditPatientAuthenticationPlatformLogDto objLog = new CreateOrEditPatientAuthenticationPlatformLogDto()
                {
                    Id = 0,
                    PAPId = input,
                    StatusId = obj.StatusId,
                    LogUserId = AbpSession.UserId,
                    TenantId = AbpSession.TenantId,
                    Action = "Delete"
                };
                var logId = await _patientAuthenticationPlatformLogRepository.InsertAndGetIdAsync(ObjectMapper.Map<PatientAuthenticationPlatformLog>(objLog));

                await _patientAuthenticationPlatformRepository.DeleteAsync(input);
            }
        }

        public async Task DeletePAPContactInformation(int input)
        {
            await _patientAuthenticationPlatformContactInformationRepository.DeleteAsync(input);
        }

        public async Task<PatientAuthenticationPlatformDto> GetPatientAuthenticationPlatformById(int input)
        {
            try
            {
                var result = new PatientAuthenticationPlatformDto();
                var Query = new List<PatientAuthenticationPlatformAttachmentDto>();
                var obj = await _patientAuthenticationPlatformRepository.GetAll().Include(x => x.PatientAuthenticationPlatformContactInformations)
                    .Include(x => x.PatientAuthenticationPlatformAttachments).Include(x => x.PatientAuthenticationPlatformSelectedEntitys).Include(x => x.Status)
                            .Where(x => x.Id == input).FirstOrDefaultAsync();

                result = ObjectMapper.Map<PatientAuthenticationPlatformDto>(obj);
                result.PatientAuthenticationPlatformSelectedEntityDtos = ObjectMapper.Map<List<PatientAuthenticationPlatformSelectedEntityDto>>(obj.PatientAuthenticationPlatformSelectedEntitys);

                var attachment = await _patientAuthenticationPlatformAttachmentRepository.GetAll().Where(x => x.PAPId == input).ToListAsync();
                result.PatientAuthenticationPlatformAttachmentDtos = ObjectMapper.Map<List<PatientAuthenticationPlatformAttachmentDto>>(attachment);

                 Query = await GetAllPAPGlobalAttachments();
                result.PatientAuthenticationPlatformAttachmentDtos.AddRange(Query);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BusinessEntitiListDto>> BusinessEntitiesByUserId(int input, int papId)
        {
            try
            {
                var currentUser = await GetCurrentUserAsync();
                var role = await _roleManager.Roles.Where(r => r.DisplayName.Trim().ToLower() == "Admin".Trim().ToLower()).FirstOrDefaultAsync();
                var users = await UserManager.GetUsersInRoleAsync(role.Name);
                bool isAdmin = users.Any(u => u.Id == currentUser.Id);
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();
                int facilityId = 0;
                var checkFacility = _facilityTypeRepository.GetAll().Where(x => x.Name.Trim().ToLower() == "Pharmacy".Trim().ToLower()).FirstOrDefault();
                 
                  if(checkFacility!=null)
                  {
                    facilityId = checkFacility.Id;
                  }
                 

                if (!isAdmin)
                {
                    var result = new List<BusinessEntitiListDto>();


                    result = await _businessEntityRepository.GetAll().WhereIf(facilityId > 0,x =>x.FacilityTypeId!=facilityId).WhereIf(!getcheckUser.Isadmin, e => getcheckUser.BusinessEntityId.Contains(e.Id))
                                .Select(x => new BusinessEntitiListDto
                                {
                                    Id = x.Id,
                                    Name = x.LicenseNumber + "-" + x.CompanyLegalName
                                }).ToListAsync();
                    return result;
                }
                else
                {
                    var result = new List<BusinessEntitiListDto>();
                    if (papId != 0)
                    {
                        var temp = await _patientAuthenticationPlatformRepository.GetAll().Include(x => x.PatientAuthenticationPlatformSelectedEntitys).ThenInclude(x => x.BusinessEntity).Where(x => x.Id == papId)
                               .Select(x => x.PatientAuthenticationPlatformSelectedEntitys).FirstOrDefaultAsync();
                        result = temp.Select(x => new BusinessEntitiListDto
                        {
                            Id = x.BusinessEntityId,
                            Name = x.BusinessEntity.LicenseNumber + "-" + x.BusinessEntity.CompanyLegalName
                        }).ToList();
                    }

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetGroupName(BusinessEntitiListDto[] input)
        {
            try
            {
                var obj = new CreateOrEditPatientAuthenticationPlatformSelectedEntityDto();
                var Ids = input.Select(x => x.Id);
                var result = "";
                result = await _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => Ids.Contains(x.BusinessEntityId))
                            .Select(x => x.EntityGroup.Name).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CreateOrEditPAPGlobalAttachment(CreateorEditPatientAuthenticationPlatformGlobalAttachmentDto input)
        {
            try
            {
                bool query = await CreateorUpdatePAPGlobalAttachmentAsync(input);

                return query;
            }
            catch(Exception) 
            {
                throw;
            }
        }

        protected virtual async Task<bool> CreateorUpdatePAPGlobalAttachmentAsync(CreateorEditPatientAuthenticationPlatformGlobalAttachmentDto input)
        {
            try
            {
                bool result = false;
                var getGlobalAttachemnt =  _patientAuthenticationPlatformGlobalAttachmentRepository.GetAll().ToList();

                if (input.PatientAuthenticationPlatformGlobalAttachmentDto.Count() > 0)
                {
                    //getGlobalAttachemnt.ForEach( x =>
                    //{
                    //     _patientAuthenticationPlatformGlobalAttachmentRepository.HardDelete(x);

                    //});
                    input.PatientAuthenticationPlatformGlobalAttachmentDto.ForEach(x =>
                    {
                        var items = new PatientAuthenticationPlatformGlobalAttachment
                        {
                            Id = 0,
                            TenantId = AbpSession.TenantId,
                            CreatorUserId = AbpSession.UserId,
                            CreationTime = DateTime.Now,
                            FileName = x.FileName,
                            Title = x.FileName,
                            Code = x.Code,
                            Static = true
                        };

                        long id =  _patientAuthenticationPlatformGlobalAttachmentRepository.InsertOrUpdateAndGetId(items);
                    });
                    result = true;

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public  async Task<List<PatientAuthenticationPlatformGlobalAttachmentDto>> GetAllPAPGlobalAttachemnt()
            {
            try
            {             
               var  query  = await _patientAuthenticationPlatformGlobalAttachmentRepository.GetAll().
                    Select(x => new PatientAuthenticationPlatformGlobalAttachmentDto()
                    {
                          Id = x.Id, 
                          FileName= x.FileName, 
                          Title = x.Title,
                          Code = x.Code,
                          Static= x.Static,    
                    }).ToListAsync();   

                return query;

            }
            catch(Exception)
            {
                throw;
            }
               
            }

        private async Task<List<PatientAuthenticationPlatformAttachmentDto>> GetAllPAPGlobalAttachments()
        {
            try
            {
                var result = new List<PatientAuthenticationPlatformAttachmentDto>();

                result = await _patientAuthenticationPlatformGlobalAttachmentRepository.GetAll().Select(res => new PatientAuthenticationPlatformAttachmentDto()
                {                    
                    FileName = res.FileName,
                    Title= res.Title,
                    Code = res.Code,
                    Static = res.Static,
                }).ToListAsync();

                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}