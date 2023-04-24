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
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.FindingReports.Import;

namespace LockthreatCompliance.Web.Controllers
{
    public class ImportController : LockthreatComplianceControllerBase
    {
        private readonly TenantManager _tenantManager;
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<ExternalAssessmentQuestion> _externalAssessmentQuestionRepository;
        private readonly IRepository<BusinessEntity> _businessEntityRepository;
        private IHostingEnvironment _hostingEnvironment;
        protected readonly IBinaryObjectManager BinaryObjectManager;
        protected readonly IBackgroundJobManager BackgroundJobManager;
        private readonly IBusinessEntitiesAppService _businessEntitiesAppService;
        private readonly IRepository<User,long> _userRepository;
        private readonly IAppNotifier _appNotifier;
        public ImportController(IRepository<Question> questionRepository, IRepository<BusinessEntity> businessEntityRepository, IHostingEnvironment hostingEnvironment, IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager,
            IBusinessEntitiesAppService businessEntitiesAppService, TenantManager tenantManager, IRepository<User,long> userRepository, IRepository<ExternalAssessmentQuestion> externalAssessmentQuestionRepository, IAppNotifier appNotifier)
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
        }

        [HttpPost]
        public async Task<JsonResult> ImportQuestions()
        {
            var file = Request.Form.Files.First();
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }

                    var questions = new List<Question>();
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        var question = new Question();
                        for (int j =0;j<4;j++)
                        {
                            if (QuestionExcelConsts.Name == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                              question.Name = sheet.GetRow(i).Cells[j]?.StringCellValue;
                            }
                            if (QuestionExcelConsts.Description == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                question.Description = sheet.GetRow(i).Cells[j]?.StringCellValue;
                            }
                            if (QuestionExcelConsts.AnswerType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                Enum.TryParse(sheet.GetRow(i).Cells[j]?.StringCellValue, out AnswerType answerType);
                                question.AnswerType = answerType;
                            }
                            if (QuestionExcelConsts.AnswerOptionWithScore == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                              
                                var temp = sheet.GetRow(i).Cells[j]?.StringCellValue.GetAnswerOptions();
                                question.AnswerOptions = temp;
                            }
                                                       
                        }
                        if (question.Name != null && question.Description != null)
                        {
                            question.TenantId = AbpSession.TenantId;
                            questions.Add(question);
                        }
                    }
                    if (questions.Count != 0)
                    {
                        await _questionRepository.InsertAllAsync(questions);
                        return Json(new AjaxResponse(new { }));
                    }
                    else
                    {
                        return Json(new AjaxResponse(new ErrorInfo("Self Assessment Question import process has failed. File is invalid, Please use the import template provided.")));
                    }

                   

                }

            }
            return Json(new AjaxResponse(new ErrorInfo("Self Assessment Question import process has failed. File is invalid, Please use the import template provided.")));
        }

        [HttpPost]
        public async Task<JsonResult> ImportExternalQuestions()
        {
            var file = Request.Form.Files.First();
            string folderName = "Upload";
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                ISheet sheet;
                string fullPath = Path.Combine(newPath, file.FileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                    stream.Position = 0;
                    if (fileExtension == ".xls")
                    {
                        HSSFWorkbook hssfwb = new HSSFWorkbook(stream); //This will read the Excel 97-2000 formats  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook  
                    }
                    else
                    {
                        XSSFWorkbook hssfwb = new XSSFWorkbook(stream); //This will read 2007 Excel format  
                        sheet = hssfwb.GetSheetAt(0); //get first sheet from workbook   
                    }

                    var externalAssessmentQuestions = new List<ExternalAssessmentQuestion>();
                    IRow headerRow = sheet.GetRow(0); //Get Header Row
                    int cellCount = headerRow.LastCellNum;
                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null || string.IsNullOrWhiteSpace(cell.ToString())) continue;
                    }
                    for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++) //Read Excel File
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue;
                        if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                        var externalAssessmentQuestion = new ExternalAssessmentQuestion();
                        for (int j = 0; j < 4; j++)
                        {
                            if (QuestionExcelConsts.Name == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                externalAssessmentQuestion.Name = sheet.GetRow(i).Cells[j]?.StringCellValue;
                            }
                            if (QuestionExcelConsts.Description == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                externalAssessmentQuestion.Description = sheet.GetRow(i).Cells[j]?.StringCellValue;
                            }
                            if (QuestionExcelConsts.AnswerType == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                Enum.TryParse(sheet.GetRow(i).Cells[j]?.StringCellValue, out AnswerType answerType);
                                externalAssessmentQuestion.AnswerType = answerType;
                            }
                            if (QuestionExcelConsts.AnswerOptionWithScore == sheet.GetRow(0).Cells[j]?.StringCellValue)
                            {
                                externalAssessmentQuestion.AnswerOptions = sheet.GetRow(i).Cells[j]?.StringCellValue.GetExternalAnswerOptions();
                            }

                        }
                        if (externalAssessmentQuestion.Name != null && externalAssessmentQuestion.Description != null)
                        {
                            externalAssessmentQuestion.TenantId = AbpSession.TenantId;
                            externalAssessmentQuestions.Add(externalAssessmentQuestion);
                        }
                     }
                    if (externalAssessmentQuestions.Count() != 0)
                    {
                        await _externalAssessmentQuestionRepository.InsertAllAsync(externalAssessmentQuestions);
                        return Json(new AjaxResponse(new { }));
                    }
                    else
                    {
                        return Json(new AjaxResponse(new ErrorInfo("External Assessment Question import process has failed. File is invalid, Please use the import template provided.")));
                    }


                }
            }
            return Json(new AjaxResponse(new ErrorInfo("External Assessment Question import process has failed. File is invalid, Please use the import template provided.")));
        }

        [HttpPost]
        public async Task<JsonResult> ImportPreRegisterEntities()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }


                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "PRE-" +random.Next(1000)+'-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);
                await BinaryObjectManager.SaveAsync(fileObject);
                await BackgroundJobManager.EnqueueAsync<ImportPreEntitiesToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom
                });

                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportBusinessEntities()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "BE-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportBusinessEntitiesToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom
                });

                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportDynamicParameters()
        {

            try
            {
                
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());             
                var userId=int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                 


                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }
                
                var tenantId = tenantid;
                Random random = new Random();
                var getRandom = "DP-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportDynamicParameterToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom,
                });

                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportDynamicValues()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = tenantid;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportDynamicParameterValueToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportControlRequirements()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "CR-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportControlRequirementsValueToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom,
                });

                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportContact()
        {

            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportContactValueToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> importSelfAssessmentResponse()
        {

            try
            {
                var file = Request.Form.Files.First();
                var list = Request.Form.ToList();
                var number = list[0].Value;
                var tenantid = int.Parse(list[1].Value);
                var userId = int.Parse(list[2].Value);
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportSelfAssessmentResponseValueToExcel, ImportSelfAssesmentResponseFromExcelJobArgs>(new ImportSelfAssesmentResponseFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    AssesmentId = Convert.ToInt32(number)
                });
                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportExternalAssessmentResponse()
        {

            try
            {
                var file = Request.Form.Files.First();
                var list = Request.Form.ToList();
                var number = list[0].Value;                
                var tenantid = int.Parse(list[1].Value);
                var userId = int.Parse(list[2].Value);
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportExternalAssessmentResponseValueToExcel, ImportExternalAssesmentResponseFromExcelJobArgs>(new ImportExternalAssesmentResponseFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    ExternalAssesmentId = Convert.ToInt32(number)
                });
                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportFacilityTypes()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = tenantid;
                Random random = new Random();
                var getRandom = "FT-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportFacilityTypesValueToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom,
                });

                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportFacilitySubTypes()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = tenantid;
                Random random = new Random();
                var getRandom = "FST-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportFacilitySubTypesValueToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom,
                });

                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportUser()
        {
            try
            {
                var file = Request.Form.Files.First();

                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportUsersToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = AbpSession.ToUserIdentifier()
                });

                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportAuditReport()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "AP-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportAuditProjectToExcel,ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom
                });
                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> importExternalFindingResponse()
        {

            try
            {
                var file = Request.Form.Files.First();
                var list = Request.Form.ToList();
                var number = list[0].Value;
                var tenantid = int.Parse(list[1].Value);
                var userId = int.Parse(list[2].Value);
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "EF-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportExternalFindingToExcelJob, ImportExternalFindingFromExcelJobArgs>(new ImportExternalFindingFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    AuditProjectId = Convert.ToInt32(number),
                    Code = getRandom
                });
                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> importExternalCAPAResponse()
        {

            try
            {
                var file = Request.Form.Files.First();
                var list = Request.Form.ToList();
                var number = list[0].Value;
                var tenantid = int.Parse(list[1].Value);
                var userId = int.Parse(list[2].Value);
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "EC-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportExternalCapaToExcelJob, ImportExternalFindingFromExcelJobArgs>(new ImportExternalFindingFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    AuditProjectId = Convert.ToInt32(number),
                    Code = getRandom
                });
                return Json(new AjaxResponse(new { }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }

        [HttpPost]
        public async Task<JsonResult> ImportCertificateData()
        {

            try
            {
                var file = Request.Form.Files.First();
                var tenantid = int.Parse(Request.Form.FirstOrDefault().Value.ToString());
                var userId = int.Parse(Request.Form.LastOrDefault().Value.ToString());
                var useridentity = new UserIdentifier(tenantid, userId);
                if (file == null)
                {
                    throw new UserFriendlyException(L("File_Empty_Error"));
                }

                if (file.Length > 1048576 * 100) //100 MB
                {
                    throw new UserFriendlyException(L("File_SizeLimit_Error"));
                }

                byte[] fileBytes;
                using (var stream = file.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                var tenantId = AbpSession.TenantId;
                Random random = new Random();
                var getRandom = "AP-" + random.Next(1000) + '-' + userId;
                var fileObject = new BinaryObject(tenantId, fileBytes);

                await BinaryObjectManager.SaveAsync(fileObject);

                await BackgroundJobManager.EnqueueAsync<ImportCertificateValueToExcelJob, ImportUsersFromExcelJobArgs>(new ImportUsersFromExcelJobArgs
                {
                    TenantId = tenantId,
                    BinaryObjectId = fileObject.Id,
                    User = useridentity,
                    Code = getRandom
                });
                return Json(new AjaxResponse(new { getRandom }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }


    }
}
