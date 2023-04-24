using AutoMapper;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using LockthreatCompliance.BusinessEntities.Exporting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DtoMappers
{
    public static class BusinessEntityDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditBusinessEntityDto, BusinessEntity>()
                .ForMember(m => m.CompanyName, options => options.MapFrom(s => s.Name))
                .ForMember(m => m.CompanyLegalName, options => options.MapFrom(s => s.LegalName))
                .ForMember(m => m.CompanyWebsite, options => options.MapFrom(s => s.WebsiteUrl))
                .ForMember(m => m.CityOrDisctrict, options => options.MapFrom(s => s.CityOrDistrict))
                .ForMember(m => m.CompanyAddress, options => options.MapFrom(s => s.Address))
                .ForMember(m => m.IsAuditableEntity, options => options.MapFrom(s => s.IsAuditable))
                .ForMember(m => m.IsParentReportingEnabled, options => options.MapFrom(s => s.ParentReportingEnabled))
                .ForMember(m => m.IsCompanyLicensed, options => options.MapFrom(s => s.IsLicensed))
                .ForMember(m => m.ConnectivityId, options => options.MapFrom(s => s.ConnectivityId))
                 .ForMember(m => m.ScannerConnectivity, options => options.MapFrom(s => s.ScannerConnectivity))
                .ReverseMap();
            
            
            configuration.CreateMap<BusinessEntityDto, BusinessEntity>().ReverseMap()
                .ForMember(m => m.Name, options => options.MapFrom(s => s.CompanyName));

            configuration.CreateMap<BusinessEntity, BusinessEntitiesExcelDto>()
                .ForMember(m => m.CountryName, options => options.MapFrom(s => s.Country.Name))
               // .ForMember(m => m.BusinessTypeName, options => options.MapFrom(s => s.BusinessType.Name))
                .ForMember(m => m.FacilityTypeName, options => options.MapFrom(s => s.FacilityType == null ? null : s.FacilityType.Name))
                .ForMember(m => m.CityName, options => options.MapFrom(s => s.CityOrDisctrict))
                .ForMember(m=> m.ControlType, options=> options.MapFrom(s=> Enum.GetName(s.ComplianceType.GetType(), s.ComplianceType)))
                .ForMember(m => m.HasParentReporting, options => options.MapFrom(s => s.IsParentReportingEnabled));
        }
    }
}
