using AutoMapper;
using LockthreatCompliance.BusinessRisks;
using LockthreatCompliance.BusinessRisks.Dtos;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.FindingReports;
using LockthreatCompliance.FindingReports.Dtos;
using LockthreatCompliance.Incidents;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LockthreatCompliance.DtoMappers
{
    public static class FindingReportDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditFindingReportDto, FindingReport>()
                .ForMember(m => m.RelatedBusinessRisks, options => options.MapFrom(s => s.RelatedBusinessRisks.Select(e => new FindingReportRelatedBusinessRisk
                {
                    BusinessRiskId = e
                }))).ForMember(m => m.RelatedExceptions, options => options.MapFrom(s => s.RelatedExceptions.Select(e => new FindingReportRelatedException
                {
                    ExceptionId = e
                }))).ForMember(m => m.RelatedIncidents, options => options.MapFrom(s => s.RelatedIncidents.Select(e => new FindingReportRelatedIncident
                {
                    IncidentId = e
                })))
                .ForMember(m => m.SelectedFindingRemediations, options => options.MapFrom(s => s.SelectedFindingRemediations.Select(e => new FindingRemediation
                {
                    RemediationId = e
                })))
                .ForMember(m => m.Code, options => options.Ignore())
                .ForMember(m => m.Attachments, options => options.Ignore());


            configuration.CreateMap<FindingReport, CreateOrEditFindingReportDto>()
                .ForMember(m => m.RelatedBusinessRisks, options => options.MapFrom(s => s.RelatedBusinessRisks.Select(e => e.BusinessRiskId)))
                .ForMember(m => m.RelatedExceptions, options => options.MapFrom(s => s.RelatedExceptions.Select(e => e.ExceptionId)))
                .ForMember(m => m.RelatedIncidents, options => options.MapFrom(s => s.RelatedIncidents.Select(e => e.IncidentId)))
                .ForMember(m => m.SelectedFindingRemediations, options => options.MapFrom(s => s.SelectedFindingRemediations.Select(e => e.RemediationId)));


            configuration.CreateMap<DocumentPath, AttachmentWithTitleDto>()
                .ForMember(m => m.Title, options => options.MapFrom(s => s.Title))
                .ForMember(m => m.Code, options => options.MapFrom(s => s.Code));


            //// Incident Mapping
            configuration.CreateMap<CreateOrEditIncidentDto, Incident>()
                .ForMember(m => m.RelatedBusinessRisks, options => options.MapFrom(s => s.RelatedBusinessRisks.Select(e => new IncidentRelatedBusinessRisk
                {
                    BusinessRiskId = e
                }))).ForMember(m => m.RelatedExceptions, options => options.MapFrom(s => s.RelatedExceptions.Select(e => new IncidentRelatedException
                {
                    ExceptionId = e
                }))).ForMember(m => m.RelatedFindings, options => options.MapFrom(s => s.RelatedFindings.Select(e => new FindingReportRelatedIncident
                {
                    FindingReportId = e
                })))
                .ForMember(m => m.SelectedIncidentRemediations, options => options.MapFrom(s => s.SelectedIncidentRemediations.Select(e => new IncidentRemediation
                {
                    RemediationId = e
                })))
                .ForMember(m => m.Code, options => options.Ignore())
                .ForMember(m => m.Attachments, options => options.Ignore())
                .ForMember(m => m.Reviewers, options => options.MapFrom(s => s.Reviewers.Select(e => new IncidentReviewer { ReviewerId = e }).ToList()));

            configuration.CreateMap<Incident, CreateOrEditIncidentDto>()
                .ForMember(m => m.RelatedBusinessRisks, options => options.MapFrom(s => s.RelatedBusinessRisks.Select(e => e.BusinessRiskId)))
                .ForMember(m => m.RelatedExceptions, options => options.MapFrom(s => s.RelatedExceptions.Select(e => e.ExceptionId)))
                .ForMember(m => m.RelatedFindings, options => options.MapFrom(s => s.RelatedFindings.Select(e => e.FindingReportId)))
                .ForMember(m => m.SelectedIncidentRemediations, options => options.MapFrom(s => s.SelectedIncidentRemediations.Select(e => e.RemediationId)))
                .ForMember(m => m.Reviewers, options => options.MapFrom(s => s.Reviewers.Select(e => e.ReviewerId).ToList()));

            //// Exception Mapping
            configuration.CreateMap<CreateOrEditExceptionDto, LockthreatCompliance.Exceptions.Exception>()
                .ForMember(m => m.ExceptionRelatedBusinessRisks, options => options.MapFrom(s => s.ExceptionRelatedBusinessRisks.Select(e => new ExceptionRelatedBusinessRisk
                {
                    BusinessRiskId = e
                }))).ForMember(m => m.RelatedFindings, options => options.MapFrom(s => s.RelatedFindings.Select(e => new FindingReportRelatedException
                {
                    FindingReportId = e
                }))).ForMember(m => m.RelatedIncidents, options => options.MapFrom(s => s.RelatedIncidents.Select(e => new IncidentRelatedException
                {
                    IncidentId = e
                })))
                .ForMember(m => m.SelectedExceptionRemediations, options => options.MapFrom(s => s.SelectedExceptionRemediations.Select(e => new ExceptionRemediation
                {
                    RemediationId = e
                })))
                .ForMember(m => m.Code, options => options.Ignore())
                .ForMember(m => m.Attachments, options => options.Ignore());


            configuration.CreateMap<LockthreatCompliance.Exceptions.Exception, CreateOrEditExceptionDto>()
                .ForMember(m => m.ExceptionRelatedBusinessRisks, options => options.MapFrom(s => s.ExceptionRelatedBusinessRisks.Select(e => e.BusinessRiskId)))
                .ForMember(m => m.RelatedFindings, options => options.MapFrom(s => s.RelatedFindings.Select(e => e.FindingReportId)))
                .ForMember(m => m.RelatedIncidents, options => options.MapFrom(s => s.RelatedIncidents.Select(e => e.IncidentId)))
                .ForMember(m => m.SelectedExceptionRemediations, options => options.MapFrom(s => s.SelectedExceptionRemediations.Select(e => e.RemediationId)));


            //// Business Risks Mapping
            configuration.CreateMap<CreateOrEditBusinessRiskDto, BusinessRisk>()
                .ForMember(m => m.RelatedExceptions, options => options.MapFrom(s => s.RelatedExceptions.Select(e => new ExceptionRelatedBusinessRisk
                {
                    ExceptionId = e
                }))).ForMember(m => m.RelatedFindings, options => options.MapFrom(s => s.RelatedFindings.Select(e => new FindingReportRelatedBusinessRisk
                {
                    FindingReportId = e
                }))).ForMember(m => m.RelatedIncidents, options => options.MapFrom(s => s.RelatedIncidents.Select(e => new IncidentRelatedBusinessRisk
                {
                    IncidentId = e
                })))
                .ForMember(m => m.SelectedBusinessRiskRemediations, options => options.MapFrom(s => s.SelectedBusinessRiskRemediations.Select(e => new BusinessRiskRemediation
                {
                    RemediationId = e
                })))
                .ForMember(m => m.BusinessRisksCompensatingControls, options => options.MapFrom(s => s.SelectedBusinessRisksCompensatingControls.Select(e => new BusinessRisksCompensatingControls
                {
                    ControlRequirementId = e
                })))
                .ForMember(m => m.BusinessRisksImpactedControls, options => options.MapFrom(s => s.SelectedBusinessRisksImpactedControls.Select(e => new BusinessRisksImpactedControls
                {
                    ControlRequirementId = e
                })))
                .ForMember(m => m.BusinessRisksMonitoringControls, options => options.MapFrom(s => s.SelectedBusinessRisksMonitoringControls.Select(e => new BusinessRisksMonitoringControls
                {
                    ControlRequirementId = e
                })))
                .ForMember(m => m.Code, options => options.Ignore())
                .ForMember(m => m.Attachments, options => options.Ignore());


            configuration.CreateMap<BusinessRisk, CreateOrEditBusinessRiskDto>()
                .ForMember(m => m.RelatedExceptions, options => options.MapFrom(s => s.RelatedExceptions.Select(e => e.ExceptionId)))
                .ForMember(m => m.RelatedFindings, options => options.MapFrom(s => s.RelatedFindings.Select(e => e.FindingReportId)))
                .ForMember(m => m.RelatedIncidents, options => options.MapFrom(s => s.RelatedIncidents.Select(e => e.IncidentId)))
                .ForMember(m => m.SelectedBusinessRisksCompensatingControls, options => options.MapFrom(s => s.BusinessRisksCompensatingControls.Select(e => e.ControlRequirementId)))
                .ForMember(m => m.SelectedBusinessRisksImpactedControls, options => options.MapFrom(s => s.BusinessRisksImpactedControls.Select(e => e.ControlRequirementId)))
                .ForMember(m => m.SelectedBusinessRisksMonitoringControls, options => options.MapFrom(s => s.BusinessRisksMonitoringControls.Select(e => e.ControlRequirementId)))
                .ForMember(m => m.SelectedBusinessRiskRemediations, options => options.MapFrom(s => s.SelectedBusinessRiskRemediations.Select(e => e.RemediationId)));

            configuration.CreateMap<FindingReportLog, FindingReportLogDto>()
                .ForMember(m => m.New_CorrectiveActionResponse, options => options.MapFrom(s => s.New_CorrectiveActionResponse ?? ""))
                .ForMember(m => m.New_ActualRootCause, options => options.MapFrom(s => s.New_ActualRootCause ?? ""))
                .ForMember(m => m.New_Reference, options => options.MapFrom(s => s.New_Reference ?? ""))
                .ReverseMap();
        }
    }
}
