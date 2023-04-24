using System;
using System.Collections.Generic;
using System.Text;

namespace LockthreatCompliance.AuthoritativeDocuments
{
    public enum ControlType
    {
        Basic,
        Transitional,
        Advanced

    }

    public static class ControlTypeExtensions
    {
        public static bool Includes(this ControlType controlType, ControlType target)
        {
            if (controlType == ControlType.Advanced)
                return true;
            if (controlType == ControlType.Transitional)
                return target == ControlType.Basic || target == ControlType.Transitional;
            if (controlType == ControlType.Basic)
                return target == ControlType.Basic;
            return false;
        }
    }
}
