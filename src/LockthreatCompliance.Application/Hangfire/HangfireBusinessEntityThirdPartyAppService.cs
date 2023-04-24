using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.WrokFlows;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.WorkFllows;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.EntityGroups;
using System.Net;
using System.Xml;
using System.IO;
using LockthreatCompliance.ThirdpartyApi.Dto;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Abp.UI;
using Abp.ObjectMapping;
using Abp.Organizations;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.Enums;
using LockthreatCompliance.Sessions;
using LockthreatCompliance.Countries;
using LockthreatCompliance.AuthoritativeDocuments;
using System.Collections.ObjectModel;
using Abp.Authorization.Users;
using Microsoft.AspNetCore.Identity;
using LockthreatCompliance.Authorization.Roles;
using System.Text;
using System.Text.RegularExpressions;

namespace LockthreatCompliance.Hangfire
{
    public class HangfireBusinessEntityThirdPartyAppService : LockthreatComplianceAppServiceBase, IHangfireBusinessEntityThirdPartyAppService
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

        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;

        private readonly IObjectMapper _objectMapper;


        private readonly OrganizationUnitManager _organizationUnitManager;

        private readonly ApplicationSession _applicationSession;

        private readonly IRepository<Country> _countriesRepository;
        private readonly IRepository<FacilityType> _facilityTypeRepository;
        private const string defaultPassword = "123qwe";
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly RoleManager _roleManager;

        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;
        private readonly IRepository<Role> _roleRepository;

        private readonly IRepository<Template, long> _templateRepository;



        public HangfireBusinessEntityThirdPartyAppService(IRepository<User, long> userRepository, IRepository<BusinessEntityThirdParty> businessEntityThirdPartyRepository,
            IRepository<StateAction, long> stateActionRepository, RoleManager roleManager, IRepository<Role> roleRepository, IRepository<WorkFlowPage, long> WorkFlowPageRepository,
            IUserEmailer userEmailer, ICustomTemplateAppService customTemplateAppServiceEmailer, IRepository<EntityApplicationSetting> entityApplicationSettingRepository,
            IRepository<AuditProject, long> auditProjectRepository, IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository,
            IRepository<BusinessEntities.BusinessEntity, int> businessEntityRepository, IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<ExternalAssessment, int> externalAssessmentRepository, OrganizationUnitManager organizationUnitManager, IPasswordHasher<User> passwordHasher,
            IRepository<AuditProjectStatus, long> auditProjectStatusRepository, IRepository<Template, long> templateRepository,
            IRepository<AssessmentStatusLog, long> assessmentStatusLogRepository, IRepository<FacilityType> facilityTypeRepository,
            IRepository<BusinessRiskStatusLog, long> businessRiskLogRepository, IRepository<FacilitySubType> facilitySubTypeRepository,
            IRepository<EntityGroupMember> entityGroupMemberRepository, IObjectMapper objectMapper, ApplicationSession applicationSession, IRepository<Country> countriesRepository

           )
        {
            _templateRepository = templateRepository;
            _WorkFlowPageRepository = WorkFlowPageRepository;
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
            _roleRepository = roleRepository;

        }
         

        public async Task SendForBusinessEntity()
        {
            try
            {
                HttpWebRequest request = CreateWebRequest();
                XmlDocument soapEnvelopeXml = new XmlDocument();
                string referanceKey = "";
                string PageCount = "";


                soapEnvelopeXml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                  <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soc=""http://www.doh.gov.ae/SOC_HFLOwners_LicensedService"">
                      <soapenv:Header xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <wsse:Security xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
                     <wsse:UsernameToken wsu:Id=""UsernameToken-31E45FB2B7422B6C7016471933944892"">
                      <wsse:Username>DOH_SOCAPP</wsse:Username>
                       <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">U@tA-2(O)25SK-s0C34DG</wsse:Password>
                      </wsse:UsernameToken>
                    </wsse:Security>
                    </soapenv:Header>
	                 <soapenv:Body>
                    <soc:FacilitiesOwnersInit>
            <soc:RequestType>ALL</soc:RequestType >
        </soc:FacilitiesOwnersInit>
    </soapenv:Body>
   </soapenv:Envelope>");



                using (Stream stream = request.GetRequestStream())
                {
                    soapEnvelopeXml.Save(stream);
                }

                using (WebResponse response = request.GetResponse())
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    XmlNodeList xmlnodeRefranceKey;
                    XmlNodeList xmlnodepagecount;
                    xmlDoc.Load(response.GetResponseStream());

                    //var fromXml = JsonConvert.SerializeXmlNode(xmlDoc, Newtonsoft.Json.Formatting.Indented);
                    //var jsonText = JsonConvert.DeserializeObject<RootObjects.Rootobject>(fromXml);
                    //string json = JsonConvert.SerializeXmlNode(xmlDoc);

                    xmlnodeRefranceKey = xmlDoc.GetElementsByTagName("soc:FacilitiesOwnersInitResponse");

                    for (int i = 0; i < xmlnodeRefranceKey.Count; i++)
                    {
                        referanceKey = string.Format("{0}", xmlnodeRefranceKey[i].ChildNodes.Item(0).InnerText);
                        PageCount = string.Format("{0}", xmlnodeRefranceKey[i].ChildNodes.Item(1).InnerText);
                        //  MessageBox.Show(str);
                    }
                }

                HttpWebRequest requestGet = CreateWebRequestGet();
                XmlDocument soapEnvelopeXmlGet = new XmlDocument();

                soapEnvelopeXmlGet.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
                  <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:soc=""http://www.doh.gov.ae/SOC_HFLOwners_LicensedService"">
                    <soapenv:Header>
                     <wsse:Security soapenv:mustUnderstand=""1"" xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"" xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"" >
                   <wsse:UsernameToken wsu:Id=""UsernameToken-31E45FB2B7422B6C7016471933944892"">
                     <wsse:Username>DOH_SOCAPP</wsse:Username>
                     <wsse:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">U@tA-2(O)25SK-s0C34DG</wsse:Password>
                     </wsse:UsernameToken>
                    </wsse:Security>
                    </soapenv:Header>
	                <soapenv:Body>
             <soc:FacilitiesOwnersGet>
            <soc:REQUESTTYPE>ALL</soc:REQUESTTYPE>
            <soc:REFERENCEKEY> </soc:REFERENCEKEY>
            <soc:PAGENUMBER>1</soc:PAGENUMBER>
           </soc:FacilitiesOwnersGet>
         </soapenv:Body>
   </soapenv:Envelope>");

                XmlNodeList xmlnodeRefranceKeys = soapEnvelopeXmlGet.GetElementsByTagName("soc:FacilitiesOwnersGet");

                for (int i = 0; i < xmlnodeRefranceKeys.Count; i++)
                {
                    xmlnodeRefranceKeys[i].ChildNodes.Item(1).InnerText = referanceKey;
                    xmlnodeRefranceKeys[i].ChildNodes.Item(2).InnerText = PageCount;

                    //  MessageBox.Show(str);
                }




                using (Stream stream = requestGet.GetRequestStream())
                {
                    soapEnvelopeXmlGet.Save(stream);

                }

                using (WebResponse responses = requestGet.GetResponse())
                {

                    XmlDocument xmlDocs = new XmlDocument();

                    xmlDocs.Load(responses.GetResponseStream());



                    XmlNodeList facility = xmlDocs.GetElementsByTagName("soc:Facility");


                    var BusinessEntityThirdParty = new List<BusinessEntityThirdPartyDto>();

                    for (int i = 0; i < facility.Count; i++)
                    {
                        var items = new BusinessEntityThirdPartyDto();
                        items.LICENSE_NO = facility[i].ChildNodes.Item(0).InnerText;
                        items.STATUS = facility[i].ChildNodes.Item(1).InnerText;
                        items.FACILITY_CATEGORY = facility[i].ChildNodes.Item(2).InnerText;
                        items.HAS_INSURANCE = facility[i].ChildNodes.Item(3).InnerText;
                        items.IS_PUBLIC_FACILITY = facility[i].ChildNodes.Item(4).InnerText;
                        items.FACILITY_GROUP = facility[i].ChildNodes.Item(5).InnerText;
                        items.DED_LICENSE = facility[i].ChildNodes.Item(6).InnerText;
                        items.DED_TEMPORARY_NUMBER = facility[i].ChildNodes.Item(7).InnerText;
                        items.FACILITY_NAME = facility[i].ChildNodes.Item(8).InnerText;
                        items.FACILITY_TYPE = facility[i].ChildNodes.Item(10).InnerText;
                        items.FACILITY_SUBTYPE = facility[i].ChildNodes.Item(11).InnerText;
                        items.ADDRESS = facility[i].ChildNodes.Item(13).InnerText;
                        items.CITY = facility[i].ChildNodes.Item(15).InnerText;
                        items.TELEPHONE = facility[i].ChildNodes.Item(16).InnerText;
                        items.EMAIL = facility[i].ChildNodes.Item(19).InnerText;
                        items.OWNERNAME_EN = facility[i].ChildNodes.Item(23).InnerText;
                        items.OWNER_TELEPHONE = facility[i].ChildNodes.Item(24).InnerText;
                        items.OWNER_EMAIL = facility[i].ChildNodes.Item(25).InnerText;
                        items.OWNER_EID = facility[i].ChildNodes.Item(26).InnerText;
                        items.DIRECTOR_INCHARGE_ENGLISH_NAME = facility[i].ChildNodes.Item(28).InnerText;
                        items.DIRECTOR_INCHARGE_TELEPHONE = facility[i].ChildNodes.Item(29).InnerText;
                        items.DIRECTOR_INCHARGE_EMAIL = facility[i].ChildNodes.Item(30).InnerText;
                        items.PRO_NAME = facility[i].ChildNodes.Item(31).InnerText;
                        items.PRO_TELEPHONE = facility[i].ChildNodes.Item(32).InnerText;
                        items.PRO_EMAIL = facility[i].ChildNodes.Item(33).InnerText;
                        items.TenantId = 1;
                        BusinessEntityThirdParty.Add(items);
                    }

                    foreach (var item in BusinessEntityThirdParty)
                    {
                        long orgId = 0;
                        int BusinessEntityId = 0;
                        string AdminName = null;
                        string AdminSirname = null;
                        string AdminEmails = null;
                        long userid = 0;


                        var check = await _businessEntityThirdPartyRepository.GetAll().Where(x => x.LICENSE_NO.Trim().ToLower() == item.LICENSE_NO.Trim().ToLower()).FirstOrDefaultAsync();
                        if (check == null)
                        {

                            if (item.DIRECTOR_INCHARGE_ENGLISH_NAME.Trim() != "" && item.DIRECTOR_INCHARGE_ENGLISH_NAME.Trim() != null && item.DIRECTOR_INCHARGE_EMAIL.Trim() != "" && item.FACILITY_NAME.Trim() != "")
                            {
                                var names = item.DIRECTOR_INCHARGE_ENGLISH_NAME.Split(' ');

                                var adminem = item.DIRECTOR_INCHARGE_EMAIL.Split(',');
                                AdminEmails = adminem.FirstOrDefault();
                                if (names.Length >= 2)
                                {
                                    AdminName = names[0];
                                    AdminSirname = names[names.Length - 1];

                                }
                                else if (names.Length >= 1)
                                {
                                    if (names[0] != "")
                                    {
                                        AdminName = names[0];
                                        AdminSirname = names[names.Length - 1];
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }

                            }
                            else

                            {
                                continue;
                            }

                            var businessEntity = _objectMapper.Map<BusinessEntityThirdParty>(item);
                            await _businessEntityThirdPartyRepository.InsertAsync(businessEntity);



                            var checkEntity = await _businessEntityRepository.FirstOrDefaultAsync(x => x.LicenseNumber.Trim().ToLower() == item.LICENSE_NO.Trim().ToLower());

                            if (checkEntity == null)
                            {
                                var checkBusinessentity = _businessEntityRepository.GetAll().ToList();
                                var checkavailable = checkBusinessentity.Where(x => x.LicenseNumber.Trim().ToLower() == item.LICENSE_NO.Trim().ToLower()).FirstOrDefault();
                                if (checkavailable == null)
                                {
                                    string DisplayName = null;
                                    EntityType type = 0;
                                    if (item.FACILITY_TYPE.Trim().ToLower() == "INSURANCE".Trim().ToLower())
                                    {
                                        DisplayName = "Insurance Facilities";
                                        type = EntityType.InsuranceFacilities;
                                    }
                                    else
                                    {
                                        DisplayName = "Healthcare Entities";
                                        type = EntityType.HealthcareEntity;
                                    }

                                    var getOraganization = await _organizationUnitRepository.GetAll().ToListAsync();
                                    var concreteOrganizationUnit = getOraganization.FirstOrDefault(e => e.DisplayName.Trim().ToLower() == (DisplayName).Trim().ToLower().ToString());
                                    var getcodes = _organizationUnitManager.GetNextChildCode(concreteOrganizationUnit.Id);

                                    var newOrganizatinUnit = new OrganizationUnit()
                                    {
                                        Id = 0,
                                        TenantId = item.TenantId,
                                        ParentId = concreteOrganizationUnit.Id,
                                        Code = getcodes.ToString(),
                                        CreatorUserId = _applicationSession.UserId,
                                        CreationTime = DateTime.Now,
                                        DisplayName = item.FACILITY_NAME
                                    };

                                    orgId = await _organizationUnitRepository.InsertAndGetIdAsync(newOrganizatinUnit);

                                    var checkfacilitySubtype = await _facilitySubTypeRepository.GetAll().Where(f => f.FacilitySubTypeName.ToLower() == item.FACILITY_SUBTYPE.ToString().ToLower()).FirstOrDefaultAsync();
                                    var checkfacility = await _facilityTypeRepository.GetAll().Where(f => f.Name.ToLower() == item.FACILITY_TYPE.ToString().ToLower()).FirstOrDefaultAsync();


                                    var businessentity = new BusinessEntity
                                    {
                                        CreationTime = DateTime.Now,
                                        Id = 0,
                                        LicenseNumber = item.LICENSE_NO,
                                        CompanyName = item.FACILITY_NAME,
                                        IsActive = true,
                                        EntityType = (EntityType)type,
                                        CompanyLegalName = item.FACILITY_NAME,
                                        IsPreAssessmentQuestionaire = true,
                                        AdminEmail = AdminEmails,
                                        AdminMobile = item.DIRECTOR_INCHARGE_TELEPHONE,
                                        Owner_EN = item.OWNERNAME_EN,
                                        Owner_Mobile = item.OWNER_TELEPHONE,
                                        Owner_Email = item.OWNER_EMAIL,
                                        FacilityTypeId = (checkfacility != null) ? checkfacility.Id : (int?)null,
                                        FacilitySubTypeId = (checkfacilitySubtype != null) ? checkfacilitySubtype.Id : (int?)null,
                                        DistrictId = null,
                                        CityOrDisctrict = null,
                                        CountryId = _countriesRepository.GetAll().Where(x => x.TenantId == item.TenantId).FirstOrDefault().Id,
                                        ThirdPartyId = null,
                                        PrimaryContactName = item.PRO_NAME,
                                        ContactNumber = item.PRO_TELEPHONE,
                                        Designation = null,
                                        OfficialEmail = item.PRO_EMAIL,
                                        BackupContactName = item.DIRECTOR_INCHARGE_ENGLISH_NAME,
                                        BackupContactNumber = item.DIRECTOR_INCHARGE_TELEPHONE,
                                        BackupDesignation = null,
                                        BackupOfficialEmail = item.DIRECTOR_INCHARGE_EMAIL,
                                        Director_Incharge_Email = item.DIRECTOR_INCHARGE_EMAIL,
                                        Director_Incharge_Mobile = item.DIRECTOR_INCHARGE_TELEPHONE,
                                        Director_Incharge_EN = item.DIRECTOR_INCHARGE_ENGLISH_NAME,
                                        AdminName = AdminName != null ? AdminName : AdminSirname,
                                        OrganizationUnitId = orgId,
                                        AdminSurname = AdminSirname != null ? AdminSirname : AdminName,
                                        Status = EntityTypeStatus.Active,
                                        IsAuditableEntity = true,
                                        HasAdminGenerated = true,
                                        TenantId = item.TenantId,
                                        ComplianceType = ControlType.Advanced,
                                        NumberOfYearsInBusiness = 0,
                                        IsGovernmentOwned = false,
                                        IsCompanyLicensed = true,
                                        IsParentReportingEnabled = false,
                                        IsSuspended = false,
                                        IsOrphan = false,
                                        IsPublic = true,
                                        IsDeleted = false,
                                        // FacilityTypeSize = item.FacilityTypeSize

                                    };

                                    BusinessEntityId = await _businessEntityRepository.InsertAndGetIdAsync(businessentity);
                                    var getuserCheck = _userRepository.GetAll().ToList();
                                    var checkUser = getuserCheck.Where(x => x.EmailAddress.ToLower() == AdminEmails.Trim().ToLower()).FirstOrDefault();

                                    if (checkUser == null)
                                    {
                                        var user = new User
                                        {
                                            Id = 0,
                                            AccessFailedCount = 0,
                                            CreationTime = DateTime.Now,
                                            ShouldChangePasswordOnNextLogin = true,
                                            UserName = AdminName != null ? AdminName.Replace(" ", string.Empty) : AdminSirname.Replace(" ", string.Empty),
                                            PhoneNumber = item.DIRECTOR_INCHARGE_TELEPHONE,
                                            TenantId = item.TenantId,
                                            EmailAddress = item.DIRECTOR_INCHARGE_EMAIL,
                                            Name = AdminName != null ? AdminName : AdminSirname,
                                            NormalizedUserName = item.DIRECTOR_INCHARGE_EMAIL.ToUpper(),
                                            NormalizedEmailAddress = item.DIRECTOR_INCHARGE_EMAIL.ToUpper(),
                                            Surname = AdminSirname != null ? AdminSirname : AdminName,
                                            IsActive = true,
                                            BusinessEntityId = BusinessEntityId,
                                            Type = type == EntityType.InsuranceFacilities ? UserOriginType.InsuranceEntity : UserOriginType.BusinessEntity,
                                            IsDeleted = false
                                        };

                                        user.Password = _passwordHasher.HashPassword(user, defaultPassword);
                                        //  user.EncryptPassword= SimpleStringCipher.Instance.Encrypt(defaultPassword);
                                        user.Roles = new Collection<UserRole>();

                                        string AssignedRoleNames = null;

                                        if (type == EntityType.HealthcareEntity)
                                        {
                                            AssignedRoleNames = "Business Entity Admin";
                                        }
                                        else
                                        {
                                            AssignedRoleNames = "Insurance Entity Admin";
                                        }

                                        var role = await _roleRepository.GetAll().Where(x => x.DisplayName.ToLower() == AssignedRoleNames.Trim().ToLower()).FirstOrDefaultAsync();

                                        user.Roles.Add(new UserRole(_applicationSession.TenantId, user.Id, role.Id));


                                        userid = await _userRepository.InsertAndGetIdAsync(user);

                                        //var users = await _userRepository.FirstOrDefaultAsync(x => x.Id == userid);
                                        //users.SetNewEmailConfirmationCode();
                                        //await _userEmailer.CreateUSerAsync(
                                        //    users,
                                        //    AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
                                        //);
                                    }

                                    userid = userid != 0 ? userid : checkUser.Id;
                                    //insert into BusinrssEntityUser Table
                                    var temp = new BusinessEntityUser()
                                    {
                                        Id = 0,
                                        TenantId = (int)_applicationSession.TenantId,
                                        UserId = userid,
                                        BusinessEntityId = BusinessEntityId
                                    };
                                    //  var insertedid = _businessEntityUserRepository.InsertAndGetId(temp);

                                    var userOrganizationUnits = new UserOrganizationUnit()
                                    {
                                        Id = 0,
                                        UserId = userid,
                                        TenantId = item.TenantId,
                                        CreationTime = DateTime.Now,
                                        OrganizationUnitId = orgId
                                    };
                                    await _userOrganizationUnitRepository.InsertAsync(userOrganizationUnits);

                                    var getpage = await _WorkFlowPageRepository.FirstOrDefaultAsync(x => x.PageName.ToLower().Trim() == LockthreatComplianceConsts.EntityonBoard.Trim().ToLower());
                                    if (getpage != null)
                                    {
                                        var checkteamplate = await _templateRepository.FirstOrDefaultAsync(x => x.WorkFlowPageId == getpage.Id);

                                        if (checkteamplate != null)
                                        {
                                            string auditbody = null;
                                            string AuditEmailsubject = null;

                                            var emails = new HashSet<string>();
                                            var ccemail = new HashSet<string>();
                                            var bccemail = new HashSet<string>();


                                            //  var item =  _businessEntityRepository.GetAll().Where(xx => xx.Id == x.Id).FirstOrDefault();
                                            var item1 = _businessEntityRepository.GetAll().Where(xx => xx.Id == BusinessEntityId).FirstOrDefault();
                                            List<string> templateSubject = new List<string>();
                                            var auditprojectsubjectBody = checkteamplate.TemplateSubject;

                                            AuditEmailsubject = checkteamplate.TemplateSubject.ToString();

                                            while (auditprojectsubjectBody.Contains("{"))
                                            {
                                                templateSubject.Add("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}");
                                                auditprojectsubjectBody = auditprojectsubjectBody.Replace("{" + auditprojectsubjectBody.Split('{', '}')[1] + "}", "");
                                            };


                                            AuditEmailsubject = ReplaceValueFunction(item1, templateSubject, AuditEmailsubject);

                                            var auditTemplate = checkteamplate.TemplateBody;

                                            var auditTo = checkteamplate.TemplateTo;
                                            List<string> auditToList = checkteamplate.TemplateTo.Split(',').ToList();
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

                                            var auditCc = checkteamplate.TemplateCc;
                                            List<string> auditCcList = checkteamplate.TemplateCc.Split(',').ToList();
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
                                                            emails.Add(item1.AdminEmail);
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
                                                            if (item1.Owner_Email != null)
                                                            {
                                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.Director_Incharge_Email != null)
                                                            {
                                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.CISO_Email != null)
                                                            {
                                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.OfficialEmail != null)
                                                            {
                                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.BackupOfficialEmail != null)
                                                            {
                                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    case "{Group_Admin}":
                                                        {
                                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
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
                                                                        emails.Add(i);
                                                                    }
                                                                }
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
                                                            ccemail.Add(item1.AdminEmail);
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
                                                            if (item1.Owner_Email != null)
                                                            {
                                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.Director_Incharge_Email != null)
                                                            {
                                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.CISO_Email != null)
                                                            {
                                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.OfficialEmail != null)
                                                            {
                                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.BackupOfficialEmail != null)
                                                            {
                                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    case "{Group_Admin}":
                                                        {
                                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
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
                                                                        ccemail.Add(i);
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        }
                                                }
                                            });

                                            var auditBcc = checkteamplate.TemplateBcc;
                                            List<string> auditBccList = checkteamplate.TemplateBcc.Split(',').ToList();
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
                                                            bccemail.Add(item1.AdminEmail);
                                                            break;
                                                        }
                                                    case "{Audit_Agency_Admin_Email}":
                                                        {

                                                            break;
                                                        }
                                                    case "{Owner_Email}":
                                                        {
                                                            if (item1.Owner_Email != null)
                                                            {
                                                                var splitEmail = item1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.Director_Incharge_Email != null)
                                                            {
                                                                var splitEmail = item1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.CISO_Email != null)
                                                            {
                                                                var splitEmail = item1.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.OfficialEmail != null)
                                                            {
                                                                var splitEmail = item1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                            if (item1.BackupOfficialEmail != null)
                                                            {
                                                                var splitEmail = item1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
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
                                                    case "{Group_Admin}":
                                                        {
                                                            var getGroup = _entityGroupMemberRepository.GetAll().Include(x => x.EntityGroup).Where(x => x.BusinessEntityId == item1.Id).FirstOrDefault();
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
                                                                        bccemail.Add(i);
                                                                    }
                                                                }
                                                            }
                                                            break;

                                                        }
                                                }

                                            });

                                            List<string> templateBody = new List<string>();
                                            var auditprojectBody = checkteamplate.TemplateBody;

                                            auditbody = checkteamplate.TemplateBody.ToString();

                                            while (auditprojectBody.Contains("{"))
                                            {
                                                templateBody.Add("{" + auditprojectBody.Split('{', '}')[1] + "}");
                                                auditprojectBody = auditprojectBody.Replace("{" + auditprojectBody.Split('{', '}')[1] + "}", "");
                                            };

                                            auditbody = ReplaceValueFunction(item1, templateBody, auditbody);

                                            await _userEmailer.EntityonBoard(emails, ccemail, bccemail, AuditEmailsubject, (int)item1.TenantId, auditbody, null,
                                                null);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private string ReplaceValueFunction(BusinessEntity item, List<string> input, string output)
        {
            var Feedbacksubject = output;
            var getbusinessentity = item;

            var mailMessage = new StringBuilder();


            return Feedbacksubject;
        }
        public static HttpWebRequest CreateWebRequest()
        {
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"https://ietest.doh.gov.ae/DOHInt/SOC/HFL/OwnersLicensedPaginationService");

                // webRequest.Headers.Add(@"SOAP:Action","urn:SOC_HFLOwners_LicensedService/FacilitiesOwnersGet");
                webRequest.Headers.Add("SOAPAction", "urn:SOC_HFLOwners_LicensedService/FacilitiesOwnersInit");
                //  webRequest.Headers.Add(@"SOAP:Action", "urn:SOC_HFLOwners_LicensedService/FacilitiesOwnersInit");
                // webRequest.ContentType = "text/xml";
                webRequest.ContentType = "application/xml";
                // webRequest.Accept = "text/xml";
                var a = webRequest.Headers["SOAPAction"];
                webRequest.Method = "POST";
                return webRequest;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static HttpWebRequest CreateWebRequestGet()
        {
            try
            {
                HttpWebRequest webRequests = (HttpWebRequest)WebRequest.Create(@"https://ietest.doh.gov.ae/DOHInt/SOC/HFL/OwnersLicensedPaginationService");
                webRequests.Headers.Add("SOAPAction", "urn:SOC_HFLOwners_LicensedService/FacilitiesOwnersGet");
                webRequests.ContentType = "application/xml";
                // webRequest.Accept = "text/xml";
                var a = webRequests.Headers["SOAPAction"];
                webRequests.Method = "POST";
                return webRequests;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
