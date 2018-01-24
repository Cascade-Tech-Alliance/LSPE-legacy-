
namespace LSPE.UserControls
{
	using System;
	using System.Security.Principal;
	using System.Drawing;
	using System.Web;
	using System.Web.Security;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for menu.
	/// </summary>
	public partial class menu : System.Web.UI.UserControl
	{
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			CreateMenu();
		}


	    private void CreateMenu()
		{
			CustomPrincipal cprPrincipal = HttpContext.Current.User as CustomPrincipal;
			
			CreateMenuItem("Home", "default.aspx", false, false);
			if (cprPrincipal.IsInRole("Administrator"))
			{
				CreateMenuItem("Plans", "plans.aspx", false, false);
				CreateMenuItem("Services", "services.aspx", false, false);
				CreateMenuItem("Users", "users.aspx", false, false);
				CreateMenuItem("Prior Year Reconciliations", "slotadjustments.aspx", false, false);
			}
			if (cprPrincipal.IsInRole("ESD Department Level"))
			{
				CreateMenuItem("Plans", "plans.aspx", false, false);
				CreateMenuItem("Services", "services.aspx", false, false);
			};
			if (cprPrincipal.IsInRole("District Primary"))
			{
				CreateMenuItem("Plans", "plans.aspx", false, false);
			};
			if (cprPrincipal.IsInRole("District Team Member"))
			{
				CreateMenuItem("Plans", "plans.aspx", false, false);
			};
			CreateMenuItem("Reports", "reports.aspx", false, false);
			//CreateMenuItem("FAQ", "faq.aspx", false, false);
			CreateMenuItem("Help", "http://www.wesd.org/lsp", false, true);
			CreateMenuItem("Change Password", "change_password.aspx", false, false);
			CreateMenuItem("Logout", "logout.aspx", true, false);
		}

		private void CreateMenuItem(string strName, string strURL, bool isLastItem, bool isAbsoluteURL)
		{
			HtmlTableCell objCell = new HtmlTableCell();
			HtmlTableCell objSeparatorCell = new HtmlTableCell();
			if ("~/" + strURL == "~" + Parent.Page.Request.Url.AbsolutePath)
			{
				objCell.Attributes.Add("class", "menu_selected");
			}
			else
			{
				objCell.Attributes.Add("class", "menu");
				objCell.Attributes.Add("onmouseover", "className='menu_hover';");
				objCell.Attributes.Add("onmouseout", "className='menu';");
			}
			
			if (!isLastItem)
			{
				objSeparatorCell.InnerHtml = "<SPAN class='menu_separator'>|</SPAN>";
			}
			
			CreateMenuLink(strName, strURL, objCell, isAbsoluteURL);
			tblMenu.Rows[0].Cells.Add(objCell);
			tblMenu.Rows[0].Cells.Add(objSeparatorCell);
		}
		
		private void CreateMenuLink(string strCaption, string strHRef, HtmlTableCell objCell, bool isAbsoluteURL) 
		{
			HyperLink hypMenu = new HyperLink();
			hypMenu.Text = strCaption;
			if (isAbsoluteURL)
			{
				hypMenu.NavigateUrl = strHRef;
				hypMenu.Target = "_blank";
			}
			else
			{
				//hypMenu.NavigateUrl = Request.ApplicationPath.ToString() + '/' + strHRef;
				hypMenu.NavigateUrl = "~/" + strHRef;
			}
			objCell.Controls.Add(hypMenu);
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion
	}
}
