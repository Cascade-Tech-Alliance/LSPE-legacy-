using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace LSPE
{
    public partial class PrintableSlotAdjustments : System.Web.UI.Page
    {
        float SATotal;
    
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SATotal = 0;
                string strConnection = (string)Application["strConnection"];
                SqlDataSource1.ConnectionString = strConnection;

                if (!(Session["strDS_NAME"] == null) && !(Session["strYR_NAME"] == null))
                {
                    lblDistrict.Text = Session["strDS_NAME"].ToString();
                    lblTitle.Text = GetLastYearString(Session["strYR_NAME"].ToString(),false) + " Adjustments";
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) 
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SATotal += float.Parse(((Label)(e.Row.FindControl("Label2"))).Text.ToString().Replace("$","").Replace(",",""), NumberStyles.AllowDecimalPoint | NumberStyles.AllowParentheses); 
            }
        }

        protected void GridView1_DataBound(object sender, EventArgs e)
        {
            lblSATotal.Text = "Adjustment Total = " + string.Format("{0:C}", SATotal);
        }

        protected String GetLastYearString(String yr, Boolean shortversion)
        {
            String newstr;
            if (shortversion)
            {
                //have to make sure we only get the beginning "20", not any others that might appear in the year 2020
                yr = "#" + yr;
                yr = yr.Replace("#20", "").Replace("-20", "-");
            }
            String part1 = yr.Substring(0, yr.IndexOf("-"));
            String part2 = yr.Substring(yr.IndexOf("-") + 1);
            Int32 int1 = Convert.ToInt32(part1) - 1;
            Int32 int2 = Convert.ToInt32(part2) - 1;
            newstr = shortversion ? int1.ToString("00") + "-" + int2.ToString("00") : int1.ToString() + "-" + int2.ToString();
            return newstr;
        }
    }
}
