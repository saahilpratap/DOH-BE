using AutoMapper;
using LockthreatCompliance.AuthoritativeDocuments;
using LockthreatCompliance.AuthoritativeDocuments.Dtos;
using LockthreatCompliance.ControlRequirements;
using LockthreatCompliance.ControlRequirements.Dtos;
using LockthreatCompliance.ControlStandards;
using LockthreatCompliance.ControlStandards.Dtos;
using LockthreatCompliance.Domains;
using LockthreatCompliance.Domains.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DtoMappers
{
    public static class AuthoritativeDocumentDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditControlRequirementDto, ControlRequirement>().ReverseMap();
            configuration.CreateMap<ControlRequirementDto, ControlRequirement>().ReverseMap();

            configuration.CreateMap<CreateOrEditControlStandardDto, ControlStandard>().ReverseMap();
            configuration.CreateMap<ControlStandardDto, ControlStandard>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditDomainDto, Domain>().ReverseMap();
            configuration.CreateMap<DomainDto, Domain>().ReverseMap();
            
            configuration.CreateMap<CreateOrEditAuthoritativeDocumentDto, AuthoritativeDocument>().ReverseMap();
            configuration.CreateMap<AuthoritativeDocumentDto, AuthoritativeDocument>().ReverseMap();
        }
    }
}
