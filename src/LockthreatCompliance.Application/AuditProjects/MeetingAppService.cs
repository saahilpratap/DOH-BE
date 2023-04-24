using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Security;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.EntityGroups;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Url;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace LockthreatCompliance.AuditProjects
{
    public class MeetingAppService : LockthreatComplianceAppServiceBase, IMeetingAppService
    {
        private readonly IRepository<AuditMeeting, long> _auditMeetingAppService;
        private readonly IRepository<AuditProject, long> _auditProjectRepository;
        private readonly IRepository<AuditProjectTeam, long> _auditProjectTeamRepository;
        private readonly IRepository<ExternalAssessment> _externalAssessmentRepository;
        private readonly IRepository<AuditDocumentPath, long> _auditDocumentPathRepository;
        private readonly IRepository<AuditDocSubModelPath, long> _auditDocSubModelPathRepository;
        private readonly ICustomDynamicAppService _customDynamicAppService;
        private readonly IExternalAssessmentsAppService _externalAssessmentsAppService;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<EntityGroup> _entityGroupRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParameterValueRepository;
        public IAppUrlService AppUrlService { get; set; }
        private readonly IUserEmailer _userEmailer;

        public MeetingAppService(IRepository<AuditProject, long> auditProjectRepository, ICustomDynamicAppService customDynamicAppService,
           IRepository<AuditProjectTeam, long> auditProjectTeamRepository, IRepository<AuditDocumentPath, long> auditDocumentPathRepository,
           IRepository<ExternalAssessment> externalAssessmentRepository, IRepository<AuditDocSubModelPath, long> auditDocSubModelPathRepository,
           IExternalAssessmentsAppService externalAssessmentsAppService, IRepository<BusinessEntity> businessEntityRepository,
           IUserEmailer userEmailer, IRepository<AuditMeeting, long> auditMeetingAppService, IRepository<EntityGroup> entityGroupRepository, IRepository<DynamicParameterValue> dynamicParameterValueRepository)
        {
            _auditProjectRepository = auditProjectRepository;
            _auditProjectTeamRepository = auditProjectTeamRepository;
            _externalAssessmentRepository = externalAssessmentRepository;
            _auditDocumentPathRepository = auditDocumentPathRepository;
            _auditDocSubModelPathRepository = auditDocSubModelPathRepository;
            _customDynamicAppService = customDynamicAppService;
            _externalAssessmentsAppService = externalAssessmentsAppService;
            _businessEntityRepository = businessEntityRepository;
            _userEmailer = userEmailer;
            AppUrlService = NullAppUrlService.Instance;
            _auditMeetingAppService = auditMeetingAppService;
            _entityGroupRepository = entityGroupRepository;
            _dynamicParameterValueRepository = dynamicParameterValueRepository;
        }

        public async Task<PagedResultDto<AuditMeetingDto>> GetAuditMeetings(GetAllMeetings input)
        {
            try
            {
                var query = _auditMeetingAppService.GetAll().Include(m => m.AuditOrg).Include(m => m.MeetingType).Include(m => m.AuditProject).Include(m => m.AuditVendor).
                    WhereIf(input.AuditProjectId > 0, m => m.AuditProjectId == input.AuditProjectId)
                    .WhereIf(input.AuditOrgId > 0, m => m.AuditOrgId == input.AuditOrgId).WhereIf(input.AuditVendorId > 0, m => m.AuditVendorId == input.AuditVendorId)
                    .WhereIf(!input.Filter.IsNullOrWhiteSpace(), u => u.MeetingTitle.Contains(input.Filter));

                var pagedAndFilteredReg = query
                   .OrderBy(input.Sorting)
                   .PageBy(input);

                var data = ObjectMapper.Map<List<AuditMeetingDto>>(pagedAndFilteredReg);
                var totalCount = await query.CountAsync();
                return new PagedResultDto<AuditMeetingDto>(totalCount, data.OrderByDescending(x => x.Id).ToList());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task AddUpdateAuditMeeting(AuditMeetingDto input)
        {
            try
            {

                string subject = null;
                List<string> toemails = new List<string>();
                List<string> ccemail = new List<string>();
                string AuditEmailsubject = null;

                if (input.Id == 0)
                {
                    if (input.EditorData != null)
                    {
                        var getauditProject = await _auditProjectRepository.GetAll().Where(x => x.Id == input.AuditProjectId).Include(x => x.AuditStage).FirstOrDefaultAsync();
                        List<string> templateSubject = new List<string>();
                        var auditprojectsubjectBody = input.EditorData;

                        AuditEmailsubject = input.EditorData.ToString();

                        while (auditprojectsubjectBody.Contains("{"))
                        {
                            templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                            auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                        };

                        AuditEmailsubject = ReplaceValueFunction(getauditProject, input, templateSubject, AuditEmailsubject);
                    }
                    input.EditorData = AuditEmailsubject;
                    var meeting = ObjectMapper.Map<AuditMeeting>(input);
                    input.Id = await _auditMeetingAppService.InsertAndGetIdAsync(meeting);
                }
                else
                {
                    AuditEmailsubject = input.EditorData;
                    var meeting = await _auditMeetingAppService.FirstOrDefaultAsync(input.Id);
                    if (input.Attachments.Any())
                    {
                        var documents = await _auditDocSubModelPathRepository.GetAll()
                            .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                            .ToListAsync();
                        foreach (var document in documents)
                        {
                            document.AuditMeetingId = meeting.Id;
                            document.Title = input.Attachments.FirstOrDefault(c => c.Code == document.Code).Title;
                        }
                    }
                    ObjectMapper.Map(input, meeting);
                }

                if (input.Attachments.Any())
                {
                    var documents = await _auditDocSubModelPathRepository.GetAll()
                        .Where(e => input.Attachments.Select(x => x.Code).Any(a => a == e.Code))
                        .ToListAsync();
                    foreach (var document in documents)
                    {
                        document.AuditMeetingId = input.Id;
                        document.Title = input.Attachments.FirstOrDefault(c => c.Code == document.Code).Title;
                    }
                }

                if (input.ToEmail != null)
                {
                    var template = input.EditorData;
                    subject = input.MeetingTitle;
                    var emailTo = input.ToEmail;
                    List<string> templatevariables = new List<string>();
                    List<string> emailToList = input.ToEmail.Split(',').ToList();

                    emailToList.ForEach(emailid =>
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

                    templatevariables.ForEach(x =>
                    {
                        switch (x)
                        {
                            case "{Business_Entity_Admin_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        toemails.Add(getbusinessadmin.AdminEmail);
                                    }
                                    break;
                                }
                            case "{Audit_Agency_Admin_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditVendorId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        toemails.Add(getbusinessadmin.AdminEmail);
                                    }
                                    break;
                                }
                            case "{Owner_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        if (getbusinessadmin.Owner_Email != null)
                                        {
                                            var splitEmail = getbusinessadmin.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    toemails.Add(i);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Director_Incharge_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        if (getbusinessadmin.Director_Incharge_Email != null)
                                        {
                                            var splitEmail = getbusinessadmin.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    toemails.Add(i);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{CISO_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        if (getbusinessadmin.CISO_Email != null)
                                        {
                                            var splitEmail = getbusinessadmin.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    toemails.Add(i);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Primary_Contact_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        if (getbusinessadmin.OfficialEmail != null)
                                        {
                                            var splitEmail = getbusinessadmin.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    toemails.Add(i);
                                                }
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "{Secondary_Contact_Email}":
                                {
                                    var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                    if (getbusinessadmin != null)
                                    {
                                        if (getbusinessadmin.BackupOfficialEmail != null)
                                        {
                                            var splitEmail = getbusinessadmin.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (var i in splitEmail)
                                            {
                                                string email = i.Trim();
                                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                                if (isEmail == true)
                                                {
                                                    toemails.Add(i);
                                                }
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

                    if (input.CCEmail != null)
                    {
                        var emailCc = input.CCEmail;
                        List<string> templateCc = new List<string>();
                        List<string> emailCcList = input.ToEmail.Split(',').ToList();

                        emailCcList.ForEach(emailid =>
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

                        templateCc.ForEach(x =>
                        {
                            switch (x)
                            {
                                case "{Business_Entity_Admin_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            ccemail.Add(getbusinessadmin.AdminEmail);
                                        }
                                        break;
                                    }
                                case "{Audit_Agency_Admin_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditVendorId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            ccemail.Add(getbusinessadmin.AdminEmail);
                                        }
                                        break;
                                    }
                                case "{Owner_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            if (getbusinessadmin.Owner_Email != null)
                                            {
                                                var splitEmail = getbusinessadmin.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                        }
                                        break;
                                    }
                                case "{Director_Incharge_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            if (getbusinessadmin.Director_Incharge_Email != null)
                                            {
                                                var splitEmail = getbusinessadmin.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                        }
                                        break;
                                    }
                                case "{CISO_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            if (getbusinessadmin.CISO_Email != null)
                                            {
                                                var splitEmail = getbusinessadmin.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                        }
                                        break;
                                    }
                                case "{Primary_Contact_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            if (getbusinessadmin.OfficialEmail != null)
                                            {
                                                var splitEmail = getbusinessadmin.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                        }
                                        break;
                                    }
                                case "{Secondary_Contact_Email}":
                                    {
                                        var getbusinessadmin = _businessEntityRepository.GetAll().Where(x => x.Id == input.AuditOrgId).FirstOrDefault();
                                        if (getbusinessadmin != null)
                                        {
                                            if (getbusinessadmin.BackupOfficialEmail != null)
                                            {
                                                var splitEmail = getbusinessadmin.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                        }
                                        break;
                                    }
                                case "{LeadAuditor_Email}":
                                    {
                                        break;
                                    }

                            }

                        });
                    }
                    var getMeetingId = _dynamicParameterValueRepository.GetAll().Where(x => x.Id == input.MeetingTypeId).FirstOrDefault();
                    if (getMeetingId != null)
                    {
                        if (getMeetingId.Value.Trim().ToLower() == "Meeting".Trim().ToLower())
                        {
                            await _userEmailer.SendmailMeeting(toemails, ccemail, subject, (int)input.TenantId, AuditEmailsubject, input.Attachments, input.StartDate, input.EndDate);
                        }
                    else
                        {
                            await _userEmailer.AuditMeetingsNotification(toemails, ccemail, subject, (int)input.TenantId, AuditEmailsubject, input.Attachments);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AuditMeetingDto> GetAuditMeetingForEdit(long id)
        {
            try
            {
                var query = _auditMeetingAppService.GetAll().Where(m => m.Id == id).Include(m => m.AuditOrg).Include(m => m.MeetingType).Include(m => m.AuditProject).Include(m => m.AuditVendor).FirstOrDefault();
                var data = ObjectMapper.Map<AuditMeetingDto>(query);
                var attachments = await _auditDocSubModelPathRepository.GetAll().Where(d => d.AuditMeetingId == query.Id).ToListAsync();
                data.Attachments = attachments.Select(e => new AttachmentWithTitleDto
                {
                    Code = e.Code,
                    Title = e.Title
                }).ToList();
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetAuditMeetingPdf(long id)
        {
            try
            {
                string data = "";
                var query = await _auditMeetingAppService.GetAll().Where(m => m.Id == id).Include(m => m.AuditOrg).Include(m => m.MeetingType).Include(m => m.AuditProject).Include(m => m.AuditVendor).FirstOrDefaultAsync();
                if (query != null)
                {
                    data = query.EditorData == null ? "" : query.EditorData;
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAuditMeeting(long id)
        {
            try
            {
                var query = _auditMeetingAppService.GetAll().Where(m => m.Id == id).FirstOrDefault();
                var attachments = await _auditDocSubModelPathRepository.GetAll().Where(d => d.AuditMeetingId == query.Id).ToListAsync();
                foreach (var item in attachments)
                {
                    await _auditDocSubModelPathRepository.HardDeleteAsync(item);
                }
                await _auditMeetingAppService.HardDeleteAsync(query);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BusinessEntityDto>> GetAllAuditEntitiesByVendor(int businessEntityId)
        {
            try
            {
                var query = await _externalAssessmentRepository.GetAll().Where(e => e.VendorId == businessEntityId && e.Status != AssessmentStatus.Approved && e.AuditProjectId > 0).Include(b => b.BusinessEntity).Select(e => e.BusinessEntity).Distinct().ToListAsync();
                var entities = ObjectMapper.Map<List<BusinessEntityDto>>(query);
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BusinessEntityDto>> GetAuditProjectByOrganization(long AuditProjectId)
        {
            try
            {
                var query = await _externalAssessmentRepository.GetAll().Where(e => e.AuditProjectId == AuditProjectId).Include(b => b.BusinessEntity).Select(e => e.BusinessEntity).Distinct().ToListAsync();
                var entities = ObjectMapper.Map<List<BusinessEntityDto>>(query);
                return entities;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AuditProjectLink>> GetAuditProjectByVendorAndEntity(int vendorId, int businessEntityId)
        {
            try
            {

                var query = await _externalAssessmentRepository.GetAll().Where(a => a.BusinessEntityId == businessEntityId && a.VendorId == vendorId && a.Status != AssessmentStatus.Approved && a.AuditProjectId > 0)
                            .Include(a => a.BusinessEntity).Include(a => a.AuthoritativeDocuments).Include(a => a.AuditProject).ToListAsync();

                List<AuditProjectLink> auditProjectDtos = new List<AuditProjectLink>();
                foreach (var item in query)
                {
                    var auditProjectDto = new AuditProjectLink
                    {
                        AuditProjectId = item.AuditProjectId.Value,
                        AuditTitle = item.AuditProject.AuditTitle,
                        BusinessEntityName = item.BusinessEntity.CompanyName
                    };

                    auditProjectDtos.Add(auditProjectDto);
                }
                return auditProjectDtos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<List<AuditProjectLink>> GetAuditProjectByVendor(int vendorId)
        {
            try
            {

                var query = await _externalAssessmentRepository.GetAll().Where(a => a.VendorId == vendorId && a.Status != AssessmentStatus.Approved && a.AuditProjectId > 0)
                            .Include(a => a.BusinessEntity).Include(a => a.AuthoritativeDocuments).Include(a => a.AuditProject).ToListAsync();

                List<AuditProjectLink> auditProjectDtos = new List<AuditProjectLink>();
                foreach (var item in query)
                {
                    var auditProjectDto = new AuditProjectLink
                    {
                        AuditProjectId = item.AuditProjectId.Value,
                        AuditTitle = item.AuditProject.AuditTitle,
                        BusinessEntityName = item.BusinessEntity.CompanyName
                    };

                    auditProjectDtos.Add(auditProjectDto);
                }
                return auditProjectDtos;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private string ReplaceValueFunction(AuditProject getauditProjectParameter, AuditMeetingDto itemParameter, List<string> input, string output)
        {
            var AuditEmailsubject = output;
            var getauditProject = getauditProjectParameter;
            var item = itemParameter;
            var checkGroupName = _entityGroupRepository.GetAll().Where(x => x.Id == getauditProject.EntityGroupId).FirstOrDefault();
            string groupName = checkGroupName == null ? "" : checkGroupName.Name;
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
                            AuditEmailsubject = (getauditProject.AuditStage != null) ? AuditEmailsubject.Replace("{AuditTypeName}", getauditProject.AuditStage.Value) : AuditEmailsubject.Replace("{AuditTypeName}", "");
                            break;
                        }
                    case "{AuditStatusName}":
                        {
                            AuditEmailsubject = (getauditProject.AuditStatus != null) ? AuditEmailsubject.Replace("{AuditStatusName}", getauditProject.AuditStatus.Value) : AuditEmailsubject.Replace("{AuditStatusName}", "");
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
                            AuditEmailsubject = (getauditProject.EntityGroupId != null) ? AuditEmailsubject.Replace("{EntityGroupName}", groupName) : AuditEmailsubject.Replace("{EntityGroupName}", "");
                            break;
                        }
                    case "{EntityName}":
                        {
                            AuditEmailsubject = (item.AuditOrgId != null) ? AuditEmailsubject.Replace("{EntityName}", _businessEntityRepository.GetAll().Where(x => x.Id == item.AuditOrgId).FirstOrDefault().CompanyName) : AuditEmailsubject.Replace("{EntityName}", "");
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
                            AuditEmailsubject = (item.AuditVendorId != null) ? AuditEmailsubject.Replace("{VendorName}", _businessEntityRepository.GetAll().Where(x => x.Id == item.AuditVendorId).FirstOrDefault().CompanyName) : AuditEmailsubject.Replace("{VendorName}", "");
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
    }
}
