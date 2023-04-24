using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using LockthreatCompliance.TableTopExercises.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.EntityGroups;
using System.Text.RegularExpressions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.Url;
using Abp.Extensions;
using System.Web;
using Abp.Runtime.Security;
using System.Text;
using LockthreatCompliance.Common;
using LockthreatCompliance.Questions;

namespace LockthreatCompliance.TableTopExercises
{
    public class TableTopExerciseAppService : LockthreatComplianceAppServiceBase, ITableTopExerciseAppService
    {
        private readonly IRepository<TableTopExerciseQuestion, long> _tableTopExerciseQuestionRepository;
        private readonly IRepository<TableTopExerciseQuestionOption> _tableTopExerciseQuestionOptionRepository;
        private readonly IRepository<TableTopExerciseEntity, long> _tableTopExerciseEntityRepository;
        private readonly IRepository<TableTopExerciseEntityResponse, long> _tableTopExerciseEntityResponseRepository;
        private readonly IRepository<TableTopExerciseGroup, long> _tableTopExerciseGroupRepository;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<Template, long> _templatenotificationRepository;
        private readonly IRepository<TableTopExerciseGroupSection> _tableTopExerciseGroupSectionRepository;
        private readonly IRepository<TableTopExerciseSection, long> _tabletopExerciseSectionRepository;
        private readonly IRepository<TableTopExerciseSectionQuestion, long> _tableTopExerciseSectionQuestionRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly ICommonLookupAppService _commonlookupManagerRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;

        private readonly IUserEmailer _userEmailerRepository;
        public IAppUrlService AppUrlService { get; set; }

        private string _emailButtonStyle =
         "padding-left: 30px; padding-right: 30px; padding-top: 8px; padding-bottom: 8px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";
        private string _emailButtonColor = "#00bb77";

        public TableTopExerciseAppService(IRepository<TableTopExerciseSection, long> tabletopExerciseSectionRepository, IRepository<EntityGroup> entityGroupRepository, ICommonLookupAppService commonlookupManagerRepository, IRepository<TableTopExerciseQuestionOption> tableTopExerciseQuestionOptionRepository, IUserEmailer userEmailerRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<User, long> userRepository, IRepository<EntityGroupMember> entityGroupMemberRepository, IRepository<TableTopExerciseSectionQuestion, long> tableTopExerciseSectionQuestionRepository, IRepository<TableTopExerciseGroupSection> tableTopExerciseGroupSectionRepository, IRepository<Template, long> templatenotificationRepositor, IRepository<WorkFlowPage, long> workflowpageRepository, IRepository<TableTopExerciseGroup, long> tableTopExerciseGroupRepository, IRepository<TableTopExerciseQuestion, long> tableTopExerciseQuestionRepository,
            IRepository<TableTopExerciseEntity, long> tableTopExerciseEntityRepository, IRepository<TableTopExerciseEntityResponse, long> tableTopExerciseEntityResponseRepository, IRepository<EntityApplicationSetting> entityApplicationSettingRepository)
        {
            _tabletopExerciseSectionRepository = tabletopExerciseSectionRepository;
            _entityGroupRepository = entityGroupRepository;
            _commonlookupManagerRepository = commonlookupManagerRepository;
            _tableTopExerciseQuestionOptionRepository = tableTopExerciseQuestionOptionRepository;
            _userEmailerRepository = userEmailerRepository;
            _businessEntityRepository = businessEntityRepository;
            _userRepository = userRepository;
            AppUrlService = NullAppUrlService.Instance;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _tableTopExerciseSectionQuestionRepository = tableTopExerciseSectionQuestionRepository;
            _tableTopExerciseGroupSectionRepository = tableTopExerciseGroupSectionRepository;
            _templatenotificationRepository = templatenotificationRepositor;
            _workflowpageRepository = workflowpageRepository;
            _tableTopExerciseQuestionRepository = tableTopExerciseQuestionRepository;
            _tableTopExerciseEntityRepository = tableTopExerciseEntityRepository;
            _tableTopExerciseEntityResponseRepository = tableTopExerciseEntityResponseRepository;
            _tableTopExerciseGroupRepository = tableTopExerciseGroupRepository;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
        }

        public async Task<PagedResultDto<GetTableTopExerciseQuestionDto>> GetAll(GetTableTopExerciseQuestionInput input)
        {
            try
            {
                var gettabletopexcerciesQuestion = _tableTopExerciseQuestionRepository.GetAll()
                                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter.Trim().ToLower()));

                var gettabletopexcerciesQuestions = await gettabletopexcerciesQuestion
                  .OrderBy(input.Sorting ?? "id desc")
                  .PageBy(input)
                  .Select(e => new GetTableTopExerciseQuestionDto()
                  {
                      Code = e.Code,
                      Id = e.Id,
                      Name = e.Name,
                      Description = e.Description
                  }).ToListAsync();


                int totalCount = await gettabletopexcerciesQuestion.CountAsync();

                return new PagedResultDto<GetTableTopExerciseQuestionDto>(
                   totalCount,
                    gettabletopexcerciesQuestions
               );
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task CreateOrUpdateTabletopExerciseQuestion(CreateOrEditTableTopExerciseQuestionDto input)
        {
            if (input.Id > 0)
            {
                await UpdateTabletopExerciseQuestionAsync(input);
            }
            else
            {
                await CreateTabletopExerciseQuestionAsync(input);
            }
        }

        protected virtual async Task CreateTabletopExerciseQuestionAsync(CreateOrEditTableTopExerciseQuestionDto input)
        {
            try
            {
                var tabletopExerciseQuestion = ObjectMapper.Map<TableTopExerciseQuestion>(input);

                if (AbpSession.TenantId != null)
                {
                    tabletopExerciseQuestion.TenantId = (int?)AbpSession.TenantId;
                }
                await _tableTopExerciseQuestionRepository.InsertAsync(tabletopExerciseQuestion);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        protected virtual async Task UpdateTabletopExerciseQuestionAsync(CreateOrEditTableTopExerciseQuestionDto input)
        {
            try
            {
                var tabletopExerciseQuestion = await _tableTopExerciseQuestionRepository.GetAll().Include(x => x.TableTopExerciseQuestionOption).Where(x => x.Id == (int)input.Id).FirstOrDefaultAsync();

                tabletopExerciseQuestion.TableTopExerciseQuestionOption.ToList().ForEach(async x =>
                {
                    await _tableTopExerciseQuestionOptionRepository.DeleteAsync(x);
                });

                if (AbpSession.TenantId != null)
                {
                    tabletopExerciseQuestion.TenantId = (int?)AbpSession.TenantId;
                }
                tabletopExerciseQuestion.Name = input.Name;
                tabletopExerciseQuestion.Description = input.Description;
                tabletopExerciseQuestion.AnswerType = input.AnswerType;
                tabletopExerciseQuestion.Mandatory = input.Mandatory;
                tabletopExerciseQuestion.CommentMandatory = input.CommentMandatory;
                tabletopExerciseQuestion.CommentRequired = input.CommentRequired;

                tabletopExerciseQuestion.TableTopExerciseQuestionOption = ObjectMapper.Map<List<TableTopExerciseQuestionOption>>(input.TableTopExerciseQuestionOption);
                long id = await _tableTopExerciseQuestionRepository.InsertOrUpdateAndGetIdAsync(tabletopExerciseQuestion);




            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task Delete(EntityDto input)
        {
            try
            {
                var resultSectionQuestion = await _tableTopExerciseSectionQuestionRepository.GetAll().Where(x => x.TableTopExerciseQuestionId == input.Id).FirstOrDefaultAsync();
                var resultEntityResponse = await _tableTopExerciseEntityResponseRepository.GetAll().Where(x => x.TableTopExerciseSectionId == input.Id).FirstOrDefaultAsync();

                if (resultSectionQuestion != null)
                {
                    throw new UserFriendlyException("This Question already present in Table top excercise Section");
                }
                if (resultEntityResponse != null)
                {
                    throw new UserFriendlyException("This Question already assign to Table top excercise entity");
                }
                await _tableTopExerciseQuestionRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<CreateOrEditTableTopExerciseQuestionDto> GetTabletopExerciseQuestionById(EntityDto input)
        {
            try
            {

                var TableTopExerciseQuestions = await _tableTopExerciseQuestionRepository.GetAll().Include(x => x.TableTopExerciseQuestionOption).Where(x => x.Id == input.Id).FirstOrDefaultAsync();

                var output = ObjectMapper.Map<CreateOrEditTableTopExerciseQuestionDto>(TableTopExerciseQuestions);

                return output;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task<List<GetAllGroupListDto>> GetallGroupList()
        {
            var result = new List<GetAllGroupListDto>();
            try
            {
                result = await _tableTopExerciseGroupRepository.GetAll().Select(x => new GetAllGroupListDto()
                {
                    Id = x.Id,
                    TableTopExerciseGroupName = x.TableTopExerciseGroupName
                }).ToListAsync();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region TTE_Entity_Attachment

        public async Task<bool> CreatetableTopExerciseEntity(CreateTTXEntityRequestDto input)
        {
            try
            {
                long TableTopExerciseEntityId = 0;
                bool result = false;

                var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.Trim().ToLower() == LockthreatComplianceConsts.TTX_PageName.Trim().ToLower());
                var getTemplate = await _templatenotificationRepository.GetAll().Where(x => x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
                if (getTemplate != null)
                {
                    input.BusinessEntityId.ForEach(obj =>
                   {
                       string Ttxbody = null;
                       string ttxsubject = null;
                       HashSet<string> toemails = new HashSet<string>();
                       HashSet<string> ccemails = new HashSet<string>();
                       HashSet<string> bccemails = new HashSet<string>();

                       var tabletopExerciseentity = new TableTopExerciseEntity()
                       {

                           BusinessEntityId = obj,
                           TableTopExerciseGroupId = input.TableTopExerciseGroupId,
                           CreationTime = DateTime.UtcNow,
                           CreatorUserId = AbpSession.UserId,
                           TenantId = AbpSession.TenantId
                       };

                       var getsection = _tableTopExerciseGroupSectionRepository.GetAll().Where(x => x.TableTopExerciseGroupId == input.TableTopExerciseGroupId).Select(x => x.TableTopExerciseSectionId).ToList();                     

                       if (getsection.Count() > 0)
                       {

                           TableTopExerciseEntityId = _tableTopExerciseEntityRepository.InsertAndGetId(tabletopExerciseentity);

                           getsection.ForEach(item =>
                           {
                               var getQuestion = _tableTopExerciseSectionQuestionRepository.GetAll().Include(x => x.TableTopExerciseQuestion).Where(x => x.TableTopExerciseSectionId == item).ToList();

                               if (getQuestion.Count() > 0)
                               {
                                   getQuestion.ForEach(obj =>
                                   {
                                       var TableTopExerciseEntityResponse = new TableTopExerciseEntityResponse()
                                       {
                                           CommentRequired = obj.TableTopExerciseQuestion.CommentRequired,
                                           TableTopExerciseSectionId = obj.TableTopExerciseSectionId,
                                           AnswerType = obj.TableTopExerciseQuestion.AnswerType,
                                           CommentMandatory = obj.TableTopExerciseQuestion.CommentMandatory,
                                           TableTopExerciseEntityId = TableTopExerciseEntityId,
                                           TableTopExerciseQuestionId = obj.TableTopExerciseQuestionId,
                                           CreatorUserId = AbpSession.UserId,
                                           CreationTime = DateTime.UtcNow
                                       };
                                  long TTXentityId= _tableTopExerciseEntityResponseRepository.InsertAndGetId(TableTopExerciseEntityResponse);

                                   });
                               }
                               else
                               {
                                   var TableTopExerciseEntityResponse = new TableTopExerciseEntityResponse()
                                   {
                                       CommentRequired = false,
                                       TableTopExerciseSectionId = item,
                                       AnswerType = AnswerType.Input,
                                       CommentMandatory = false,
                                       TableTopExerciseEntityId = TableTopExerciseEntityId,
                                       TableTopExerciseQuestionId = null,
                                       CreatorUserId = AbpSession.UserId,
                                       CreationTime = DateTime.UtcNow
                                   };
                                long ttxresponseId=   _tableTopExerciseEntityResponseRepository.InsertAndGetId(TableTopExerciseEntityResponse);
                               }
                           });




                           var businessEntities = _businessEntityRepository.GetAll().Where(x => x.Id == obj).ToList();

                           List<string> templateSubject = new List<string>();
                           var ttxtemplateSubject = getTemplate.TemplateSubject;
                           ttxsubject = getTemplate.TemplateSubject.ToString();

                           while (ttxtemplateSubject.Contains("{"))
                           {
                               templateSubject.Add("{" + ttxtemplateSubject.Split('{', '}')[1] + "}");
                               ttxtemplateSubject = ttxtemplateSubject.Replace("{" + ttxtemplateSubject.Split('{', '}')[1] + "}", "");
                           };

                           ttxsubject = ReplaceSubjectFunction(templateSubject, ttxsubject, businessEntities);

                           List<string> templatevariables = new List<string>();
                           List<string> toLists = getTemplate.TemplateTo.Split(',').ToList();

                           toLists.ForEach(emailid =>
                           {
                               if (emailid.Contains("{"))
                               {
                                   templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                               }
                               else
                               {
                                   string email = emailid.Trim();
                                   bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                   if (isEmail == true)
                                   {
                                       toemails.Add(email);
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
                                       ccemails.Add(email);
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
                                       bccemails.Add(email);
                                   }
                               }
                           });

                           var queryto = GetToCCBCCMail(templatevariables, businessEntities);
                           queryto.Result.ForEach(obj =>
                           {
                               toemails.Add(obj);
                           });

                           var querycc = GetToCCBCCMail(templateCc, businessEntities);
                           querycc.Result.ForEach(obj =>
                           {
                               ccemails.Add(obj);
                           });

                           var querybcc = GetToCCBCCMail(templatebCc, businessEntities);
                           querybcc.Result.ForEach(obj =>
                           {
                               bccemails.Add(obj);
                           });


                           List<string> templateBody = new List<string>();
                           var TTXEntityBody = getTemplate.TemplateBody;

                           Ttxbody = getTemplate.TemplateBody.ToString();

                           while (TTXEntityBody.Contains("{"))
                           {
                               templateBody.Add("{" + TTXEntityBody.Split('{', '}')[1] + "}");
                               TTXEntityBody = TTXEntityBody.Replace("{" + TTXEntityBody.Split('{', '}')[1] + "}", "");
                           };

                           Ttxbody = ReplaceBodyFucntion(templateBody, Ttxbody, TableTopExerciseEntityId);

                           result = true;

                           _userEmailerRepository.SendMailForPAPStatus(toemails, ccemails, bccemails, ttxsubject, (int)AbpSession.TenantId, Ttxbody, getpage.Id);

                       }



                   });
                }

                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }



        private async Task<List<string>> GetToCCBCCMail(List<string> templatevariables, List<BusinessEntity> businessEntityslist)
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
                                if (yy.OfficialEmail != null)
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


                }
            });

            return result;

        }
        #endregion
        private string ReplaceBodyFucntion(List<string> input, string output, long TTXexrciseGroupId)
        {
            var TTXEntitybody = output;
            var mailMessage = new StringBuilder();
            input.ForEach(x =>
            {
                switch (x)
                {

                    case "{Link}":
                        {

                            var link = AppUrlService.CreateTtxNotificationUrlFormat(AbpSession.TenantId.Value, TTXexrciseGroupId);
                            var temp = link.Split("/public/");
                            link = "" + temp[0] + "/#/public/" + temp[1];
                            if (!link.IsNullOrEmpty())
                            {
                                link = EncryptauditProjectQueryParameters(link);
                            }
                            if (!link.IsNullOrEmpty())
                            {
                                //link = EncryptQueryParameters(link);
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("Submit") + "</a>");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
                            }
                            TTXEntitybody = TTXEntitybody.Replace("{Link}", mailMessage.ToString());
                            break;
                        }


                }

            });
            return TTXEntitybody;
        }


        private string ReplaceSubjectFunction(List<string> input, string output, List<BusinessEntity> businessEntityslist)
        {
            var TTXsubject = output;
            var mailMessage = new StringBuilder();
            var BusinessEntityget = businessEntityslist.FirstOrDefault();
            input.ForEach(x =>
            {
                switch (x)
                {

                    case "{Link}":
                        {
                            break;

                        }

                    case "{Business_Entity_Name}":
                        {
                            TTXsubject = TTXsubject.Replace("{Business_Entity_Name}", BusinessEntityget.CompanyLegalName);
                            break;
                        }

                    case "{Group_Name}":
                        {
                            var groupname = _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == BusinessEntityget.Id).FirstOrDefault();
                            if (groupname != null)
                            {
                                var name = _entityGroupRepository.GetAll().Where(x => x.Id == groupname.EntityGroupId).FirstOrDefault();
                                if (name != null)
                                {
                                    TTXsubject = TTXsubject.Replace("{Group_Name}", name.Name.ToString());
                                }
                            }

                            break;
                        }
                }

            });
            return TTXsubject;


        }

        public async Task<List<GetAllSectionGroupListDto>> GetallTabletopExerciseGroup()
        {
            try
            {
                var result = new List<GetAllSectionGroupListDto>();
                try
                {
                    result = await _tableTopExerciseGroupRepository.GetAll().Select(x => new GetAllSectionGroupListDto()
                    {
                        Id = x.Id,
                        TableTopExerciseGroupName = x.TableTopExerciseGroupName,
                    }).ToListAsync();

                    return result;

                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private string EncryptauditProjectQueryParameters(string link, string encrptedParameterName = "tteId")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');
            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }
        #region TTEEntityResponse 

        protected static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = "data:image/gif;base64," + Convert.ToBase64String(imageBytes);
            return base64String;
        }

        public async Task<GetTTEEntityReponsesDto> GetTTEEntityResponsesByTTEEntityId(int input)
        {
            try
            {
                var result = new GetTTEEntityReponsesDto();
                string webRootPath = await _entityApplicationSettingRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).Select(x => x.Attachmentpath).FirstOrDefaultAsync();
                var globalpath = System.IO.Path.Combine(webRootPath, "TTXFiles");
                var globalpath1 = globalpath.Replace('\\', '/');

                var tteObj = await _tableTopExerciseEntityRepository.GetAll().Include(x => x.TableTopExerciseGroup).Include(x => x.TableTopExerciseEntityAttachments).Where(x => x.Id == input).FirstOrDefaultAsync();

                if (tteObj != null)
                {
                    result.Submitted = tteObj.Submitted;
                    result.GroupName = tteObj.TableTopExerciseGroup.TableTopExerciseGroupName;
                    result.GroupDescription = tteObj.TableTopExerciseGroup.TableTopExerciseDescription;
                    result.TableTopExerciseEntityAttachments = ObjectMapper.Map<List<TableTopExerciseEntityAttachmentDto>>(tteObj.TableTopExerciseEntityAttachments);
                }

                var TTEEntityResponses = await _tableTopExerciseEntityResponseRepository.GetAll().Include(x => x.TableTopExerciseEntity).Include(x => x.TableTopExerciseSection).ThenInclude(x => x.TableTopExerciseSectionAttachement)
                    .Include(x => x.TableTopExerciseQuestion)
                   .ThenInclude(x => x.TableTopExerciseQuestionOption)
                   . Where(x => x.TableTopExerciseEntityId == input)
                    .Select(x => new TableTopExerciseEntityResponseDto
                    {
                        Id = x.Id,
                        AnswerType = x.AnswerType,
                        TableTopExerciseEntityId = x.TableTopExerciseEntityId,
                        TableTopExerciseSectionId = x.TableTopExerciseSectionId,
                       TableTopExerciseQuestionId =(long) x.TableTopExerciseQuestionId,
                        QuestionComment = x.QuestionComment,
                        QuestionMandatory = x.TableTopExerciseQuestion.Mandatory,
                        CommentRequired = x.CommentRequired,
                        CommentMandatory = x.CommentMandatory,
                        Response = x.Response,
                        QuestionName = x.TableTopExerciseQuestion.Name,
                        SectionName = x.TableTopExerciseSection.SectionName,
                        CounterLimit = x.TableTopExerciseSection.CounterLimit,
                      //  TableTopExerciseSectionAttachements = ObjectMapper.Map<List<TableTopExerciseSectionAttachementDto>>(x.TableTopExerciseSection.TableTopExerciseSectionAttachement),
                        TableTopExerciseSectionAttachements = x.TableTopExerciseSection.TableTopExerciseSectionAttachement.Select(xy => new TableTopExerciseSectionAttachementDto
                        {
                            FileName = xy.FileName,
                            Title = globalpath1 + "/" + xy.Code,
                            Code = xy.Code,
                            FullPath = GetBase64StringForImage(globalpath + "\\" + xy.Code)
                        }).ToList(),
                       ResponseOptions = x.TableTopExerciseQuestion.TableTopExerciseQuestionOption.Select(x => new CodeNameDto { Code = x.Value, Name = x.Value }).ToList()
                    }).ToListAsync();

                var testResult = TTEEntityResponses.GroupBy(x => x.SectionName).Select(x => new SectionAttachmentQuestion
                {
                    SectionName = x.Key,
                    CounterLimit = x.FirstOrDefault().CounterLimit,
                    TableTopExerciseSectionAttachements = x.FirstOrDefault().TableTopExerciseSectionAttachements,
                    TableTopExerciseEntityResponses = x.ToList()
                }).ToList();

                //result.TableTopExerciseEntityResponses = (TTEEntityResponses);
                result.SectionAttachmentQuestions = testResult;

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }

        public async Task UpdateTTEEntityResponses(List<TableTopExerciseEntityResponseDto> input)
        {
            try
            {
                input.ForEach(async x =>
                {
                    var id = await _tableTopExerciseEntityResponseRepository.InsertOrUpdateAndGetIdAsync(ObjectMapper.Map<TableTopExerciseEntityResponse>(x));
                });

                var tteObj = await _tableTopExerciseEntityRepository.GetAll().Where(x => x.Id == input.FirstOrDefault().TableTopExerciseEntityId).FirstOrDefaultAsync();
                tteObj.Submitted = true;
                var id = await _tableTopExerciseEntityRepository.InsertOrUpdateAndGetIdAsync(tteObj);

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
        }


        #endregion


        public async Task<PagedResultDto<TabletopExerciseEntityList>> GetAllTabletopExerciseEntity(TableTopExerciseEntityDtoInput input)
        {
            try
            {
                var getcheckUser = await _commonlookupManagerRepository.GetallBusinessEntity();


                var getttxentityresponse = _tableTopExerciseEntityRepository.GetAll()
                                   .Include(x => x.BusinessEntity).Include(x => x.TableTopExerciseGroup)
                                   .Where(x => x.TableTopExerciseGroup.IsDeleted == false)
                                   .WhereIf(!getcheckUser.Isadmin && getcheckUser.BusinessEntityId.Count > 0, e => getcheckUser.BusinessEntityId.Contains(e.BusinessEntityId) && e.BusinessEntity.Status == EntityTypeStatus.Active)
                                   .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u => u.BusinessEntity.CompanyLegalName.Contains(input.Filter.Trim().ToLower()) ||
                                   u.BusinessEntity.LicenseNumber.Contains(input.Filter.Trim().ToLower()));


                var getttxentityresponses = await getttxentityresponse
                 .OrderBy(input.Sorting ?? "id desc")
                 .PageBy(input)
                 .Select(xx => new TabletopExerciseEntityList()
                 {
                     Code = xx.Code,
                     Id = xx.Id,
                     EntityName = xx.BusinessEntity.CompanyLegalName,
                     TableTopExerciseGroupName = xx.TableTopExerciseGroup.TableTopExerciseGroupName,
                     LicenseNumber = xx.BusinessEntity.LicenseNumber,
                     Submitted = xx.Submitted
                 }).ToListAsync();


                int totalCount = await getttxentityresponse.CountAsync();

                return new PagedResultDto<TabletopExerciseEntityList>(
                   totalCount,
                    getttxentityresponses
               );
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
