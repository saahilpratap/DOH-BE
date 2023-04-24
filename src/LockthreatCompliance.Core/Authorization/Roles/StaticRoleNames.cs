namespace LockthreatCompliance.Authorization.Roles
{
    public static class StaticRoleNames
    {
        public static class Host
        {
            public const string Admin = "Admin";
        }

        public static class Tenants
        {
            public const string Admin = "Admin";

            public const string User = "User";

            public struct BusinessEntity
            {
                public const string Admin = "Business Entity Admin";

                public const string InsuranceAdmin = "Insurance Entity Admin";

                public const string User = "Business Entity User";

                public const string Manager = "Business Entity Management";
            }
            public struct ExternalAudit
            {
                public const string Admin = "External Audit Admin";

                public const string User = "External Auditors";

                public const string Manager = "External Auditor Management";
            }

            public struct Insurance 
            {
                public const string Admin = "Insurance Entity Admin";

                public const string User = "Insurance Entity User";

                public const string Manager = "Insurance Entity Management";
            }



        }
    }
}