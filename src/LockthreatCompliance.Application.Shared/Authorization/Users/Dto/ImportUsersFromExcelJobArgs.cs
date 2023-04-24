using System;
using Abp;

namespace LockthreatCompliance.Authorization.Users.Dto
{
    public class ImportUsersFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }

        public string Code { get; set; }
    }
}
