using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.DynamicEntityParameters;
using Abp.Extensions;
using Abp.Localization.Sources;
using Abp.ObjectMapping;
using Abp.Organizations;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.Authorization.Users;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.Countries;
using LockthreatCompliance.Countries.Dtos;
using LockthreatCompliance.DataExporting.Excel.NPOI;
using LockthreatCompliance.DynamicEntityParameters;
using LockthreatCompliance.Enums;
using LockthreatCompliance.FacilityTypes;
using LockthreatCompliance.PreregistrationEntity;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LockthreatCompliance.BusinessEntities.Importing
{
    public class PreEntityListExcelDataReader : NpoiExcelImporterBase<PreRegistrationImportDto>, IPreEntityListExcelDataReader
    {
        private readonly IRepository<PreRegisterBusinessEntity> _preRegisterEntityRepository;
        private readonly IRepository<FacilityType> _facilityTypeRepository;
        private readonly IDynamicParameterAppService _dynamicParameterAppService;
        private readonly IDynamicParameterValueAppService _dynamicParameterValueAppService;
        private readonly IRepository<DynamicParameter> _dynamicParamRepository;
        private readonly IRepository<DynamicParameterValue> _dynamicParamValueRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<FacilitySubType> _facilitysubTypesRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private readonly IRepository<User,long> _userRepository;
        private readonly IRepository<Country> _countriesRepository;
        public PreEntityListExcelDataReader(IRepository<PreRegisterBusinessEntity> preRegisterEntityRepository,
            IRepository<FacilitySubType> facilitysubTypesRepository, IRepository<Country> countriesRepository,
            IRepository<FacilityType> facilityTypeRepository, IDynamicParameterAppService dynamicParameterAppService,
            IDynamicParameterValueAppService dynamicParameterValueAppService, IRepository<DynamicParameter> dynamicParamRepository,
            IRepository<DynamicParameterValue> dynamicParamValueRepository, IRepository<OrganizationUnit, long> organizationUnitRepository, IObjectMapper objectMapper,
            IRepository<BusinessEntity> businessEntityRepository, IRepository<User, long> userRepository)
        {
            _countriesRepository = countriesRepository;
            _facilitysubTypesRepository = facilitysubTypesRepository;
            _organizationUnitRepository = organizationUnitRepository;
            _preRegisterEntityRepository = preRegisterEntityRepository;
            _facilityTypeRepository = facilityTypeRepository;
            _dynamicParameterAppService = dynamicParameterAppService;
            _dynamicParameterValueAppService = dynamicParameterValueAppService;
            _dynamicParamRepository = dynamicParamRepository;
            _dynamicParamValueRepository = dynamicParamValueRepository;
            _objectMapper = objectMapper;
            _businessEntityRepository = businessEntityRepository;
            _userRepository = userRepository;
        }

        [AbpAllowAnonymous]
        public List<PreRegistrationImportDto> GetPreEntitiesFromExcel(byte[] fileBytes, int? tenantId)
        {
            var preRegisters = new List<PreRegistrationImportDto>();
            var InvalidMessage = new PreRegistrationImportDto();
            InvalidMessage.InvalidCount = true;
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                }

                //// Check District Added Or Not
                int districtKeyId = 0;
                var allDynamicKeys = _dynamicParamRepository.GetAll().ToList();
                bool isDistrictAdded = allDynamicKeys.Any(d => d.ParameterName == "Districts");
                if (!isDistrictAdded)
                {
                    DynamicParameter input = new DynamicParameter
                    {
                        InputType = "COMBOBOX",
                        ParameterName = "Districts",
                        TenantId = tenantId,
                    };

                    districtKeyId = _dynamicParamRepository.InsertAndGetId(input);
                }
                else
                {
                    districtKeyId = _dynamicParamRepository.GetAll().FirstOrDefault(d => d.ParameterName == "Districts").Id;
                }
                int entitytype = 0;

                 List<String> headerList = new List<string>();
                for (int i = 0; i <= 0; i++)
                {
                    string header = "";
                    for (int j=0;j< cellCount;j++ )
                    {
                        header = sheet.GetRow(0).Cells[j]?.StringCellValue;
                        headerList.Add(header);
                    }   
                    

                }
                var list = headerList;
                int checkHeaderCount = 0;
                int checkInvalidCount = 0;
                List<string> staticHeader = new List<string>();
                staticHeader.Add("Status"); staticHeader.Add("Country"); staticHeader.Add("CityOrDisctrict");staticHeader.Add("EntityGroup");
                staticHeader.Add("Entity_Type"); staticHeader.Add("IsPrimary"); staticHeader.Add("LICENSENUMBER"); staticHeader.Add("FACILITY_EN");
                staticHeader.Add("PUBLIC_PRIVATE"); staticHeader.Add("District"); staticHeader.Add("FacilityType"); staticHeader.Add("Fac_Sub_Type");
                staticHeader.Add("Ftypesize"); staticHeader.Add("LicenseStatus"); staticHeader.Add("FHLName"); staticHeader.Add("FACILITY_EMAIL"); staticHeader.Add("OWNER_EN");
                staticHeader.Add("OWNER_EMAIL"); staticHeader.Add("OWNER_MOBILE"); staticHeader.Add("DIRECTOR_INCHARGE_EN"); staticHeader.Add("DIRECTOR_INCHARGE_EMAIL");
                staticHeader.Add("DIRECTOR_INCHARGE_MOBILE"); staticHeader.Add("Pro_En"); staticHeader.Add("Pro_Email"); staticHeader.Add("Pro_Mobile");

                staticHeader.Add("Admin_mail"); staticHeader.Add("Admin_Phone"); staticHeader.Add("PrimaryContactName"); staticHeader.Add("ContactNumber");
                staticHeader.Add("Designation"); staticHeader.Add("OfficialEmail"); staticHeader.Add("BackupContactName"); staticHeader.Add("BackupContactNumber");


                staticHeader.Add("BackupDesignation"); staticHeader.Add("BackupOfficialEmail"); staticHeader.Add("AdminName"); staticHeader.Add("AdminSurName");
                staticHeader.Add("NumberOfYearsInBusiness");
                var list2 = list.Except(staticHeader).ToList();
                foreach(var item in staticHeader)
                {
                    foreach(var item2 in list)
                    {
                        if(item == item2)
                        {
                            checkHeaderCount++;
                        }
                        else
                        {
                            checkInvalidCount++;
                        }
                    }
                }
                var count = checkHeaderCount;
                if (count == 38)
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;


                        //// Check Facility Added Or Not

                        int facilityId = 0;
                        ControlType controlType = ControlType.Basic;


                        //// Check district Value Added Or Not

                        int districtValueId = 0;
                        var districtValues = _dynamicParamValueRepository.GetAll().Where(d => d.DynamicParameterId == districtKeyId).ToList();


                        int countryId = 0;
                        var country = _countriesRepository.GetAll().ToList();
                        //// Check facility SubType Value Added Or Not

                        int facilitySubTypeValueId = 0;

                        var preRegister1 = new PreRegistrationImportDto();
                        for (int j = 0; j < cellCount; j++)
                        {
                            if (PreRegisterBusinessEntityConsts.Status == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    if (temp == "1")
                                    {
                                        preRegister1.Status = PreregistrationStatus.AbouttoExpire;
                                    }
                                    else if (temp == "2")
                                    {
                                        preRegister1.Status = PreregistrationStatus.Expired;
                                    }
                                    else if (temp == "3")
                                    {
                                        preRegister1.Status = PreregistrationStatus.TemporaryClosed;
                                    }
                                    else
                                    {
                                        preRegister1.Status = PreregistrationStatus.Active;
                                    }

                                }
                                else
                                {
                                    preRegister1.Status = PreregistrationStatus.Active;
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.Country == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    bool isDistrictValueAdded = country.Any(d => d.Name.ToLower() == temp.Trim().ToString().ToLower());
                                    if (!isDistrictValueAdded)
                                    {
                                        Country dto = new Country
                                        {
                                            Id = 0,
                                            Name = temp.Trim(),
                                            TenantId = tenantId
                                        };
                                        countryId = _countriesRepository.InsertAndGetId(dto);
                                    }
                                    else
                                    {
                                        countryId = country.FirstOrDefault(d => d.Name.ToLower() == temp.Trim().ToString().ToLower()).Id;
                                    }
                                }
                                preRegister1.CountryId = countryId;
                            }
                            if (PreRegisterBusinessEntityConsts.CityOrDisctrict == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.CityOrDisctrict = GetRequiredValueFromRowOrNull(row, j);
                            }
                           

                            if (PreRegisterBusinessEntityConsts.EntityGroup == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.EntityGroupName = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Entity_Type == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    preRegister1.EntityType = (temp == "0") ? EntityType.HealthcareEntity : EntityType.InsuranceFacilities;
                                }
                                else
                                {
                                    preRegister1.EntityType = 0;
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.IsPrimary == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp == null)
                                {
                                    preRegister1.IsPrimaryEntity = false;
                                }
                                else
                                {
                                    preRegister1.IsPrimaryEntity = temp.ToLower() == "false" ? false : true;
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.LICENSENUMBER == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    preRegister1.LicenseNumber = temp.Trim();
                                }
                                else
                                {
                                    preRegister1.Exception += "Licence Number is Empty, ";
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.FACILITY_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    preRegister1.Facility_EN = temp.Trim();
                                }
                                else
                                {
                                    preRegister1.Exception += "Facility_En is Empty, ";
                                }

                            }
                            if (PreRegisterBusinessEntityConsts.PUBLIC_PRIVATE == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp == null)
                                {
                                    preRegister1.IsPublic = false;
                                }
                                else
                                {
                                    preRegister1.IsPublic = temp.ToLower() == "false" ? false : true;
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.District == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    bool isDistrictValueAdded = districtValues.Any(d => d.Value.ToLower() == temp.Trim().ToString().ToLower());
                                    if (!isDistrictValueAdded)
                                    {
                                        DynamicParameterValue dto = new DynamicParameterValue
                                        {
                                            Id = 0,
                                            DynamicParameterId = districtKeyId,
                                            TenantId = tenantId,
                                            Value = temp.ToString()
                                        };

                                        districtValueId = _dynamicParamValueRepository.InsertAndGetId(dto);
                                    }
                                    else
                                    {
                                        districtValueId = districtValues.FirstOrDefault(d => d.Value.ToLower() == temp.Trim().ToString().ToLower()).Id;
                                    }

                                }
                                else
                                {
                                    preRegister1.Exception += "District Name is Empty,";
                                }
                                preRegister1.DistrictName = temp;
                                preRegister1.DistrictId = districtValueId;
                            }
                            if (PreRegisterBusinessEntityConsts.FacilityType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    FacilityType checkFacility = _facilityTypeRepository.GetAll().Where(f => f.Name.ToLower() == temp.Trim().ToString().ToLower()).FirstOrDefault();
                                    if (checkFacility == null)
                                    {
                                        FacilityType facilityType = new FacilityType
                                        {
                                            TenantId = tenantId,
                                            Name = temp.ToString()
                                        };

                                        facilityId = _facilityTypeRepository.InsertAndGetId(facilityType);
                                    }
                                    else
                                    {
                                        facilityId = checkFacility.Id;
                                        controlType = checkFacility.ControlType;
                                    }

                                }
                                preRegister1.FacilityTypeId = facilityId;
                                preRegister1.FacilityTypeName = temp;
                                preRegister1.ControlType = controlType;
                            }
                            if (PreRegisterBusinessEntityConsts.Fac_Sub_Type == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    if (facilityId != 0)
                                    {
                                        ControlType controlTypes = ControlType.Basic;
                                        FacilitySubType checkFacilitysubtype = _facilitysubTypesRepository.GetAll().FirstOrDefault(f => f.FacilitySubTypeName.ToLower() == temp.Trim().ToString().ToLower() && f.FacilityTypeId == facilityId);
                                        if (checkFacilitysubtype == null)
                                        {

                                            FacilitySubType facilitysubType = new FacilitySubType
                                            {
                                                Id = 0,
                                                CreationTime = DateTime.Now,
                                                TenantId = tenantId,
                                                FacilityTypeId = facilityId,
                                                FacilitySubTypeName = temp.ToString()
                                            };

                                            facilitySubTypeValueId = _facilitysubTypesRepository.InsertAndGetId(facilitysubType);
                                        }
                                        else
                                        {
                                            facilitySubTypeValueId = checkFacilitysubtype.Id;
                                            controlTypes = checkFacilitysubtype.ControlType;
                                        }
                                    }

                                }
                                preRegister1.FacilitySubTypeId = facilitySubTypeValueId;
                                preRegister1.FacilitySubTypeName = temp;

                            }
                            if (PreRegisterBusinessEntityConsts.Ftypesize == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                preRegister1.FacilityTypeSize = Convert.ToInt32(temp);
                            }
                            if (PreRegisterBusinessEntityConsts.LicenseStatus == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp == null)
                                {
                                    preRegister1.IsActive = true;

                                }
                                else
                                {
                                    preRegister1.IsActive = temp.ToLower() == "false" ? false : true;
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.FHLName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    preRegister1.HFLName = temp.Trim();
                                    preRegister1.Name = temp.Trim();
                                }
                                else
                                {
                                    preRegister1.Exception += "FHLName is Empty,";
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.FACILITY_EMAIL == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Facility_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OWNER_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Owner_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OWNER_EMAIL == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Owner_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OWNER_MOBILE == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Owner_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.DIRECTOR_INCHARGE_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Director_Incharge_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.DIRECTOR_INCHARGE_EMAIL == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Director_Incharge_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.DIRECTOR_INCHARGE_MOBILE == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Director_Incharge_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Pro_En == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Pro_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Pro_Email == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Pro_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Pro_Mobile == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Pro_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Admin_mail == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.AdminEmail = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Admin_Phone == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.AdminMobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.PrimaryContactName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.PrimaryContactName = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.ContactNumber == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.ContactNumber = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Designation == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.Designation = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OfficialEmail == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.OfficialEmail = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupContactName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.BackupContactName = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupContactNumber == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.BackupContactNumber = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupDesignation == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.BackupDesignation = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupOfficialEmail == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                preRegister1.BackupOfficialEmail = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.AdminName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                preRegister1.AdminName = temp;
                            }
                            if (PreRegisterBusinessEntityConsts.AdminSurName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                preRegister1.AdminSurname = temp;
                            }
                            if (PreRegisterBusinessEntityConsts.NumberOfYearsInBusiness == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                var temp2 = temp == null ? "0" : temp;
                                preRegister1.NumberOfYearsInBusiness = Convert.ToInt32(temp2);
                            }
                        }
                        preRegister1.InvalidCount = true;
                        preRegister1.FacilitySubTypeId = preRegister1.FacilitySubTypeId == 0 ? null : preRegister1.FacilitySubTypeId;
                        preRegister1.FacilityTypeId = preRegister1.FacilityTypeId == 0 ? null : preRegister1.FacilityTypeId;
                        preRegister1.DistrictId = preRegister1.DistrictId == 0 ? null : preRegister1.DistrictId;
                        preRegister1.TenantId = tenantId;
                        preRegister1.IsLicenseValid = false;
                        preRegister1.IsHFLNameValid = false;
                        preRegister1.IsFacilityENValid = false;
                        preRegister1.IsAdminEmailValid = false;
                        preRegister1.IsAdminNameValid = false;
                        preRegister1.IsAdminSurnameValid = false;
                        bool duplicate = false;
                        int rowno = Convert.ToInt32(i) + 1;
                        preRegister1.RowName = rowno.ToString();
                        if (preRegister1.LicenseNumber != null)
                        {
                            var getLicenceNumber = preRegisters.Where(x => x.LicenseNumber != null);
                            duplicate = getLicenceNumber.Any(p => p.LicenseNumber.Trim().ToString().ToUpper() == preRegister1.LicenseNumber.Trim().ToString().ToUpper());
                            if(!duplicate)
                            {
                                preRegister1.LicenseNumber = preRegister1.LicenseNumber;
                                preRegister1.IsLicenseValid = true;
                            }
                            else
                            {
                                preRegister1.LicenseNumber = "Duplicate License Number";
                                preRegister1.IsLicenseValid = false;
                            }
                        }
                        else
                        {
                            preRegister1.LicenseNumber = "License Number is Empty";
                            preRegister1.IsLicenseValid = false;
                        }
                        if(preRegister1.HFLName != null)
                        {
                            preRegister1.HFLName = preRegister1.HFLName;
                            preRegister1.IsHFLNameValid = true;
                        }
                        else
                        {
                            preRegister1.HFLName = "FHLName is Empty";
                            preRegister1.IsHFLNameValid = false;
                        }
                        if(preRegister1.Facility_EN != null)
                        {
                            preRegister1.Facility_EN = preRegister1.Facility_EN;
                            preRegister1.IsFacilityENValid = true;
                        }
                        else
                        {
                            preRegister1.Facility_EN = "Facility EN is Empty";
                            preRegister1.IsFacilityENValid = false;
                        }
                        if(preRegister1.AdminEmail !=null)
                        {
                            string email = preRegister1.AdminEmail.Trim();
                            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                            if (isEmail == true)
                            {
                                preRegister1.IsAdminEmailValid = true;
                                preRegister1.AdminEmail = preRegister1.AdminEmail;
                            }
                            else
                            {
                                preRegister1.IsAdminEmailValid = false;
                                preRegister1.AdminEmail = "Invalid Email Id";
                            }
                        }
                        else
                        {
                            preRegister1.IsAdminEmailValid = false;
                            preRegister1.AdminEmail = "Admin Email Is Empty";
                        }
                        if(preRegister1.AdminName != null)
                        {
                            preRegister1.IsAdminNameValid = true;
                            preRegister1.AdminName = preRegister1.AdminName;
                        }
                        else
                        {
                            preRegister1.IsAdminNameValid = false;
                            preRegister1.AdminName = "Admin Name is Empty";
                        }
                        if (preRegister1.AdminSurname != null)
                        {
                            preRegister1.IsAdminSurnameValid = true;
                            preRegister1.AdminSurname = preRegister1.AdminSurname;
                        }
                        else
                        {
                            preRegister1.IsAdminSurnameValid = false;
                            preRegister1.AdminSurname = "Admin Surname is Empty";
                        }
                        
                        if(preRegister1.Owner_Email !=null)
                        {
                            var splitEmail = preRegister1.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegister1.Owner_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegister1.Owner_Email += item +",";
                                }
                            }                           
                        }
                        if (preRegister1.Director_Incharge_Email != null)
                        {
                            var splitEmail = preRegister1.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegister1.Director_Incharge_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegister1.Director_Incharge_Email += item + ",";
                                }
                            }
                        }
                        if (preRegister1.Pro_Email != null)
                        {
                            var splitEmail = preRegister1.Pro_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegister1.Pro_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegister1.Pro_Email += item + ",";
                                }
                            }
                        }
                        if (preRegister1.OfficialEmail != null)
                        {
                            var splitEmail = preRegister1.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegister1.OfficialEmail = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegister1.OfficialEmail += item + ",";
                                }
                            }
                        }
                        if (preRegister1.BackupOfficialEmail != null)
                        {
                            var splitEmail = preRegister1.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            preRegister1.BackupOfficialEmail = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    preRegister1.BackupOfficialEmail += item + ",";
                                }
                            }
                        }


                        if (preRegister1.IsLicenseValid == false || preRegister1.IsHFLNameValid == false || preRegister1.IsFacilityENValid == false)
                        {
                            preRegister1.Message = "Not Inserted";
                        }
                        else
                        {
                            if(preRegister1.IsAdminEmailValid == false || preRegister1.IsAdminNameValid ==false || preRegister1.IsAdminSurnameValid ==false)
                            {
                                preRegister1.Message = "Inserted But Not Approved";
                            }
                            else
                            {
                                preRegister1.Message = "Inserted";
                            }
                        }

                        preRegisters.Add(preRegister1);
                        //if (preRegister1.LicenseNumber != null)
                        //{
                        //    var getLicenceNumber = preRegisters.Where(x => x.LicenseNumber != null);
                        //    duplicate = getLicenceNumber.Any(p => p.LicenseNumber.Trim().ToString().ToUpper() == preRegister1.LicenseNumber.Trim().ToString().ToUpper());
                        //}

                        //preRegister1.CanBeImported = false;

                        //preRegister1.RowName = rowno.ToString();
                        //if (!duplicate)
                        //{
                        //    if (preRegister1.Name != null && preRegister1.LicenseNumber != null && preRegister1.Facility_EN != null)
                        //    {
                        //        preRegister1.CanBeImported = true;

                        //    }
                        //    if (preRegister1.AdminEmail != null)
                        //    {
                        //        string email = preRegister1.AdminEmail.Trim();
                        //        bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                        //        if (isEmail == true)
                        //        {
                        //            preRegister1.CanBeImported = true;

                        //        }
                        //        else
                        //        {
                        //            preRegister1.CanBeImported = false;
                        //            preRegister1.Exception += "Admin Email is Invalid,";
                        //        }
                        //    }
                        //    preRegisters.Add(preRegister1);

                        //}
                    }
                }
                else
                {
                    InvalidMessage.InvalidCount = false;
                    foreach (var item in list2.Take(3))
                    {
                        InvalidMessage.InvalidName += item +", ";
                    }
                    preRegisters.Add(InvalidMessage);

                }
            }
            return preRegisters;
        }

        public List<ImportBusinessEntityUpdateDto> GetBusinessEntitiesFromExcel(byte[] fileBytes, int? tenantId)
        {
            var businessEntities = new List<ImportBusinessEntityUpdateDto>();
            var InvalidMessage = new ImportBusinessEntityUpdateDto();
            InvalidMessage.InvalidCount = true;
            ISheet sheet;
            using (var stream = new MemoryStream(fileBytes))
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                IRow headerRow = sheet.GetRow(0); //Get Header Row
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                }
                List<String> headerList = new List<string>();
                for (int i = 0; i <= 0; i++)
                {
                    string header = "";
                    for (int j = 0; j < cellCount; j++)
                    {
                        header = sheet.GetRow(0).Cells[j]?.StringCellValue;
                        headerList.Add(header);
                    }


                }
                var list = headerList;
                int checkHeaderCount = 0;
                int checkInvalidCount = 0;
                List<string> staticHeader = new List<string>();
                staticHeader.Add("Status"); staticHeader.Add("Country"); staticHeader.Add("CityOrDisctrict"); staticHeader.Add("EntityGroup");
                staticHeader.Add("Entity_Type"); staticHeader.Add("IsPrimary"); staticHeader.Add("LICENSENUMBER"); staticHeader.Add("FACILITY_EN");
                staticHeader.Add("PUBLIC_PRIVATE"); staticHeader.Add("District"); staticHeader.Add("FacilityType"); staticHeader.Add("Fac_Sub_Type");
                staticHeader.Add("Ftypesize"); staticHeader.Add("LicenseStatus"); staticHeader.Add("FHLName"); staticHeader.Add("FACILITY_EMAIL"); staticHeader.Add("OWNER_EN");
                staticHeader.Add("OWNER_EMAIL"); staticHeader.Add("OWNER_MOBILE"); staticHeader.Add("DIRECTOR_INCHARGE_EN"); staticHeader.Add("DIRECTOR_INCHARGE_EMAIL");
                staticHeader.Add("DIRECTOR_INCHARGE_MOBILE"); staticHeader.Add("Pro_En"); staticHeader.Add("Pro_Email"); staticHeader.Add("Pro_Mobile");

                staticHeader.Add("Admin_mail"); staticHeader.Add("Admin_Phone"); staticHeader.Add("PrimaryContactName"); staticHeader.Add("ContactNumber");
                staticHeader.Add("Designation"); staticHeader.Add("OfficialEmail"); staticHeader.Add("BackupContactName"); staticHeader.Add("BackupContactNumber");


                staticHeader.Add("BackupDesignation"); staticHeader.Add("BackupOfficialEmail"); staticHeader.Add("AdminName"); staticHeader.Add("AdminSurName");
                staticHeader.Add("NumberOfYearsInBusiness");
                var list2 = list.Except(staticHeader).ToList();
                foreach (var item in staticHeader)
                {
                    foreach (var item2 in list)
                    {
                        if (item == item2)
                        {
                            checkHeaderCount++;
                        }
                        else
                        {
                            checkInvalidCount++;
                        }
                    }
                }
                var count = checkHeaderCount;


                if (count == 38)
                {
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                        var businessEntity = new ImportBusinessEntityUpdateDto();

                        businessEntity.TenantId = tenantId;
                        for (int j = 0; j < cellCount; j++)
                        {
                            if (PreRegisterBusinessEntityConsts.LICENSENUMBER == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                var temp = GetRequiredValueFromRowOrNull(row, j);
                                if (temp != null)
                                {
                                    businessEntity.LicenseNumber = temp;
                                }
                                else
                                {
                                    businessEntity.Exception += "Licence Number is Empty, ";
                                }
                            }
                            if (PreRegisterBusinessEntityConsts.FACILITY_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.CompanyName = GetRequiredValueFromRowOrNull(row, j);
                                businessEntity.CompanyLegalName = businessEntity.CompanyName;
                            }

                            if (PreRegisterBusinessEntityConsts.FACILITY_EMAIL == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Facility_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.CityOrDisctrict == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.CityOrDisctrict = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.AdminName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.AdminName = GetRequiredValueFromRowOrNull(row, j); ;

                            }
                            if (PreRegisterBusinessEntityConsts.AdminSurName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {

                                businessEntity.AdminSurname = GetRequiredValueFromRowOrNull(row, j);

                            }
                            if (PreRegisterBusinessEntityConsts.Designation == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Designation = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Admin_Phone == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.AdminMobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Admin_mail == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {

                                businessEntity.AdminEmail = GetRequiredValueFromRowOrNull(row, j);

                            }
                            if (PreRegisterBusinessEntityConsts.OWNER_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Owner_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OWNER_EMAIL == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Owner_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OWNER_MOBILE == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Owner_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Pro_En == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Pro_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Pro_Email == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Pro_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Pro_Mobile == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Pro_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.CISO_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.CISO_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.CISO_Email == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.CISO_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.CISO_Mobile == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.CISO_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.PrimaryContactName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.PrimaryContactName = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Designation == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Designation = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.ContactNumber == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.ContactNumber = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.OfficialEmail == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.OfficialEmail = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupContactName == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.BackupContactName = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupDesignation == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.BackupDesignation = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupContactNumber == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.BackupContactNumber = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.BackupOfficialEmail == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.BackupOfficialEmail = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.DIRECTOR_INCHARGE_EN == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Director_Incharge_EN = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.DIRECTOR_INCHARGE_EMAIL == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Director_Incharge_Email = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.DIRECTOR_INCHARGE_MOBILE == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                businessEntity.Director_Incharge_Mobile = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Country == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                //Not Impoting
                                businessEntity.CountryName = GetRequiredValueFromRowOrNull(row, j);
                            }
                            if (PreRegisterBusinessEntityConsts.Status == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                //Not Impoting
                            }
                            if (PreRegisterBusinessEntityConsts.District == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                //Not Impoting
                            }
                        }
                        int rowno = Convert.ToInt32(i) + 1;
                        businessEntity.RowName = rowno.ToString();
                        businessEntity.InvalidCount = true;
                        bool duplicate = false;
                        businessEntity.IsLicenseValid = false;
                        businessEntity.IsCityValid = false;
                        if (businessEntity.LicenseNumber != null)
                        {
                            var getLicenceNumber = businessEntities.Where(x => x.LicenseNumber != null);
                            duplicate = getLicenceNumber.Any(p => p.LicenseNumber.Trim().ToString().ToUpper() == businessEntity.LicenseNumber.Trim().ToString().ToUpper());
                            if (!duplicate)
                            {
                                var checkLicenseExist = _businessEntityRepository.GetAll().Where(x => x.LicenseNumber.Trim().ToLower() == businessEntity.LicenseNumber.Trim().ToLower()).ToList().FirstOrDefault();
                                if (checkLicenseExist != null)
                                {
                                    businessEntity.LicenseNumber = businessEntity.LicenseNumber;
                                    businessEntity.IsLicenseValid = true;
                                }
                                else
                                {
                                    businessEntity.LicenseNumber = "License Number Does Not Exist";
                                    businessEntity.IsLicenseValid = false;
                                }
                            }
                            else
                            {
                                businessEntity.LicenseNumber = "Duplicate License Number";
                                businessEntity.IsLicenseValid = false;
                            }
                        }
                        else
                        {
                            businessEntity.LicenseNumber = "License Number is Empty";
                            businessEntity.IsLicenseValid = false;
                        }
                        if (businessEntity.Owner_Email != null)
                        {
                            var splitEmail = businessEntity.Owner_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            businessEntity.Owner_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    businessEntity.Owner_Email += item + ",";
                                }
                            }
                        }
                        if (businessEntity.Pro_Email != null)
                        {
                            var splitEmail = businessEntity.Pro_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            businessEntity.Pro_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    businessEntity.Pro_Email += item + ",";
                                }
                            }
                        }
                        if (businessEntity.CISO_Email != null)
                        {
                            var splitEmail = businessEntity.CISO_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            businessEntity.CISO_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    businessEntity.CISO_Email += item + ",";
                                }
                            }
                        }
                        if (businessEntity.OfficialEmail != null)
                        {
                            var splitEmail = businessEntity.OfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            businessEntity.OfficialEmail = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    businessEntity.OfficialEmail += item + ",";
                                }
                            }
                        }
                        if (businessEntity.BackupOfficialEmail != null)
                        {
                            var splitEmail = businessEntity.BackupOfficialEmail.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            businessEntity.BackupOfficialEmail = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    businessEntity.BackupOfficialEmail += item + ",";
                                }
                            }
                        }
                        if (businessEntity.Director_Incharge_Email != null)
                        {
                            var splitEmail = businessEntity.Director_Incharge_Email.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            businessEntity.Director_Incharge_Email = null;
                            foreach (var item in splitEmail)
                            {
                                string email = item.Trim();
                                bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                                if (isEmail == true)
                                {
                                    businessEntity.Director_Incharge_Email += item + ",";
                                }
                            }
                        }
                        if(businessEntity.CityOrDisctrict != null)
                        {
                            businessEntity.CityOrDisctrict = businessEntity.CityOrDisctrict;
                            businessEntity.IsCityValid = true;
                        }
                        else
                        {
                            businessEntity.CityOrDisctrict = "City Name Is Empty";
                            businessEntity.IsCityValid = false;
                        }

                        if (businessEntity.IsLicenseValid == false)
                        {
                            businessEntity.Message = "Not Updated";
                        }

                            businessEntities.Add(businessEntity);
                 
                    }
                }
                else
                {

                    InvalidMessage.InvalidCount = false;
                    foreach (var item in list2.Take(3))
                    {
                        InvalidMessage.InvalidName += item + ", ";
                    }
                    businessEntities.Add(InvalidMessage);
                }
            }
            return businessEntities;
        }


        private PreRegisterBusinessEntity ProcessExcelRow(ISheet worksheet, int row)
        {
            var entity = new PreRegisterBusinessEntity();

            try
            {

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }

            return entity;
        }

        private string GetRequiredValueFromRowOrNull(IRow row, int j)
        {
            ICell cell = row.GetCell(j, MissingCellPolicy.RETURN_BLANK_AS_NULL);
            if (cell != null)
            {
                return cell.ToString();
            }
            else
            {
                return null;
            }
        }


    }
}
