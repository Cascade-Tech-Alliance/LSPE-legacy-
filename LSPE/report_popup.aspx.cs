//using System;
//using System.Data;
//using System.Configuration;
//using System.Collections;
//using System.Web;
//using System.Web.Security;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.HtmlControls;

//public partial class report_popup : System.Web.UI.Page
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {

//    }
//}

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using Microsoft.Reporting.WebForms;

namespace LSPE
{
    public partial class report_popup : System.Web.UI.Page
    {

    
        protected void Page_Load(object sender, EventArgs e)
        {   
            string reportName = Request.QueryString["report"];
            if (!Page.IsPostBack)
            {

            
                string reportPath = @"/lspe/" + reportName;
                        
                rpvReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                rpvReportViewer.ServerReport.ReportServerCredentials = new MyReportServerCredentials();
            

                // Get the Report Server endpoint from the config file
                rpvReportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServerEndPoint"]);
                rpvReportViewer.ServerReport.ReportPath = reportPath;

                // Set default parameter values
                if (reportName == "PlanHistorical")
                {
                    if (Session["intDS_ID"] != null && Session["intYR_ID"] != null)
                    {
                        string chvDS_ID = GetCurrentDistrictIDString();
                        if (chvDS_ID != "nothing")
                        {
                            string intDS_ID = Session["intDS_ID"].ToString();
                            string intYR_ID = Session["intYR_ID"].ToString();
                            ReportParameter p1 = new ReportParameter("chvDS_ID", chvDS_ID);
                            ReportParameter p2 = new ReportParameter("intDS_ID", intDS_ID);
                            ReportParameter p3 = new ReportParameter("intYR_ID", intYR_ID);
                            rpvReportViewer.ServerReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                        }
                    }
                    else {
                        string chvDS_ID = GetCurrentDistrictIDString();
                        if (chvDS_ID != "nothing")
                        {
                            ReportParameter p1 = new ReportParameter("chvDS_ID", chvDS_ID);
                            rpvReportViewer.ServerReport.SetParameters(new ReportParameter[] { p1 });
                        }
                    }
                }
                else if (reportName == "ServicesDescsOnly")
                {
                    if (Session["intYR_ID"] != null)
                    {
                        string intYR_ID = Session["intYR_ID"].ToString();
                        ReportParameter p1 = new ReportParameter("intYR_ID", intYR_ID);
                        rpvReportViewer.ServerReport.SetParameters(new ReportParameter[] { p1 });
                    }
                }

            }
            if (reportName == "PlanHistorical")
            { Page.Title = "Plan"; }
            else if (reportName == "PlanSummary")
            { Page.Title = "Plan Summary"; }
            else if (reportName == "Services")
            { Page.Title = "Services"; }
            else if (reportName == "ServicesDescsOnly")
            { Page.Title = "Services (Descriptions)"; }
            else if (reportName == "Programs")
            { Page.Title = "Programs"; }
            else if (reportName == "ProgramsListed")
            { Page.Title = "Programs Listed"; }
            else if (reportName == "ProgramSummaries")
            { Page.Title = "Program Summaries"; }
            else if (reportName == "ProgramsRecommendedOnly")
            { Page.Title = "Programs (Recommended)"; }
            else if (reportName == "ProgramsResolved")
            { Page.Title = "Programs (Resolved)"; }
            else if (reportName == "SlotAdjustmentsByService")
            { Page.Title = "Prior Year Reconciliation By Service"; }
        }

        protected string GetCurrentDistrictIDString()
        { 
            CustomPrincipal cprPrincipal = (CustomPrincipal)HttpContext.Current.User;

            if (cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level"))
            {
                return "%";
            }
            else {
                DataSet ds = new DataSet();
                //SqlConnection sqlConnection;
                string strConnection = (string)Application["strConnection"];
                using (SqlConnection connection = new SqlConnection(strConnection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = new SqlCommand("prSelectDistrictsByUser", connection);
                    adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));
                    adapter.SelectCommand.Parameters["@intUS_ID"].Value = cprPrincipal.UserId;
                    adapter.Fill(ds);
                }

                //sqlConnection = new SqlConnection(strConnection);            
                //adapter.Connection = sqlConnection;
                //sqlCommand.CommandText = "prSelectDistrictsByUser";
                //sqlCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));
                //sqlCommand.Parameters["@intUS_ID"].Value = cprPrincipal.UserId;
                //sqlCommand.CommandType = CommandType.StoredProcedure;
                //sqlCommand.Fill(ds);

                if (ds.Tables.Count > 0){
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        return ds.Tables[0].Rows[0]["DS_ID"].ToString();
                    }
                    else
                    {
                        return "nothing";
                    }
                }
                else
                {
                    return "nothing";
                }
            }
        }

        class MyReportServerCredentials : IReportServerCredentials
        {

            public MyReportServerCredentials()
            {
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;  // Use default identity.
                }
            }

            public System.Net.ICredentials NetworkCredentials
            {
                get
                {
                    return new System.Net.NetworkCredential("lspe_reports", "1rpt!s23", "DWSOUTH");
                }
            }

            public bool GetFormsCredentials(out System.Net.Cookie authCookie,
                out string user, out string password, out string authority)
            {
                authCookie = null;
                user = password = authority = null;
                return false;  // Not use forms credentials to authenticate.
            }
        }
    }
}

