using System.Threading.Tasks;
using Abp.Dependency;

namespace LockthreatCompliance.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}