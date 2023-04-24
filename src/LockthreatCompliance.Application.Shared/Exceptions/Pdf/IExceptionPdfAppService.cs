using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LockthreatCompliance.Exceptions.Pdf
{
  public  interface IExceptionPdfAppService: IApplicationService
    {
        Task<String> GenaratePdf();
    }
}
