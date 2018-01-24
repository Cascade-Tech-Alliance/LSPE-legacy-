using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;

namespace LSPE
{
	/// <summary>
	/// Summary description for admin.
	/// </summary>
	public partial class slotadjustments : System.Web.UI.Page
	{
        SqlConnection sqlConnection;
        CustomPrincipal cprPrincipal = (CustomPrincipal)HttpContext.Current.User;
        int m_intUS_ID = 0;
        float SATotal;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            MaintainScrollPositionOnPostBack = true;
            SATotal = 0;
            m_intUS_ID = cprPrincipal.UserId;

            string strConnection = (string)Application["strConnection"];
            sqlConnection = new SqlConnection(strConnection);

            SqlDataSource1.ConnectionString = strConnection;
            SqlDataSource2.ConnectionString = strConnection;
            if (!Page.IsPostBack)
            {
                if (!cprPrincipal.IsInRole("Administrator") && !cprPrincipal.IsInRole("ESD Department Level"))
                {
                    form1.Visible = false;
                    lblNotAdminNotify.Visible = true;
                }
                else
                {
                    BindDistrictDDL();
                    BindYearDDL();
                    if (Session["intDS_ID"] != null)
                    {
                        ddlDistricts.SelectedValue = Session["intDS_ID"].ToString();
                        if (Session["intYR_ID"] != null)
                        {
                            ddlYears.SelectedValue = Session["intYR_ID"].ToString();
                            btnGetSlotAdjustments_Click(null, null);
                        }                    
                    }
                
                    
                    GridView1.Visible = (Convert.ToInt32(ddlDistricts.SelectedValue) > 0 && Convert.ToInt32(ddlYears.SelectedValue) > 0);
                }
            }

            btnPrintable.Visible = GridView1.Rows.Count > 0 ? true : false;
		}

        private void BindDistrictDDL()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;

            if (cprPrincipal.IsInRole("District Team Member") || cprPrincipal.IsInRole("District Primary"))
            {
                sqlCommand.CommandText = "prSelectDistrictsByUser";
                sqlCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));
                sqlCommand.Parameters["@intUS_ID"].Value = m_intUS_ID;
            }
            else
            {
                sqlCommand.CommandText = "prSelectAllDistricts";

            }

            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlConnection.Open();
            SqlDataReader sqlReader = sqlCommand.ExecuteReader();

            //Set up the data binding.
            ddlDistricts.DataSource = sqlReader;
            ddlDistricts.DataTextField = "DS_Name";
            ddlDistricts.DataValueField = "DS_ID";
            ddlDistricts.DataBind();

            //Close the connection.
            sqlConnection.Close();
            sqlReader.Close();

            //Add the item at the first position.
            ddlDistricts.Items.Insert(0, "<-------- Select District -------->");
            ddlDistricts.Items[0].Value = "0";
        }

        private void BindYearDDL()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "prSelectAllActiveYears";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlConnection.Open();
            SqlDataReader sqlReader = sqlCommand.ExecuteReader();

            //Set up the data binding.
            ddlYears.DataSource = sqlReader;
            ddlYears.DataTextField = "YR_Desc";
            ddlYears.DataValueField = "YR_ID";
            ddlYears.DataBind();

            //Close the connection.
            sqlConnection.Close();
            sqlReader.Close();

            //Add the item at the first position.
            ddlYears.Items.Insert(0, "<-- Select Year -->");
            ddlYears.Items[0].Value = "0";
        }

        protected void btnGetSlotAdjustments_Click(object sender, System.EventArgs e) 
        {
            int intDS_ID = Convert.ToInt32(ddlDistricts.SelectedValue);
            string strDS_NAME = ddlDistricts.SelectedItem.Text;
            int intYR_ID;
            string strYR_NAME = ddlYears.SelectedItem.Text;
            intYR_ID = Convert.ToInt32(ddlYears.SelectedValue);
            if (intDS_ID > 0 && intYR_ID > 0 )
            {
                lblSAHeader.Text = strDS_NAME + ", " + strYR_NAME + " (" + GetLastYearString(strYR_NAME,true) + " adjustments)";
                phSAHeader.Visible = true;
                Session["intDS_ID"] = intDS_ID;
                Session["intYR_ID"] = intYR_ID;
                Session["strDS_NAME"] = strDS_NAME;
                Session["strYR_NAME"] = strYR_NAME;
                SqlDataSource1.SelectParameters["DS_ID"].DefaultValue = intDS_ID.ToString();
                SqlDataSource1.SelectParameters["YR_ID"].DefaultValue = intYR_ID.ToString();
                GridView1.DataBind();
                btnPrintable.Visible = GridView1.Rows.Count > 0 ? true : false;
                GridView1.Visible = true;
            }
        }

        protected void GridView1_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            SqlDataSource1.UpdateParameters["SA_ID"].DefaultValue = ((Label)(GridView1.Rows[GridView1.EditIndex].FindControl("lblSA_ID"))).Text;
            SqlDataSource1.UpdateParameters["SA_FinalSlotCost"].DefaultValue = ((TextBox)(GridView1.Rows[GridView1.EditIndex].FindControl("txtFinalSlotCost"))).Text;
            SqlDataSource1.UpdateParameters["SA_Adjustment"].DefaultValue = ((TextBox)(GridView1.Rows[GridView1.EditIndex].FindControl("txtAdjustment"))).Text; 
        }

        protected void dlServices_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)(sender)).Items.Insert(0, "<-- Select Service -->");
            ((DropDownList)(sender)).Items[0].Value = "0";
        }

        protected void dlEmptyInsert_DataBound(object sender, EventArgs e)
        {
            ((DropDownList)(sender)).Items.Insert(0, "<-- Select Service -->");
            ((DropDownList)(sender)).Items[0].Value = "0";
        }

        protected void GridView1_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e) 
        {
            SqlDataSource1.DeleteParameters["SA_ID"].DefaultValue = ((Label)((GridView)(sender)).Rows[e.RowIndex].FindControl("lblSA_ID")).Text;
        }

        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e) 
        { 
            ((DropDownList)(GridView1.FooterRow.FindControl("dlServices"))).DataBind();
        }
        
        protected void btnInsert_Click(object sender, EventArgs e)
        {
            SqlDataSource1.InsertParameters["SV_ID"].DefaultValue = ((DropDownList)(GridView1.FooterRow.FindControl("dlServices"))).SelectedValue;
            SqlDataSource1.InsertParameters["SA_FinalSlotCost"].DefaultValue = ((TextBox)(GridView1.FooterRow.FindControl("txtFinalSlotCostInsert"))).Text;
            SqlDataSource1.InsertParameters["Adjustment"].DefaultValue = ((TextBox)(GridView1.FooterRow.FindControl("txtAdjustmentInsert"))).Text;
            SqlDataSource1.Insert();
        }

        protected void btnEmptyInsert_Click(object sender, EventArgs e)
        {
            SqlDataSource1.InsertParameters["SV_ID"].DefaultValue = ((DropDownList)(((LinkButton)(sender)).NamingContainer.FindControl("dlEmptyInsert"))).SelectedValue;
            SqlDataSource1.InsertParameters["SA_FinalSlotCost"].DefaultValue = ((TextBox)(((LinkButton)(sender)).NamingContainer.FindControl("txtFinalSlotCostEmpty"))).Text;
            SqlDataSource1.InsertParameters["Adjustment"].DefaultValue = ((TextBox)(((LinkButton)(sender)).NamingContainer.FindControl("txtEmptyInsert"))).Text;
            SqlDataSource1.Insert();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && GridView1.EditIndex < 0)
            {
                SATotal += float.Parse(((Label)(e.Row.FindControl("lblAdjustment"))).Text.ToString().Replace("$", "").Replace(",", ""), NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint);
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            if (((GridView)(sender)).Rows.Count > 0 && GridView1.EditIndex < 0)
            {
                lblSATotal.Visible = true;
                lblSATotal.Text = "Adjustment Total = " + string.Format("{0:C}",SATotal);
            }
            else
            {
                lblSATotal.Visible = false;
            }
        }

        protected String GetLastYearString(String yr, Boolean shortversion)
        {
            String newstr;
            if (shortversion){
                //have to make sure we only get the beginning "20", not any others that might appear in the year 2020
                yr = "#" + yr; 
                yr = yr.Replace("#20", "").Replace("-20","-");
            }
            String part1 = yr.Substring(0,yr.IndexOf("-"));
            String part2 = yr.Substring(yr.IndexOf("-") + 1);
            Int32 int1 = Convert.ToInt32(part1) - 1;
            Int32 int2 = Convert.ToInt32(part2) - 1;
            newstr = shortversion ? int1.ToString("00") + "-" + int2.ToString("00") : int1.ToString() + "-" + int2.ToString();
            return newstr;
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
