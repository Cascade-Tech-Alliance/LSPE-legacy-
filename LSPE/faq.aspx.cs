using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

namespace LSPE
{
	/// <summary>
	/// Summary description for faq.
	/// </summary>
	public partial class faq : CustomPage
	{
		CustomPrincipal cprPrincipal = HttpContext.Current.User as CustomPrincipal;
		SqlConnection sqlConnection;
		int m_intUS_ID = 0;
		bool m_blnNewRecord = false;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			m_intUS_ID = cprPrincipal.UserId;
			
			string strConnection = (string)Application["strConnection"];
			sqlConnection = new SqlConnection(strConnection);
			
			lnkNew.Visible = cprPrincipal.IsInRole("Administrator");
			
			if (!IsPostBack)
			{
				BindGrid();
			}
		}

		public void BindGrid()
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandText = "prSelectFAQs";
			SqlDataReader sqlReader;

			sqlConnection.Open();
			sqlReader = sqlCommand.ExecuteReader();
			
		
			dtgFAQs.DataSource = sqlReader;
			dtgFAQs.DataBind();
			
            sqlConnection.Close();
			sqlReader.Close();
		}
		
		public void ItemCreated(Object sender, DataGridItemEventArgs e)
		{			
			ListItemType itemType = e.Item.ItemType;
			
			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.Cells[1].FindControl("lnkDelete");
				LinkButton lnkEdit = (LinkButton)e.Item.Cells[1].FindControl("lnkEdit");
				if (cprPrincipal.IsInRole("Administrator"))
				{
					lnkDelete.Attributes.Add("onClick", "return confirm_delete();");
					lnkDelete.Visible = true;
					lnkEdit.Visible = true;
				}
				else
				{
					lnkDelete.Visible = false;
					lnkEdit.Visible = false;
				}
			}
			
			if (itemType == ListItemType.Header)
			{
				// Show controls for adding a new record.
				e.Item.Visible = m_blnNewRecord;
			}
		}

		public void Edit_Click(Object sender, DataGridCommandEventArgs e) 
		{
			dtgFAQs.EditItemIndex = (int)e.Item.ItemIndex;
			BindGrid();
		}

		public void Cancel_Click(Object sender, DataGridCommandEventArgs e)
		{
			dtgFAQs.EditItemIndex = -1;
			BindGrid();
		}

		public void Delete_Click(Object sender, DataGridCommandEventArgs e)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prDeleteFAQ";
			sqlCommand.CommandType = CommandType.StoredProcedure;
							
			sqlCommand.Parameters.Add(new SqlParameter("@intFAQ_ID", SqlDbType.Int));				
			sqlCommand.Parameters["@intFAQ_ID"].Value = dtgFAQs.DataKeys[(int)e.Item.ItemIndex];
				
			sqlCommand.Connection.Open();

			try
			{
				sqlCommand.ExecuteNonQuery();
				dtgFAQs.EditItemIndex = -1;
			}
			catch (SqlException exc)
			{
				Response.Write("ERROR: Could not update record, SQL Exception<br>");
				Response.Write(exc.Message + "<br>" + exc.StackTrace);
			}

			sqlCommand.Connection.Close();

			BindGrid();
		}

		public void Update_Click(Object sender, DataGridCommandEventArgs e)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prUpdateFAQs";
			sqlCommand.CommandType = CommandType.StoredProcedure;
						
			sqlCommand.Parameters.Add(new SqlParameter("@intFAQ_ID", SqlDbType.Int));
			sqlCommand.Parameters.Add(new SqlParameter("@chvFAQ_Question", SqlDbType.VarChar, 255));
			sqlCommand.Parameters.Add(new SqlParameter("@chvFAQ_Answer", SqlDbType.VarChar, 1000));
			sqlCommand.Parameters.Add(new SqlParameter("@intFAQ_ModifiedBy_US_ID", SqlDbType.Int));
			
			sqlCommand.Parameters["@intFAQ_ID"].Value = dtgFAQs.DataKeys[(int)e.Item.ItemIndex];
			sqlCommand.Parameters["@chvFAQ_Question"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtQuestion")).Text;
			sqlCommand.Parameters["@chvFAQ_Answer"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtAnswer")).Text;
			sqlCommand.Parameters["@intFAQ_ModifiedBy_US_ID"].Value = m_intUS_ID;
			
			sqlCommand.Connection.Open();

			try
			{
				sqlCommand.ExecuteNonQuery();
				dtgFAQs.EditItemIndex = -1;
			}
			catch (SqlException exc)
			{
				Response.Write("ERROR: Could not update record, SQL Exception<br>");
				Response.Write(exc.Message + "<br>" + exc.StackTrace);
			}

			sqlCommand.Connection.Close();
			
			BindGrid();
		}

		public void dtgFAQs_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "SaveNew":
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;
					sqlCommand.CommandText = "prInsertFAQ";
					sqlCommand.CommandType = CommandType.StoredProcedure;
								
					sqlCommand.Parameters.Add(new SqlParameter("@chvFAQ_Question", SqlDbType.VarChar, 255));
					sqlCommand.Parameters.Add(new SqlParameter("@chvFAQ_Answer", SqlDbType.VarChar, 1000));
					sqlCommand.Parameters.Add(new SqlParameter("@intFAQ_ModifiedBy_US_ID", SqlDbType.Int));
					
					sqlCommand.Parameters["@chvFAQ_Question"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtQuestionNew")).Text;
					sqlCommand.Parameters["@chvFAQ_Answer"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtAnswerNew")).Text;
					sqlCommand.Parameters["@intFAQ_ModifiedBy_US_ID"].Value = m_intUS_ID;
					
					sqlCommand.Connection.Open();
					try
					{
						sqlCommand.ExecuteNonQuery();
						m_blnNewRecord = false;
					}
					catch (SqlException exc)
					{
						Response.Write("ERROR: Could not update record, SQL Exception<br>");
						Response.Write(exc.Message + "<br>" + exc.StackTrace);
					}
					sqlCommand.Connection.Close();
					
					BindGrid();
					break;
			}
		}

		protected void lnkNew_Click(object sender, System.EventArgs e)
		{
			dtgFAQs.EditItemIndex = -1;
			m_blnNewRecord = true;
			BindGrid();
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
