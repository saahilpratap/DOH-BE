﻿using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using LockthreatCompliance.MultiTenancy.Accounting.Dto;

namespace LockthreatCompliance.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
