using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Abp.Organizations;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.Authorization.Roles;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Countries;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.WorkFllows;
using LockthreatCompliance.WrokFlows;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Xml;
using System.IO;
using LockthreatCompliance.ThirdpartyApi.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.UI;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.AuditProjects.Dtos;
using Abp.DynamicEntityParameters;
using System.Web;
using Abp.Runtime.Security;
using LockthreatCompliance.Url;
using Abp.Collections.Extensions;
using Abp.Extensions;


namespace LockthreatCompliance.Hangfire
{
    public class AuditPreRequesReminderHangFireAppservice : LockthreatComplianceAppServiceBase, IAuditPreRequesReminderHangFireAppservice
    {


        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<StateAction, long> _stateActionRepository;
        private readonly IRepository<WorkFlowPage, long> _WorkFlowPageRepository;
        private readonly IUserEmailer _userEmailer;
        private readonly ICustomTemplateAppService _customTemplateAppServiceEmailer;
        private readonly IRepository<EntityApplicationSetting> _entityApplicationSettingRepository;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<BusinessEntities.BusinessEntity, int> _businessEntityRepository;
        private readonly IRepository<ExternalAssessment, int> _externalAssessmentRepository;
        private readonly IRepository<AuditProjectStatus, long> _auditProjectStatusRepository;
        private readonly IRepository<AssessmentStatusLog, long> _assessmentStatusLogRepository;
        private readonly IRepository<BusinessRiskStatusLog, long> _businessRiskLogRepository;
        private readonly IRepository<EntityGroupMember> _entityGroupMemberRepository;
        private readonly IRepository<BusinessEntityThirdParty> _businessEntityThirdPartyRepository;
        private readonly IRepository<FacilitySubType> _facilitySubTypeRepository;
        private readonly ICustomDynamicAppService _customDynamicAppServiceRepository;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<EntityGroup, int> _entityGroupRepository;
        private readonly IObjectMapper _objectMapper;

        public IAppUrlService AppUrlService { get; set; }
        private readonly OrganizationUnitManager _organizationUnitManager;

        private readonly ApplicationSession _applicationSession;

        private readonly IRepository<Country> _countriesRepository;
        private readonly IRepository<FacilityType> _facilityTypeRepository;
        private const string defaultPassword = "123qwe";
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RoleManager _roleManager;

        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<WorkFlowPage, long> _workflowpageRepository;
        private readonly IRepository<Template, long> _templateRepository;

        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<EmailNotificationTemplate, long> _emailnotificationRepository;
        private readonly IRepository<EmailReminderTemplate, long> _emailReminderTemplateRepository;

        public AuditPreRequesReminderHangFireAppservice(IRepository<EmailReminderTemplate, long> emailReminderTemplateRepository, IRepository<EntityGroup, int> entityGroupRepository, IRepository<EmailNotificationTemplate, long> emailnotificationRepository, ICustomDynamicAppService customDynamicAppServiceRepository, IRepository<User, long> userRepository, IRepository<BusinessEntityThirdParty> businessEntityThirdPartyRepository,
            IRepository<StateAction, long> stateActionRepository, RoleManager roleManager, IRepository<Role> roleRepository,
            IUserEmailer userEmailer, ICustomTemplateAppService customTemplateAppServiceEmailer, IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
            IRepository<AuditProject, long> auditProjectRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<BusinessEntities.BusinessEntity, int> businessEntityRepository, IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<ExternalAssessment, int> externalAssessmentRepository, OrganizationUnitManager organizationUnitManager, IPasswordHasher<User> passwordHasher,
            IRepository<AuditProjectStatus, long> auditProjectStatusRepository, IRepository<Template, long> templateRepository,
            IRepository<AssessmentStatusLog, long> assessmentStatusLogRepository, IRepository<FacilityType> facilityTypeRepository, IRepository<WorkFlowPage, long> workflowpageRepository,
            IRepository<BusinessRiskStatusLog, long> businessRiskLogRepository, IRepository<FacilitySubType> facilitySubTypeRepository,
            IRepository<EntityGroupMember> entityGroupMemberRepository, IObjectMapper objectMapper, ApplicationSession applicationSession, IRepository<Country> countriesRepository

           )
        {
            _emailReminderTemplateRepository = emailReminderTemplateRepository;
            _entityGroupRepository = entityGroupRepository;
            AppUrlService = NullAppUrlService.Instance;
            _emailnotificationRepository = emailnotificationRepository;
            _customDynamicAppServiceRepository = customDynamicAppServiceRepository;
            _roleRepository = roleRepository;
            _templateRepository = templateRepository;
            _workflowpageRepository = workflowpageRepository;
            _facilitySubTypeRepository = facilitySubTypeRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _facilityTypeRepository = facilityTypeRepository;
            _countriesRepository = countriesRepository;
            _applicationSession = applicationSession;
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _objectMapper = objectMapper;
            _businessEntityThirdPartyRepository = businessEntityThirdPartyRepository;
            _userRepository = userRepository;
            _stateActionRepository = stateActionRepository;
            _userEmailer = userEmailer;
            _customTemplateAppServiceEmailer = customTemplateAppServiceEmailer;
            _entityApplicationSettingRepository = entityApplicationSettingRepository;
            _auditProjectRepository = auditProjectRepository;
            _businessEntityRepository = businessEntityRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditProjectStatusRepository = auditProjectStatusRepository;
            _assessmentStatusLogRepository = assessmentStatusLogRepository;
            _businessRiskLogRepository = businessRiskLogRepository;
            _entityGroupMemberRepository = entityGroupMemberRepository;

        }

        public async Task SendReaminderToAuditProjectPrerequest()
         {
            try
            {
                var auditstauss = _customDynamicAppServiceRepository.GetAuditStatus(LockthreatComplianceConsts.AuditStatus.Trim().ToLower());
                if (auditstauss != null)
                {
                    var checkAuditStaus = auditstauss.Result.Where(x => x.Name.Trim().ToLower() == LockthreatComplianceConsts.PreAuditInformationRequest.Trim().ToLower()).FirstOrDefault();

                    var checkAuditProject = await _auditProjectRepository.GetAll().Where(x =>x.AuditStatusId==checkAuditStaus.Id).Select(x => x.Id).ToListAsync();

                    var getpage = _workflowpageRepository.GetAll().Where(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.Auditpage.Trim().ToLower()).FirstOrDefault();


                        var auditentitydetails = new List<AuditFacilityDto>();
                        var getother = new List<DynamicParameterValue>();

                        auditstauss.Result.ForEach(obj =>
                        {
                            var item = new DynamicParameterValue();
                            item.Id = obj.Id;
                            item.Value = obj.Name;
                            getother.Add(item);

                        });


                    var getReminderdays = _emailReminderTemplateRepository.GetAll().Where(x => x.AuditStatusId == checkAuditStaus.Id && x.WorkFlowPageId == getpage.Id).FirstOrDefault();

                    if (getReminderdays != null)
                    {

                        checkAuditProject.ForEach(obj =>
                            {
                                string auditbody = null;
                                string AuditEmailsubject = null;
                                HashSet<string> emails = new HashSet<string>();
                                HashSet<string> ccemail = new HashSet<string>();
                                HashSet<string> bccemail = new HashSet<string>();
                                var getTemplate = new EmailReminderTemplate();

                                var getauditProjects = _auditProjectRepository.GetAll().Where(x => x.Id == obj).FirstOrDefault();
                                if (getauditProjects.LastModificationTime != null)
                                {
                                    var getcheckAuidtProject = _auditProjectRepository.GetAll().Where(x => x.Id == obj).FirstOrDefault();


                                    DateTime CheckDate = getauditProjects.LastModificationTime.Value.AddDays(getReminderdays.Days);

                                    if (CheckDate.ToShortDateString() == DateTime.UtcNow.ToShortDateString())
                                    {

                                        var getcheck = _externalAssessmentRepository.GetAll().Include(x => x.BusinessEntity).Where(x => x.AuditProjectId == obj).ToList();

                                        foreach (var item in getcheck)
                                        {
                                            var getauditProject = _auditProjectRepository.GetAll().Where(x => x.Id == item.AuditProjectId).FirstOrDefault();
                                            getTemplate = _emailReminderTemplateRepository.GetAll().Where(x => x.AuditStatusId == getauditProject.AuditStatusId && x.WorkFlowPageId == getpage.Id).FirstOrDefault();
                                            if (getTemplate != null)
                                            {

                                                if (getauditProject.EntityGroupId != null)
                                                {

                                                    var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == (int)_applicationSession.TenantId).FirstOrDefault();

                                                    List<string> templateSubject = new List<string>();
                                                    var auditprojectsubjectBody = getTemplate.Subject;

                                                    AuditEmailsubject = getTemplate.Subject.ToString();

                                                    while (auditprojectsubjectBody.Contains("{"))
                                                    {
                                                        templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                                        auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                                    };

                                                    AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                                    var auditTemplate = getTemplate.EmailBody;

                                                    var auditTo = getTemplate.To;
                                                    List<string> templatevariables = new List<string>();

                                                    while (auditTo.Contains("{"))
                                                    {
                                                        templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                                                        auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                                    };

                                                    var auditCc = getTemplate.Cc;
                                                    List<string> templateCc = new List<string>();

                                                    while (auditCc.Contains("{"))
                                                    {
                                                        templateCc.Add("{" + auditCc.Split('{', '}')[1] + "}");
                                                        auditCc = auditCc.Replace("{" + auditCc.Split('{', '}')[1] + "}", "");
                                                    };

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
                                                                    break;
                                                                }

                                                        }

                                                    });

                                                    var auditBcc = getTemplate.Bcc;

                                                    List<string> templateBcc = new List<string>();
                                                    while (auditBcc.Contains("{"))
                                                    {
                                                        templateBcc.Add("{" + auditBcc.Split('{', '}')[1] + "}");
                                                        auditBcc = auditBcc.Replace("{" + auditBcc.Split('{', '}')[1] + "}", "");
                                                    };

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
                                                                    break;
                                                                }

                                                        }

                                                    });

                                                    if (getadminemail != null)
                                                    {
                                                        ccemail.Add(getadminemail.EmailAddress);
                                                    }

                                                    List<string> templateBody = new List<string>();
                                                    var auditprojectBody = getTemplate.EmailBody;

                                                    auditbody = getTemplate.EmailBody.ToString();

                                                    while (auditprojectBody.Contains("{"))
                                                    {
                                                        templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                                        auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                                    };

                                                    auditbody = ReplaceBodyFucntion(getauditProject, item, auditentitydetails, getother, templateBody, auditbody);

                                                }

                                                else
                                                {
                                                    var getadminemail = _userRepository.GetAll().Where(x => x.TenantId == 1).FirstOrDefault();

                                                    if (getTemplate != null)
                                                    {


                                                        List<string> templateSubject = new List<string>();
                                                        var auditprojectsubjectBody = getTemplate.Subject;

                                                        AuditEmailsubject = getTemplate.Subject.ToString();

                                                        while (auditprojectsubjectBody.Contains("{"))
                                                        {
                                                            templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                                            auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                                        };

                                                        AuditEmailsubject = ReplaceValueFunction(getauditProject, item, templateSubject, AuditEmailsubject);

                                                        var auditTemplate = getTemplate.EmailBody;
                                                        var auditTo = getTemplate.To;
                                                        List<string> templatevariables = new List<string>();

                                                        while (auditTo.Contains("{"))
                                                        {
                                                            templatevariables.Add("{" + auditTo.Split('{', '}')[1] + "}");
                                                            auditTo = auditTo.Replace("{" + auditTo.Split('{', '}')[1] + "}", "");
                                                        };

                                                        var auditCc = getTemplate.Cc;
                                                        List<string> templateCc = new List<string>();

                                                        while (auditCc.Contains("{"))
                                                        {
                                                            templateCc.Add("{" + auditCc.Split('{', '}')[1] + "}");
                                                            auditCc = auditCc.Replace("{" + auditCc.Split('{', '}')[1] + "}", "");
                                                        };

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
                                                                        break;
                                                                    }

                                                            }

                                                        });

                                                        var auditBcc = getTemplate.Bcc;

                                                        List<string> templateBcc = new List<string>();
                                                        while (auditBcc.Contains("{"))
                                                        {
                                                            templateBcc.Add("{" + auditBcc.Split('{', '}')[1] + "}");
                                                            auditBcc = auditBcc.Replace("{" + auditBcc.Split('{', '}')[1] + "}", "");
                                                        };

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
                                                                        break;
                                                                    }

                                                            }

                                                        });

                                                        if (getadminemail != null)
                                                        {
                                                            ccemail.Add(getadminemail.EmailAddress);
                                                        }

                                                        List<string> templateBody = new List<string>();
                                                        var auditprojectBody = getTemplate.EmailBody;

                                                        auditbody = getTemplate.EmailBody.ToString();

                                                        while (auditprojectBody.Contains("{"))
                                                        {
                                                            templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                                            auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                                        };

                                                        auditbody = ReplaceBodyFucntion(getauditProject, item, auditentitydetails, getother, templateBody, auditbody);


                                                    }

                                                }
                                            }
                                        }

                                        _userEmailer.AuditProjectEntityNotifications(emails, ccemail, bccemail, AuditEmailsubject, (int)_applicationSession.TenantId, auditbody, (int)getauditProjects.AuditStatusId, getauditProjects.Id,
                                                        AppUrlService.CreateAuditProjectNotificationUrlFormat((int)_applicationSession.TenantId, (long)getauditProjects.Id));

                                    }
                                }

                            });
                    }
                    

                }

            }
            catch (Exception ex)
            {
                throw;
            }
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
                            var link = AppUrlService.CreateAuditProjectNotificationUrlFormat((int)_applicationSession.TenantId, (long)item.AuditProjectId);
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
                                var link = AppUrlService.CreateAuditProjectNotificationUrlFormat((int)_applicationSession.TenantId, (long)item.AuditProjectId);
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
    }
}
