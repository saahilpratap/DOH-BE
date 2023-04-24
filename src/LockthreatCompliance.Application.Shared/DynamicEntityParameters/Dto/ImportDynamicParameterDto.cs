using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.DynamicEntityParameters.Dto
{
   public class ImportDynamicParameterDto
    {
        public string ParameterName { get; set; }

        public string InputType { get; set; }

        public string Permission { get; set; }

        public int? TenantId { get; set; }

        /// <summary>
        /// Can be set when reading data from excel or when importing Dynamic Parameter
        /// </summary>
        public string Exception { get; set; }

        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(Exception);
        }
    }
}
