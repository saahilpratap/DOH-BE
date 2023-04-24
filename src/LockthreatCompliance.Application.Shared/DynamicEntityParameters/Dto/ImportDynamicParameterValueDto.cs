using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Dto
{
   public class ImportDynamicParameterValueDto
    {
        public int? Id { get; set; }
        public string EntityFullName { get; set; }

        public string DynamicParameterName { get; set; }

        public int DynamicParameterId { get; set; }

        public int? TenantId { get; set; }
        public int SrNo { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing Dynamic Parameter Value
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
