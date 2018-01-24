using System;
using System.Collections;
using System.Drawing;
using System.Web.UI.WebControls;

namespace LSPE.UserControls
{
    /// <summary>
	///		Summary description for TabStrip.
	/// </summary>
	public partial class TabStrip : System.Web.UI.UserControl
	{

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindData();
			}
		}

		private void BindData()
		{
			rptTabStrip.DataSource = Tabs;
			rptTabStrip.DataBind();
		}

		public Color SetBackColor(object elem)
		{
			RepeaterItem item = (RepeaterItem) elem;
			if (item.ItemIndex == CurrentTabIndex)
				return SelectedBackColor;
			return BackColor;
		}

		public Color SetForeColor(object elem)
		{
			RepeaterItem item = (RepeaterItem) elem;
			if (item.ItemIndex == CurrentTabIndex)
				return SelectedForeColor;
			return ForeColor;
		}

		public Color SetBorderColor(object elem)
		{
			RepeaterItem item = (RepeaterItem) elem;
			if (item.ItemIndex == CurrentTabIndex)
				//return SelectedBackColor;
				return Color.Black;
			return Color.FromArgb(226, 213, 194);
		}

		public void ItemCommand(object sender, RepeaterCommandEventArgs e)
		{
			Select(e.Item.ItemIndex);
		}

		public void ItemCreated(object sender, RepeaterItemEventArgs e)
		{
			//
		}

		// Tabs to display
		public ArrayList Tabs = new ArrayList();

		// Current Tab
		public int CurrentTabIndex
		{
			get {return (int) ViewState["CurrentTabIndex"];}
		}
		
		// CssClass
		public string CssClass
		{
			get {return (string) ViewState["CssClass"];}
			set {ViewState["CssClass"] = value;}
		}	

		// Background color
		public Color BackColor
		{
			get {return (Color) ViewState["BackColor"];}
			set {ViewState["BackColor"] = value;}
		}

		// Selected background color
		public Color SelectedBackColor
		{
			get {return (Color) ViewState["SelectedBackColor"];}
			set {ViewState["SelectedBackColor"] = value;}
		}

		// Foreground color
		public Color ForeColor
		{
			get {return (Color) ViewState["ForeColor"];}
			set {ViewState["ForeColor"] = value;}
		}

		// Selected foreground color
		public Color SelectedForeColor
		{
			get {return (Color) ViewState["SelectedForeColor"];}
			set {ViewState["SelectedForeColor"] = value;}
		}

		// Select method
		public void Select(int index) 
		{
			// Ensure the index is a valid value
			if (index <0 || index >=Tabs.Count)
				index = 0;

			// Updates the current index. Must write to the view state
			// because the CurrentTabIndex property is read-only
			ViewState["CurrentTabIndex"] = index;

			// Refresh the UI
			BindData();

			// Fire the event to the client
			SelectionChangedEventArgs ev = new SelectionChangedEventArgs();
			ev.Position = CurrentTabIndex;
			OnSelectionChanged(ev);
		}

		// Custom event class
		public class SelectionChangedEventArgs : EventArgs
		{
			public int Position;
		}

		public delegate void SelectionChangedEventHandler(object sender, SelectionChangedEventArgs e);
		public event SelectionChangedEventHandler SelectionChanged;

		// Helper function that fires the event by executing user-defined code
		protected virtual void OnSelectionChanged(SelectionChangedEventArgs e)
		{
			// SelectionChanged is the event property
			if (SelectionChanged != null)
				SelectionChanged(this, e);
		}
		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			
			if (ViewState["CssClass"] == null)
				CssClass = "tabstrip_normal";
			if (ViewState["SelectedBackColor"] == null)
				//SelectedBackColor = Color.FromArgb(226, 213, 194);
				SelectedBackColor = Color.FromArgb(204, 204, 204);
			if (ViewState["SelectedForeColor"] == null)
				//SelectedForeColor = Color.Black;
				SelectedForeColor = Color.Black;
			if (ViewState["BackColor"] == null)
				//BackColor = Color.FromArgb(238, 232, 221);
				BackColor = Color.FromArgb(241, 241, 241);
			if (ViewState["ForeColor"] == null)
				//ForeColor = Color.Black;
				ForeColor = Color.Black;
            if (ViewState["CurrentTabIndex"] == null)
				ViewState["CurrentTabIndex"] = 0;

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
