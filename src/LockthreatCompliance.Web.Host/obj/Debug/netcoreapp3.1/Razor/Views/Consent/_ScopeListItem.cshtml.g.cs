#pragma checksum "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6c409d2d2b3e65d4066cd86b9206bf61beb67e5d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Consent__ScopeListItem), @"mvc.1.0.view", @"/Views/Consent/_ScopeListItem.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\_ViewImports.cshtml"
using Abp.Localization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 1 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
using LockthreatCompliance.Web.Models.Consent;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6c409d2d2b3e65d4066cd86b9206bf61beb67e5d", @"/Views/Consent/_ScopeListItem.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ed8b4c2571f654121e69d7cccf21f7034b81e992", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Consent__ScopeListItem : LockthreatCompliance.Web.Views.LockthreatComplianceRazorPage<ScopeViewModel>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<li class=\"list-group-item\">\r\n    <label>\r\n        <input type=\"checkbox\"\r\n               name=\"ScopesConsented\"");
            BeginWriteAttribute("id", "\r\n               id=\"", 183, "\"", 222, 2);
            WriteAttributeValue("", 204, "scopes_", 204, 7, true);
#nullable restore
#line 7 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
WriteAttributeValue("", 211, Model.Name, 211, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("value", "\r\n               value=\"", 223, "\"", 258, 1);
#nullable restore
#line 8 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
WriteAttributeValue("", 247, Model.Name, 247, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("checked", "\r\n               checked=\"", 259, "\"", 299, 1);
#nullable restore
#line 9 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
WriteAttributeValue("", 285, Model.Checked, 285, 14, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            BeginWriteAttribute("disabled", "\r\n               disabled=\"", 300, "\"", 342, 1);
#nullable restore
#line 10 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
WriteAttributeValue("", 327, Model.Required, 327, 15, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n\r\n");
#nullable restore
#line 12 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
         if (Model.Required)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <input type=\"hidden\"\r\n                   name=\"ScopesConsented\"");
            BeginWriteAttribute("value", "\r\n                   value=\"", 466, "\"", 505, 1);
#nullable restore
#line 16 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
WriteAttributeValue("", 494, Model.Name, 494, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" />\r\n");
#nullable restore
#line 17 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        <strong>&nbsp;");
#nullable restore
#line 19 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
                 Write(L(Model.DisplayName));

#line default
#line hidden
#nullable disable
            WriteLiteral("</strong>\r\n\r\n");
#nullable restore
#line 21 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
         if (Model.Emphasize)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <span class=\"glyphicon glyphicon-exclamation-sign\"></span>\r\n");
#nullable restore
#line 24 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </label>\r\n\r\n");
#nullable restore
#line 28 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
     if (Model.Required)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <span><em>(");
#nullable restore
#line 30 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
              Write(L("Required"));

#line default
#line hidden
#nullable disable
            WriteLiteral(")</em></span>\r\n");
#nullable restore
#line 31 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 33 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
     if (Model.Description != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div>\r\n            <label");
            BeginWriteAttribute("for", " for=\"", 890, "\"", 914, 2);
            WriteAttributeValue("", 896, "scopes_", 896, 7, true);
#nullable restore
#line 36 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
WriteAttributeValue("", 903, Model.Name, 903, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 36 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
                                       Write(L(Model.Description));

#line default
#line hidden
#nullable disable
            WriteLiteral("</label>\r\n        </div>\r\n");
#nullable restore
#line 38 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Consent\_ScopeListItem.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<ScopeViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
