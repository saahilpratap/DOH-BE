using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using LockthreatCompliance.Dto;

namespace LockthreatCompliance.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
