using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using LSPE.UserControls;

namespace LSPE
{
	/// <summary>
	/// Summary description for users.
	/// </summary>
	public partial class users : CustomPage
	{
		SqlConnection sqlConnection;
		CustomPrincipal cprPrincipal = HttpContext.Current.User as CustomPrincipal;
		int m_intUS_ID = 0;
		bool m_blnNewRecord = false;
		DataTable m_dttOrganization;
		int m_intOrganizationUserEditItemIndex = -1;
		bool m_blnNewOrganizationUserRecord = false;
		string m_strOrganizationUserUniqueID = "";
		int m_intUT_ID = 0;
				
		string[] m_strarrUserTypes = new string[] {"Administrators|2", "District Primary Users|1", "District Team Members|4", "ESD Department Level Users|3"};
	
		protected void Page_Load(object sender, System.EventArgs e)
		{
			m_intUS_ID = cprPrincipal.UserId;
			
			string strConnection = (string)Application["strConnection"];
			sqlConnection = new SqlConnection(strConnection);
			
			foreach(string strUserType in m_strarrUserTypes)
			{
				tbsUserTypeSelector.Tabs.Add(strUserType.Split('|')[0]);
			}
			
			m_intUT_ID = Convert.ToInt32(m_strarrUserTypes[tbsUserTypeSelector.CurrentTabIndex].Split('|')[1]);	

			if (!IsPostBack)
			{
				BindGrid();
			}
		}

		public void SelectionChanged(object sender, TabStrip.SelectionChangedEventArgs e)
		{
			m_intUT_ID = Convert.ToInt32(m_strarrUserTypes[tbsUserTypeSelector.CurrentTabIndex].Split('|')[1]);
			BindGrid();			
		}

		public void BindGrid()
		{
			SqlDataAdapter sqlAdapter = new SqlDataAdapter("prSelectUsers", sqlConnection);
			sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter sqlParameter;			
			sqlParameter = sqlAdapter.SelectCommand.Parameters.Add("@intUT_ID", SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.Input;
			sqlParameter.Value = m_intUT_ID;

			DataSet dstUsers = new DataSet();
			
			sqlAdapter.TableMappings.Add("Table", "Users");
			sqlAdapter.TableMappings.Add("Table1", "Organizations");
			
			dstUsers.EnforceConstraints = false;

			sqlConnection.Open();
			sqlAdapter.Fill(dstUsers);
			
			DataRelation dataRelation = dstUsers.Relations.Add("OrganizationUsers", 
				dstUsers.Tables["Users"].Columns["US_ID"], 
				dstUsers.Tables["Organizations"].Columns["OR_US_ID"]);					
			
			sqlConnection.Close();
								
			dtgUsers.DataSource=dstUsers;
			dtgUsers.DataMember="Users";
			dtgUsers.DataBind();
		}

		public void ItemCreated(Object sender, DataGridItemEventArgs e)
		{			
			ListItemType itemType = e.Item.ItemType;
			
			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.Cells[1].FindControl("lnkDelete");
				lnkDelete.Attributes.Add("onClick", "return confirm_delete();");
			}
			
			if (itemType == ListItemType.Header)
			{
				// Show controls for adding a new record.
				e.Item.Visible = m_blnNewRecord;
			}
		}

		public void OrganizationUsersItemCreated(Object sender, DataGridItemEventArgs e)
		{			
			ListItemType itemType = e.Item.ItemType;
			
			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDeleteOrganizationUser = (LinkButton)e.Item.Cells[1].FindControl("lnkDeleteOrganizationUser");
				lnkDeleteOrganizationUser.Attributes.Add("onClick", "return confirm_delete();");
			}
			
			if (itemType == ListItemType.Header)
			{
				// Show controls for adding a new record.
				e.Item.Visible = m_blnNewOrganizationUserRecord;
			}
		}

		public void ItemDataBound(Object sender, DataGridItemEventArgs e)
		{
			ListItemType itemType = e.Item.ItemType;
			
			if (itemType == ListItemType.EditItem)
			{
				TextBox txtPassword = ((TextBox)e.Item.Cells[1].FindControl("txtPassword"));
				txtPassword.Attributes.Add("value","**********");
				txtPassword.Attributes.Add("onFocus", "this.value = ''");
				TextBox txtPasswordReenter = ((TextBox)e.Item.Cells[1].FindControl("txtPasswordReenter"));
				txtPasswordReenter.Attributes.Add("value","**********");
				txtPasswordReenter.Attributes.Add("onFocus","this.value = ''");
			}

			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
			{
				DataGrid dtgOrganizationUsers = ((DataGrid)e.Item.Cells[3].FindControl("dtgOrganizationUsers"));
				if(dtgOrganizationUsers.UniqueID == m_strOrganizationUserUniqueID)
				{
					dtgOrganizationUsers.EditItemIndex = m_intOrganizationUserEditItemIndex;
					m_strOrganizationUserUniqueID = null;
					dtgOrganizationUsers.DataBind();
					((PlaceHolder)e.Item.Cells[3].FindControl("plhOrganizationUsers")).Visible = true;
					((ImageButton)e.Item.Cells[1].FindControl("imgbtnExpand")).ImageUrl = "images/Minus.gif";
				}
			}
		}

		public void OrganizationUsersItemDataBound(Object sender, DataGridItemEventArgs e)
		{
			// 
		}
		
		public void Edit_Click(Object sender, DataGridCommandEventArgs e) 
		{
			dtgUsers.EditItemIndex = (int)e.Item.ItemIndex;
			BindGrid();
		}

		public void EditOrganizationUser_Click(Object sender, DataGridCommandEventArgs e) 
		{
			dtgUsers.EditItemIndex = -1;
			m_intOrganizationUserEditItemIndex = (int)e.Item.ItemIndex;
			m_strOrganizationUserUniqueID = ((DataGrid)sender).UniqueID;
			m_dttOrganization = CreateOrganizationTable();
			BindGrid();
		}

		public void Cancel_Click(Object sender, DataGridCommandEventArgs e)
		{
			dtgUsers.EditItemIndex = -1;
			BindGrid();
		}

		public void CancelOrganizationUser_Click(Object sender, DataGridCommandEventArgs e)
		{
			m_intOrganizationUserEditItemIndex = -1;
			m_strOrganizationUserUniqueID = ((DataGrid)sender).UniqueID;
			BindGrid();
		}

		public void Delete_Click(Object sender, DataGridCommandEventArgs e)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prDeleteUser";
			sqlCommand.CommandType = CommandType.StoredProcedure;
							
			sqlCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));				
			sqlCommand.Parameters["@intUS_ID"].Value = dtgUsers.DataKeys[(int)e.Item.ItemIndex];
				
			sqlCommand.Connection.Open();

			try
			{
				sqlCommand.ExecuteNonQuery();
				dtgUsers.EditItemIndex = -1;
			}
			catch (SqlException exc)
			{
				Response.Write("ERROR: Could not update record, SQL Exception<br>");
				Response.Write(exc.Message + "<br>" + exc.StackTrace);
			}

			sqlCommand.Connection.Close();
			
			BindGrid();
		}

		public void DeleteOrganizationUser_Click(Object sender, DataGridCommandEventArgs e)
		{
			DataGrid dtgOrganizationUsers = ((DataGrid)sender);
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			
			if (m_intUT_ID == 1 || m_intUT_ID == 4) // District Level User
			{
				sqlCommand.CommandText = "prDeleteDistrictUser";	
				sqlCommand.Parameters.Add(new SqlParameter("@intDSU_ID", SqlDbType.Int));				
				sqlCommand.Parameters["@intDSU_ID"].Value = dtgOrganizationUsers.DataKeys[(int)e.Item.ItemIndex];
			}
			else if (m_intUT_ID == 3) // Department Level User
			{
				sqlCommand.CommandText = "prDeleteDepartmentUser";	
				sqlCommand.Parameters.Add(new SqlParameter("@intDPU_ID", SqlDbType.Int));				
				sqlCommand.Parameters["@intDPU_ID"].Value = dtgOrganizationUsers.DataKeys[(int)e.Item.ItemIndex];
			}
			sqlCommand.Connection.Open();

			try
			{
				sqlCommand.ExecuteNonQuery();
				dtgUsers.EditItemIndex = -1;
			}
			catch (SqlException exc)
			{
				Response.Write("ERROR: Could not update record, SQL Exception<br>");
				Response.Write(exc.Message + "<br>" + exc.StackTrace);
			}

			sqlCommand.Connection.Close();
			
			m_strOrganizationUserUniqueID = dtgOrganizationUsers.UniqueID;
			BindGrid();
		}

		public void Update_Click(Object sender, DataGridCommandEventArgs e)
		{
			if (Page.IsValid) 
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandText = "prUpdateUser";
				sqlCommand.CommandType = CommandType.StoredProcedure;
							
				sqlCommand.Parameters.Add(new SqlParameter("@intUS_ID", SqlDbType.Int));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Username", SqlDbType.VarChar, 30));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_PasswordHash", SqlDbType.VarChar, 40));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Salt", SqlDbType.VarChar, 10));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_NameFirst", SqlDbType.VarChar, 50));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_NameLast", SqlDbType.VarChar, 50));
				sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Email", SqlDbType.VarChar, 100));
				sqlCommand.Parameters.Add(new SqlParameter("@bitUS_IsInactive", SqlDbType.Bit));
				sqlCommand.Parameters.Add(new SqlParameter("@intUS_ModifiedBy_US_ID", SqlDbType.Int));
				
				sqlCommand.Parameters["@intUS_ID"].Value = dtgUsers.DataKeys[(int)e.Item.ItemIndex];
				sqlCommand.Parameters["@chvUS_Username"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtUsername")).Text;
				if (((TextBox)e.Item.Cells[1].FindControl("txtPassword")).Text == "**********")
				{
					sqlCommand.Parameters["@chvUS_PasswordHash"].Value = DBNull.Value;
					sqlCommand.Parameters["@chvUS_Salt"].Value = DBNull.Value;
				}
				else
				{
					string strSalt = CreateSalt(5);
					string strPasswordHash = CreatePasswordHash(((TextBox)e.Item.Cells[1].FindControl("txtPassword")).Text, strSalt);
					sqlCommand.Parameters["@chvUS_PasswordHash"].Value = strPasswordHash;
					sqlCommand.Parameters["@chvUS_Salt"].Value = strSalt;
				}
				sqlCommand.Parameters["@chvUS_NameFirst"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtFirstName")).Text;
				sqlCommand.Parameters["@chvUS_NameLast"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtLastName")).Text;
				sqlCommand.Parameters["@chvUS_Email"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtEmail")).Text;
				sqlCommand.Parameters["@bitUS_IsInactive"].Value = 0;
				sqlCommand.Parameters["@intUS_ModifiedBy_US_ID"].Value = m_intUS_ID;
				
				sqlCommand.Connection.Open();

				try
				{
					sqlCommand.ExecuteNonQuery();
					dtgUsers.EditItemIndex = -1;
				}
				catch (SqlException exc)
				{
					Response.Write("ERROR: Could not update record, SQL Exception<br>");
					Response.Write(exc.Message + "<br>" + exc.StackTrace);
				}

				sqlCommand.Connection.Close();
				BindGrid();
			}
		}

		public void UpdateOrganizationUser_Click(Object sender, DataGridCommandEventArgs e)
		{
			if (Page.IsValid) 
			{
				// Get reference to dtgServices DataGrid.
				DataGrid dtgOrganizationUsers = (DataGrid)sender;
				
				// Get reference to current Item of parent DataGrid (dtgServiceTypes) and use that item's index
				// to return foreign key value needed by child DataGrid's (dtgServices) datasource.
				DataGridItem dgiUser = (DataGridItem)dtgOrganizationUsers.Parent.Parent.Parent;
				
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;

				if (m_intUT_ID == 1 || m_intUT_ID == 4) // District Level User
				{
					int intDSU_US_ID = (int)dtgUsers.DataKeys[dgiUser.ItemIndex];
				
					sqlCommand.CommandText = "prUpdateDistrictUser";
					sqlCommand.CommandType = CommandType.StoredProcedure;
							
					sqlCommand.Parameters.Add(new SqlParameter("@intDSU_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@intDSU_US_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@intDSU_DS_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@intDSU_ModifiedBy_US_ID", SqlDbType.Int));
								
					sqlCommand.Parameters["@intDSU_ID"].Value = dtgOrganizationUsers.DataKeys[(int)e.Item.ItemIndex];
					sqlCommand.Parameters["@intDSU_US_ID"].Value = intDSU_US_ID;
					sqlCommand.Parameters["@intDSU_DS_ID"].Value = ((DropDownList)e.Item.Cells[1].FindControl("ddlOrganization")).SelectedValue;
					sqlCommand.Parameters["@intDSU_ModifiedBy_US_ID"].Value = m_intUS_ID;
				}
				else if (m_intUT_ID == 3) // Department Level User
				{
					int intDPU_US_ID = (int)dtgUsers.DataKeys[dgiUser.ItemIndex];
							
					sqlCommand.CommandText = "prUpdateDepartmentUser";
					sqlCommand.CommandType = CommandType.StoredProcedure;
							
					sqlCommand.Parameters.Add(new SqlParameter("@intDPU_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@intDPU_US_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@intDPU_DP_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@intDPU_ModifiedBy_US_ID", SqlDbType.Int));
								
					sqlCommand.Parameters["@intDPU_ID"].Value = dtgOrganizationUsers.DataKeys[(int)e.Item.ItemIndex];
					sqlCommand.Parameters["@intDPU_US_ID"].Value = intDPU_US_ID;
					sqlCommand.Parameters["@intDPU_DP_ID"].Value = ((DropDownList)e.Item.Cells[1].FindControl("ddlOrganization")).SelectedValue;
					sqlCommand.Parameters["@intDPU_ModifiedBy_US_ID"].Value = m_intUS_ID;
				}
				
				sqlCommand.Connection.Open();

				try
				{
					sqlCommand.ExecuteNonQuery();
					m_strOrganizationUserUniqueID = dtgOrganizationUsers.UniqueID;
					m_intOrganizationUserEditItemIndex = -1;
				}
				catch (SqlException exc)
				{
					Response.Write("ERROR: Could not update record, SQL Exception<br>");
					Response.Write(exc.Message + "<br>" + exc.StackTrace);
				}

				sqlCommand.Connection.Close();
				BindGrid();
			}
		}

		public void dtgUsers_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "SaveNew":
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;
					sqlCommand.CommandText = "prInsertUser";
					sqlCommand.CommandType = CommandType.StoredProcedure;
						
					sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Username", SqlDbType.VarChar, 30));
					sqlCommand.Parameters.Add(new SqlParameter("@chvUS_PasswordHash", SqlDbType.VarChar, 40));
					sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Salt", SqlDbType.VarChar, 10));
					sqlCommand.Parameters.Add(new SqlParameter("@chvUS_NameFirst", SqlDbType.VarChar, 50));
					sqlCommand.Parameters.Add(new SqlParameter("@chvUS_NameLast", SqlDbType.VarChar, 50));
					sqlCommand.Parameters.Add(new SqlParameter("@chvUS_Email", SqlDbType.VarChar, 100));
					sqlCommand.Parameters.Add(new SqlParameter("@intUS_UT_ID", SqlDbType.Int));
					sqlCommand.Parameters.Add(new SqlParameter("@bitUS_IsInactive", SqlDbType.Bit));
					sqlCommand.Parameters.Add(new SqlParameter("@intUS_ModifiedBy_US_ID", SqlDbType.Int));
			
					sqlCommand.Parameters["@chvUS_Username"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtUsernameNew")).Text;
					string strSalt = CreateSalt(5);
					string strPasswordHash = CreatePasswordHash(((TextBox)e.Item.Cells[1].FindControl("txtPasswordNew")).Text, strSalt);
					sqlCommand.Parameters["@chvUS_PasswordHash"].Value = strPasswordHash;
					sqlCommand.Parameters["@chvUS_Salt"].Value = strSalt;
					sqlCommand.Parameters["@chvUS_NameFirst"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtFirstNameNew")).Text;
					sqlCommand.Parameters["@chvUS_NameLast"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtLastNameNew")).Text;
					sqlCommand.Parameters["@chvUS_Email"].Value = ((TextBox)e.Item.Cells[1].FindControl("txtEmailNew")).Text;
					sqlCommand.Parameters["@intUS_UT_ID"].Value = m_strarrUserTypes[tbsUserTypeSelector.CurrentTabIndex].Split('|')[1];
					sqlCommand.Parameters["@bitUS_IsInactive"].Value = 0;
					sqlCommand.Parameters["@intUS_ModifiedBy_US_ID"].Value = m_intUS_ID;
			
					sqlCommand.Connection.Open();

					try
					{
						sqlCommand.ExecuteNonQuery();
						dtgUsers.EditItemIndex = -1;
					}
					catch (SqlException exc)
					{
						Response.Write("ERROR: Could not update record, SQL Exception<br>");
						Response.Write(exc.Message + "<br>" + exc.StackTrace);
					}

					sqlCommand.Connection.Close();
					BindGrid();
					break;

				case "Expand":
					PlaceHolder ChildRows;
					ChildRows = (PlaceHolder)e.Item.Cells[3].FindControl("plhOrganizationUsers");
					ChildRows.Visible = !ChildRows.Visible;
										
					ImageButton imgbtnExpand;
					imgbtnExpand = (ImageButton)e.Item.Cells[1].FindControl("imgbtnExpand");
					
					if (imgbtnExpand.ImageUrl == "images/Plus.gif")
					{
						imgbtnExpand.ImageUrl = "images/Minus.gif";
					}
					else
					{
						imgbtnExpand.ImageUrl = "images/Plus.gif";
					}

					break;
			}
		}

		public void dtgOrganizationUsers_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "SaveNew":
					// Get reference to dtgOrganizationUsers DataGrid.
					DataGrid dtgOrganizationUsers = (DataGrid)sender;
						
					// Get reference to current Item of parent DataGrid (dtgUsers) and use that item's index
					// to return foreign key value needed by child DataGrid's (dtgOrganizationUsers) datasource.
					DataGridItem dgiUser = (DataGridItem)dtgOrganizationUsers.Parent.Parent.Parent;
										
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;
					sqlCommand.CommandType = CommandType.StoredProcedure;

					if (m_intUT_ID == 1 || m_intUT_ID == 4) // District Level User
					{
						int intDSU_US_ID = (int)dtgUsers.DataKeys[dgiUser.ItemIndex];
						
						sqlCommand.CommandText = "prInsertDistrictUser";
						
						sqlCommand.Parameters.Add(new SqlParameter("@intDSU_US_ID", SqlDbType.Int));
						sqlCommand.Parameters.Add(new SqlParameter("@intDSU_DS_ID", SqlDbType.Int));
						sqlCommand.Parameters.Add(new SqlParameter("@intDSU_ModifiedBy_US_ID", SqlDbType.Int));
								
						sqlCommand.Parameters["@intDSU_US_ID"].Value = intDSU_US_ID;
						sqlCommand.Parameters["@intDSU_DS_ID"].Value = ((DropDownList)e.Item.Cells[1].FindControl("ddlOrganizationNew")).SelectedValue;
						sqlCommand.Parameters["@intDSU_ModifiedBy_US_ID"].Value = m_intUS_ID;

					}
					else if (m_intUT_ID == 3) // Department Level User
					{
						int intDPU_US_ID = (int)dtgUsers.DataKeys[dgiUser.ItemIndex];
						
						sqlCommand.CommandText = "prInsertDepartmentUser";
						
						sqlCommand.Parameters.Add(new SqlParameter("@intDPU_US_ID", SqlDbType.Int));
						sqlCommand.Parameters.Add(new SqlParameter("@intDPU_DP_ID", SqlDbType.Int));
						sqlCommand.Parameters.Add(new SqlParameter("@intDPU_ModifiedBy_US_ID", SqlDbType.Int));
								
						sqlCommand.Parameters["@intDPU_US_ID"].Value = intDPU_US_ID;
						sqlCommand.Parameters["@intDPU_DP_ID"].Value = ((DropDownList)e.Item.Cells[1].FindControl("ddlOrganizationNew")).SelectedValue;
						sqlCommand.Parameters["@intDPU_ModifiedBy_US_ID"].Value = m_intUS_ID;		
					}					
								
					sqlCommand.Connection.Open();

					try
					{
						sqlCommand.ExecuteNonQuery();
						m_blnNewOrganizationUserRecord = false;
					}
					catch (SqlException exc)
					{
						Response.Write("ERROR: Could not update record, SQL Exception<br>");
						Response.Write(exc.Message + "<br>" + exc.StackTrace);
					}

					sqlCommand.Connection.Close();
					m_strOrganizationUserUniqueID = dtgOrganizationUsers.UniqueID;
					BindGrid();
					break;
			}
		}

		public void NewCancel_Click(object sender, System.EventArgs e)
		{
			m_blnNewRecord = false;
			BindGrid();
		}

		public void NewOrganizationUserCancel_Click(object sender, System.EventArgs e)
		{
			m_blnNewOrganizationUserRecord = false;
			m_strOrganizationUserUniqueID = ((DataGrid)sender).UniqueID;
			BindGrid();
		}

		protected void lnkNew_Click(object sender, System.EventArgs e)
		{
			dtgUsers.EditItemIndex = -1;
			m_blnNewRecord = true;
			BindGrid();
		}

		public void lnkNewOrganizationUser_Click(object sender, System.EventArgs e)
		{
			LinkButton lnkNewOrganizationUser = (LinkButton)sender;
			DataGridItem dgiUser = (DataGridItem)lnkNewOrganizationUser.Parent.Parent.Parent;
			DataGrid dtgOrganizationUsers = ((DataGrid)dgiUser.Cells[3].FindControl("dtgOrganizationUsers"));
					
			m_intOrganizationUserEditItemIndex = -1;
			m_blnNewOrganizationUserRecord = true;
			m_strOrganizationUserUniqueID = dtgOrganizationUsers.UniqueID;

			m_dttOrganization = CreateOrganizationTable();
			BindGrid();
		}

		private static string CreateSalt(int intSize)
		{
			// Generate a cryptographic random number using the cryptographic service provider
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] buff = new byte[intSize];
			rng.GetBytes(buff);
			// Return a Base64 string representation of the random number
			return Convert.ToBase64String(buff);
		}

		private static string CreatePasswordHash(string strPassword, string strSalt)
		{
			string strSaltAndPwd = String.Concat(strPassword, strSalt);
			string strHashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(strSaltAndPwd, "SHA1");
			return strHashedPwd;
		}

		private DataTable CreateOrganizationTable()
		{
			DataTable dttOrganization = new DataTable();
			DataRow dtrOrganization;
			DataColumn dtcOrganizationName = new DataColumn("OR_Name");
			dttOrganization.Columns.Add(dtcOrganizationName);
			DataColumn dtcOrganizationId = new DataColumn("OR_ID");
			dttOrganization.Columns.Add(dtcOrganizationId);

			dtrOrganization = dttOrganization.NewRow();
			dtrOrganization[0] = "";
			dtrOrganization[1] = "0";
			dttOrganization.Rows.Add(dtrOrganization);
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			
			if (m_intUT_ID == 2) // Administrative Level User
			{
				return dttOrganization;
			}
			else if (m_intUT_ID == 1 || m_intUT_ID == 4) // District Level User
			{
				sqlCommand.CommandText = "prSelectDistricts";			
			}
			else if (m_intUT_ID == 3) // Department Level User
			{
				sqlCommand.CommandText = "prSelectDepartments";		
			}

			sqlConnection.Open();
			SqlDataReader sqlReader = sqlCommand.ExecuteReader();
			while(sqlReader.Read())
			{
				dtrOrganization = dttOrganization.NewRow();
				dtrOrganization[0] = sqlReader[0];
				dtrOrganization[1] = sqlReader[1];
				dttOrganization.Rows.Add(dtrOrganization);
			}
			sqlConnection.Close();
			sqlReader.Close();
			
			return dttOrganization;
		}

		protected DataTable GetOrganizationList()
		{
			// m_dttOrganization is a class-level DataTable
			return m_dttOrganization;
		}

		protected int GetSelectedOrganization(string strOR_Name)
		{
			for(int i=0; i < m_dttOrganization.Rows.Count; i++)
			{
				if(m_dttOrganization.Rows[i]["OR_Name"].ToString() == strOR_Name)
				{
					return i;
				}
			}

			//If there is no match, return 0
			return 0;
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
