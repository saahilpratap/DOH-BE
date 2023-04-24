using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using LockthreatCompliance.Authorization.Users.Dto;
using LockthreatCompliance.Storage;
using Abp.BackgroundJobs;
using Abp.Runtime.Session;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using LockthreatCompliance.Questions;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Importing;
using LockthreatCompliance.DynamicEntityParameters.Importing;
using LockthreatCompliance.MultiTenancy;
using LockthreatCompliance.Authorization.Users;
using Abp;
using LockthreatCompliance.ControlRequirements.Importing;
using LockthreatCompliance.Contacts.Importing;
using LockthreatCompliance.Assessments.Importing;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.FacilityTypes.Importing;
using LockthreatCompliance.FacilitySubtypes.Importing;
using LockthreatCompliance.Authorization.Users.Importing;
using LockthreatCompliance.AuditProjects.Importing;
using LockthreatCompliance.ExternalAssessments.Importing;
using LockthreatCompliance.ExternalAssessments.Dtos;
using LockthreatCompliance.Notifications;
using Abp.Threading;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Abp.Authorization;

namespace LockthreatCompliance.Web.Controllers
{
    [AbpAuthorize]
  
    public class ApiController : LockthreatComplianceControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<ExternalAssessmentQuestion> _externalAssessmentQuestionRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private IHostingEnvironment _hostingEnvironment;
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly IBusinessEntitiesAppService _businessEntitiesAppService;
        private readonly IRepository<User, long> _userRepository;
        private readonly IAppNotifier _appNotifier;
        private readonly IRepository<PreRegEntity> _preRegEntityRepository;
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IAntiforgery _antiforgery;
        public ApiController(IRepository<Question> questionRepository, IRepository<BusinessEntity> businessEntityRepository, IHostingEnvironment hostingEnvironment, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager,
            IBusinessEntitiesAppService businessEntitiesAppService, TenantManager tenantManager, IRepository<User, long> userRepository, IRepository<ExternalAssessmentQuestion> externalAssessmentQuestionRepository, IAppNotifier appNotifier, IRepository<PreRegEntity> preRegEntityRepository, IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository, IAntiforgery antiforgery)
        {

            _tenantManager = tenantManager;
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _businessEntityRepository = businessEntityRepository;
            _hostingEnvironment = hostingEnvironment;
            BinaryObjectManager = binaryObjectManager;
            BackgroundJobManager = backgroundJobManager;
            _businessEntitiesAppService = businessEntitiesAppService;
            _externalAssessmentQuestionRepository = externalAssessmentQuestionRepository;
            _appNotifier = appNotifier;
            _preRegEntityRepository = preRegEntityRepository;
            _preRegisterEntityRepository = preRegisterEntityRepository;
            _antiforgery = antiforgery;
        }
        [HttpPost]
      
        [AllowAnonymous]
        public ActionResult PreRegistrationApi([FromBody] PreRegEntityDto preRegEntity)
        {
            var preRegEntityDto = new PreRegEntity();

            if (preRegEntity.License_Number != null)
            {
                if (preRegEntity.Facility_Name != null)
                {
                    var checkLicenseInPreRegistration = _preRegisterEntityRepository.GetAll().Where(x => x.LicenseNumber.Trim().ToLower() == preRegEntity.License_Number.ToLower().Trim()).ToList().FirstOrDefault();
                    if (checkLicenseInPreRegistration == null)
                    {
                        preRegEntity.Newentity = true;
                        preRegEntity.IsUpdateEntity = false;
                    }
                    else
                    {
                        preRegEntity.Newentity = false;
                        preRegEntity.IsUpdateEntity = true;
                    }

                    var checkLicensePreRegEntity = _preRegEntityRepository.GetAll().Where(x => x.LicenseNumber.Trim().ToLower() == preRegEntity.License_Number.ToLower().Trim()).ToList().FirstOrDefault();
                    if (checkLicensePreRegEntity == null)
                    {


                        preRegEntityDto.Newentity = preRegEntity.Newentity;
                        preRegEntityDto.IsUpdateEntity = preRegEntity.IsUpdateEntity;
                        preRegEntityDto.LicenseNumber = preRegEntity.License_Number;
                        preRegEntityDto.Facility_EN = preRegEntity.Facility_Name;
                        preRegEntityDto.Group = preRegEntity.Group;
                        var chekIsPublicValid = (Boolean.TryParse(preRegEntity.IS_PUBLIC_FACILITY, out Boolean temp));
                        if (chekIsPublicValid == true)
                        {
                            preRegEntityDto.IsPublic = Convert.ToBoolean(preRegEntity.IS_PUBLIC_FACILITY);
                        }
                        var chekIsLicenseValid = (Boolean.TryParse(preRegEntity.LicenseStatus, out Boolean temp2));
                        if (chekIsLicenseValid == true)
                        {
                            preRegEntityDto.IsActive = Convert.ToBoolean(preRegEntity.LicenseStatus);
                        }
                        var checkIssueDateValid = (DateTime.TryParse(preRegEntity.Issue_date, out DateTime temp3));
                        if (checkIssueDateValid == true)
                        {
                            preRegEntityDto.IssueDate = Convert.ToDateTime(preRegEntity.Issue_date);
                        }

                        preRegEntityDto.Director_Incharge_EN = preRegEntity.DIRECTOR_INCHARGE_NAME;
                        preRegEntityDto.Director_Incharge_Mobile = preRegEntity.DIRECTOR_INCHARGE_TELEPHONE;
                        preRegEntityDto.Owner_EN = preRegEntity.OWNERNAME_EN;
                        preRegEntityDto.Owner_Mobile = preRegEntity.OWNER_TELEPHONE;
                        preRegEntityDto.INPATIENT_BED_CAPACITY = preRegEntity.INPATIENT_BED_CAPACITY;
                        preRegEntityDto.Pro_EN = preRegEntity.PRO_NAME;
                        preRegEntityDto.Pro_Mobile = preRegEntity.PRO_TELEPHONE;
                        preRegEntityDto.Region = preRegEntity.Region;
                        preRegEntityDto.CityOrDisctrict = preRegEntity.City;
                        preRegEntityDto.FacilityTypeName = preRegEntity.FacilityType;
                        preRegEntityDto.FacilitySubTypeName = preRegEntity.FacilitySubType;
                        if (preRegEntity.OWNER_EMAIL != null)
                        {
                            var splitEmail = preRegEntity.OWNER_EMAIL.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegEntityDto.Owner_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegEntityDto.Owner_Email += item + ",";
                                }
                            }
                        }
                        if (preRegEntity.DIRECTOR_INCHARGE_EMAIL != null)
                        {
                            var splitEmail = preRegEntity.DIRECTOR_INCHARGE_EMAIL.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegEntityDto.Director_Incharge_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegEntityDto.Director_Incharge_Email += item + ",";
                                }
                            }
                        }
                        _preRegEntityRepository.Insert(preRegEntityDto);
                        return Json("Inserted PreRegEntity");
                    }
                    else
                    {
                        var Owner_Email = "";
                        var Director_Email = "";
                        checkLicensePreRegEntity.Newentity = preRegEntity.Newentity;
                        checkLicensePreRegEntity.IsUpdateEntity = preRegEntity.IsUpdateEntity;
                        checkLicensePreRegEntity.LicenseNumber = preRegEntity.License_Number == null ? checkLicensePreRegEntity.LicenseNumber : preRegEntity.License_Number;
                        checkLicensePreRegEntity.Facility_EN = preRegEntity.Facility_Name == null ? checkLicensePreRegEntity.Facility_EN : preRegEntity.Facility_Name;
                        checkLicensePreRegEntity.Group = preRegEntity.Group == null ? checkLicensePreRegEntity.Group : preRegEntity.Group;
                        var chekIsPublicValid = (Boolean.TryParse(preRegEntity.IS_PUBLIC_FACILITY, out Boolean temp));
                        if (chekIsPublicValid == true)
                        {
                            checkLicensePreRegEntity.IsPublic = Convert.ToBoolean(preRegEntity.IS_PUBLIC_FACILITY);
                        }
                        var chekIsLicenseValid = (Boolean.TryParse(preRegEntity.LicenseStatus, out Boolean temp2));
                        if (chekIsLicenseValid == true)
                        {
                            checkLicensePreRegEntity.IsActive = Convert.ToBoolean(preRegEntity.LicenseStatus);
                        }
                        var checkIssueDateValid = (DateTime.TryParse(preRegEntity.Issue_date, out DateTime temp3));
                        if (checkIssueDateValid == true)
                        {
                            checkLicensePreRegEntity.IssueDate = Convert.ToDateTime(preRegEntity.Issue_date);
                        }
                        checkLicensePreRegEntity.Director_Incharge_EN = preRegEntity.DIRECTOR_INCHARGE_NAME == null ? checkLicensePreRegEntity.Director_Incharge_EN : preRegEntity.DIRECTOR_INCHARGE_NAME;
                        checkLicensePreRegEntity.Director_Incharge_Mobile = preRegEntity.DIRECTOR_INCHARGE_TELEPHONE == null ? checkLicensePreRegEntity.Director_Incharge_Mobile : preRegEntity.DIRECTOR_INCHARGE_TELEPHONE;
                        checkLicensePreRegEntity.Owner_EN = preRegEntity.OWNERNAME_EN == null ? checkLicensePreRegEntity.Owner_EN : preRegEntity.OWNERNAME_EN;
                        checkLicensePreRegEntity.Owner_Mobile = preRegEntity.OWNER_TELEPHONE == null ? checkLicensePreRegEntity.Owner_Mobile : preRegEntity.OWNER_TELEPHONE;
                        checkLicensePreRegEntity.INPATIENT_BED_CAPACITY = preRegEntity.INPATIENT_BED_CAPACITY == null ? checkLicensePreRegEntity.INPATIENT_BED_CAPACITY : preRegEntity.INPATIENT_BED_CAPACITY;
                        checkLicensePreRegEntity.Pro_EN = preRegEntity.PRO_NAME == null ? checkLicensePreRegEntity.Pro_EN : preRegEntity.PRO_NAME;
                        checkLicensePreRegEntity.Pro_Mobile = preRegEntity.PRO_TELEPHONE == null ? checkLicensePreRegEntity.Pro_Mobile : preRegEntity.PRO_TELEPHONE;
                        checkLicensePreRegEntity.Region = preRegEntity.Region == null ? checkLicensePreRegEntity.Region : preRegEntity.Region;
                        checkLicensePreRegEntity.CityOrDisctrict = preRegEntity.City == null ? checkLicensePreRegEntity.CityOrDisctrict : preRegEntity.City;
                        checkLicensePreRegEntity.FacilityTypeName = preRegEntity.FacilityType == null ? checkLicensePreRegEntity.FacilityTypeName : preRegEntity.FacilityType;
                        checkLicensePreRegEntity.FacilitySubTypeName = preRegEntity.FacilitySubType == null ? checkLicensePreRegEntity.FacilitySubTypeName : preRegEntity.FacilitySubType;
                        if (preRegEntity.OWNER_EMAIL != null)
                        {

                            var splitEmail = preRegEntity.OWNER_EMAIL.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegEntityDto.Owner_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    Owner_Email += item + ",";
                                }
                            }
                        }
                        if (preRegEntity.DIRECTOR_INCHARGE_EMAIL != null)
                        {
                            var splitEmail = preRegEntity.DIRECTOR_INCHARGE_EMAIL.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegEntityDto.Director_Incharge_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    Director_Email += item + ",";
                                }
                            }
                        }

                        checkLicensePreRegEntity.Owner_Email = Owner_Email == "" ? checkLicensePreRegEntity.Owner_Email : Owner_Email;
                        checkLicensePreRegEntity.Director_Incharge_Email = Director_Email == "" ? checkLicensePreRegEntity.Director_Incharge_Email : Director_Email;

                        _preRegEntityRepository.Update(checkLicensePreRegEntity);
                        return Json("Updated PreRegEntity");
                    }

                }
                else
                {
                    return Json("facility_Name is Empty OR Invalid");
                }
            }
            return Json("License Number is Empty OR Invalid");

        }
    }
}
