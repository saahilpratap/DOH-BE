using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.Net.Emailing
{
   public interface IUserCredentialProvider
    {
        string GetDefaultTemplate(int? tenantId);
    }
}
