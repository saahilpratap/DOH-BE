using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using LockthreatCompliance.Dto;
using LockthreatCompliance.Exceptions;
using LockthreatCompliance.Exceptions.Dtos;
using LockthreatCompliance.Incidents;
using Microsoft.AspNetCore.Mvc;
using LockthreatCompliance.Incidents.Dtos;
using LockthreatCompliance.AuditProjects;
using LockthreatCompliance.AssessmentSchedules;
using LockthreatCompliance.AuditReports;
using LockthreatCompliance.BusinessEntities;
using LockthreatCompliance.BusinessEntities.Dtos;
using Abp.Application.Services.Dto;

namespace LockthreatCompliance.Web.Controllers
{

    public class ExceptionController : LockthreatComplianceControllerBase
    {
        private IConverter _converter;
        private readonly IExceptionsAppService _iExceptionsAppServiceRepository;
        private readonly IIncidentsAppService _incidentsAppServiceRepository;
        private readonly IAuditProjectAppService _auditProjectAppService;
        private readonly IExtAssementScheduleAppService _extAssementScheduleAppService;
        private readonly IAuditReportAppService _auditReportAppService;
        private readonly IBusinessEntitiesAppService _businessEntitiesAppService;

        public ExceptionController(IConverter converter, IIncidentsAppService incidentsAppServiceRepository,
            IExceptionsAppService iExceptionsAppServiceRepository, IAuditProjectAppService auditProjectAppService,
            IExtAssementScheduleAppService extAssementScheduleAppService, IAuditReportAppService auditReportAppService,
            IBusinessEntitiesAppService businessEntitiesAppService)
        {
            _iExceptionsAppServiceRepository = iExceptionsAppServiceRepository;
            _incidentsAppServiceRepository = incidentsAppServiceRepository;
            _auditProjectAppService = auditProjectAppService;
            _extAssementScheduleAppService = extAssementScheduleAppService;
            _auditReportAppService = auditReportAppService;
            _businessEntitiesAppService = businessEntitiesAppService;
            _converter = converter;
        }


        [HttpGet]
        public async Task<FileResult> CreatePDF()
        {

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A6,
                Margins = new MarginSettings() { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Exception Details",

            };
            var pages = new List<string>();
            await GetSummaryHTMLString(pages);
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
            };

            foreach (var item in pages)
            {
                var objectSettings = new ObjectSettings
                {

                    PagesCount = true,
                    HtmlContent = item,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "AAMEN Programme Portal" }
                };
                pdf.Objects.Add(objectSettings);
            }
            try
            {
                var file = _converter.Convert(pdf);
                return File(file, "application/pdf");

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }

        private async Task<List<string>> GetSummaryHTMLString(List<string> d)
        {
            var employees = new List<ExceptionDto>();

            employees = await _iExceptionsAppServiceRepository.GetAllException();
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Exception Details!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>ID</th>
                                        <th>Title</th>
                                        <th>Business Entity Name</th>
                                        <th>ReviewStatus</th>
                                       
                                    </tr>");
            foreach (var emp in employees)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>                                
                                  </tr>", emp.Code, emp.Title, emp.BusinessEntityName, emp.ReviewStatus);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            d.Add(sb.ToString());
            return d;
        }


        [HttpGet]
        public async Task<FileResult> IncidentPdf()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A6,
                Margins = new MarginSettings() { Top = 5, Bottom = 5, Left = 5, Right = 5 },
                DocumentTitle = "Incident",

            };
            var pages = new List<string>();
            await GetIncidentHTMLString(pages);
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
            };

            foreach (var item in pages)
            {
                var objectSettings = new ObjectSettings
                {

                    PagesCount = true,
                    HtmlContent = item,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Emirsec Technologies" }
                };
                pdf.Objects.Add(objectSettings);
            }
            try
            {
                var file = _converter.Convert(pdf);
                return File(file, "application/pdf");

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }


        private async Task<List<string>> GetIncidentHTMLString(List<string> d)
        {
            var incidents = new List<IncidentDto>();

            incidents = await _incidentsAppServiceRepository.GetIncidntPdf();
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Incident!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>ID</th>
                                        <th>Title</th>
                                        <th>Business Entity Name</th>
                                        <th>Status</th>
                                       
                                    </tr>");
            foreach (var emp in incidents)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>                                
                                  </tr>", emp.Code, emp.Title, emp.BusinessEntityName, emp.Status);
            }
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            d.Add(sb.ToString());
            return d;
        }

        [HttpGet]
        public async Task<FileResult> AuditProjectPdf(int Id)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A6,
                Margins = new MarginSettings() { Top = 5, Bottom = 5, Left = 2, Right = 2 },
                DocumentTitle = "Audit Project",

            };
            var pages = new List<string>();
            await GetAuditProjectHTMLString(pages, Id);
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
            };

            foreach (var item in pages)
            {
                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = item,
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "reportBody.css") },
                };
                pdf.Objects.Add(objectSettings);
            }
            try
            {
                var file = _converter.Convert(pdf);
                return File(file, "application/pdf");

            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }


        private async Task<List<string>> GetAuditProjectHTMLString(List<string> d, long auditProjectId)
        {
            var documentTypes = await _extAssementScheduleAppService.GetAssessmentTypes();
            var oldAuditProject = await _auditProjectAppService.GetAuditProjectForEdit(auditProjectId);
            var auditProjectInfo = await _auditProjectAppService.AuditProjectPdfById(auditProjectId);
            var auditReport = await _auditReportAppService.GetAuditReportInfoByAuditProjectId(auditProjectId);
            var facilityNames = await _auditProjectAppService.GetAuditProjectGroup(auditProjectId);

            var facilityCount = facilityNames.BusinessEntity.Count();

            var auditTeamStateInfo = auditReport.AuditReportTeamStageList;
            var stage1Info = new List<string>();
            var stage2Info = auditTeamStateInfo.GroupBy(x => x.DominName).Select(x => new { name = x, list = x.ToList() }).ToList();

            var totalStage2Count = stage2Info.Sum(x => x.list.Count());

            stage2Info.ForEach(x =>
            {
                var templist = x.list.Select(y => y.ControlRequirement).ToList();
                templist.ForEach(y =>
                {
                    stage1Info.Add(y);
                });
            });

            var sb = new StringBuilder();
            sb.Append(@"<html> <head> </head> <body><style>body{font-family:'Times New Roman';}</style>");

            sb.Append(@"<div id='wrapper' #PageOne>
                            <div id='content'>
                              <div style='font-style: italic; margin: 0;'>
                                <h1 style='color: darkblue; margin: 0; padding: 0;'>
                                  ADHICS REPORT N. XYZ123
                                </h1>
                              </div>
                              <div style='width: 100%; display: flex;'>
                                <table style='border-collapse: collapse; width: 100%;' border='1'>
                                  <tr>
                                    <th>DoH License - Facility Name/ Group Name</th>
                                    <td>Assessment dates</td>
                                    <td>Bob</td>
                                  </tr>
                                  <tr>
                                    <th rowspan='2'>Assessment dates</th>
                                    <td>Stage 1 Audit</td>
                                   <td>From: " +((DateTime)auditProjectInfo.auditProjectDto.StartDate).ToString("dd-MM-yyyy") + " To: " +((DateTime) auditProjectInfo.auditProjectDto.EndDate).ToString("dd-MM-yyyy"));
            sb.Append(@"</td>
                                  </tr>
                                  <tr>
                                    <td>Stage 2 Audit</td>
                                    <td>From: " + ((DateTime)auditProjectInfo.auditProjectDto.StageStartDate).ToString("dd-MM-yyyy") + " To: " + ((DateTime)auditProjectInfo.auditProjectDto.StageEndDate).ToString("dd-MM-yyyy"));
            sb.Append(@"</td>
                                  </tr>
                                  <tr>
                                    <th rowspan='4'>Audit Type, Scope and Criteria</th>
                                    <td colspan='2'>");

            documentTypes.ForEach(item =>
            {
                var checkedString = "";
                if (oldAuditProject.AssessmentTypeId == item.Id)
                {
                    checkedString = "checked";
                }
                sb.Append(@"<input type='checkbox' " + checkedString + ">");
                sb.Append(@"<label> " + item.Name + "</label><br/>");
            });

            //ם Certification ם Recertification ם Surveillance ם Extra
            sb.Append(@"</td>
                                  </tr>
                                  <tr>
                                    <th colspan='2'>
                                      Type of facilities or entities in the scope of audit:
                                    </th>
                                  </tr>
                                  <tr>
                                    <td colspan='2'>
                                      Facilities in the Group (see Annex)<br />");
            facilityNames.BusinessEntity.ForEach(x =>
            {
                sb.Append(@"" + x.Name + "<br />");
            });

            sb.Append(@" </td><br />
                                    </td>
                                  </tr>
                                  <tr>
                                    <td colspan='2'>Exclusions</td>
                                  </tr>");

            if (facilityNames.BusinessEntity.Count()>0)
            {
                sb.Append(@"<tr><th rowspan='" + facilityNames.BusinessEntity.Count() + "'>");
                sb.Append(@"Facilities for onsite audit and Location Details");
                sb.Append(@"</th><td colspan='2'>" + facilityNames.BusinessEntity[0].Name + "</td></tr>");

                for (int i = 1; i < facilityNames.BusinessEntity.Count(); i++)
                {
                    if (i != facilityCount)                   
                        sb.Append(@"" + "<tr><td colspan='2'>" + facilityNames.BusinessEntity[i].Name + "</td>");                   
                    else
                        sb.Append(@"" + "<tr><td></td><td colspan='2'>" + facilityNames.BusinessEntity[i].Name + "</td>");
                }
            }
            else
            {
                sb.Append(@"<tr><th rowspan='" + facilityNames.BusinessEntity.Count() + "'>");
                sb.Append(@"Facilities for onsite audit and Location Details");
                sb.Append(@"</th><td colspan='2'></td></tr>");
            }

           

            sb.Append(@"<tr>
                                    <th rowspan='6'>
                                      Facility/TPAs Audit Management Representative/s
                                    </th>
                                    <th>Name</th>
                                    <th>Positions</th>
                                  </tr>");
            for (int i = 0; i < 5; i++)
                sb.Append(@"<tr><td style='height:25px;'></td><td></td></tr>");
            sb.Append(@"<tr>
                                    <th>Number of Auditors:</th>
                                    <td colspan='2'></td>
                                  </tr>
                                  <tr>
                                    <th>Audit Methodology</th>
                                    <td colspan='2'><input type='checkbox'> Onsite <input type='checkbox'> Remote Desktop</td>
                                  </tr>
                                  <tr>
                                    <th>Lead Auditor:</th>
                                    <td colspan='2'></td>
                                  </tr>
                                </table>
                              </div>
                            </div>
                          </div>");

            sb.Append(@"<div id='content' style='margin-top:15px;' #PageTwo>
                          <div style='display: flex;'>
                            <div style='border: 1px solid black; width: 100%;'>
                              <div style='background-color: #3d4692;'>
                                <h3 style='color: white; text-align: left; margin: 0; padding: 8px;'>Audit conclusions</h3>
                              </div>
                              <div style='background-color: white;margin: 0;color: black;padding: 10px 5px;'>
                                <p style='text-align: justify'>
                                  The audit procedures rely on information and representations made available to Audit team of TRBA by the concerned Management of audited facility. Our audit procedures comprise inquiries, identify non-compliant on a sample basis, as per the scope.
                                  Accordingly, the audit procedures may not detect noncompliance.
                                  TRBA Audit review does not in any way diminish the responsibilities of the Management. The development, implementation, awareness of the management system for compliance to ADHICS, remains responsibility of the Facility or
                                  Third-Party management.
                                </p>
                                <p style='text-align: justify'>
                                  Scores and Grades are indicative of a level of compliance, but due to the sampling method, they cannot be utilized for claiming full or partial compliance to ADHICS.
                                </p>
                                <p>
                                <h4>Closure of previous findings: " + auditReport.AuditReport.ClosureFinding + " </h4></p>");
            sb.Append(@"<p>
                                <h4>Graph of major risks detected: " + "" + " </h4></p>");
            sb.Append(@"<p>
                                <h4> Areas of improvements: " + auditReport.AuditReport.AreaImprovement + " </h4></p>");
            sb.Append(@"<p>
                                <h4> Recommendations: " + auditReport.AuditReport.Recommendation + " </h4></p>");

            var performance1Yes = auditReport.AuditReport.Performance1 == "Yes" ? "checked" : "";
            var performance1No = auditReport.AuditReport.Performance1 == "No" ? "checked" : "";
            var performance2Yes = auditReport.AuditReport.Performance2 == "Yes" ? "checked" : "";
            var performance2No = auditReport.AuditReport.Performance2 == "No" ? "checked" : "";

            sb.Append(@"<p>Does the AUDIT team confirm that the AUDIT objectives have been achieved? YES <input type='radio' " + performance1Yes + "> NO <input type='radio' " + performance1No + "></p>");
            sb.Append(@"<p> In the AUDIT team’s judgement, is the certification scope adequate? YES <input type='radio' " + performance2Yes + "> NO <input type='radio' " + performance2No + "> </p>");
            sb.Append(@"<p style='text-align: justify'>
                                  On the basis of the documentation examined and the audits performed, we consider that the Management system is compliant with the reference standard.
                                </p>
                                <p style='text-align: justify'> We therefore propose to issue/confirm/restate/suspend/revoke the Certificate of Compliance as established by the Rules.
                                </p>
                                <p> Next audit will be performed within 12 months.
                                </p>
                                <br>
                                <br>
                                <br>
                              </div>
                            </div>
                          </div>
                        </div>");

            sb.Append(@"  <div id='wrapper' #ThirdPage>
                            <div id='content' style='margin-top:15px;'>
                              <div>
                                <div style='border: 1px solid black; width: 100%;'>
                                  <div style='background-color: #363a57;'>
                                    <h3 style='color: white; text-align: center; margin: 0; padding: 0;'>
                                      STAGE 1
                                    </h3>
                                  </div>
                                  <div style='text-align: center;margin: 5px auto 10px auto;'>
                                    <h style='font-weight: 600;'>
                                      Summary of results
                                    </h>
                                  </div>
                                  <div style='width: 100%;'>
                                    <table border='1' style='border-collapse: collapse;width: 100%;'>
                                      <tr style='background-color: #3d4692;color: white;width: 100%;'>
                                        <td style='white-space: pre-line;font-weight: 600;width: 70%;'>Check List</td>
                                        <td style='white-space: pre-line;font-weight: 600;width: 15%;'>Individual</td>
                                        <td style='white-space: pre-line;font-weight: 600;width: 15%;'>Group</td>
                                      </tr>");

            stage1Info.ForEach(y =>
            {
                sb.Append(@"<tr><td style='background-color: lightgray;'>" + y + "</td><td>Y</td><td>N</td></tr>");
            });

            sb.Append(@"</table>
                                  </div>

                                  <div style='background-color: #363a57;margin-top:20px;'>
                                    <h3 style='color: white; text-align: center; margin: 0; padding: 0;'>
                                      STAGE 2
                                    </h3>
                                  </div>
                                  <div style='text-align: center;margin: 5px auto 10px auto;'>
                                    <h style='font-weight: 600;'>
                                      Summary of results
                                    </h>
                                  </div>
                                  <div style='width: 100%;'>
                                    <table border='1' style='border-collapse: collapse;width: 100%;'>
                                      <tr style='background-color: #3d4692;color: white;'>
                                        <td style='white-space: pre-line;font-weight: 600;' colspan='2'>
                                          Checklist 2:
                                          Compliance by Domains in the corporate or individual
                                        </td>
                                        <td style='white-space: pre-line;font-weight: 600;'>Checklist 2: Compliance by Domains in the group
                                        </td>
                                        <td style='white-space: pre-line;font-weight: 600;'>Auditors main findings</td>
                                      </tr>");

            stage2Info.ForEach(x =>
            {
                sb.Append(@"<tr><td style='background-color: lightgray; width:80%;'>" + x.name.Key + "</td>");
                var val1 = x.list.Count();
                var tempPer = (val1 * 100) / totalStage2Count;
                var percentageValue = Math.Round((Double)(tempPer), 2);
                sb.Append(@"<td style='width:5%;'>" + percentageValue + "% </td>");
                sb.Append(@"<td style='width:5%;'>" + percentageValue + "% </td>");
                sb.Append(@"<td style='width:10%;'></td></tr>");
            });

            sb.Append(@"
                                    </table>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>");

            sb.Append(@"<div id='wrapper' #FourPage>
                            <div id='content' style='margin-top:15px;'>
                              <div>
                                <div style='border: 1px solid black; width: 100%;'>
                                  <div style='background-color: #3d4692; margin: -1px;'>
                                    <h3 style='color: white; text-align: left; margin: 0; padding: 8px;'>
                                      Approach of Assessment
                                    </h3>
                                  </div>
                                  <div style='background-color: white; margin: 0; color: black; padding: 10px 5px;'>
                                    <p style='margin-bottom:0;'>
                                    <h3 style='margin: 0;'> Before the Audit:</h3>
                                    <ul style='list-style: square;margin: 0;'>
                                      <li>Request updates of General and Technical Information</li>
                                      <li>Sending the audit plan</li>
                                    </ul>
                                    </p>

                                    <p style='margin-bottom:0;'>
                                    <h3 style='margin: 0;'> During the Audit:</h3>
                                    <ul style='list-style: square;margin: 0;'>
                                      <li>Audit by desktop, site visit or remote access</li>
                                      <li>Obtaining information on your processes</li>
                                      <li>Observing implemented procedures and evaluating records</li>
                                    </ul>
                                    </p>

                                    <p style='margin-bottom:0;'>
                                    <h3 style='margin: 0;'> After the Audit:</h3>
                                    <ul style='list-style: square;margin: 0;'>
                                      <li>Reporting compliance and non-compliance</li>
                                      <li>Finalizing audit reports for stage 1 and stage 2</li>
                                      <li>Receiving corrective and action plan from the Auditee</li>
                                      <li>Sending proposal for certification or recertification to the Decision Making or Technical Committee
                                      </li>
                                      <li>Issue of certificate (if pass) or reissue</li>
                                    </ul>
                                    </p>
                                  </div>

                                  <div style='background-color: #3d4692; margin: 10px -1px; border: 1px solid black;'>
                                    <h3 style='color: white; text-align: left; margin: 0; padding: 8px;'>
                                      ACTION PLAN FOR CORRECTIVE ACTIONS
                                    </h3>
                                  </div>
                                  <div style=' background-color: white; margin: 0; color: black; padding: 0px 5px;'>
                                    <p style='margin-top: 0;'>
                                      It is important to understand the non-compliance and the Facility/Provider shall act to eliminate the
                                      causes and to prevent any recurrence. Corrective actions shall be appropriate to the effects of the
                                      noncompliance encountered.
                                    </p>
                                    <p style='margin: 0;'>
                                      An action plan must be developed as explained below:
                                    <ul style='list-style:lower-alpha;padding: 0 30px;margin: 0;'>
                                      <li>Reviewing of noncompliance</li>
                                      <li>Determining the causes of noncompliance</li>
                                      <li>Evaluating the need for action to ensure that noncompliance does not recur </li>
                                      <li>Determining and implementing action needed</li>
                                      <li>Records of the results of action taken and</li>
                                      <li>Reviewing the effectiveness of the corrective action taken</li>
                                    </ul>
                                    </p>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>");

            sb.Append(@"<div id='wrapper' #FifthPage>
                            <div id='content' style='margin-top:15px;'>
                              <div>
                                <div style='border: 1px solid black; width: 100%;'>
                                  <div style='background-color: #3d4692; margin: -1px;'>
                                    <h3 style='color: white; text-align: left; margin: 0; padding: 8px;'>
                                      ACKNOWLEDGEMENT OF THE REPORT
                                    </h3>
                                  </div>
                                  <div style='display: flex'>
                                    <table border='1' style='border-collapse: collapse;width: 100%;'>
                                      <tr style='background-color: lightgray;color: #000000;'>
                                        <td style='white-space: pre-line;font-weight: 600;'>Prepared by:
                                        </td>
                                        <td style='white-space: pre-line;font-weight: 600;'>Reviewed by:</td>
                                        <td style='white-space: pre-line;font-weight: 600;line-height: 0.8;'>Acknowledged by:<br>
                                          Facility Head/Representative
                                        </td>
                                      </tr>
                                      <tr style='height: 50px !important;'>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                      </tr>
                                    </table>
                                  </div>
                                </div>
                              </div>
                              <div>
                                <div style='width:100%;background-color: #3d4692; margin: 10px -1px; border: 1px solid black;'>
                                  <h3 style='color: white; text-align: left; margin: 0; padding: 8px;'>
                                    Annex List of facilities in the Group
                                  </h3>
                                </div>
                              </div>
                            </div>
                          </div>");

            sb.Append(@"<div id='wrapper'>
                            <div id='content' #sixthPage>
                              <div  style='margin-top:15px;'>
                                <div style='border: 1px solid black; width: 100%;'>
                                  <div>
                                    <h3 style='text-align: left; margin: 0; padding: 4px;'>
                                      Detailed Findings:
                                    </h3>
                                  </div>
                                  <div style='display: flex'>
                                    <div style='display: flex;'>
                                      <ol type='a'>
                                        <li style='color: #4F81BD;'><u><strong>Annex checklist 1 </strong></u></li>
                                        <li style='color: #4F81BD;'><u><strong>Annex checklist 2 </strong></u></li>
                                      </ol>
                                    </div>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>");

            sb.Append(@"</body> </html>");
            d.Add(sb.ToString());
            return d;
        }
    }


}