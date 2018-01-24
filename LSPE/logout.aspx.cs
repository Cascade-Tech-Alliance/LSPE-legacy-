using System;
using System.Web.Security;

namespace LSPE
{
	/// <summary>
	/// Summary description for logout.
	/// </summary>
	public partial class logout : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			FormsAuthentication.SignOut();
			Session.Abandon();
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
