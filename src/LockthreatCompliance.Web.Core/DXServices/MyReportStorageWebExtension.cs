using Abp.UI;
using DevExpress.XtraReports.UI;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AuditProjects.Dtos;
using LockthreatCompliance.Web.DXServices.DataHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LockthreatCompliance.Web.DXServices
{
    /// <summary>
    /// Added By Vaibhav Patil
    /// </summary>
    public class MyReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
      
        public override bool IsValidUrl(string url)
        {
            // Determines whether or not the URL passed to the current Report Storage is valid. 
            // For instance, implement your own logic to prohibit URLs that contain white spaces or some other special characters. 
            // This method is called before the CanSetData and GetData methods.

            //All URLs will be valid as not the same name as index.
            return true;
        }

        public override byte[] GetData(string url)
        {
            string reportName = url.Substring(0, url.IndexOf("?"));
            string paramName = url.Substring(url.IndexOf("?") + 1, url.IndexOf("=") - (url.IndexOf("?") + 1));
            string paramValue = url.Substring(url.IndexOf("=") + 1);
            int ids = Convert.ToInt32(paramValue);
            var checkpath = new ReportFileUploadDto();
            using (MemoryStream ms = new MemoryStream())
            {
                switch (reportName)
                {

                    case "Audit_Project_Consolidated_Report":
                        XtraReport auditPlanReport = new XtraReport();
                        auditPlanReport = new LockthreatCompliance.Web.DXServices.Reports.AuditMainReport();
                        string repJsonAuditPlanReport = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.GetAuditProject2(ids));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)auditPlanReport.DataSource).JsonSource).Json = repJsonAuditPlanReport;
                        auditPlanReport.DataMember = null;
                        auditPlanReport.SaveLayoutToXml(ms);

                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.AUditProjectConsolidate);
                        if (checkpath.FilePath != null)
                        {
                            auditPlanReport.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }
                        break;

                    case "Stage_1_Audit_Finding_Report":

                        XtraReport generateStage1 = new XtraReport();
                        generateStage1 = new LockthreatCompliance.Web.DXServices.Reports.NewFindingsReportStage1();
                        string repJsonPGenerateStage1 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.FindingReportStageWise(ids, FindingReports.FindingReportCategory.Stage1));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)generateStage1.DataSource).JsonSource).Json = repJsonPGenerateStage1;
                        generateStage1.DataMember = null;
                        generateStage1.SaveLayoutToXml(ms);

                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.AuditFinding_1);
                        if (checkpath.FilePath != null)
                        {
                            generateStage1.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;
                    case "Stage_2_Audit_Finding_Report": 

                        XtraReport generateStage2 = new XtraReport();
                        generateStage2 = new LockthreatCompliance.Web.DXServices.Reports.NewFindingsReportStage2();
                        string repJsonPGenerateStage2 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.FindingReportStageWise(ids, FindingReports.FindingReportCategory.Stage2));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)generateStage2.DataSource).JsonSource).Json = repJsonPGenerateStage2;
                        generateStage2.DataMember = null;
                        generateStage2.SaveLayoutToXml(ms);

                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.AuditFinding_2);
                        if (checkpath.FilePath != null)
                        {
                            generateStage2.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;

                    case "Stage_1_CAPA_Report":

                        XtraReport generateUpdateStage1 = new XtraReport();
                        generateUpdateStage1 = new LockthreatCompliance.Web.DXServices.Reports.CorrectiveActionPlanStage1();
                        string repJsonPGenerateUpdateStage1 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.CorrectiveActionReportStageWise(ids, FindingReports.FindingReportCategory.Stage1));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)generateUpdateStage1.DataSource).JsonSource).Json = repJsonPGenerateUpdateStage1;
                        generateUpdateStage1.DataMember = null;
                        generateUpdateStage1.SaveLayoutToXml(ms);
                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.CAPA_1);
                        if (checkpath.FilePath != null)
                        {
                            generateUpdateStage1.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;
                    case "Stage_2_CAPA_Report":

                        XtraReport generateUpdateStage2 = new XtraReport();
                        generateUpdateStage2 = new LockthreatCompliance.Web.DXServices.Reports.CorrectiveActionPlanStage2();
                        string repJsonPGenerateUpdateStage2 = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.CorrectiveActionReportStageWise(ids, FindingReports.FindingReportCategory.Stage2));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)generateUpdateStage2.DataSource).JsonSource).Json = repJsonPGenerateUpdateStage2;
                        generateUpdateStage2.DataMember = null;
                        generateUpdateStage2.SaveLayoutToXml(ms);
                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.CAPA_2);
                        if (checkpath.FilePath != null)
                        {
                            generateUpdateStage2.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;


                    case "Audit_Project_Certification_Report":
                        XtraReport reportCertification = new LockthreatCompliance.Web.DXServices.Reports.CertificationProposalReport();

                        string repJsonCertification = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.CertificationProposalReport(ids));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)reportCertification.DataSource).JsonSource).Json = repJsonCertification;
                        reportCertification.DataMember = null;
                        reportCertification.SaveLayoutToXml(ms);

                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.Certificate);
                        if (checkpath.FilePath != null)
                        {
                            reportCertification.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;



                    case "AUDITLOG":
                        XtraReport report = new LockthreatCompliance.Web.DXServices.Reports.AuditLogRpt();

                        string repJsonP = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.getauditLogMaster());

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)report.DataSource).JsonSource).Json = repJsonP;
                        report.DataMember = null;
                        report.SaveLayoutToXml(ms);

                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.CAPA_1);
                        if (checkpath.FilePath != null)
                        {
                            report.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;


                        



                    case "TEST":
                        XtraReport reporTest = new LockthreatCompliance.Web.DXServices.Reports.CorrectiveActionPlanStage1();
                        string repJsonPTest = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.CorrectiveActionReportStageWise(ids, FindingReports.FindingReportCategory.Stage1));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)reporTest.DataSource).JsonSource).Json = repJsonPTest;
                        reporTest.DataMember = null;
                        reporTest.SaveLayoutToXml(ms);
                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.CertificationProposal);

                        if (checkpath.FilePath != null)
                        {
                            reporTest.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;
                    case "Stage_1_and_Stage_2_Finding_Report":
                        XtraReport reporTestNew = new LockthreatCompliance.Web.DXServices.Reports.FindingReportAllStageWiseNew();
                        string repJsonPTestNew = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.FindingReportAllStageWise(ids));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)reporTestNew.DataSource).JsonSource).Json = repJsonPTestNew;
                        reporTestNew.DataMember = null;
                        reporTestNew.SaveLayoutToXml(ms);
                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.Stage_1_And_Stage_2_Finding);

                        if (checkpath.FilePath != null)
                        {
                            reporTestNew.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;

                    case "Capa_1_and_Capa_2_Report":
                        XtraReport capareport  = new LockthreatCompliance.Web.DXServices.Reports.CorrectiveActionPlan_Stage_1_and_Stage_2();
                        string capareportnew  = JsonConvert.SerializeObject(
                                                    ReportDataHandlerBase.CorrectiveActionReportStageAll(ids));

                        ((DevExpress.DataAccess.Json.CustomJsonSource)((DevExpress.DataAccess.Json.JsonDataSource)capareport.DataSource).JsonSource).Json = capareportnew;
                        capareport.DataMember = null;
                        capareport.SaveLayoutToXml(ms);
                        checkpath = ReportDataHandlerBase.Checkpath(ids, reportName, ReportTypes.Capa_1_And_Capa_2);

                        if (checkpath.FilePath != null)
                        {
                            capareport.ExportToPdf(checkpath.FilePath);
                        }
                        else
                        {
                            throw new UserFriendlyException("File Path Doe's not Exit");
                        }

                        break;

                    default:
                        break;
                }
                return ms.ToArray();
            }
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            base.SetData(report, url);
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            return base.SetNewData(report, defaultUrl);
        }
    }
}
