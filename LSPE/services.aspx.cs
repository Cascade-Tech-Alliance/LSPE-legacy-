using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;
using LSPE.UserControls;

namespace LSPE
{
    /// <summary>
    /// Summary description for services.
    /// </summary>
    public partial class services : CustomPage
    {
        SqlConnection sqlConnection;
        CustomPrincipal cprPrincipal = HttpContext.Current.User as CustomPrincipal;
        bool m_blnNewRecord = false;
        bool m_blnNewServiceRecord = false;
        bool m_blnNewServiceDetailRecord = false;
        int m_intUS_ID = 0;
        int m_intServiceEditItemIndex = -1;
        int m_intServiceDetailEditItemIndex = -1;
        string m_strServiceUniqueID = "";
        string m_strServiceDetailUniqueID = "";
        private DataTable m_dttYear;
        private DataTable m_dttUnitMeasure;

        string[] m_strarrDepartments;
    
        protected void Page_Load(object sender, System.EventArgs e)
        {
            m_intUS_ID = cprPrincipal.UserId;
            
            string strConnection = (string)Application["strConnection"];
            sqlConnection = new SqlConnection(strConnection);
            
            m_strarrDepartments = (string[])Session["strarrDepartments"];

            foreach(string strDepartment in m_strarrDepartments)
            {
                tbsDeptSelector.Tabs.Add(strDepartment.Split('|')[0]);
            }

            if (!Page.IsPostBack)
            {
                hypServicesReport.NavigateUrl = "~/report_popup.aspx?report=Services";
                hypServicesReport.Target = "_blank";
                BindGrid();
            }
        }

        public void SelectionChanged(object sender, TabStrip.SelectionChangedEventArgs e)
        {
            dtgServiceTypes.EditItemIndex = -1;
            dtgServiceTypes.CurrentPageIndex = 0;
            BindGrid();			
        }

        public void BindGrid()
        {
            SqlDataAdapter sqlAdapter = new SqlDataAdapter("prSelectServiceTypesByDepartment", sqlConnection);
            sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter sqlParameter;

            sqlParameter = sqlAdapter.SelectCommand.Parameters.Add("@intDP_ID", SqlDbType.Int);
            sqlParameter.Direction = ParameterDirection.Input;
            sqlParameter.Value = m_strarrDepartments[tbsDeptSelector.CurrentTabIndex].Split('|')[1];	
                    
            DataSet dstServiceTypes = new DataSet();
            sqlAdapter.TableMappings.Add("Table", "ServiceTypes");
            sqlAdapter.TableMappings.Add("Table1", "Services");
            sqlAdapter.TableMappings.Add("Table2", "ServiceDetails");
                    
            sqlConnection.Open();
            sqlAdapter.Fill(dstServiceTypes);
            sqlConnection.Close();

            DataRelation dataRelationServices = dstServiceTypes.Relations.Add("RelatedServices", 
                dstServiceTypes.Tables["ServiceTypes"].Columns["ST_ID"], 
                dstServiceTypes.Tables["Services"].Columns["SV_ST_ID"]);

            DataRelation dataRelationServiceDetails = dstServiceTypes.Relations.Add("RelatedServiceDetails", 
                dstServiceTypes.Tables["Services"].Columns["SV_ID"], 
                dstServiceTypes.Tables["ServiceDetails"].Columns["SD_SV_ID"]);

            //dstServiceTypes.EnforceConstraints = false;
        
            dtgServiceTypes.DataSource=dstServiceTypes;
            dtgServiceTypes.DataMember="ServiceTypes";
            dtgServiceTypes.DataBind();
        }

        public void ItemCreated(Object sender, DataGridItemEventArgs e)
        {			
            ListItemType itemType = e.Item.ItemType;
            
            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDelete = (LinkButton)e.Item.Cells[2].FindControl("lnkDelete");
                lnkDelete.Attributes.Add("onClick", "return confirm_delete();");
            }
            
            if (itemType == ListItemType.Header)
            {
                // Show controls for adding a new record.
                e.Item.Visible = m_blnNewRecord;
            }

            if (itemType == ListItemType.Pager) 
            {
                TableCell pager = (TableCell)e.Item.Controls[0];
                //pager.HorizontalAlign = HorizontalAlign.Center;
                for (int n=0; n<pager.Controls.Count; n+=2) 
                {
                    try 
                    {
                        Label l = (Label) pager.Controls[n];
                        l.Text = "page " + l.Text; 
                        l.CssClass = "PagerSpan";
                    } 
                    catch 
                    {
                        LinkButton h = (LinkButton) pager.Controls[n];
                        h.Text = "[ " + h.Text + " ]"; 
                        h.CssClass = "PagerLink";
                    }
                }
            }
        }

        public void ServiceItemCreated(Object sender, DataGridItemEventArgs e)
        {			
            ListItemType itemType = e.Item.ItemType;
            
            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDeleteService = (LinkButton)e.Item.Cells[2].FindControl("lnkDeleteService");
                lnkDeleteService.Attributes.Add("onClick", "return confirm_delete();");
            }
            
            if (itemType == ListItemType.Header)
            {
                // Show controls for adding a new record.
                e.Item.Visible = m_blnNewServiceRecord;
            }
        }

        public void ServiceDetailItemCreated(Object sender, DataGridItemEventArgs e)
        {			
            ListItemType itemType = e.Item.ItemType;
            
            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkDeleteServiceDetail = (LinkButton)e.Item.Cells[1].FindControl("lnkDeleteServiceDetail");
                lnkDeleteServiceDetail.Attributes.Add("onClick", "return confirm_delete();");
            }
            
            if (itemType == ListItemType.Header)
            {
                // Show controls for adding a new record.
                e.Item.Visible = m_blnNewServiceDetailRecord;
            }
        }

        public void dtgServiceTypes_OnPageIndexChanged(Object sender, DataGridPageChangedEventArgs e)
        {
            dtgServiceTypes.CurrentPageIndex = e.NewPageIndex;
            BindGrid();
        }

        public void Edit_Click(Object sender, DataGridCommandEventArgs e) 
        {
            dtgServiceTypes.EditItemIndex = (int)e.Item.ItemIndex;
            BindGrid();
        }

        public void EditService_Click(Object sender, DataGridCommandEventArgs e) 
        {
            dtgServiceTypes.EditItemIndex = -1;
            m_intServiceEditItemIndex = (int)e.Item.ItemIndex;
            m_strServiceUniqueID = ((DataGrid)sender).UniqueID;
            m_dttYear = CreateYearTable();
            BindGrid();
        }

        public void EditServiceDetail_Click(Object sender, DataGridCommandEventArgs e) 
        {
            DataGrid dtgServiceDetails = (DataGrid)sender;
            DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;
            
            dtgServiceTypes.EditItemIndex = -1;
            m_intServiceEditItemIndex = -1;
            m_strServiceUniqueID = dtgServices.UniqueID;

            m_intServiceDetailEditItemIndex = (int)e.Item.ItemIndex;
            m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;
            
            m_dttUnitMeasure = CreateUnitMeasureTable();
            BindGrid();
        }

        public void Cancel_Click(Object sender, DataGridCommandEventArgs e)
        {
            dtgServiceTypes.EditItemIndex = -1;
            BindGrid();
        }

        public void CancelService_Click(Object sender, DataGridCommandEventArgs e)
        {
            m_intServiceEditItemIndex = -1;
            m_strServiceUniqueID = ((DataGrid)sender).UniqueID;
            BindGrid();
        }

        public void CancelServiceDetail_Click(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid dtgServiceDetails = (DataGrid)sender;
            DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;
            
            m_intServiceDetailEditItemIndex = -1;
            m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;
            m_strServiceUniqueID = dtgServices.UniqueID;
            BindGrid();			
        }

        public void Delete_Click(Object sender, DataGridCommandEventArgs e)
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "prDeleteServiceType";
            sqlCommand.CommandType = CommandType.StoredProcedure;
                            
            sqlCommand.Parameters.Add(new SqlParameter("@intST_ID", SqlDbType.Int));				
            sqlCommand.Parameters["@intST_ID"].Value = dtgServiceTypes.DataKeys[(int)e.Item.ItemIndex];
                
            sqlCommand.Connection.Open();

            try
            {
                sqlCommand.ExecuteNonQuery();
                dtgServiceTypes.EditItemIndex = -1;
            }
            catch (SqlException exc)
            {
                Response.Write("ERROR: Could not update record, SQL Exception<br>");
                Response.Write(exc.Message + "<br>" + exc.StackTrace);
            }

            sqlCommand.Connection.Close();
            
            BindGrid();
        }

        public void DeleteService_Click(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid dtgServices = ((DataGrid)sender);
            
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "prDeleteService";
            sqlCommand.CommandType = CommandType.StoredProcedure;
                            
            sqlCommand.Parameters.Add(new SqlParameter("@intSV_ID", SqlDbType.Int));				
            sqlCommand.Parameters["@intSV_ID"].Value = dtgServices.DataKeys[(int)e.Item.ItemIndex];
                
            sqlCommand.Connection.Open();

            try
            {
                sqlCommand.ExecuteNonQuery();
                dtgServiceTypes.EditItemIndex = -1;
            }
            catch (SqlException exc)
            {
                Response.Write("ERROR: Could not update record, SQL Exception<br>");
                Response.Write(exc.Message + "<br>" + exc.StackTrace);
            }

            sqlCommand.Connection.Close();
            
            m_strServiceUniqueID = dtgServices.UniqueID;
            BindGrid();
        }

        public void DeleteServiceDetail_Click(Object sender, DataGridCommandEventArgs e)
        {
            DataGrid dtgServiceDetails = (DataGrid)sender;
            DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;
                        
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "prDeleteServiceDetail";
            sqlCommand.CommandType = CommandType.StoredProcedure;
                            
            sqlCommand.Parameters.Add(new SqlParameter("@intSD_ID", SqlDbType.Int));				
            sqlCommand.Parameters["@intSD_ID"].Value = dtgServiceDetails.DataKeys[(int)e.Item.ItemIndex];
                
            sqlCommand.Connection.Open();

            try
            {
                sqlCommand.ExecuteNonQuery();
                dtgServiceTypes.EditItemIndex = -1;
                m_intServiceEditItemIndex = -1;
            }
            catch (SqlException exc)
            {
                Response.Write("ERROR: Could not update record, SQL Exception<br>");
                Response.Write(exc.Message + "<br>" + exc.StackTrace);
            }

            sqlCommand.Connection.Close();
            
            m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;
            m_strServiceUniqueID = dtgServices.UniqueID;
            
            BindGrid();
        }

        public void ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            ListItemType itemType = e.Item.ItemType;
            
            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
            {
                DataGrid dtgServices = ((DataGrid)e.Item.Cells[3].FindControl("dtgServices"));
                if(dtgServices.UniqueID == m_strServiceUniqueID)
                {
                    dtgServices.EditItemIndex = m_intServiceEditItemIndex;
                    m_strServiceUniqueID = null;
                    dtgServices.DataBind();
                    ((PlaceHolder)e.Item.Cells[3].FindControl("ChildRows")).Visible = true;
                    ((ImageButton)e.Item.Cells[1].FindControl("imgbtnExpand")).ImageUrl = "images/Minus.gif";
                }
            }
        }

        public void ServiceItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            ListItemType itemType = e.Item.ItemType;
            
            if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
            {
                DataGrid dtgServiceDetails = ((DataGrid)e.Item.Cells[3].FindControl("dtgServiceDetails"));
                if(dtgServiceDetails.UniqueID == m_strServiceDetailUniqueID)
                {
                    dtgServiceDetails.EditItemIndex = m_intServiceDetailEditItemIndex;
                    //m_strServiceDetailUniqueID = null;
                    dtgServiceDetails.DataBind();
                    ((PlaceHolder)e.Item.Cells[3].FindControl("plhServiceDetails")).Visible = true;
                    ((ImageButton)e.Item.Cells[1].FindControl("imgbtnExpandServiceDetails")).ImageUrl = "images/Minus.gif";
                }
            }
        }

        public void ServiceDetailItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            //
        }

        public void Update_Click(Object sender, DataGridCommandEventArgs e)
        {
            if (Page.IsValid)
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "prUpdateServiceType";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                            
                sqlCommand.Parameters.Add(new SqlParameter("@intST_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@chvST_Name", SqlDbType.VarChar, 100));
                sqlCommand.Parameters.Add(new SqlParameter("@chvST_Desc", SqlDbType.VarChar, 3000));
                sqlCommand.Parameters.Add(new SqlParameter("@intST_ModifiedBy_US_ID", SqlDbType.Int));
                
                sqlCommand.Parameters["@intST_ID"].Value = dtgServiceTypes.DataKeys[(int)e.Item.ItemIndex];
                sqlCommand.Parameters["@chvST_Name"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceType")).Text;
                sqlCommand.Parameters["@chvST_Desc"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceTypeDesc")).Text;
                sqlCommand.Parameters["@intST_ModifiedBy_US_ID"].Value = m_intUS_ID;
                
                sqlCommand.Connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                    dtgServiceTypes.EditItemIndex = -1;
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

        public void UpdateService_Click(Object sender, DataGridCommandEventArgs e)
        {
            if (Page.IsValid)
            {
                // Get reference to dtgServices DataGrid.
                DataGrid dtgServices = (DataGrid)sender;
                
                // Get reference to current Item of parent DataGrid (dtgServiceTypes) and use that item's index
                // to return foreign key value needed by child DataGrid's (dtgServices) datasource.
                DataGridItem dgiServiceType = (DataGridItem)dtgServices.Parent.Parent.Parent;
                int intSV_ST_ID = (int)dtgServiceTypes.DataKeys[dgiServiceType.ItemIndex];
                
                // Update the database.
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "prUpdateService";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                
                sqlCommand.Parameters.Add(new SqlParameter("@intSV_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@intSV_ST_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@chvSV_Name", SqlDbType.VarChar, 50));
                sqlCommand.Parameters.Add(new SqlParameter("@chvSV_Description", SqlDbType.VarChar, 500));
                sqlCommand.Parameters.Add(new SqlParameter("@chvSV_AccountNumber", SqlDbType.VarChar, 25));
                sqlCommand.Parameters.Add(new SqlParameter("@intSV_YR_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@bitSV_IsMOEInEffect", SqlDbType.Bit));
                sqlCommand.Parameters.Add(new SqlParameter("@bitSV_IsCoreService", SqlDbType.Bit));
                sqlCommand.Parameters.Add(new SqlParameter("@bitSV_IsFTEShown", SqlDbType.Bit));
                sqlCommand.Parameters.Add(new SqlParameter("@intSV_ModifiedBy_US_ID", SqlDbType.Int));
                
                sqlCommand.Parameters["@intSV_ID"].Value = dtgServices.DataKeys[(int)e.Item.ItemIndex];
                sqlCommand.Parameters["@intSV_ST_ID"].Value = intSV_ST_ID;
                sqlCommand.Parameters["@chvSV_Name"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtService")).Text;
                sqlCommand.Parameters["@chvSV_Description"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceDesc")).Text;
                sqlCommand.Parameters["@chvSV_AccountNumber"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtAccount")).Text;
                sqlCommand.Parameters["@intSV_YR_ID"].Value = ((DropDownList)e.Item.Cells[2].FindControl("ddlYear")).SelectedValue;	
                sqlCommand.Parameters["@bitSV_IsMOEInEffect"].Value = (Convert.ToInt16(((CheckBox)e.Item.Cells[2].FindControl("chkMOE")).Checked)) * -1;
                sqlCommand.Parameters["@bitSV_IsCoreService"].Value = (Convert.ToInt16(((CheckBox)e.Item.Cells[2].FindControl("chkCore")).Checked)) * -1;
                sqlCommand.Parameters["@bitSV_IsFTEShown"].Value = (Convert.ToInt16(((CheckBox)e.Item.Cells[2].FindControl("chkFTE")).Checked)) * -1;
                sqlCommand.Parameters["@intSV_ModifiedBy_US_ID"].Value = m_intUS_ID;
                
                sqlCommand.Connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                    m_strServiceUniqueID = dtgServices.UniqueID;
                    m_intServiceEditItemIndex = -1;
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

        public void UpdateServiceDetail_Click(Object sender, DataGridCommandEventArgs e)
        {
            if (Page.IsValid)
            {
                // Get reference to dtgServiceDetails DataGrid.
                DataGrid dtgServiceDetails = (DataGrid)sender;

                // Get reference to dtgServices.
                DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;

                // Get reference to current Item of parent DataGrid (dtgServices) and use that item's index
                // to return foreign key value needed by child DataGrid's (dtgServiceDetails) datasource.
                DataGridItem dgiService = (DataGridItem)dtgServiceDetails.Parent.Parent.Parent;
                int intSD_SV_ID = (int)dtgServices.DataKeys[dgiService.ItemIndex];
                
                // Update the database.
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "prUpdateServiceDetail";
                sqlCommand.CommandType = CommandType.StoredProcedure;
                            
                sqlCommand.Parameters.Add(new SqlParameter("@intSD_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@intSD_SV_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@intSD_UM_ID", SqlDbType.Int));
                sqlCommand.Parameters.Add(new SqlParameter("@monSD_UnitCost", SqlDbType.Money));
                sqlCommand.Parameters.Add(new SqlParameter("@intSD_ModifiedBy_US_ID", SqlDbType.Int));
                
                sqlCommand.Parameters["@intSD_ID"].Value = dtgServiceDetails.DataKeys[(int)e.Item.ItemIndex];
                sqlCommand.Parameters["@intSD_SV_ID"].Value = intSD_SV_ID;
                sqlCommand.Parameters["@intSD_UM_ID"].Value = ((DropDownList)e.Item.Cells[1].FindControl("ddlUnitMeasure")).SelectedValue;
                sqlCommand.Parameters["@monSD_UnitCost"].Value = Decimal.Parse(((TextBox)e.Item.Cells[1].FindControl("txtUnitCost")).Text, System.Globalization.NumberStyles.Currency);
                sqlCommand.Parameters["@intSD_ModifiedBy_US_ID"].Value = m_intUS_ID;
                
                sqlCommand.Connection.Open();

                try
                {
                    sqlCommand.ExecuteNonQuery();
                    m_strServiceUniqueID = dtgServices.UniqueID;
                    m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;
                    m_intServiceDetailEditItemIndex = -1;
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

        public void dtgServiceTypes_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "SaveNew":
                    if (Page.IsValid)
                    {
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "prInsertServiceType";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        
                        sqlCommand.Parameters.Add(new SqlParameter("@chvST_Name", SqlDbType.VarChar, 100));
                        sqlCommand.Parameters.Add(new SqlParameter("@chvST_Desc", SqlDbType.VarChar, 3000));
                        sqlCommand.Parameters.Add(new SqlParameter("@intST_DP_ID", SqlDbType.Int));
                        sqlCommand.Parameters.Add(new SqlParameter("@intST_ModifiedBy_US_ID", SqlDbType.Int));
                        
                            
                        sqlCommand.Parameters["@chvST_Name"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceTypeNew")).Text;
                        sqlCommand.Parameters["@chvST_Desc"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceTypeDescNew")).Text;
                        sqlCommand.Parameters["@intST_DP_ID"].Value = m_strarrDepartments[tbsDeptSelector.CurrentTabIndex].Split('|')[1];
                        sqlCommand.Parameters["@intST_ModifiedBy_US_ID"].Value = m_intUS_ID;
                        
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
                    }
                    break;

                case "Expand":
                    PlaceHolder ChildRows;
                    ChildRows = (PlaceHolder)e.Item.Cells[3].FindControl("ChildRows"); 
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

        public void dtgServices_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "SaveNew":
                    if (Page.IsValid)
                    {
                        // Get reference to dtgServices DataGrid.
                        DataGrid dtgServices = (DataGrid)sender;
                        
                        // Get reference to current Item of parent DataGrid (dtgServiceTypes) and use that item's index
                        // to return foreign key value needed by child DataGrid's (dtgServices) datasource.
                        DataGridItem dgiServiceType = (DataGridItem)dtgServices.Parent.Parent.Parent;
                        int intSV_ST_ID = (int)dtgServiceTypes.DataKeys[dgiServiceType.ItemIndex];
                
                        // Insert new record into the database.
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "prInsertService";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        
                        sqlCommand.Parameters.Add(new SqlParameter("@intSV_ST_ID", SqlDbType.Int));
                        sqlCommand.Parameters.Add(new SqlParameter("@chvSV_Name", SqlDbType.VarChar, 50));
                        sqlCommand.Parameters.Add(new SqlParameter("@chvSV_Description", SqlDbType.VarChar, 500));
                        sqlCommand.Parameters.Add(new SqlParameter("@chvSV_AccountNumber", SqlDbType.VarChar, 25));
                        sqlCommand.Parameters.Add(new SqlParameter("@intSV_YR_ID", SqlDbType.Int));
                        sqlCommand.Parameters.Add(new SqlParameter("@bitSV_IsMOEInEffect", SqlDbType.Bit));
                        sqlCommand.Parameters.Add(new SqlParameter("@bitSV_IsCoreService", SqlDbType.Bit));
                        sqlCommand.Parameters.Add(new SqlParameter("@bitSV_IsFTEShown", SqlDbType.Bit));

                        sqlCommand.Parameters.Add(new SqlParameter("@intSV_ModifiedBy_US_ID", SqlDbType.Int));
                        
                        sqlCommand.Parameters["@intSV_ST_ID"].Value = intSV_ST_ID;	
                        sqlCommand.Parameters["@chvSV_Name"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceNew")).Text;
                        sqlCommand.Parameters["@chvSV_Description"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtServiceDescNew")).Text;
                        sqlCommand.Parameters["@chvSV_AccountNumber"].Value = ((TextBox)e.Item.Cells[2].FindControl("txtAccountNew")).Text;
                        sqlCommand.Parameters["@intSV_YR_ID"].Value = ((DropDownList)e.Item.Cells[2].FindControl("ddlYearNew")).SelectedValue;	
                        sqlCommand.Parameters["@bitSV_IsMOEInEffect"].Value = (Convert.ToInt16(((CheckBox)e.Item.Cells[2].FindControl("chkMOENew")).Checked)) * -1;
                        sqlCommand.Parameters["@bitSV_IsCoreService"].Value = (Convert.ToInt16(((CheckBox)e.Item.Cells[2].FindControl("chkCoreNew")).Checked)) * -1;
                        sqlCommand.Parameters["@bitSV_IsFTEShown"].Value = (Convert.ToInt16(((CheckBox)e.Item.Cells[2].FindControl("chkFTENew")).Checked)) * -1;
                        sqlCommand.Parameters["@intSV_ModifiedBy_US_ID"].Value = m_intUS_ID;
                        
                        sqlCommand.Connection.Open();
                        try
                        {
                            sqlCommand.ExecuteNonQuery();
                            m_blnNewServiceRecord = false;
                        }
                        catch (SqlException exc)
                        {
                            Response.Write("ERROR: Could not update record, SQL Exception<br>");
                            Response.Write(exc.Message + "<br>" + exc.StackTrace);
                        }
                        sqlCommand.Connection.Close();
                        
                        m_strServiceUniqueID = dtgServices.UniqueID;
                        BindGrid();
                    }
                    break;

                case "Expand":
                    PlaceHolder plhServiceDetails;
                    plhServiceDetails = (PlaceHolder)e.Item.Cells[3].FindControl("plhServiceDetails"); 
                    plhServiceDetails.Visible = !plhServiceDetails.Visible;
                
                    ImageButton imgbtnExpand;
                    imgbtnExpand = (ImageButton)e.Item.Cells[1].FindControl("imgbtnExpandServiceDetails");

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

        public void dtgServiceDetails_ItemCommand(object sender, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "SaveNew":
                    if (Page.IsValid)
                    {
                        // Get reference to dtgServices DataGrid.
                        DataGrid dtgServiceDetails = (DataGrid)sender;

                        // Get reference to dtgServices.
                        DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;
                
                        // Get reference to current Item of parent DataGrid (dtgServices) and use that item's index
                        // to return foreign key value needed by child DataGrid's (dtgServiceDetails) datasource.
                        DataGridItem dgiService = (DataGridItem)dtgServiceDetails.Parent.Parent.Parent;
                        int intSD_SV_ID = (int)dtgServices.DataKeys[dgiService.ItemIndex];
                
                        // Insert new record into the database.
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandText = "prInsertServiceDetail";
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        
                        sqlCommand.Parameters.Add(new SqlParameter("@intSD_SV_ID", SqlDbType.Int));
                        sqlCommand.Parameters.Add(new SqlParameter("@intSD_UM_ID", SqlDbType.Int));
                        sqlCommand.Parameters.Add(new SqlParameter("@monSD_UnitCost", SqlDbType.Money));
                        sqlCommand.Parameters.Add(new SqlParameter("@intSD_ModifiedBy_US_ID", SqlDbType.Int));
                
                        sqlCommand.Parameters["@intSD_SV_ID"].Value = intSD_SV_ID;
                        sqlCommand.Parameters["@intSD_UM_ID"].Value = ((DropDownList)e.Item.Cells[1].FindControl("ddlUnitMeasureNew")).SelectedValue;
                        sqlCommand.Parameters["@monSD_UnitCost"].Value = Decimal.Parse(((TextBox)e.Item.Cells[1].FindControl("txtUnitCostNew")).Text, System.Globalization.NumberStyles.Currency);
                        sqlCommand.Parameters["@intSD_ModifiedBy_US_ID"].Value = m_intUS_ID;
                        
                        sqlCommand.Connection.Open();
                        try
                        {
                            sqlCommand.ExecuteNonQuery();
                            m_blnNewServiceDetailRecord = false;
                        }
                        catch (SqlException exc)
                        {
                            Response.Write("ERROR: Could not update record, SQL Exception<br>");
                            Response.Write(exc.Message + "<br>" + exc.StackTrace);
                        }
                        sqlCommand.Connection.Close();
                        
                        m_strServiceUniqueID = dtgServices.UniqueID;
                        m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;
                        BindGrid();
                    }
                    break;
            }
        }

        public void NewCancel_Click(object sender, System.EventArgs e)
        {
            m_blnNewRecord = false;
            BindGrid();
        }

        public void NewServiceCancel_Click(object sender, System.EventArgs e)
        {
            m_blnNewServiceRecord = false;
            m_strServiceUniqueID = ((DataGrid)sender).UniqueID;
            BindGrid();
        }

        public void NewServiceDetailCancel_Click(object sender, System.EventArgs e)
        {
            DataGrid dtgServiceDetails = (DataGrid)sender;
            DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;
            
            m_blnNewServiceDetailRecord = false;
            m_strServiceUniqueID = dtgServices.UniqueID;
            m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;
            BindGrid();
        }

        protected void lnkNew_Click(object sender, System.EventArgs e)
        {
            dtgServiceTypes.EditItemIndex = -1;
            m_blnNewRecord = true;
            BindGrid();
        }

        public void lnkNewService_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkNewService = (LinkButton)sender;
            DataGridItem dgiServiceType = (DataGridItem)lnkNewService.Parent.Parent.Parent;
            DataGrid dtgServices = ((DataGrid)dgiServiceType.Cells[3].FindControl("dtgServices"));
                    
            m_intServiceEditItemIndex = -1;
            m_blnNewServiceRecord = true;
            m_strServiceUniqueID = dtgServices.UniqueID;

            m_dttYear = CreateYearTable();
            BindGrid();
        }

        public void lnkNewServiceDetail_Click(object sender, System.EventArgs e)
        {
            LinkButton lnkNewServiceDetail = (LinkButton)sender;
            DataGridItem dgiService = (DataGridItem)lnkNewServiceDetail.Parent.Parent.Parent;
            DataGrid dtgServiceDetails = ((DataGrid)dgiService.Cells[3].FindControl("dtgServiceDetails"));
            DataGrid dtgServices = (DataGrid)dtgServiceDetails.Parent.Parent.Parent.Parent.Parent;

            m_intServiceDetailEditItemIndex = -1;
            m_blnNewServiceDetailRecord = true;
            m_strServiceUniqueID = dtgServices.UniqueID;
            m_strServiceDetailUniqueID = dtgServiceDetails.UniqueID;

            m_dttUnitMeasure = CreateUnitMeasureTable();
            BindGrid();
        }

        public DataTable CreateUnitMeasureTable()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "prSelectUnitMeasures";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            DataTable dttUnitMeasure = new DataTable();
            DataRow dtrUnitMeasure;
            DataColumn dtcUnitMeasureName = new DataColumn("UM_Name");
            dttUnitMeasure.Columns.Add(dtcUnitMeasureName);
            DataColumn dtcUnitMeasureId = new DataColumn("UM_ID");
            dttUnitMeasure.Columns.Add(dtcUnitMeasureId);

            dtrUnitMeasure = dttUnitMeasure.NewRow();
            dtrUnitMeasure[0] = "";
            dtrUnitMeasure[1] = "0";
            dttUnitMeasure.Rows.Add(dtrUnitMeasure);
            
            sqlConnection.Open();
            SqlDataReader sqlReader = sqlCommand.ExecuteReader();
            while(sqlReader.Read())
            {
                dtrUnitMeasure = dttUnitMeasure.NewRow();
                dtrUnitMeasure[0] = sqlReader[0];
                dtrUnitMeasure[1] = sqlReader[1];
                dttUnitMeasure.Rows.Add(dtrUnitMeasure);
            }
            sqlConnection.Close();
            sqlReader.Close();
            
            return dttUnitMeasure;
        }

        private DataTable CreateYearTable()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "prSelectAllYears";
            sqlCommand.CommandType = CommandType.StoredProcedure;

            DataTable dttYear = new DataTable();
            DataRow dtrYear;
            DataColumn dtcYearDesc = new DataColumn("YR_Desc");
            dttYear.Columns.Add(dtcYearDesc);
            DataColumn dtcYearId = new DataColumn("YR_ID");
            dttYear.Columns.Add(dtcYearId);

            dtrYear = dttYear.NewRow();
            dtrYear[0] = "";
            dtrYear[1] = "0";
            dttYear.Rows.Add(dtrYear);
            
            sqlConnection.Open();
            SqlDataReader sqlReader = sqlCommand.ExecuteReader();
            while(sqlReader.Read())
            {
                dtrYear = dttYear.NewRow();
                dtrYear[0] = sqlReader[0];
                dtrYear[1] = sqlReader[1];
                dttYear.Rows.Add(dtrYear);
            }
            sqlConnection.Close();
            sqlReader.Close();
            
            return dttYear;
        }
        
        protected string GetYesNo(bool blnChecked)
        {
            if (blnChecked)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        protected DataTable GetYearList()
        {
            // m_dttYear is a class-level DataTable
            return m_dttYear;
        }

        protected int GetSelectedYear(string strYR_Desc)
        {
            for(int i=0; i < m_dttYear.Rows.Count; i++)
            {
                if(m_dttYear.Rows[i]["YR_Desc"].ToString() == strYR_Desc)
                {
                    return i;
                }
            }

            //If there is no match, return 0
            return 0;
        }

        protected DataTable GetUnitMeasureList()
        {
            // m_dttUnitMeasure is a class-level DataTable
            return m_dttUnitMeasure;
        }

        protected int GetSelectedUnitMeasure(string strUM_Name)
        {
            for(int i=0; i < m_dttUnitMeasure.Rows.Count; i++)
            {
                if(m_dttUnitMeasure.Rows[i]["UM_Name"].ToString() == strUM_Name)
                {
                    return i;
                }
            }

            //If there is no match, return 0
            return 0;
        }

        protected void validateNumeric(object source, ServerValidateEventArgs args)
        {
            bool blnIsValid = true;
            foreach (char c in args.Value.ToCharArray())
            {
                if (!Char.IsNumber(c) && !Char.Equals(c, '.') && !Char.Equals(c, ','))
                {
                    blnIsValid = false;
                    break;
                };
            }
            args.IsValid = blnIsValid;
        }

        protected void validateCurrency(object source, ServerValidateEventArgs args)
        {
            bool blnIsValid = true;
            foreach (char c in args.Value.ToCharArray())
            {
                if (!Char.IsNumber(c) && !Char.Equals(c, '$') && !Char.Equals(c, '.') && !Char.Equals(c, ','))
                {
                    blnIsValid = false;
                    break;
                };
            }
            args.IsValid = blnIsValid;
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


