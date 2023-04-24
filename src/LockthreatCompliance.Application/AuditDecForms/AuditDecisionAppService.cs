using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.AuditDecForms.Dto;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.AuthoritityDepartments;
using NPOI.XSSF.UserModel.Helpers;
using System.Text.RegularExpressions;
using LockthreatCompliance.AuditProjects.Dtos;
using Abp.DynamicEntityParameters;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.Url;
using System.Web;
using Abp.Runtime.Security;
using Abp.Extensions;
using Abp.Domain.Uow;

namespace LockthreatCompliance.AuditDecForms
{
    public class AuditDecisionAppService : LockthreatComplianceAppServiceBase, IAuditDecisionAppService
    {
        private readonly IRepository<AuditDecForm> _auditDecFormRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<EntityGroup, int> _entityGroupRepository;
        private readonly IRepository<BusinessEntityWorkFlowActor> _businessEntityWorkFlowRepository;
        private readonly IRepository<User, long> _UserRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<AuditProjectAuthoritativeDocument> _auditProjectAuthoritativeDocumentRepository;
        private readonly IRepository<AuditDecUsers> _auditDecUsersRepository;
        private readonly IRepository<AuthorityDepartment> _authorityDepartmentRepository;
        private readonly IRepository<Authorityworkflowactor> _authorityworkflowactorRepository;
        private readonly IRepository<ReviewData> _reviewDataRepository;
        private readonly IRepository<DynamicParameter> _dynamicParameterManager;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IRepository<Template, long> _templatenotificationRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IUserEmailer _userEmailer;
        public IAppUrlService AppUrlService { get; set; }

        public AuditDecisionAppService(IRepository<AuditDecForm> auditDecFormRepository, IRepository<AuditDecUsers> auditDecUsersRepository,
            IRepository<AuditProjectAuthoritativeDocument> auditProjectAuthoritativeDocumentRepository, IRepository<AuthorityDepartment> authorityDepartmentRepository,
            IRepository<BusinessEntity> businessEntityRepository, IRepository<BusinessEntityWorkFlowActor> businessEntityWorkFlowRepository, IRepository<User, long> UserRepository,
            IRepository<EntityGroup, int> entityGroupRepository, IRepository<Authorityworkflowactor> authorityworkflowactorRepository,
            IRepository<AuditProject, long> auditProjectRepository,
            IRepository<ExternalAssessment> externalAssessmentRepository,
            IRepository<ReviewData> reviewDataRepository,
            IRepository<DynamicParameter> dynamicParameterManager,
            IRepository<WorkFlowPage, long> workflowpageRepository,
            IRepository<EmailNotificationTemplate, long> emailnotificationRepository,
            IRepository<Template, long> templatenotificationRepository,
            IRepository<DynamicParameterValue> dynamicParameterValueRepository,
            IRepository<EntityGroupMember> entityGroupMemberRepository, IUnitOfWorkManager unitOfWorkManager,
            IUserEmailer userEmailer)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _authorityworkflowactorRepository = authorityworkflowactorRepository;
            _authorityDepartmentRepository = authorityDepartmentRepository;
            _auditDecUsersRepository = auditDecUsersRepository;
            _auditProjectAuthoritativeDocumentRepository = auditProjectAuthoritativeDocumentRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectRepository = auditProjectRepository;
            _businessEntityWorkFlowRepository = businessEntityWorkFlowRepository;
            _auditDecFormRepository = auditDecFormRepository;
            _businessEntityRepository = businessEntityRepository;
            _entityGroupRepository = entityGroupRepository;
            _UserRepository = UserRepository;
            _reviewDataRepository = reviewDataRepository;
            _dynamicParameterManager = dynamicParameterManager;
            _workflowpageRepository = workflowpageRepository;
            _emailnotificationRepository = emailnotificationRepository;
            _templatenotificationRepository = templatenotificationRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;

        }

        public async Task AddorUpdateAuditDecison(AuditDecisionDto input, bool flag)
        {
            try
            {

                List<ReviewDataReportDto> result2 = new List<ReviewDataReportDto>();
                List<ReviewDataReportDto> result3 = new List<ReviewDataReportDto>();
                List<ReviewDataReportDto> result4 = new List<ReviewDataReportDto>();
                if (input.Id > 0)
                {
                    if (AbpSession.TenantId != null)
                    {
                        input.TenantId = (int?)AbpSession.TenantId;
                    }
                    var getdata = _auditDecFormRepository.GetAll().Where(x => x.Id == input.Id).FirstOrDefault();
                    var BusinessEntityCertificateFlag = "";

                    if (input.BusinessEntityNames != null)
                    {
                        if (input.BusinessEntityNames.Length > 0)
                        {
                            var ids = input.BusinessEntityNames.Split(',');
                            ids = ids.Where(x => x != "").Select(y => y).ToArray();
                            var businessId = Array.ConvertAll(ids, s => int.Parse(s));

                            for (int i = 0; i < businessId.Length; i++)
                            {
                                BusinessEntityCertificateFlag = BusinessEntityCertificateFlag + (businessId[i] + ":false,");
                                input.BusinessEntityNames = (BusinessEntityCertificateFlag).ToString();
                            }
                        }
                    }

                    //var getupdate = ObjectMapper.Map(input, getdata);
                    getdata.DecisionDate = input.DecisionDate;
                    getdata.ExpireDate = input.ExpireDate;
                    getdata.AuditProjectId = input.AuditProjectId;
                    getdata.EntityGroupId = input.EntityGroupId;
                    getdata.FacilityTypeId = input.FacilityTypeId;
                    getdata.DocumentCheck = input.DocumentCheck;
                    getdata.OtherApplicable = input.OtherApplicable;
                    getdata.OutPutConClusion = input.OutPutConClusion;
                    getdata.Judgement = input.Judgement;
                    getdata.Decision = input.Decision;
                    getdata.DoHApprover = input.DoHApprover;
                    getdata.AuditAgencyApprover = input.AuditAgencyApprover;
                    getdata.DoHSign = input.DoHSign;
                    getdata.AuditVensign = input.AuditVensign;
                    getdata.BeforeCAPAScore = input.BeforeCAPAScore;
                    getdata.AfterCAPAScore = input.AfterCAPAScore;
                    getdata.BusinessEntityNames = input.BusinessEntityNames;

                    var externalAssessmentIds = _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == input.AuditProjectId).Select(x => x.Id).ToList();
                    var externalAssessmentObj = _reviewDataRepository.GetAll().Where(x => externalAssessmentIds.Contains((int)x.ExternalAssessmentId)).OrderByDescending(x => x.Id).Select(x => x.ExternalAssessment).FirstOrDefault();

                    if (externalAssessmentObj != null)
                    {
                        var tempList = _reviewDataRepository.GetAll().Include(x => x.ControlRequirement).Where(x => x.ExternalAssessmentId == externalAssessmentObj.Id && x.ResponseType != ReviewDataResponseType.NotSelected)
                         .Select(x => new
                         {
                             DomainName = x.ControlRequirement.DomainName,
                             ResponseType = x.ResponseType,
                             UpdatedResponseType = x.UpdatedResponseType,
                             Comment = x.Comment,
                             UpdatedMarks =
                            (x.UpdatedResponseType == ReviewDataResponseType.NotSelected || x.UpdatedResponseType == ReviewDataResponseType.NotSelected)
                             ? (x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "")
                             : (x.UpdatedResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.UpdatedResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.UpdatedResponseType == ReviewDataResponseType.NonCompliant ? "0" : ""),                        
                             Marks = x.ResponseType == ReviewDataResponseType.FullyCompliant ? "100" : x.ResponseType == ReviewDataResponseType.PartiallyCompliant ? "50" : x.ResponseType == ReviewDataResponseType.NonCompliant ? "0" : "",
                         }).ToList();

                        if (tempList.Count() != 0)
                        {

                            result2 = tempList.GroupBy(x => x.DomainName).Select(y => new ReviewDataReportDto
                            {
                                DomainName = y.Key.ToString(),
                                ResponsePercent = "" + (int)Math.Round(Convert.ToDecimal(y.Where(x => x.Marks != "").Sum(x => Convert.ToInt32(x.Marks)) / y.Count())) + ".00",
                                CapaResponsePercent =
                                (y.Count(yy => yy.UpdatedResponseType == 0) == y.Count()) ?
                                "" :
                                ("" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00") == "1.00" ? "" : "" + (int)Math.Round(Convert.ToDecimal((y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks)) == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Sum(x => Convert.ToInt32(x.UpdatedMarks))) / (y.Where(x => x.UpdatedMarks != "").Count() == 0 ? -1 : y.Where(x => x.UpdatedMarks != "").Count()))) + ".00",
                                Comment = ""
                            }).Where(x => x.DomainName.ToLower().Contains("Domain ".ToLower())).OrderBy(x => int.Parse(x.DomainName.Split('-')[0].Split(' ')[1])).ToList();

                           
                            decimal totalPercentage = 0;
                            decimal totalCapaRespPercentage = 0;
                            foreach (var item2 in result2)
                            {
                                if (item2.ResponsePercent != "")
                                {
                                    totalPercentage = totalPercentage + Convert.ToDecimal(item2.ResponsePercent);
                                }
                            }
                            var getAfterCapaCount = result2.Where(x => x.CapaResponsePercent != "").Select(x => x.CapaResponsePercent).Count();
                            if (getAfterCapaCount != 0 && getAfterCapaCount != null)
                            {
                                foreach (var item2 in result2)
                                {
                                    if (item2.CapaResponsePercent != "")
                                    {
                                        totalCapaRespPercentage = totalCapaRespPercentage + Convert.ToDecimal(item2.CapaResponsePercent);
                                    }
                                    else
                                    {
                                        totalCapaRespPercentage = totalCapaRespPercentage + Convert.ToDecimal(item2.ResponsePercent);
                                    }
                                }
                            }
                            var finalCaparesponsePerctage = "";
                            if (totalCapaRespPercentage != 0)
                            {
                                finalCaparesponsePerctage = "" + +decimal.Round((totalCapaRespPercentage / result2.Count()), 2, MidpointRounding.AwayFromZero);
                            }
                           

                            getdata.BeforeCAPAScore = "" + decimal.Round((totalPercentage / result2.Count()), 2, MidpointRounding.AwayFromZero);
                            getdata.AfterCAPAScore = finalCaparesponsePerctage;
                        }




                    
                    }
                    _auditDecFormRepository.Update(getdata);

                    if (input.AuditDecUser.Count > 0)
                    {
                        foreach (var item in input.AuditDecUser)
                        {
                            var getuser = _auditDecUsersRepository.GetAll().Where(x => x.Id == item.Id).FirstOrDefault();
                            getuser.AuditDecFormId = input.Id;
                            getuser.MemberNameId = item.MemberNameId;
                            getuser.Signature = item.Signature;
                            getuser.Type = item.Type;

                            _auditDecUsersRepository.Update(ObjectMapper.Map<AuditDecUsers>(getuser));
                        }
                    }
                    if (input.AuthorityUser.Count > 0)
                    {
                        foreach (var item in input.AuthorityUser)
                        {
                            var getuser = _auditDecUsersRepository.GetAll().Where(x => x.Id == item.Id).FirstOrDefault();
                            getuser.AuditDecFormId = input.Id;
                            getuser.MemberNameId = item.MemberNameId;
                            getuser.Signature = item.Signature;
                            getuser.Type = item.Type;

                            _auditDecUsersRepository.Update(ObjectMapper.Map<AuditDecUsers>(getuser));
                        }
                    }

                }

                else
                {
                    input.TenantId = AbpSession.TenantId;

                    string BusinessEntityCertificateFlag = "";

                    if (input.BusinessEntityNames.Length > 0)
                    {
                        var ids = input.BusinessEntityNames.Split(',');
                        ids = ids.Where(x => x != "").Select(y => y).ToArray();
                        var businessId = Array.ConvertAll(ids, s => int.Parse(s));

                        for (int i = 0; i < businessId.Length; i++)
                        {
                            BusinessEntityCertificateFlag = BusinessEntityCertificateFlag + (businessId[i] + ":false,");
                            input.BusinessEntityNames = (BusinessEntityCertificateFlag).ToString();
                        }
                    }

                    int auditDecisionId = await _auditDecFormRepository.InsertAndGetIdAsync(ObjectMapper.Map<AuditDecForm>(input));

                    if (input.AuditDecUser.Count > 0)
                    {
                        foreach (var item in input.AuditDecUser)
                        {
                            item.AuditDecFormId = auditDecisionId;
                            await _auditDecUsersRepository.InsertOrUpdateAsync(ObjectMapper.Map<AuditDecUsers>(item));
                        }
                    }

                    if (input.AuthorityUser.Count > 0)
                    {
                        foreach (var item in input.AuthorityUser)
                        {
                            item.AuditDecFormId = auditDecisionId;
                            await _auditDecUsersRepository.InsertOrUpdateAsync(ObjectMapper.Map<AuditDecUsers>(item));
                        }
                    }
                }

                if (flag == true)
                {
                    //send mail

                    var auditentitydetails = new List<AuditFacilityDto>();                  
                    var item2 = input.AuditProjectId;

                    var getadminemail = _UserRepository.GetAll().Where(x => x.TenantId == AbpSession.TenantId).FirstOrDefault();
                    var getauditProjecttemp = await _auditProjectRepository.GetAll().Where(x => x.Id == item2).FirstOrDefaultAsync();
                    string auditbody = null;
                    string AuditEmailsubject = null;
                    List<string> emails = new List<string>();
                    List<string> ccemail = new List<string>();
                    List<string> bccemail = new List<string>();
                    List<string> tofilter = new List<string>();
                    List<string> ccfilter = new List<string>();
                    List<string> bccfilter = new List<string>();
                    var getTemplate = new Template();
                    var getcheck = await _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == item2).ToListAsync();
                    var getcheckId = await _dynamicParameterManager.GetAll().Where(x => x.ParameterName.Trim().ToLower() == LockthreatComplianceConsts.AuditStatus.Trim().ToLower()).FirstOrDefaultAsync();
                    var getpage = await _workflowpageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.CertificateRequestPage.Trim().ToLower());
                    if (getpage != null)
                    {
                         getTemplate = await _templatenotificationRepository.GetAll().Where(x => x.WorkFlowPageId == getpage.Id).FirstOrDefaultAsync();
                    }
                    var getother = await _dynamicParameterValueRepository.GetAll().Where(l => l.DynamicParameterId == getcheckId.Id).ToListAsync();

                    if (getTemplate.Id != 0)
                    {
                        foreach (var item in getcheck)
                        {
                            var getauditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == item.AuditProjectId).FirstOrDefaultAsync();                            
                            if (getauditProject.EntityGroupId != null)
                            {
                                var checkprimaryEntity = await _entityGroupMemberRepository.GetAll().Where(x => x.BusinessEntityId == item.BusinessEntityId).FirstOrDefaultAsync();
                                if (checkprimaryEntity != null)
                                {
                                    
                                    if (getTemplate != null)
                                    {

                                        List<string> templateSubject = new List<string>();
                                        var auditprojectsubjectBody = getTemplate.TemplateSubject;

                                        AuditEmailsubject = getTemplate.TemplateSubject.ToString();

                                        while (auditprojectsubjectBody.Contains("{"))
                                        {
                                            templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                            auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                        };

                                        AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                        var auditTemplate = getTemplate.TemplateBody;

                                        var auditTo = getTemplate.TemplateTo;
                                        List<string> auditToList = getTemplate.TemplateTo.Split(',').ToList();
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

                                        //while (auditTo.Contains("{"))
                                        //{
                                        //    templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                                        //    auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                        //};

                                        var auditCc = getTemplate.TemplateCc;
                                        List<string> auditCcList = getTemplate.TemplateCc.Split(',').ToList();
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

                                        //while (auditCc.Contains("{"))
                                        //{
                                        //    templateCc.Add("{" + auditCc.Split('{', '}')[1] + "}");
                                        //    auditCc = auditCc.Replace("{" + auditCc.Split('{', '}')[1] + "}", "");
                                        //};

                                        templatevariables.ForEach(x =>
                                        {
                                            switch (x)
                                            {
                                                case "{Business_Entity_Admin_Email}":
                                                    {
                                                        emails.Add(item.BusinessEntity.AdminEmail);
                                                        break;
                                                    }
                                                case "{Audit_Agency_Admin_Email}":
                                                    {
                                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                        if (getbusinessadmin != null)
                                                        {
                                                            emails.Add(getbusinessadmin.AdminEmail);
                                                        }
                                                        break;
                                                    }
                                                case "{Owner_Email}":
                                                    {
                                                        if (item.BusinessEntity.Owner_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.Director_Incharge_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.CISO_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.OfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.BackupOfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        var leadauditormail = _UserRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                        if (leadauditormail != null)
                                                        {
                                                            emails.Add(leadauditormail.EmailAddress);
                                                        }
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
                                                        ccemail.Add(item.BusinessEntity.AdminEmail);
                                                        break;
                                                    }
                                                case "{Audit_Agency_Admin_Email}":
                                                    {
                                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                        if (getbusinessadmin != null)
                                                        {
                                                            ccemail.Add(getbusinessadmin.AdminEmail);
                                                        }
                                                        break;
                                                    }
                                                case "{Owner_Email}":
                                                    {
                                                        if (item.BusinessEntity.Owner_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.Director_Incharge_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.CISO_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.OfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.BackupOfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        var leadauditormail = _UserRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                        if (leadauditormail != null)
                                                        {
                                                            ccemail.Add(leadauditormail.EmailAddress);
                                                        }
                                                        break;
                                                    }

                                            }

                                        });

                                        var auditBcc = getTemplate.TemplateBcc;
                                        List<string> auditBccList = getTemplate.TemplateBcc.Split(',').ToList();
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

                                        //while (auditBcc.Contains("{"))
                                        //{
                                        //    templateBcc.Add("{" + auditBcc.Split('{', '}')[1] + "}");
                                        //    auditBcc = auditBcc.Replace("{" + auditBcc.Split('{', '}')[1] + "}", "");
                                        //};

                                        templateBcc.ForEach(x =>
                                        {
                                            switch (x)
                                            {
                                                case "{Business_Entity_Admin_Email}":
                                                    {
                                                        bccemail.Add(item.BusinessEntity.AdminEmail);
                                                        break;
                                                    }
                                                case "{Audit_Agency_Admin_Email}":
                                                    {
                                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                        if (getbusinessadmin != null)
                                                        {
                                                            bccemail.Add(getbusinessadmin.AdminEmail);
                                                        }
                                                        break;
                                                    }
                                                case "{Owner_Email}":
                                                    {
                                                        if (item.BusinessEntity.Owner_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.Director_Incharge_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.CISO_Email != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.OfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        if (item.BusinessEntity.BackupOfficialEmail != null)
                                                        {
                                                            var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                        var leadauditormail = _UserRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                        if (leadauditormail != null)
                                                        {
                                                            bccemail.Add(leadauditormail.EmailAddress);
                                                        }
                                                        break;
                                                    }

                                            }

                                        });

                                        if (getadminemail != null)
                                        {
                                            ccemail.Add(getadminemail.EmailAddress);
                                        }

                                        List<string> templateBody = new List<string>();
                                        var auditprojectBody = getTemplate.TemplateBody;

                                        auditbody = getTemplate.TemplateBody.ToString();

                                        while (auditprojectBody.Contains("{"))
                                        {
                                            templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                            auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                        };

                                        auditbody = ReplaceBodyFucntion(getauditProject, item, auditentitydetails, getother, templateBody, auditbody);



                                    }
                                    //   }
                                }
                            }

                            else
                            {
                                //var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == 1).FirstOrDefault();
                                // var getTemplate = _emailnotificationRepository.GetAll().Where(x => x.AuditStatusId == input.AuditStatusId && x.WorkFlowPageId == getpage.Id).FirstOrDefault();
                                if (getTemplate != null)
                                {

                                    List<string> templateSubject = new List<string>();
                                    var auditprojectsubjectBody = getTemplate.TemplateSubject;

                                    AuditEmailsubject = getTemplate.TemplateSubject.ToString();

                                    while (auditprojectsubjectBody.Contains("{"))
                                    {
                                        templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                        auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                    };

                                    AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                    var auditTemplate = getTemplate.TemplateBody;
                                    var auditTo = getTemplate.TemplateTo;
                                    List<string> templatevariables = new List<string>();
                                    List<string> auditToList = getTemplate.TemplateTo.Split(',').ToList();

                                    auditToList.ForEach(emailid =>
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
                                                emails.Add(email);
                                            }
                                        }
                                    });

                                    var auditCc = getTemplate.TemplateCc;
                                    List<string> templateCc = new List<string>();
                                    List<string> auditCcList = getTemplate.TemplateCc.Split(',').ToList();

                                    auditCcList.ForEach(emailid =>
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

                                    templatevariables.ForEach(x =>
                                    {
                                        switch (x)
                                        {
                                            case "{Business_Entity_Admin_Email}":
                                                {
                                                    emails.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        emails.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    var leadauditormail = _UserRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                    if (leadauditormail != null)
                                                    {
                                                        emails.Add(leadauditormail.EmailAddress);
                                                    }
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
                                                    ccemail.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        ccemail.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    var leadauditormail = _UserRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                    if (leadauditormail != null)
                                                    {
                                                        ccemail.Add(leadauditormail.EmailAddress);
                                                    }
                                                    break;
                                                }
                                        }

                                    });

                                    var auditBcc = getTemplate.TemplateBcc;
                                    List<string> auditBccList = getTemplate.TemplateBcc.Split(',').ToList();
                                    List<string> templateBcc = new List<string>();

                                    auditBccList.ForEach(emailid =>
                                    {
                                        if (emailid.Contains("{"))
                                        {
                                            templateBcc.Add("{" + emailid.Split('{', '}')[1] + "}");
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
                                                    bccemail.Add(item.BusinessEntity.AdminEmail);
                                                    break;
                                                }
                                            case "{Audit_Agency_Admin_Email}":
                                                {
                                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == item.VendorId).FirstOrDefault();
                                                    if (getbusinessadmin != null)
                                                    {
                                                        bccemail.Add(getbusinessadmin.AdminEmail);
                                                    }
                                                    break;
                                                }
                                            case "{Owner_Email}":
                                                {
                                                    if (item.BusinessEntity.Owner_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.Director_Incharge_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.CISO_Email != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.OfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    if (item.BusinessEntity.BackupOfficialEmail != null)
                                                    {
                                                        var splitEmail = item.BusinessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    var leadauditormail = _UserRepository.GetAll().Where(x => x.Id == item.AuditProject.LeadAuditorId).FirstOrDefault();
                                                    if (leadauditormail != null)
                                                    {
                                                        bccemail.Add(leadauditormail.EmailAddress);
                                                    }
                                                    break;
                                                }
                                        }

                                    });

                                    if (getadminemail != null)
                                    {
                                        ccemail.Add(getadminemail.EmailAddress);
                                    }

                                    List<string> templateBody = new List<string>();
                                    var auditprojectBody = getTemplate.TemplateBody;

                                    auditbody = getTemplate.TemplateBody.ToString();

                                    while (auditprojectBody.Contains("{"))
                                    {
                                        templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                        auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                    };

                                    auditbody = ReplaceBodyFucntion(getauditProject, item, auditentitydetails, getother, templateBody, auditbody);


                                }

                            }

                        }

                        tofilter.AddRange(emails.Distinct());
                        ccfilter.AddRange(ccemail.Distinct());
                        bccfilter.AddRange(bccemail.Distinct());
                        await _userEmailer.AuditProjectEntityNotification(tofilter, ccfilter, bccfilter, AuditEmailsubject, (int)getauditProjecttemp.TenantId, auditbody, (int)getauditProjecttemp.AuditStatusId, getauditProjecttemp.Id,
                                         AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)getauditProjecttemp.Id));

                    }

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Record Not Insert Or Update !");
            }

        }

        public async Task<PagedResultDto<AuditDecisionListDto>> GetAllAuditDecisionList(AuditDecisionInputDto input)
        {
            try
            {
                var query = _auditDecFormRepository.GetAll()
                             .WhereIf(input.Filter != null, x => x.Decision == input.Filter);


                var auditdecisionCount = await query.CountAsync();

                var auditDecisionItem = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var auditdecisions = ObjectMapper.Map<List<AuditDecisionListDto>>(auditDecisionItem);

                return new PagedResultDto<AuditDecisionListDto>(
                   auditdecisionCount,
                   auditdecisions
                   );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteAuditDecisonForm(EntityDto input)
        {
            try
            {
                var auditDecision = await _auditDecFormRepository.GetAsync(input.Id);
                await _auditDecFormRepository.DeleteAsync(auditDecision);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuditDecisionDto> GetAuditDecisionEdit(EntityDto input)
        {
            try
            {
                var auditProject = new AuditDecisionDto();

                var item = await _auditDecFormRepository.GetAllIncluding().IgnoreQueryFilters().Where(x => !x.IsDeleted && x.Id == input.Id).FirstOrDefaultAsync();

                if (item != null)
                {
                    auditProject = ObjectMapper.Map<AuditDecisionDto>(item);


                    auditProject.AuditDecUser = await _auditDecUsersRepository.GetAll().Where(x => x.AuditDecFormId == input.Id).Select(x => new AuditDecUsersDto()
                    {
                        Id = x.Id,
                        MemberNameId = (long)x.MemberNameId,
                        AuditDecFormId = x.AuditDecFormId,
                        Signature = x.Signature,
                        Type = x.Type
                    }).ToListAsync();
                }
                return auditProject;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EntityPrimaryDto> GetPrimaryEntityByEntityGroupId(int id)
        {
            var query = new EntityPrimaryDto();
            try
            {
                var entityPrimaryId = new EntityGroup();
                using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    entityPrimaryId = _entityGroupRepository.GetAll().Where(x => x.Id == id).FirstOrDefault();



                    if (entityPrimaryId != null && !entityPrimaryId.IsDeleted)
                    {

                        query = await _businessEntityRepository.GetAll().Include(x => x.FacilityType).Where(x => x.Id == entityPrimaryId.PrimaryEntityId)
                        .Select(x => new EntityPrimaryDto
                        {
                            LicenseNumber = x.LicenseNumber,
                            Name = x.CompanyName,
                            FacilityTypeId = x.FacilityTypeId,
                            FacilityType = x.FacilityType.Name
                        }).FirstOrDefaultAsync();

                    }

                    else
                    {
                        query = await _businessEntityRepository.GetAll().Include(x => x.FacilityType).Where(x => x.Id == entityPrimaryId.PrimaryEntityId)
                        .Select(x => new EntityPrimaryDto
                        {
                            LicenseNumber = x.LicenseNumber,
                            Name = x.CompanyName,
                            FacilityTypeId = x.FacilityTypeId,
                            FacilityType = x.FacilityType.Name
                        }).FirstOrDefaultAsync();
                    }
                }
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuditDecisionDto> GetAuditDecisionByProjectId(long auditProjectId)
        {
            var query = new AuditDecisionDto();
            try
            {
                var item = await _auditDecFormRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.AuditProjectId == auditProjectId).FirstOrDefaultAsync();

                if (item != null)
                {
                    query = ObjectMapper.Map<AuditDecisionDto>(item);


                    query.AuditDecUser = await _auditDecUsersRepository.GetAll().Where(x => x.AuditDecFormId == item.Id && x.Type == BusinessEntityWorkflowActorType.Authority).Include(x => x.MemberName).Select(x => new AuditDecUsersDto()
                    {
                        Id = x.Id,
                        MemberNameId = (long)x.MemberNameId,
                        Name = (x.MemberName == null) ? "" : x.MemberName.FullName,
                        AuditDecFormId = x.AuditDecFormId,
                        Signature = x.Signature,
                        Type = x.Type
                    }).ToListAsync();
                    query.AuthorityUser = await _auditDecUsersRepository.GetAll().Where(x => x.AuditDecFormId == item.Id && x.Type == BusinessEntityWorkflowActorType.Authority).Include(x => x.MemberName).Select(x => new AuditDecUsersDto()
                    {
                        Id = x.Id,
                        MemberNameId = (long)x.MemberNameId,
                        Name = (x.MemberName == null) ? "" : x.MemberName.FullName,
                        AuditDecFormId = x.AuditDecFormId,
                        Signature = x.Signature,
                        Type = x.Type
                    }).ToListAsync();

                }
                return query;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<EntityPrimaryDto> GetBusinessEntity(int businessentityId)
        {
            var query = new EntityPrimaryDto();
            try
            {
                query = await _businessEntityRepository.GetAll().Include(x => x.FacilityType).Where(x => x.Id == businessentityId)
                .Select(x => new EntityPrimaryDto
                {
                    LicenseNumber = x.LicenseNumber,
                    Name = x.CompanyName,
                    FacilityTypeId = x.FacilityTypeId,
                    FacilityType = x.FacilityType.Name
                }).FirstOrDefaultAsync();

                return query;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<TechnicalCommiteDto> GetAllTechnicalCommite(int? projectId, int? BusinessEntityId)
        {
            var items = new TechnicalCommiteDto
            {
                Approval = new List<ApprovalDto>(),
                Authority = new List<AuthorityDto>(),
                AuditAgency = new List<AuditAgencyDto>()
            };

            var auditProjectObj = await _auditProjectRepository.GetAll().Where(x => x.Id == projectId).FirstOrDefaultAsync();
            try
            {
                if (auditProjectObj.LeadAuditorId != null)
                {
                    var obj = await _UserRepository.GetAll().Where(x => x.Id == auditProjectObj.LeadAuditorId).Select(x => new ApprovalDto
                    {
                        Id = x.Id,
                        Name = x != null ? x.FullName : null,
                        Signature = null,
                        Type = BusinessEntityWorkflowActorType.Approver
                    }).FirstOrDefaultAsync();

                    items.Approval.Add(obj);
                }

                var authorityUser = await _UserRepository.GetAll().Where(x => x.Type == UserOriginType.Authority).Select(x => new ApprovalDto
                {
                    Id = x.Id,
                    Name = x != null ? x.FullName : null,
                    Signature = null,
                    Type = BusinessEntityWorkflowActorType.Reviewer
                }).FirstOrDefaultAsync();

                if (authorityUser != null)
                {
                    items.Approval.Add(authorityUser);
                }

                //items.Approval = await _businessEntityWorkFlowRepository.GetAll().Where(x => x.BusinessEntityId == BusinessEntityId && (x.Type == BusinessEntityWorkflowActorType.Approver || x.Type == BusinessEntityWorkflowActorType.Reviewer) && x.UserId != null).Include(x => x.User).Select(x => new ApprovalDto()
                //{
                //    Id = x.UserId,
                //    Name = x.User != null ? x.User.FullName : null,
                //    Signature = null,
                //    Type = x.Type

                //}).ToListAsync();
                var getchechauditagency = await _externalAssessmentRepository.GetAll().Where(x => x.AuditProjectId == projectId).Select(x => x.VendorId).FirstOrDefaultAsync();
                if (getchechauditagency != null)
                {
                    items.AuditAgency = await _businessEntityWorkFlowRepository.GetAll().Where(x => x.BusinessEntityId == getchechauditagency && (x.Type == BusinessEntityWorkflowActorType.Authority)).Include(x => x.User).Select(x => new AuditAgencyDto()
                    {
                        MemberNameId = x.UserId,
                        Id = x.Id,
                        Name = x.User != null ? x.User.FullName : null,
                        Signature = null,
                        AuditDecFormId = null,
                        Type = x.Type
                    }).ToListAsync();
                }
                var getAutorativeDocument = await _auditProjectAuthoritativeDocumentRepository.GetAll().Where(x => x.AuditProjectId == projectId).Select(x => x.AuthoritativeDocumentId).FirstOrDefaultAsync();



                items.Authority = await _authorityworkflowactorRepository.GetAll().Where(x => x.AuthoritativeDocumentId == getAutorativeDocument && x.Type == BusinessEntityWorkflowActorType.Approver).Include(x => x.User).Select(x => new AuthorityDto
                {

                    Id = x.Id,
                    MemberNameId = (x.User != null) ? x.UserId : 0,
                    Name = x.User != null ? x.User.FullName : null,
                    Signature = null,
                    AuditDecFormId = null,
                    Type = BusinessEntityWorkflowActorType.Authority

                }).ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string ReplaceBodyFucntion(AuditProject getauditProjectParameter, ExternalAssessment itemParameter, List<AuditFacilityDto> auditentitydetailsParameter, List<DynamicParameterValue> getotherParameter, List<string> input, string output)
        {
            var auditbody = output;
            var getauditProject = getauditProjectParameter;
            var auditentitydetails = auditentitydetailsParameter;
            var getother = getotherParameter;
            var item = itemParameter;

            input.ForEach(x =>
            {
                switch (x)
                {
                    case "{Code}":
                        {
                            auditbody = (getauditProject.Code != null) ? auditbody.Replace("{Code}", getauditProject.Code) : auditbody.Replace("{Code}", "");
                            break;
                        }
                    case "{AuditTitle}":
                        {
                            auditbody = (getauditProject.AuditTitle != null) ? auditbody.Replace("{AuditTitle}", getauditProject.AuditTitle) : auditbody.Replace("{AuditTitle}", "");
                            break;
                        }
                    case "{FiscalYear}":
                        {
                            auditbody = (getauditProject.FiscalYear != null) ? auditbody.Replace("{FiscalYear}", getauditProject.FiscalYear) : auditbody.Replace("{FiscalYear}", "");
                            break;
                        }
                    case "{AuditScope}":
                        {
                            auditbody = (getauditProject.FiscalYear != null) ? auditbody.Replace("{AuditScope}", getauditProject.FiscalYear) : auditbody.Replace("{AuditScope}", "");
                            break;
                        }
                    case "{AuditObjective}":
                        {
                            auditbody = (getauditProject.AuditObjective != null) ? auditbody.Replace("{AuditObjective}", getauditProject.AuditObjective) : auditbody.Replace("{AuditObjective}", "");
                            break;
                        }
                    case "{AuditAreaName}":
                        {
                            auditbody = (getauditProject.AuditArea != null) ? auditbody.Replace("{AuditAreaName}", getauditProject.AuditArea.Value) : auditbody.Replace("{AuditAreaName}", "");
                            break;
                        }
                    case "{AuditTypeName}":
                        {
                            auditbody = (getauditProject.AuditArea != null) ? auditbody.Replace("{AuditTypeName}", getauditProject.AuditType.Value) : auditbody.Replace("{AuditTypeName}", "");
                            break;
                        }
                    case "{AuditStatusName}":
                        {
                            auditbody = (getauditProject.AuditArea != null) ? auditbody.Replace("{AuditStatusName}", getauditProject.AuditStatus.Value) : auditbody.Replace("{AuditStatusName}", "");
                            break;
                        }
                    case "{AuditCriteria}":
                        {
                            auditbody = (getauditProject.AuditCriteria != null) ? auditbody.Replace("{AuditCriteria}", getauditProject.AuditCriteria) : auditbody.Replace("{AuditCriteria}", "");
                            break;
                        }
                    case "{AuditManagerName}":
                        {
                            auditbody = (getauditProject.AuditManager != null) ? auditbody.Replace("{AuditManagerName}", getauditProject.AuditManager.Name) : auditbody.Replace("{AuditManagerName}", "");
                            break;
                        }
                    case "{AuditCoordinatorName}":
                        {
                            auditbody = (getauditProject.AuditCoordinator != null) ? auditbody.Replace("{AuditCoordinatorName}", getauditProject.AuditCoordinator.Name) : auditbody.Replace("{AuditCoordinatorName}", "");
                            break;
                        }
                    case "{EntityGroupName}":
                        {
                            auditbody = (getauditProject.EntityGroup != null) ? auditbody.Replace("{EntityGroupName}", getauditProject.EntityGroup.Name) : auditbody.Replace("{EntityGroupName}", "");
                            break;
                        }
                    case "{EntityName}":
                        {
                            if (getauditProject.EntityGroupId != null)
                            {
                                auditbody = (item.BusinessEntity != null) ? auditbody.Replace("{EntityName}", item.BusinessEntity.CompanyName) : auditbody.Replace("{EntityName}", "");
                            }
                            break;
                        }
                    case "{LeadAuditorName}":
                        {
                            auditbody = (getauditProject.LeadAuditor != null) ? auditbody.Replace("{LeadAuditorName}", getauditProject.LeadAuditor.Name) : auditbody.Replace("{LeadAuditorName}", "");
                            break;
                        }
                    case "{StartDate}":
                        {
                            auditbody = (getauditProject.StartDate != null) ? auditbody.Replace("{StartDate}", Convert.ToDateTime(getauditProject.StartDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{StartDate}", "");
                            break;
                        }
                    case "{EndDate}":
                        {
                            auditbody = (getauditProject.EndDate != null) ? auditbody.Replace("{EndDate}", Convert.ToDateTime(getauditProject.EndDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{EndDate}", "");
                            break;
                        }
                    case "{StageStartDate}":
                        {
                            auditbody = (getauditProject.StageStartDate != null) ? auditbody.Replace("{StageStartDate}", Convert.ToDateTime(getauditProject.StageStartDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{StageStartDate}", "");
                            break;
                        }
                    case "{StageEndDate}":
                        {
                            auditbody = (getauditProject.StageEndDate != null) ? auditbody.Replace("{StageEndDate}", Convert.ToDateTime(getauditProject.StageEndDate).ToString("dd-MMM-yyyy")) : auditbody.Replace("{StageEndDate}", "");
                            break;
                        }
                    case "{StageAuditDuration}":
                        {
                            auditbody = (getauditProject.StageAuditDuration != null) ? auditbody.Replace("{StageAuditDuration}", getauditProject.StageAuditDuration) : auditbody.Replace("{StageAuditDuration}", "");
                            break;
                        }
                    case "{AuditDuration}":
                        {
                            auditbody = (getauditProject.AuditDuration != null) ? auditbody.Replace("{AuditDuration}", getauditProject.AuditDuration) : auditbody.Replace("{AuditDuration}", "");
                            break;
                        }
                    case "{Address}":
                        {
                            auditbody = (getauditProject.Address != null) ? auditbody.Replace("{Address}", getauditProject.Address) : auditbody.Replace("{Address}", "");
                            break;
                        }
                    case "{City}":
                        {
                            auditbody = (getauditProject.City != null) ? auditbody.Replace("{City}", getauditProject.City) : auditbody.Replace("{City}", "");
                            break;
                        }
                    case "{PostalCode}":
                        {
                            auditbody = (getauditProject.PostalCode != null) ? auditbody.Replace("{PostalCode}", getauditProject.PostalCode) : auditbody.Replace("{PostalCode}", "");
                            break;
                        }
                    case "{CountryName}":
                        {
                            auditbody = (getauditProject.Country != null) ? auditbody.Replace("{CountryName}", getauditProject.Country.Name) : auditbody.Replace("{CountryName}", "");
                            break;
                        }
                    case "{VendorName}":
                        {
                            auditbody = (item.VendorId != null) ? auditbody.Replace("{VendorName}", item.Vendor.CompanyName) : auditbody.Replace("{VendorName}", "");
                            break;
                        }
                    case "{AuditStatus}":
                        {
                            auditbody = (getauditProject != null) ? auditbody.Replace("{AuditStatus}", getauditProject.AuditStatus.Value) : auditbody.Replace("{AuditStatus}", "");
                            break;
                        }
                    case "{EntityList}":
                        {

                            if (auditentitydetails.Count > 0)
                            {
                                var sb = "";
                                sb = sb + "<div style='float:center !important'>";

                                sb = sb + "<table style='width:100%; font-size: 12px;border-collapse: collapse'>";
                                sb = sb + "<tr style='background-color:sandybrown; border:solid 1px black; font-size: 8px !important'>";
                                sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>LICENSE NUMBER</th>";
                                sb = sb + "<th style='width:55%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY NAME</th>";
                                sb = sb + "<th style='width:15%; border:solid 1px black;font-family:Arial, Helvetica;text-align: center; font-size: 12px !important'>FACILITY TYPE</th></tr>";
                                foreach (var item in auditentitydetails)
                                {
                                    sb = sb + "<tr style='border:solid 1px black'>";
                                    sb = sb + "<td style='border: solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.LicenseNumber + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.CompanyName + "</td>";
                                    sb = sb + "<td style='border:solid 1px black; font-family:Arial, Helvetica, sans-serif;text-align: center;font-size: 8px !important'>" + item.FacilityType + "</td>";
                                    sb = sb + "</tr>";
                                }
                                sb = sb + "</table></div>";

                                auditbody = auditbody.Replace("{EntityList}", sb);
                            }
                            break;
                        }
                    case "{Link}":
                        {
                            var checkstatusEntityAccepted = getother.Where(x => x.Value.ToLower().Trim() == (" Entity Notified").ToLower().Trim()).FirstOrDefault();
                            if (checkstatusEntityAccepted.Id == getauditProject.AuditStatusId)
                            {
                                var link = AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId);
                                var temp = link.Split("/account/");
                                link = "" + temp[0] + "/#/account/" + temp[1];
                                if (!link.IsNullOrEmpty())
                                {
                                    link = EncryptauditProjectQueryParameters(link);
                                }
                                auditbody = auditbody.Replace("{Link}", link);
                            }
                            else
                            {
                                auditbody = auditbody.Replace("{Link}", "");
                            }
                            break;
                        }
                }

            });
            return auditbody;
        }

        private string EncryptauditProjectQueryParameters(string link, string encrptedParameterName = "auditProjectId")
        {
            if (!link.Contains("?"))
            {
                return link;
            }

            var basePath = link.Substring(0, link.IndexOf('?'));
            var query = link.Substring(link.IndexOf('?')).TrimStart('?');
            return basePath + "?" + encrptedParameterName + "=" + HttpUtility.UrlEncode(SimpleStringCipher.Instance.Encrypt(query));
        }

        private string ReplaceValueFunction(AuditProject getauditProjectParameter, ExternalAssessment itemParameter, List<string> input, string output)
        {
            var AuditEmailsubject = output;
            var getauditProject = getauditProjectParameter;
            var item = itemParameter;

            input.ForEach(x =>
            {
                switch (x)
                {
                    case "{Code}":
                        {
                            AuditEmailsubject = (getauditProject.AuditTitle != null) ? AuditEmailsubject.Replace("{Code}", getauditProject.Code) : AuditEmailsubject.Replace("{Code}", "");
                            break;
                        }
                    case "{AuditTitle}":
                        {
                            AuditEmailsubject = (getauditProject.AuditTitle != null) ? AuditEmailsubject.Replace("{AuditTitle}", getauditProject.AuditTitle) : AuditEmailsubject.Replace("{AuditTitle}", "");
                            break;
                        }
                    case "{FiscalYear}":
                        {
                            AuditEmailsubject = (getauditProject.FiscalYear != null) ? AuditEmailsubject.Replace("{FiscalYear}", getauditProject.FiscalYear) : AuditEmailsubject.Replace("{FiscalYear}", "");
                            break;
                        }
                    case "{AuditScope}":
                        {
                            AuditEmailsubject = (getauditProject.AuditScope != null) ? AuditEmailsubject.Replace("{AuditScope}", getauditProject.AuditScope) : AuditEmailsubject.Replace("{AuditScope}", "");
                            break;
                        }
                    case "{AuditObjective}":
                        {
                            AuditEmailsubject = (getauditProject.AuditObjective != null) ? AuditEmailsubject.Replace("{AuditObjective}", getauditProject.AuditObjective) : AuditEmailsubject.Replace("{AuditObjective}", "");
                            break;
                        }
                    case "{AuditAreaName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditArea != null) ? AuditEmailsubject.Replace("{AuditAreaName}", getauditProject.AuditArea.Value) : AuditEmailsubject.Replace("{AuditAreaName}", "");
                            break;
                        }
                    case "{AuditTypeName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditArea != null) ? AuditEmailsubject.Replace("{AuditTypeName}", getauditProject.AuditType.Value) : AuditEmailsubject.Replace("{AuditTypeName}", "");
                            break;
                        }
                    case "{AuditStatusName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditArea != null) ? AuditEmailsubject.Replace("{AuditStatusName}", getauditProject.AuditStatus.Value) : AuditEmailsubject.Replace("{AuditStatusName}", "");
                            break;
                        }
                    case "{AuditCriteria}":
                        {
                            AuditEmailsubject = (getauditProject.AuditCriteria != null) ? AuditEmailsubject.Replace("{AuditCriteria}", getauditProject.AuditCriteria) : AuditEmailsubject.Replace("{AuditCriteria}", "");
                            break;
                        }
                    case "{AuditManagerName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditManager != null) ? AuditEmailsubject.Replace("{AuditManagerName}", getauditProject.AuditManager.Name) : AuditEmailsubject.Replace("{AuditManagerName}", "");
                            break;
                        }
                    case "{AuditCoordinatorName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditCoordinator != null) ? AuditEmailsubject.Replace("{AuditCoordinatorName}", getauditProject.AuditCoordinator.Name) : AuditEmailsubject.Replace("{AuditCoordinatorName}", "");
                            break;
                        }
                    case "{EntityGroupName}":
                        {
                            AuditEmailsubject = (getauditProject.EntityGroup != null) ? AuditEmailsubject.Replace("{EntityGroupName}", getauditProject.EntityGroup.Name) : AuditEmailsubject.Replace("{EntityGroupName}", "");
                            break;
                        }
                    case "{EntityName}":
                        {
                            AuditEmailsubject = (item.BusinessEntity != null) ? AuditEmailsubject.Replace("{EntityName}", item.BusinessEntity.CompanyName) : AuditEmailsubject.Replace("{EntityName}", "");
                            break;
                        }
                    case "{LeadAuditorName}":
                        {
                            AuditEmailsubject = (getauditProject.LeadAuditor != null) ? AuditEmailsubject.Replace("{LeadAuditorName}", getauditProject.LeadAuditor.Name) : AuditEmailsubject.Replace("{LeadAuditorName}", "");
                            break;
                        }
                    case "{StartDate}":
                        {
                            AuditEmailsubject = (getauditProject.StartDate != null) ? AuditEmailsubject.Replace("{StartDate}", Convert.ToDateTime(getauditProject.StartDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{StartDate}", "");
                            break;
                        }
                    case "{EndDate}":
                        {
                            AuditEmailsubject = (getauditProject.EndDate != null) ? AuditEmailsubject.Replace("{EndDate}", Convert.ToDateTime(getauditProject.EndDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{EndDate}", "");
                            break;
                        }
                    case "{StageStartDate}":
                        {
                            AuditEmailsubject = (getauditProject.StageStartDate != null) ? AuditEmailsubject.Replace("{StageStartDate}", Convert.ToDateTime(getauditProject.StageStartDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{StageStartDate}", "");
                            break;
                        }
                    case "{StageEndDate}":
                        {
                            AuditEmailsubject = (getauditProject.StageEndDate != null) ? AuditEmailsubject.Replace("{StageEndDate}", Convert.ToDateTime(getauditProject.StageEndDate).ToString("dd-MMM-yyyy")) : AuditEmailsubject.Replace("{StageEndDate}", "");
                            break;
                        }
                    case "{StageAuditDuration}":
                        {
                            AuditEmailsubject = (getauditProject.StageAuditDuration != null) ? AuditEmailsubject.Replace("{StageAuditDuration}", getauditProject.StageAuditDuration) : AuditEmailsubject.Replace("{StageAuditDuration}", "");
                            break;
                        }
                    case "{AuditDuration}":
                        {
                            AuditEmailsubject = (getauditProject.AuditDuration != null) ? AuditEmailsubject.Replace("{AuditDuration}", getauditProject.AuditDuration) : AuditEmailsubject.Replace("{AuditDuration}", "");
                            break;
                        }
                    case "{Address}":
                        {
                            AuditEmailsubject = (getauditProject.Address != null) ? AuditEmailsubject.Replace("{Address}", getauditProject.Address) : AuditEmailsubject.Replace("{Address}", "");
                            break;
                        }
                    case "{City}":
                        {
                            AuditEmailsubject = (getauditProject.City != null) ? AuditEmailsubject.Replace("{City}", getauditProject.City) : AuditEmailsubject.Replace("{City}", "");
                            break;
                        }
                    case "{PostalCode}":
                        {
                            AuditEmailsubject = (getauditProject.PostalCode != null) ? AuditEmailsubject.Replace("{PostalCode}", getauditProject.PostalCode) : AuditEmailsubject.Replace("{PostalCode}", "");
                            break;
                        }
                    case "{CountryName}":
                        {
                            AuditEmailsubject = (getauditProject.Country != null) ? AuditEmailsubject.Replace("{CountryName}", getauditProject.Country.Name) : AuditEmailsubject.Replace("{CountryName}", "");
                            break;
                        }
                    case "{VendorName}":
                        {
                            AuditEmailsubject = (item.VendorId != null) ? AuditEmailsubject.Replace("{VendorName}", item.Vendor.CompanyName) : AuditEmailsubject.Replace("{VendorName}", "");
                            break;
                        }
                    case "{AuditStatus}":
                        {
                            AuditEmailsubject = (getauditProject != null) ? AuditEmailsubject.Replace("{AuditStatus}", getauditProject.AuditStatus.Value) : AuditEmailsubject.Replace("{AuditStatus}", "");
                            break;
                        }
                    case "{Link}":
                        {
                            var link = AppUrlService.CreateAuditProjectNotificationUrlFormat(AbpSession.TenantId.Value, (long)item.AuditProjectId);
                            var temp = link.Split("/account/");
                            link = "" + temp[0] + "/#/account/" + temp[1];
                            if (!link.IsNullOrEmpty())
                            {
                                link = EncryptauditProjectQueryParameters(link);
                            }
                            AuditEmailsubject = AuditEmailsubject.Replace("{Link}", link);
                            break;
                        }
                }

            });

            return AuditEmailsubject;
        }


    }
}
