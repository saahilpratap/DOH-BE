using AutoMapper;
using LockthreatCompliance.Assessments.Dto;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.Questions;
using LockthreatCompliance.Questions.Dtos;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DtoMappers
{
    public static class AssessmentDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Assessment, AssessmentDto>()
                //.ForMember(m => m.AssessmentType, options => options.MapFrom(s => s.Type))
                .ForMember(m => m.ReportingDate, options => options.MapFrom(s => s.ReportingDeadLine))
                .ForMember(m => m.AssessmentDate, options => options.MapFrom(s => s.Date));
            //.ForMember(m=>m.BusinessEntityName, options => options.MapFrom(s=> s))

            configuration.CreateMap<ReviewData, ReviewDataDto>()
                .ForMember(m => m.AssessmentName, options => options.MapFrom(s => s.Assessment.Name))
                .ForMember(m => m.ControlRequirementDescription, options => options.MapFrom(s => s.ControlRequirement.Description))
                .ForMember(m => m.ControlRequirementDomainName, options => options.MapFrom(s => s.ControlRequirement.DomainName))
                .ForMember(m => m.Type, options => options.MapFrom(s => s.ResponseType))
                .ForMember(m => m.ControlRequirementOriginalId, options => options.MapFrom(s => s.ControlRequirement.OriginalId))
                .ForMember(m => m.Iscored, options => options.MapFrom(s => s.ControlRequirement.Iscored))
                .ForMember(m => m.Clarification, options => options.MapFrom(s => s.RequestComment))
                .ForMember(m => m.AdditionalComment, options => options.MapFrom(s => s.ControlRequirement.ControlType));

            configuration.CreateMap<ReviewQuestion, ReviewQuestionDto>()
                .ForMember(m => m.QuestionName, options => options.MapFrom(s => s.Question.Name))
                .ForMember(m => m.AnswerType, options => options.MapFrom(s => s.Question.AnswerType))
                .ForMember(m => m.QuestionDescription, options => options.MapFrom(s => s.Question.Description))
                .ForMember(m => m.AnswerOptions, options => options.MapFrom(s => s.Question.AnswerOptions));

            configuration.CreateMap<AnswerOption, AnswerOptionDto>();

            configuration.CreateMap<DocumentPath, AttachmentDto>()
                .ForMember(m => m.Code, options => options.MapFrom(s => s.Code))
                .ForMember(m => m.FileName, options => options.MapFrom(s => s.FileName));
        }
    }
}
