using AutoMapper;
using LockthreatCompliance.AssessmentSchedules.Dto;
using LockthreatCompliance.AssessmentSchedules.ExternalAsssementSchedules;
using LockthreatCompliance.ExternalAssessments;
using LockthreatCompliance.ExternalAssessments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.DtoMappers
{
    public static class ExternalAssessmentDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditExternalAssessmentDto, ExternalAssessment>()
                .ForMember(m => m.AuthoritativeDocuments, options => options.MapFrom(s => s.AuthoritativeDocumentIds.Select(e => new ExternalAssessmentAuthoritativeDocument
                {
                    AuthoritativeDocumentId = e
                })));

            configuration.CreateMap<ExternalAssessment, CreateOrEditExternalAssessmentDto>()
               .ForMember(m => m.AuthoritativeDocumentIds, options => options.MapFrom(s => s.AuthoritativeDocuments.Select(e => e.AuthoritativeDocumentId).ToList()));

            configuration.CreateMap<ExternalAssessment, GetExternalAssessmentForEditOutput>()
                .ForMember(m => m.ExternalAssessment, options => options.MapFrom(e => e))
                .ForMember(m => m.AuditeeEmail, options => options.MapFrom(s => s.BusinessEntityLeadAssessor.EmailAddress))
                .ForMember(m => m.AuditeeName, options => options.MapFrom(s => s.BusinessEntityLeadAssessor.Name))
                .ForMember(m => m.AuditeeSurname, options => options.MapFrom(s => s.BusinessEntityLeadAssessor.Surname))
                .ForMember(m => m.AuditeePhone, options => options.MapFrom(s => s.BusinessEntityLeadAssessor.PhoneNumber))
                .ForMember(m => m.LeadAssessorEmail, options => options.MapFrom(s => s.LeadAssessor.EmailAddress))
                .ForMember(m => m.LeadAssessorPhone, options => options.MapFrom(s => s.LeadAssessor.PhoneNumber));



            configuration.CreateMap<ExternalAssessment, ExternalAssessmentDto>()
                .ForMember(m => m.BusinessEntityName, options => options.MapFrom(s => s.BusinessEntity.CompanyName));

              configuration.CreateMap<ExternalAssessment, ExternalAssessmentWIthPrimaryEnrityDto>()
                .ForMember(m => m.BusinessEntityName, options => options.MapFrom(s => s.BusinessEntity.CompanyName))
                .ForMember(m => m.IsPrimaryEntity, options => options.MapFrom(e => e.EntityGroupId == null ? true : (e.EntityGroup.PrimaryEntityId == e.BusinessEntityId) ? true : false));
            

            configuration.CreateMap<ExternalAssessment, ExernalAssessmentWithQuestionsDto>().ReverseMap();
            configuration.CreateMap<ExternalAssessmentAuditWorkPaperDto, ExternalAssessmentAuditWorkPaper>().ReverseMap();
            configuration.CreateMap<ExternalAssessmentScheduleDto, ExternalAssessmentSchedule>()
                .ForMember(m => m.AuthoritativeDocuments, options => options.MapFrom(s => s.AuthoritativeDocumentIds.Select(e => new ExtAssSchAuthoritativeDocument
                {
                    AuthoritativeDocumentId = e
                })));


            configuration.CreateMap<ExternalAssessmentScheduleDetailDto, ExternalAssessmentScheduleDetail>()
                .ForMember(m => m.AuthoritativeDocuments, options => options.MapFrom(s => s.AuthoritativeDocumentIds.Select(e => new ExtAssSchDetailAuthoritativeDocument
                {
                    AuthoritativeDocumentId = e
                })));
        }
    }
}
