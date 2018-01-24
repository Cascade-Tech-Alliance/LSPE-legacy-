using System;
using System.Web;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;

namespace LSPE
{
	/// <summary>
	/// Summary description for report_menu.
	/// </summary>
	public partial class reports : CustomPage
	{
		protected System.Web.UI.WebControls.LinkButton LinkButton1;
		protected System.Web.UI.WebControls.LinkButton lnkRun;

        private string ReportServerURL = "http://sqlreports/reportserver/ReportService.asmx";
		private string ReportPath = "/rxpress/plan";
		private string ReportUser = "joe.hamilton";
		private string UserPassword = "";
		private string Domain = "wesd.org";


		private string Format = "PDF";
		private string Zoom = "100";


		public ReportParameter[] reportParametersArray = null;
		public ParameterValue[] values = null;


		//  this is a flag that will help deciding if we'll render the report
		public bool reportCanBeRendered = true;


		// NOTE: The following placeholder declaration is required by the Web Form Designer.
		// Do not delete or move it.
		private System.Object designerPlaceholderDeclaration;
		CustomPrincipal cprPrincipal = (CustomPrincipal)HttpContext.Current.User;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			//hypPlanReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
			hypPlanSummaryReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
			hypServicesReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
			hypProgramsReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
			hypProgramSummariesReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
			hypServicesDescsOnlyReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
			hypProgramTotals.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
            hypProgramTotalsNoTransit.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
            hypRateComparisons.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
            hypSlotAdjustmentsReport.Visible = cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level");
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    

		}
		#endregion

	}
}
