

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using LockthreatCompliance.Questions.Exporting;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.Dto;
using Abp.Application.Services.Dto;
using LockthreatCompliance.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using LockthreatCompliance.EntityFrameworkCore.RepositoryExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using Microsoft.AspNetCore.Hosting;
using LockthreatCompliance.ControlRequirements.Dtos;
using Abp.UI;

namespace LockthreatCompliance.Questions
{
    [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Questions)]
    public class QuestionsAppService : LockthreatComplianceAppServiceBase, IQuestionsAppService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IQuestionsExcelExporter _questionsExcelExporter;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<ExternalAssessmentQuestion> _externalAssessmentQuestionRepository;
        private readonly IRepository<ExternalQuestionAnswerOption> _externalAnswerOptionRepository;
        private readonly IRepository<AnswerOption> _answerOptionRepository;

        public QuestionsAppService(IRepository<Question> questionRepository,
            IQuestionsExcelExporter questionsExcelExporter, IHostingEnvironment hostingEnvironment,
            IRepository<ExternalAssessmentQuestion> externalAssessmentQuestionRepository,
            IRepository<ExternalQuestionAnswerOption> externalAnswerOptionRepository,
            IRepository<AnswerOption> answerOptionRepository)
        {
            _hostingEnvironment = hostingEnvironment;
            _questionRepository = questionRepository;
            _questionsExcelExporter = questionsExcelExporter;
            _externalAnswerOptionRepository = externalAnswerOptionRepository;
            _answerOptionRepository = answerOptionRepository;
            _externalAssessmentQuestionRepository = externalAssessmentQuestionRepository;

        }

        public async Task<PagedResultDto<QuestionDto>> GetAll(GetAllQuestionsInput input)
        {
            try
            {
                var filteredQuestions = _questionRepository.GetAll().WhereIf(
                    !input.Filter.IsNullOrWhiteSpace(),
                    u =>
                        u.Name.Contains(input.Filter.Trim().ToLower()) ||
                        u.Description.Contains(input.Filter.Trim().ToLower())
                );


                var pagedAndFilteredQuestions = filteredQuestions
                    .OrderBy(input.Sorting)
                    .PageBy(input);

                var questions = pagedAndFilteredQuestions.Select(
                                e => new QuestionDto
                                {
                                    Code = e.Code,
                                    Name = e.Name,
                                    Description = e.Description,
                                    Id = e.Id,
                                    Mandatory= e.Mandatory
                                });

                var totalCount = await filteredQuestions.CountAsync();

                return new PagedResultDto<QuestionDto>(
                    totalCount,
                    await questions.ToListAsync()
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Questions_Edit)]
        public async Task<GetQuestionForEditOutput> GetQuestionForEdit(EntityDto input)
        {
            var question = await _questionRepository.GetIncluding(e => e.Id == input.Id, "AnswerOptions");

            var output = new GetQuestionForEditOutput { Question = ObjectMapper.Map<CreateOrEditQuestionDto>(question) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Questions_Create, AppPermissions.Pages_ComplianceManagement_Questions_Edit)]
        public async Task CreateOrEdit(CreateOrEditQuestionDto input)
        {
            if (input.removedAnswers!= null)
            {
                foreach (var id in input.removedAnswers)
                {
                    var ans = await _answerOptionRepository.FirstOrDefaultAsync(a => a.Id == id);
                    await _answerOptionRepository.DeleteAsync(ans);
                }
            }

            var data = ObjectMapper.Map<Question>(input);
            data.TenantId = AbpSession.TenantId;
            await _questionRepository.InsertOrUpdateAsync(data);
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Questions_Create)]
        protected virtual async Task Create(CreateOrEditQuestionDto input)
        {
            var question = ObjectMapper.Map<Question>(input);

            if (AbpSession.TenantId != null)
            {
                question.TenantId = (int?)AbpSession.TenantId;
            }


            await _questionRepository.InsertAsync(question);
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Questions_Edit)]
        protected virtual async Task Update(CreateOrEditQuestionDto input)
        {
            var question = await _questionRepository.GetIncluding(e => e.Id == input.Id, "AnswerOptions");
            ObjectMapper.Map(input, question);
        }

        [AbpAuthorize(AppPermissions.Pages_ComplianceManagement_Questions_Delete)]
        public async Task Delete(EntityDto input)
        {
            var check = _answerOptionRepository.GetAll().Any(x => x.QuestionId == input.Id);
            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            }
            else
            {
                await _questionRepository.DeleteAsync(input.Id);
            }
        }


        #region Import&Export
        public async Task<FileDto> GetQuestionsToExcel(GetAllQuestionsForExcelInput input)
        {

            var query = _questionRepository.GetAll()
                .Include("AnswerOptions")
                .Select(o => new GetQuestionForViewDto()
                {
                    Question = new QuestionDto
                    {
                        Code = o.Code,
                        TenantId = (int)o.TenantId,
                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                        Mandatory=o.Mandatory,
                        AnswerType = Enum.GetName(o.AnswerType.GetType(), o.AnswerType),
                        AnswerOptionsWithScores = o.GetFlattenedAnswersOptionsWithScores()
                    }
                });
            var questionListDtos = await query.ToListAsync();
            return _questionsExcelExporter.ExportToFile(questionListDtos);
        }

        public async Task ImportQuestions([FromForm]IFormFile file)
        {
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

                        var questionId = row.GetCell(0).ToString();
                        var questionName = row.GetCell(1).ToString();
                        var questionDescription = row.GetCell(2).ToString();
                        Enum.TryParse(row.GetCell(3).ToString(), out AnswerType answerType);
                        var answerOptions = row.GetCell(4).ToString().GetAnswerOptions();
                        var question = new Question
                        {
                            AnswerOptions = answerOptions,
                            AnswerType = answerType,
                            Description = questionDescription,
                            Name = questionName,
                            TenantId = AbpSession.TenantId
                        };
                        questions.Add(question);
                    }
                    await _questionRepository.InsertAllAsync(questions);

                }
            }
        }
    
        public async Task<FileDto> GetExternalQuestionsToExcel(GetAllQuestionsForExcelInput input)
        {

            var query = _externalAssessmentQuestionRepository.GetAll()
                .Include("AnswerOptions")
                .Select(o => new ExternalQuestionDto()
                {
                   
                        Code = o.Code,
                        TenantId = (int)o.TenantId,
                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                        Mandatory = o.Mandatory,
                        AnswerType = Enum.GetName(o.AnswerType.GetType(), o.AnswerType),
                        AnswerOptionsWithScores = o.GetFlattenedAnswersOptionsWithScores()
                    
                });
            var questionListDtos = await query.ToListAsync();
            return _questionsExcelExporter.ExportExternalQuestionToFile(questionListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions)]
        public async Task<PagedResultDto<ExternalQuestionDto>> GetAllExternalQuestions(GetAllQuestionsInput input)
        {
            try
            {
                var filteredQuestions = _externalAssessmentQuestionRepository.GetAll()
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
                                e => new ExternalQuestionDto
                                {
                                    Code = e.Code,
                                    Name = e.Name,
                                    Description = e.Description,
                                    Id = e.Id,
                                    Mandatory = e.Mandatory
                                });

                var totalCount = await filteredQuestions.CountAsync();

                return new PagedResultDto<ExternalQuestionDto>(
                    totalCount,
                    await questions.ToListAsync()
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Edit)]
        public async Task<GetEditExternalQuestionForEditOutput> GetExternalQuestionForEdit(EntityDto input)
        {
            var question = await _externalAssessmentQuestionRepository.GetIncluding(e => e.Id == input.Id, "AnswerOptions");

            var output = new GetEditExternalQuestionForEditOutput { Question = ObjectMapper.Map<CreateOrEditExternalAssessmentQuestionDto>(question) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Create, AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Edit)]
        public async Task CreateOrEditExternalQuestion(CreateOrEditExternalAssessmentQuestionDto input)
        {
            if (input.removedAnswers!=null)
            {
                foreach (var id in input.removedAnswers)
                {
                    var ans = await _externalAnswerOptionRepository.FirstOrDefaultAsync(a => a.Id == id);
                    await _externalAnswerOptionRepository.DeleteAsync(ans);
                }
            }

            var data = ObjectMapper.Map<ExternalAssessmentQuestion>(input);
            data.TenantId = AbpSession.TenantId;
            await _externalAssessmentQuestionRepository.InsertOrUpdateAsync(data);
        }


        protected virtual async Task CreateExternalAssessmentQuestion(CreateOrEditExternalAssessmentQuestionDto input)
        {
            var question = ObjectMapper.Map<ExternalAssessmentQuestion>(input);

            if (AbpSession.TenantId != null)
            {
                question.TenantId = (int?)AbpSession.TenantId;
            }


            await _externalAssessmentQuestionRepository.InsertAsync(question);
        }

        protected virtual async Task UpdateExternalAssessmentQuestion(CreateOrEditExternalAssessmentQuestionDto input)
        {
            var question = await _externalAssessmentQuestionRepository.GetIncluding(e => e.Id == input.Id, "AnswerOptions");
            ObjectMapper.Map(input, question);
        }

        [AbpAuthorize(AppPermissions.Pages_AuditManagement_ExternalAssessment_Questions_Delete)]
        public async Task DeleteExternalQuestion(EntityDto input)
        {
            var check = _externalAnswerOptionRepository.GetAll().Any(x => x.QuestionId == input.Id);
            if (check)
            {
                throw new UserFriendlyException("The related records of the following record still exist. Please delete child records to delete this ! ");

            }
            else
            {
                await _externalAssessmentQuestionRepository.DeleteAsync(input.Id);
            }
        }

        #endregion

    }
}