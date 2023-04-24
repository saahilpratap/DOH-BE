using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace LockthreatCompliance.ExceptionTypes.Dtos
{
    public class GetExceptionTypeForEditOutput
    {
		public CreateOrEditExceptionTypeDto ExceptionType { get; set; }


    }
}