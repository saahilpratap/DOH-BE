using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using LockthreatCompliance.Feedback;
using LockthreatCompliance.FeedBacks.Dtos;
using LockthreatCompliance.Questions.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Abp.Authorization;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Url;
using System.Web;
using Abp.Runtime.Security;
using System.Text.RegularExpressions;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Authorization.Accounts.Dto;
using LockthreatCompliance.WorkFllows.Dto;
using LockthreatCompliance.Dto;
using LockthreatCompliance.FeedBacks.Exporting;

namespace LockthreatCompliance.FeedBacks
{
    [AbpAuthorize]
    public class FeedbacksAppService : LockthreatComplianceAppServiceBase, IFeedbacksAppService
    {
        private readonly IRepository<FeedBackQuestioner> _feedBackQuestionerRepository;
        private readonly IRepository<FeedbackQuestionAnswerOption> _feedbackQuestionAnswerOptionnRepository;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<FeedbackDetail> _feedbackDetailRepository;
        private readonly IRepository<FeedbackDetailQuestion> _feedbackdetailQuestionRepository;
        private readonly IRepository<FeedBackEntity> _feedbackentityRepository;
        private readonly IRepository<FeedBackEntityResponse> _feedBackEntityResponseRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<CertificateImport> _certificateImportRepository;
        private readonly IFeedBackExcelExporter _feedbackresponseExcelExporter;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IUserEmailer _userEmailer;

        private string _emailButtonStyle =
           "padding-left: 30px; padding-right: 30px; padding-top: 8px; padding-bottom: 8px; color: #ffffff; background-color: #00bb77; font-size: 14pt; text-decoration: none;";
        private string _emailButtonColor = "#00bb77";
        public FeedbacksAppService(IUserEmailer userEmailer, IFeedBackExcelExporter feedbackresponseExcelExporter,
            IRepository<CertificateImport> certificateImportRepository, IRepository<BusinessEntity> businessEntityRepository, IRepository<EmailNotificationTemplate, long> emailnotificationRepository, IRepository<WorkFlowPage, long> workflowpageRepository, IRepository<FeedbackDetail> feedbackDetailRepository, IRepository<FeedBackEntityResponse> feedBackEntityResponseRepository, IRepository<FeedBackEntity> feedbackentityRepository, IRepository<FeedbackDetailQuestion> feedbackdetailQuestionRepository, IRepository<FeedBackQuestioner> feedBackQuestionerRepository, IRepository<FeedbackQuestionAnswerOption> feedbackQuestionAnswerOptionnRepository, IHostingEnvironment hostingEnvironment)
        {
            _feedbackresponseExcelExporter = feedbackresponseExcelExporter;
            _certificateImportRepository = certificateImportRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _businessEntityRepository = businessEntityRepository;
            _emailnotificationRepository = emailnotificationRepository;
            _workflowpageRepository = workflowpageRepository;
            _feedBackEntityResponseRepository = feedBackEntityResponseRepository;
            _feedbackentityRepository = feedbackentityRepository;
            _feedbackdetailQuestionRepository = feedbackdetailQuestionRepository;
            _feedbackDetailRepository = feedbackDetailRepository;
            _feedBackQuestionerRepository = feedBackQuestionerRepository;
            _feedbackQuestionAnswerOptionnRepository = feedbackQuestionAnswerOptionnRepository;
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task DeleteFeedback(EntityDto input)
        {
            await _feedBackQuestionerRepository.DeleteAsync(input.Id);
        }

        public async Task CreateOrEditFeedbackQuestion(CreateOrEditFeedbackQuestionDto input)
        {
            if (input.removedAnswers != null)
            {
                foreach (var id in input.removedAnswers)
                {
                    var ans = await _feedbackQuestionAnswerOptionnRepository.FirstOrDefaultAsync(a => a.Id == id);
                    await _feedbackQuestionAnswerOptionnRepository.DeleteAsync(ans);
                }
            }

            var data = ObjectMapper.Map<FeedBackQuestioner>(input);
            data.AnswerOptions = ObjectMapper.Map<List<FeedbackQuestionAnswerOption>>(input.AnswerOptions);
            data.TenantId = AbpSession.TenantId;
            await _feedBackQuestionerRepository.InsertOrUpdateAsync(data);
        }

        public async Task<CreateOrEditFeedbackQuestionDto> GetFeedbackQuestionForEdit(EntityDto input)
        {
            var question = await _feedBackQuestionerRepository.GetIncluding(e => e.Id == input.Id, "AnswerOptions");

            var output = ObjectMapper.Map<CreateOrEditFeedbackQuestionDto>(question);

            return output;
        }



        public async Task<PagedResultDto<GetAllFeedbackQuestionDto>> GetAllFeedbackQuestion(GetAllQuestionsInput input)
        {
            var filteredQuestions = _feedBackQuestionerRepository.GetAll()
                .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                u =>
                    u.Name.Contains(input.Filter.Trim().ToLower()) ||
                    u.Description.Contains(input.Filter.Trim().ToLower())
            );

            var pagedAndFilteredQuestions = filteredQuestions
                .OrderBy(input.Sorting)
                .PageBy(input);

            var questions = pagedAndFilteredQuestions.Select(
                            e => new GetAllFeedbackQuestionDto
                            {
                                Code = e.Code,
                                Name = e.Name,
                                Description = e.Description,
                                Id = e.Id
                            });

            var totalCount = await filteredQuestions.CountAsync();

            return new PagedResultDto<GetAllFeedbackQuestionDto>(
                totalCount,
                await questions.ToListAsync()
            );
        }

        public async Task<PagedResultDto<FeedBackDetailDto>> GetallFeedbackQuestionDetail(FeedbackDetailQuestionInputDto input)
        {
            var filteredQuestions = _feedbackDetailRepository.GetAll().Include(x => x.FeedbackDetailQuestions)
                .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                u =>
                    u.Title.Contains(input.Filter.Trim().ToLower())
            );

            var pagedAndFilteredQuestions = filteredQuestions
                .OrderBy(input.Sorting)
                .PageBy(input);

            var questions = pagedAndFilteredQuestions.Select(
                            e => new FeedBackDetailDto
                            {
                                Id = e.Id,
                                Code = e.Code,
                                Title = e.Title,
                                ActionDate = e.ActionDate

                            });


            var totalCount = await filteredQuestions.CountAsync();

            return new PagedResultDto<FeedBackDetailDto>(
                totalCount,
                  await questions.ToListAsync()
            );
        }


        public async Task<PagedResultDto<EntityFeedBackDto>> GetEntityFeedbackResponse(EntityFeedbackInputDto input)
        {
            var filteredQuestions = _feedbackentityRepository.GetAll().Include(x => x.FeedBackEntityResponses)
                .WhereIf(
                !input.Filter.IsNullOrWhiteSpace(),
                u =>
                    u.FeedbackDetail.Title.Contains(input.Filter.Trim().ToLower()) || u.BusinessEntity.CompanyLegalName.Contains(input.Filter.Trim().ToLower())
            );

            var pagedAndFilteredQuestions = filteredQuestions
                .OrderBy(input.Sorting)
                .PageBy(input);

            var questions = pagedAndFilteredQuestions.Select(
                            e => new EntityFeedBackDto
                            {
                                Id = e.Id,
                                Code = e.Code,
                                BusinessEntityName = e.BusinessEntity.CompanyLegalName,
                                FeedbackName = e.FeedbackDetail.Title

                            });


            var totalCount = await filteredQuestions.CountAsync();

            return new PagedResultDto<EntityFeedBackDto>(
                totalCount,
                  await questions.ToListAsync()
            );
        }


        public async Task<FeedBackDetailDto> GetFeedbackDetailForEdit(EntityDto input)
        {
            var questionfeedback = new FeedBackDetailDto();

            questionfeedback = await _feedbackDetailRepository.GetAll().Where(x => x.Id == input.Id).Include(x => x.FeedbackDetailQuestions).
               Select(x => new FeedBackDetailDto()
               {
                   Code = x.Code,
                   Id = x.Id,
                   Title = x.Title,
                   ActionDate = x.ActionDate,
                   LinkValidationDay = x.LinkValidationDay
               }).FirstOrDefaultAsync();

            questionfeedback.FeedbackDetailQuestions = _feedbackdetailQuestionRepository.GetAll().Where(x => x.FeedbackDetailId == input.Id).
                Select(x => new FeedbackQuestionDto
                {
                    Id = x.Id,
                    Description = x.Question.Description,
                    QuestionId = x.QuestionId,
                    QuestionSrNo = x.QuestionSrNo
                }).ToList();


            return questionfeedback;


        }
        public async Task CreateFeedBackDetail(FeedBackDetailDto input)
        {
            int FeedbackId = 0;
            if (input.Id == null || input.Id == 0)
            {
                var question = new FeedbackDetail()
                {
                    Title = input.Title,
                    ActionDate = input.ActionDate,
                    LinkValidationDay = input.LinkValidationDay
                };

                if (AbpSession.TenantId != null)
                {
                    question.TenantId = (int?)AbpSession.TenantId;
                }
                FeedbackId = await _feedbackDetailRepository.InsertAndGetIdAsync(question);

                input.FeedbackDetailQuestions.ForEach(obj =>
                {
                    var item = new FeedbackDetailQuestion()
                    {
                        QuestionId = obj.QuestionId,
                        FeedbackDetailId = FeedbackId,
                        QuestionSrNo = obj.QuestionSrNo
                    };
                    _feedbackdetailQuestionRepository.Insert(item);
                });


            }
            else
            {
                var controlRequirement = await _feedbackDetailRepository.GetAll().Where(x => x.Id == input.Id).Include(x => x.FeedbackDetailQuestions).FirstOrDefaultAsync();

                controlRequirement.FeedbackDetailQuestions = input.FeedbackDetailQuestions.Select(e => new FeedbackDetailQuestion
                {
                    QuestionId = e.QuestionId,
                    QuestionSrNo = e.QuestionSrNo

                }).ToList();
            }

        }

        public async Task DeletefeedbackDetail(EntityDto input)
        {
            await _feedbackDetailRepository.DeleteAsync(input.Id);
        }

        public async Task<bool> CrateFeedbackEntity(BusinessEntityFeedbackIds input)
        {
            int feedbackentityId = 0;

            try
            {
                var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.FeedbackPage.Trim().ToLower());
                var getTemplate = await _emailnotificationRepository.GetAll().Where(x => x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
                if (getTemplate != null)
                {
                    input.BusinessEntityId.ForEach(obj =>
                    {
                    string auditbody = null;
                    string AuditEmailsubject = null;
                    List<string> emails = new List<string>();
                    List<string> ccemail = new List<string>();
                    List<string> bccemail = new List<string>();

                    var feedbackentity = new FeedBackEntity()
                    {
                        BusinessEntityId = obj,
                        FeedbackDetailId = input.FeedbackDetailId,
                        TenantId = AbpSession.TenantId
                    };
                    feedbackentityId = _feedbackentityRepository.InsertAndGetId(feedbackentity);

                    var getquestion = _feedbackdetailQuestionRepository.GetAll().Where(x => x.FeedbackDetailId == input.FeedbackDetailId).ToList();

                    getquestion.ForEach(obj =>
                    {
                        var feedbackentityresponse = new FeedBackEntityResponse()
                        {
                            FeedBackEntityId = feedbackentityId,
                            QuestionId = obj.QuestionId,
                        };

                        _feedBackEntityResponseRepository.Insert(feedbackentityresponse);
                    });


                    var item = _businessEntityRepository.GetAll().Where(x => x.Id == obj).FirstOrDefault();

                    List<string> templateSubject = new List<string>();
                    var auditprojectsubjectBody = getTemplate.Subject;

                    AuditEmailsubject = getTemplate.Subject.ToString();

                    while (auditprojectsubjectBody.Contains("{"))
                    {
                        templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                        auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                    };


                    AuditEmailsubject = ReplaceValueFunction(item, templateSubject, AuditEmailsubject, feedbackentityId);

                    var auditTemplate = getTemplate.EmailBody;

                    var auditTo = getTemplate.To;
                    List<string> auditToList = getTemplate.To.Split(',').ToList();
                    List<string> templatevariables = new List<string>();

                    auditToList.ForEach(emailid =>
                    {
                        if (emailid.Contains("{"))
                        {
                            templatevariables.Add("{" + emailid.Split('{', '}')[1] + "}");
                                //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                            }
                        else
                        {
                            string email = emailid.Trim();
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                emails.Add(email);
                            }
                        }
                    });

                    var auditCc = getTemplate.Cc;
                    List<string> auditCcList = getTemplate.Cc.Split(',').ToList();
                    List<string> templateCc = new List<string>();

                    auditCcList.ForEach(emailid =>
                    {
                        if (emailid.Contains("{"))
                        {
                            templateCc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
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

                    templatevariables.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    emails.Add(item.AdminEmail);
                                    break;
                                }
                            case "{Audit_Agency_Admin_Email}":
                                {
                                        //var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                        //if (getbusinessadmin != null)
                                        //{
                                        //    emails.Add(getbusinessadmin.AdminEmail);
                                        //}
                                        break;
                                }
                            case "{Owner_Email}":
                                {
                                    if (item.Owner_Email != null)
                                    {
                                        var splitEmail = item.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    if (item.Director_Incharge_Email != null)
                                    {
                                        var splitEmail = item.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{CISO_Email}":
                                {
                                    if (item.CISO_Email != null)
                                    {
                                        var splitEmail = item.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    if (item.OfficialEmail != null)
                                    {
                                        var splitEmail = item.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    if (item.BackupOfficialEmail != null)
                                    {
                                        var splitEmail = item.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                emails.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{LeadAuditor_Email}":
                                {

                                    break;
                                }

                        }
                    });

                    templateCc.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    ccemail.Add(item.AdminEmail);
                                    break;
                                }
                            case "{Audit_Agency_Admin_Email}":
                                {
                                        //var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                        //if (getbusinessadmin != null)
                                        //{
                                        //    ccemail.Add(getbusinessadmin.AdminEmail);
                                        //}
                                        break;
                                }
                            case "{Owner_Email}":
                                {
                                    if (item.Owner_Email != null)
                                    {
                                        var splitEmail = item.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    if (item.Director_Incharge_Email != null)
                                    {
                                        var splitEmail = item.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{CISO_Email}":
                                {
                                    if (item.CISO_Email != null)
                                    {
                                        var splitEmail = item.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    if (item.OfficialEmail != null)
                                    {
                                        var splitEmail = item.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    if (item.BackupOfficialEmail != null)
                                    {
                                        var splitEmail = item.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                ccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{LeadAuditor_Email}":
                                {

                                    break;
                                }

                        }

                    });

                    var auditBcc = getTemplate.Bcc;
                    List<string> auditBccList = getTemplate.Bcc.Split(',').ToList();
                    List<string> templateBcc = new List<string>();

                    auditBccList.ForEach(emailid =>
                    {
                        if (emailid.Contains("{"))
                        {
                            templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
                                //  auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                            }
                        else
                        {
                            string email = emailid.Trim();
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                bccemail.Add(email);
                            }
                        }
                    });

                    templateBcc.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    bccemail.Add(item.AdminEmail);
                                    break;
                                }
                            case "{Audit_Agency_Admin_Email}":
                                {

                                    break;
                                }
                            case "{Owner_Email}":
                                {
                                    if (item.Owner_Email != null)
                                    {
                                        var splitEmail = item.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    if (item.Director_Incharge_Email != null)
                                    {
                                        var splitEmail = item.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{CISO_Email}":
                                {
                                    if (item.CISO_Email != null)
                                    {
                                        var splitEmail = item.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    if (item.OfficialEmail != null)
                                    {
                                        var splitEmail = item.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    if (item.BackupOfficialEmail != null)
                                    {
                                        var splitEmail = item.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (var i in splitEmail)
                                        {
                                            string email = i.Trim();
                                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                            if (isEmail == true)
                                            {
                                                bccemail.Add(i);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{LeadAuditor_Email}":
                                {

                                    break;
                                }

                        }

                    });

                    List<string> templateBody = new List<string>();
                    var auditprojectBody = getTemplate.EmailBody;

                    auditbody = getTemplate.EmailBody.ToString();

                    while (auditprojectBody.Contains("{"))
                    {
                        templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                        auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                    };
                    auditbody = ReplaceValueFunction(item, templateBody, auditbody, feedbackentityId);

                    _userEmailer.FeedbackResponseNotification(emails, ccemail, bccemail, AuditEmailsubject, (int)item.TenantId, auditbody, null,
                       AppUrlService.CreateFeedbackSubmitUrlFormat(AbpSession.TenantId.Value, (long)feedbackentityId));
                });
                    return true;
                }
                else
                {
                    throw new UserFriendlyException("Please configure feedback entity template");
                }

                  
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string ReplaceValueFunction(BusinessEntity item, List<string> input, string output, long feedbackentityId)
        {
            var Feedbacksubject = output;
            var getbusinessentity = item;

            var mailMessage = new StringBuilder();
            input.ForEach(x =>
            {
                switch (x)
                {

                    case "{Link}":
                        {
                            var link = AppUrlService.CreateFeedbackSubmitUrlFormat(AbpSession.TenantId.Value, feedbackentityId);
                            var temp = link.Split("/account/");
                            link = "" + temp[0] + "/#/account/" + temp[1];
                            if (!link.IsNullOrEmpty())
                            {
                                link = EncryptauditProjectQueryParameters(link);
                            }
                            if (!link.IsNullOrEmpty())
                            {
                                //link = EncryptQueryParameters(link);
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<a style=\"" + _emailButtonStyle + "\" bg-color=\"" + _emailButtonColor + "\" href=\"" + link + "\">" + L("SubmitFeedback") + "</a>");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<br />");
                                mailMessage.AppendLine("<span style=\"font-size: 9pt;\">" + L("EmailMessage_CopyTheLinkBelowToYourBrowser") + "</span><br />");
                                mailMessage.AppendLine("<span style=\"font-size: 8pt;\">" + link + "</span>");
                            }

                            Feedbacksubject = Feedbacksubject.Replace("{Link}", mailMessage.ToString());
                            break;
                        }
                }

            });

            return Feedbacksubject;
        }

        private string EncryptauditProjectQueryParameters(string link, string encrptedParameterName = "entityFeedbackId")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');
            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        public async Task<List<FeedbackQuestionDto>> GetAllFeedbackQuestions()
        {
            var feedback = new List<FeedbackQuestionDto>();
            try
            {

                feedback = _feedBackQuestionerRepository.GetAll().Select(x => new FeedbackQuestionDto()
                {
                    QuestionId = x.Id,
                    Description = x.Description
                }).OrderBy(x => x.QuestionId).ToList();

                return feedback;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FeedbackListDto>> GetAllFeedbackList()
        {
            var result = new List<FeedbackListDto>();
            try
            {
                result = _feedbackDetailRepository.GetAll().Select(x => new FeedbackListDto()
                {
                    Id = x.Id,
                    Title = x.Title

                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AbpAllowAnonymous]
        public async Task<GetFeedBackDto> GetAllFeedBackResponse(string input)
        {
            var result = new GetFeedBackDto();

            try
            {
                var id = DecriptFeedBackId(input);
                var query = new List<FeedBackEntityResponse>();

                result.GetFeedbackQuestionResponseList = new List<GetFeedbackQuestionResponseList>();
                var feedbackentityObj = _feedbackentityRepository.GetAll().Where(x => x.Id == id.Result).Include(x => x.FeedbackDetail).ThenInclude(x => x.FeedbackDetailQuestions).FirstOrDefault();

                var orderList = feedbackentityObj.FeedbackDetail.FeedbackDetailQuestions.OrderBy(x => x.QuestionSrNo).Select(y => new
                {
                    QuestionId = y.QuestionId,
                    SrNo = y.QuestionSrNo
                }).ToList();

                var tempQuery = _feedBackEntityResponseRepository.GetAll().Where(x => x.FeedBackEntityId == id.Result).Include(x => x.Question).ThenInclude(x => x.AnswerOptions).OrderBy(x => x.QuestionId).ToList();

                orderList.ForEach(yy =>
                {
                    var temp1 = tempQuery.FirstOrDefault(x => x.QuestionId == yy.QuestionId);
                    query.Add(temp1);
                });

                query.ForEach(obj =>
                {
                    GetFeedbackQuestionResponseList temp = new GetFeedbackQuestionResponseList()
                    {
                        Id = obj.Id,
                        QuestionId = obj.QuestionId,
                        FeedBackEntityId = obj.FeedBackEntityId,
                        Question = obj.Question.Description,
                        AnswerType = obj.Question.AnswerType,
                        Response = obj.Response,
                        Mandatory = obj.Question.Mandatory,
                        ResponseOptions = obj.Question.AnswerOptions.Select(x => new ResponseOptions { QuestionOption = x.QuestionOption }).ToList()
                    };
                    result.GetFeedbackQuestionResponseList.Add(temp);
                });

                if (feedbackentityObj != null)
                {
                    DateTime newDate;
                    newDate = feedbackentityObj.CreationTime.AddDays(feedbackentityObj.FeedbackDetail.LinkValidationDay);
                    if (newDate >= DateTime.Now)
                    {
                        result.flag = false;
                    }
                    else
                    {
                        result.flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }


        [AbpAllowAnonymous]
        public async Task<GetCertificateImport> GetAllEntityCertificate(string input)
        {
            var result = new GetCertificateImport();

            try
            {
                if (input != null)
                {
                    //  var id = DecriptEntityCertificateId(input.c);


                    result = _certificateImportRepository.GetAll().Where(x => x.Id == Convert.ToInt64(input)).Select(x => new GetCertificateImport()
                    {
                        Id = x.Id,
                        EntityName = x.EntityName,
                        LicenseNumber = x.LicenseNumber,
                        Status = x.IsActiveStatus.ToString(),
                        IssueDate = x.IssueDate == null ? "" : Convert.ToDateTime(x.IssueDate).ToString("dd/MM/yyyy"),
                        ExpireDate = x.ExpireDate == null ? "" : Convert.ToDateTime(x.ExpireDate).ToString("dd/MM/yyyy")

                    }).FirstOrDefault();

                }

            }
            catch (Exception ex)
            {
                throw;
            }


            return result;
        }


        public async Task<GetFeedBackDto> GetallFeedbackQuestionResponse(EntityDto input)
        {
            var result = new GetFeedBackDto();

            try
            {


                result.GetFeedbackQuestionResponseList = new List<GetFeedbackQuestionResponseList>();
                var feedbackentityObj = _feedbackentityRepository.GetAll().Where(x => x.Id == input.Id).Include(x => x.FeedbackDetail).FirstOrDefault();
                var query = _feedBackEntityResponseRepository.GetAll().Where(x => x.FeedBackEntityId == input.Id).Include(x => x.Question).ThenInclude(x => x.AnswerOptions).ToList();

                query.ForEach(obj =>
                {
                    GetFeedbackQuestionResponseList temp = new GetFeedbackQuestionResponseList()
                    {
                        Id = obj.
                        QuestionId = obj.QuestionId,
                        FeedBackEntityId = obj.FeedBackEntityId,
                        Question = obj.Question.Description,
                        AnswerType = obj.Question.AnswerType,
                        Response = obj.Response,
                        Mandatory = obj.Question.Mandatory,
                        ResponseOptions = obj.Question.AnswerOptions.Select(x => new ResponseOptions { QuestionOption = x.QuestionOption }).ToList()
                    };
                    result.GetFeedbackQuestionResponseList.Add(temp);
                });

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Record not found..!");
            }
        }

        [AbpAllowAnonymous]
        public async Task<int> UpdateFeedBackResponse(List<GetFeedbackQuestionResponseList> input)
        {
            var result = input.First();
            var query = _feedBackEntityResponseRepository.GetAll().Where(x => x.FeedBackEntityId == result.FeedBackEntityId).ToList();

            for (int i = 0; i < input.Count(); i++)
            {
                var obj = query.Where(x => x.QuestionId == input[i].QuestionId).FirstOrDefault();
                obj.Response = input[i].Response;
                _feedBackEntityResponseRepository.Update(obj);
            }

            return result.FeedBackEntityId;
        }

        [AbpAllowAnonymous]
        public Task<int?> DecriptFeedBackId(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["entityFeedbackId"] == null)
            {
                return Task.FromResult<int?>(null);
            }
            var tenantId = Convert.ToInt32(query["entityFeedbackId"]) as int?;
            return Task.FromResult(tenantId);
        }

        [AbpAllowAnonymous]
        public async Task<int> DecriptEntityCertificateId(string input)
        {
            var result = 0;
            try
            {
                if (input != null)
                {
                    if (input.Contains('`'))
                        input = input.Replace('`', '/');
                    var input2 = input;
                    result = int.Parse(SimpleStringCipher.Instance.Decrypt(input2.Trim()));
                }



                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<List<IdAndNameDto>> FeedBackDetailsIdAndNames()
        {
            var result = new List<IdAndNameDto>();
            try
            {
                result = await _feedbackDetailRepository.GetAll().Select(x => new IdAndNameDto
                {
                    Id = x.Id,
                    Name = x.Title
                }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<FeedBackQuestionGroupWiseDto> FeedBackResponseEntitiwiseByFeedBackId(int id)
        {
            var result = new FeedBackQuestionGroupWiseDto();

            try
            {
                var feedBackInfo = _feedbackDetailRepository.GetAll().Include(x => x.FeedbackDetailQuestions).Where(x => x.Id == id).FirstOrDefault();
                result.Title = feedBackInfo.Title;
                var QuestionIds = feedBackInfo.FeedbackDetailQuestions.Select(x => x.Id).ToList();

                var feedBackEntitiesWithCompanyName = _feedbackentityRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.FeedbackDetailId == id)
                    .Select(x => new
                    {
                        FeedBackId = x.Id,
                        EntityName = x.BusinessEntity.CompanyName
                    }).ToList();

                var feedBackTempIds = feedBackEntitiesWithCompanyName.Select(x => x.FeedBackId).ToList();

                var allFeedBaclList = _feedBackEntityResponseRepository.GetAll().Include(x => x.Question)
                    .Include(x => x.FeedBackEntity).ThenInclude(x => x.BusinessEntity)
                    .Where(x => feedBackTempIds.Contains(x.FeedBackEntityId))
                    .Select(x => new
                    {
                        QuestionId = x.QuestionId,
                        QuestionName = x.Question.Name,
                        Response = x.Response,
                        EntityName = x.FeedBackEntity.BusinessEntity.CompanyName
                    }).ToList();

                var query = allFeedBaclList.GroupBy(x => x.QuestionId).ToList();

                query.ForEach(obj =>
                {
                    var innerDate = obj.ToList();
                    QuestionResponseGroupDto temp = new QuestionResponseGroupDto()
                    {
                        QuestionId = obj.Key,
                        QuestionName = obj.FirstOrDefault().QuestionName
                    };

                    var innerLoop = innerDate.GroupBy(x => x.EntityName).ToList();
                    innerLoop.ForEach(obj1 =>
                    {
                        EntityWithCount countObj = new EntityWithCount()
                        {
                            EntityName = "" + obj1.Key,
                            Count = "" + obj1.ToList().Count()
                        };
                        temp.EntityWithCounts.Add(countObj);
                    });

                    result.QuestionResponseGroupDtos.Add(temp);
                });





                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<DashboardFeedbackDto> DashboardFeedback()
        {
            var result = new DashboardFeedbackDto();
            try
            {

                var feedBankDetailsInfo = await _feedbackDetailRepository.GetAll().ToListAsync();

                result.FeedBankDetailsInfo = feedBankDetailsInfo.Select(x => new FeedBackDetailDto()
                {
                    Code = x.Code,
                    Id = x.Id,
                    Title = x.Title,
                }).ToList();

                var feedbackInfo = await _feedBackEntityResponseRepository.GetAll().ToListAsync();

                result.TotalResponse = feedbackInfo.GroupBy(x => x.FeedBackEntityId).Count();

                feedbackInfo.GroupBy(x => x.FeedBackEntityId).ToList().ForEach(x =>
                {
                    var questionCount = x.ToList().Count();
                    var answerquestionCount = x.Where(x => x.Response != null).ToList().Count();
                    if (questionCount == answerquestionCount)
                    {
                        result.ReceiveResponse = result.ReceiveResponse + 1;
                    }
                });

                result.PendingResponse = result.TotalResponse - result.ReceiveResponse;
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<QuestionChartDto>> AllQuestionResponseByFeedbackDetailsId(int input)
        {
            var result = new List<QuestionChartDto>();
            var QuestionResponses = new List<QuestionResponseDto>();
            try
            {

                //  var feedBankDetailsInfo = await _feedBackEntityResponseRepository.GetAll().Include(x => x.Question).ThenInclude(x => x.AnswerOptions).Where(x => x.FeedBackEntityId == input).ToListAsync();
                var feedBackEntitiesId = await _feedbackentityRepository.GetAll().Where(x => x.FeedbackDetailId == input).Select(x => x.Id).ToListAsync();
                var feedBankEntityResponseInfo = await _feedBackEntityResponseRepository.GetAll().Where(x => feedBackEntitiesId.Contains(x.FeedBackEntityId))
                    .Include(x => x.Question).ThenInclude(x => x.AnswerOptions).OrderBy(xx => xx.Question.Id).ToListAsync();
                feedBankEntityResponseInfo.GroupBy(x => x.QuestionId).ToList().ForEach(x =>
                    {
                        QuestionChartDto obj = new QuestionChartDto();
                        obj.QuestionId = x.Key;
                        obj.QuestionName = "" + x.LastOrDefault().Question.Description;

                        obj.QuestionResponses = x.LastOrDefault().Question.AnswerOptions.Select(y => new QuestionResponseDto
                        {
                            Answer = y.QuestionOption,
                            Count = x.ToList().Where(x => x.Response == y.QuestionOption).Count()
                        }).ToList();
                        result.Add(obj);
                    });

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<FileDto> ExportFeedBackResponse(int input)
        {
            var result1 = new FileDto();
            var result = new List<FeedBackExcelDto>();
            var QuestionResponses = new List<QuestionResponseDto>();

            try
            {
                var feedBackEntitiesId = await _feedbackentityRepository.GetAll().Where(x => x.FeedbackDetailId == input).Select(x=>x.Id).ToListAsync();
                var feedBankEntityResponseInfo = await _feedBackEntityResponseRepository.GetAll().Where(x => feedBackEntitiesId.Contains(x.FeedBackEntityId))
                    .Include(x => x.Question).ThenInclude(x => x.AnswerOptions).OrderBy(xx => xx.Question.Id).ToListAsync();

                var optionsList = new List<string>();
                optionsList.Add("");

                feedBankEntityResponseInfo.GroupBy(x => x.QuestionId).ToList().ForEach(y =>
                  {
                      if (y.FirstOrDefault().Question.AnswerType != Questions.AnswerType.Input)
                      {
                          FeedBackExcelDto matrix = new FeedBackExcelDto();
                          matrix.QuestionName = y.FirstOrDefault().Question.Description;
                          matrix.OptionWithCount = "";

                          if (y.FirstOrDefault().Question.AnswerType == Questions.AnswerType.List)
                          {
                              foreach (var item in y.FirstOrDefault().Question.AnswerOptions)
                              {
                                  matrix.OptionWithCount = matrix.OptionWithCount + "" +
                                  item.QuestionOption + "-" + y.ToList().Where(yy => yy.Response == item.QuestionOption).Count() + ",";
                              }
                          }
                          else if (y.FirstOrDefault().Question.AnswerType == Questions.AnswerType.Multiselect)
                          {
                              foreach (var item in y.FirstOrDefault().Question.AnswerOptions)
                              {
                                  matrix.OptionWithCount = matrix.OptionWithCount + "" +
                                  item.QuestionOption + "-" + y.ToList().Where(yy => yy.Response == item.QuestionOption).Count() + ",";
                              }
                          }
                          else if (y.FirstOrDefault().Question.AnswerType == Questions.AnswerType.Logical)
                          {
                              matrix.OptionWithCount = matrix.OptionWithCount + "" + "Yes-" + y.ToList().Where(yy => yy.Response == "1").Count() + ",";
                              matrix.OptionWithCount = matrix.OptionWithCount + "" + "No-" + y.ToList().Where(yy => yy.Response == "0").Count() + ",";
                          }

                          result.Add(matrix);
                      }

                  });

                result1 = _feedbackresponseExcelExporter.ExportToFile(result);
                return result1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<string>> GetBusinessEntitiesByQuestionIdAns(int feedBackId, int questionId, string ans)
        {
            var result = new List<string>();
            try
            {
                result = await _feedBackEntityResponseRepository.GetAll().Include(x => x.Question).ThenInclude(x => x.AnswerOptions)
                    .Where(x => x.QuestionId == questionId && x.Response.ToLower() == ans.ToLower())
                    .Select(x => x.FeedBackEntity.BusinessEntity.CompanyName).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
