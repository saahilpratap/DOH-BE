#pragma checksum "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eb27cdd9faaec51141d618f8cddcd828a197033f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Ui_Index), @"mvc.1.0.view", @"/Views/Ui/Index.cshtml")]
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
#line 1 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
using Abp.Web.Security.AntiForgery;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
using Microsoft.AspNetCore.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
using LockthreatCompliance.Authorization;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
using LockthreatCompliance.Web.Common;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"eb27cdd9faaec51141d618f8cddcd828a197033f", @"/Views/Ui/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ed8b4c2571f654121e69d7cccf21f7034b81e992", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_Ui_Index : LockthreatCompliance.Web.Views.LockthreatComplianceRazorPage<LockthreatCompliance.Web.Models.Ui.HomePageModel>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("href", "~/view-resources/Views/Ui/Index.css", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("rel", new global::Microsoft.AspNetCore.Html.HtmlString("stylesheet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.LinkTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LinkTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 7 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
  
    AbpAntiForgeryManager.SetCookie(Context, cookieOptions: new CookieOptions()
    {
        SameSite = SameSiteMode.Strict,
        Secure = true
    });

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("head", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "eb27cdd9faaec51141d618f8cddcd828a197033f5322", async() => {
                WriteLiteral("\r\n    <title>LockthreatCompliance</title>\r\n\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("link", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "eb27cdd9faaec51141d618f8cddcd828a197033f5631", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_LinkTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LinkTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LinkTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_LinkTagHelper.Href = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
#nullable restore
#line 17 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_LinkTagHelper.AppendVersion = true;

#line default
#line hidden
#nullable disable
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_LinkTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.HeadTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_HeadTagHelper);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n<div class=\"main-content\">\r\n    <div class=\"user-name\">");
#nullable restore
#line 20 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
                      Write(L("YouAreAlreadyLoggedInWithUser"));

#line default
#line hidden
#nullable disable
            WriteLiteral(" : ");
#nullable restore
#line 20 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
                                                            Write(Html.Raw(Model.GetShownLoginName()));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n    <div>\r\n        <ul class=\"content-list\">\r\n");
#nullable restore
#line 23 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
             if (WebConsts.SwaggerUiEnabled)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li><a");
            BeginWriteAttribute("href", " href=\"", 858, "\"", 893, 1);
#nullable restore
#line 25 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
WriteAttributeValue("", 865, WebConsts.SwaggerUiEndPoint, 865, 28, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Swagger UI</a></li>\r\n");
#nullable restore
#line 26 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 27 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
             if (WebConsts.HangfireDashboardEnabled && IsGranted(AppPermissions.Pages_Administration_HangfireDashboard))
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li><a");
            BeginWriteAttribute("href", " href=\"", 1090, "\"", 1133, 1);
#nullable restore
#line 29 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
WriteAttributeValue("", 1097, WebConsts.HangfireDashboardEndPoint, 1097, 36, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Hangfire</a></li>\r\n");
#nullable restore
#line 30 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
#nullable restore
#line 31 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
             if (WebConsts.GraphQL.Enabled && WebConsts.GraphQL.PlaygroundEnabled)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <li><a");
            BeginWriteAttribute("href", " href=\"", 1290, "\"", 1334, 1);
#nullable restore
#line 33 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
WriteAttributeValue("", 1297, WebConsts.GraphQL.PlaygroundEndPoint, 1297, 37, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">GraphQL Playground</a></li>\r\n");
#nullable restore
#line 34 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </ul>\r\n    </div>\r\n    <div class=\"logout\">\r\n        <a");
            BeginWriteAttribute("href", " href=\"", 1443, "\"", 1504, 1);
#nullable restore
#line 38 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
WriteAttributeValue("", 1450, Url.Action("Logout", "Ui", new {area = string.Empty}), 1450, 54, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 38 "C:\Users\saahi\OneDrive\Desktop\New folder\DOH_Compliance_BackEnd\src\LockthreatCompliance.Web.Host\Views\Ui\Index.cshtml"
                                                                    Write(L("Logout"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</a>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IAbpAntiForgeryManager AbpAntiForgeryManager { get; private set; } = default!;
        #nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<LockthreatCompliance.Web.Models.Ui.HomePageModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
