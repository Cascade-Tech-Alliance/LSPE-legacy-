using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSPE.UserControls;

namespace LSPE
{
	/// <summary>
	/// Summary description for plans.
	/// </summary>
	public partial class plans : CustomPage
	{
		SqlConnection sqlConnection;
		CustomPrincipal cprPrincipal = (CustomPrincipal)HttpContext.Current.User;
		bool m_blnEditMode = false;
		bool m_blnNewRecord = false;
		//bool m_blnAllowRecommendedUnitsEdit = false;
		bool m_blnAllowTransitUnitsEdit = false;
		bool m_blnAllowPurchasedUnitsEdit = false;
		string m_strTransitUnitsValue;
		int m_intSV_ID = 0;
		int m_intSD_ID = 0;
		DataTable m_dttServiceDetail;
		decimal m_decTransitUnits = 0;
		//decimal m_decRecommendedUnits = 0;
		decimal m_decResolutionUnits = 0;
		decimal m_decResolutionCost = 0;
		decimal m_decContractedUnits = 0;
		decimal m_decContractedCost = 0;
		decimal m_decTotalUnits = 0;
		decimal m_decTotalCost = 0;
		decimal m_decResolutionDollarsUsed = 0;
		decimal m_decContractedDollarsPaid = 0;
		string m_strPlanDetailNotes = "";
		decimal m_decUnitCost = 0;
		string m_strUnitMeasure = "";
		bool m_blnIsPlanLevelMOE = false;
		bool m_blnIsServiceLevelMOE = false;
		bool m_blnIsCoreService = false;
		int m_intUS_ID = 0;
		bool m_blnIsFinalized = false;
		string[] m_strarrDepartments;
				
		protected void Page_Load(object sender, System.EventArgs e)
		{
			m_intUS_ID = cprPrincipal.UserId;

			string strConnection = (string)Application["strConnection"];
			sqlConnection = new SqlConnection(strConnection);
			
			m_strarrDepartments = (string[])Session["strarrDepartments"];
			
			if (cprPrincipal.IsInRole("Administrator"))
			{
				//m_blnAllowRecommendedUnitsEdit = true;
				m_blnAllowTransitUnitsEdit = true;
				m_blnAllowPurchasedUnitsEdit = true;
			}
			else if (cprPrincipal.IsInRole("ESD Department Level"))
			{
				//m_blnAllowRecommendedUnitsEdit = true;
				m_blnAllowPurchasedUnitsEdit = false;
				m_blnAllowTransitUnitsEdit = false;
			}
			else
			{
				m_blnAllowTransitUnitsEdit = false;
				m_blnAllowPurchasedUnitsEdit = true;
			}

			foreach(string strDepartment in m_strarrDepartments)
			{
				tbsDeptSelector.Tabs.Add(strDepartment.Split('|')[0]);
			}

			//lblYear.Visible = false;
			//ddlYears.Visible = !cprPrincipal.IsInRole("District Team Member") 
			//	&& !cprPrincipal.IsInRole("District Primary");
			
			m_dttServiceDetail = CreateServiceDetailTable(m_intSD_ID);

			if (!IsPostBack)
			{
				BindDistrictDDL();
				BindYearDDL();
				if (Session["intDS_ID"] != null)
				{
					ddlDistricts.SelectedValue = Session["intDS_ID"].ToString();
				}
				if (Session["intYR_ID"] != null)
				{
					ddlYears.SelectedValue = Session["intYR_ID"].ToString();
				}				
				ShowPlan();
			}   

		}
		
		public void SelectionChanged(object sender, TabStrip.SelectionChangedEventArgs e)
		{
			dtgPlanDetails.EditItemIndex = -1;
			dtgPlanDetails.CurrentPageIndex = 0;
			ShowPlan();			
		}
		
		public void ShowPlan()
		{
			bool blnShowPlan = false;
			int intDS_ID = 0;
			int intYR_ID = 0;

			if (Session["intDS_ID"] != null && Session["intYR_ID"] != null)
			{
				blnShowPlan = true;
				intDS_ID = (int)Session["intDS_ID"];
				intYR_ID = (int)Session["intYR_ID"];
			}
						
			if (blnShowPlan)
			{				
				ShowPlanStatus();

				if (BindPlan())
				{
					int intCR_YR_ID = (int)Application["CR_YR_ID"];
					if (cprPrincipal.IsInRole("District Team Member") || cprPrincipal.IsInRole("ESD Department Level"))
					{
						lnkNew.Visible = false;
					}

					if (cprPrincipal.IsInRole("District Primary"))
					{
						lnkNew.Visible = ((intYR_ID == intCR_YR_ID) && !m_blnIsFinalized);
					}				
									
					lblNoPlanMessage.Visible = false;
					plhPlan.Visible = true;
					hypPlanReport.Visible = true;
					AdjustPlanDisplay();
					BindGrid();
					//if (cprPrincipal.IsInRole("Administrator") || cprPrincipal.IsInRole("ESD Department Level"))
					//{
					hypPlanReport.NavigateUrl = "~/report_popup.aspx?report=PlanHistorical";//&intDS_ID=
						//    + Session["intDS_ID"].ToString() + "&intYR_ID=" + Session["intYR_ID"].ToString();
						//hypPlanReport.NavigateUrl = "http://198.237.190.4/ReportServer?%2frxpress%2fPlan&rc%3aParameters=true&intDS_ID=" 
						//    + Session["intDS_ID"].ToString() + "&intYR_ID=" + Session["intYR_ID"].ToString();
					hypPlanReport.Target = "_blank";
					//}
					//else
					//{
					//    hypPlanReport.NavigateUrl = "javascript:openDialog('reportviewer.aspx?report=/rxpress/plan&intDS_ID=" 
					//        + Session["intDS_ID"].ToString() + "&intYR_ID=" + Session["intYR_ID"].ToString() + "', 675, 600, '', 'Plan');";
					//}
					//ddlDistricts.SelectedIndex = 0;
					//ddlYears.SelectedIndex = 0;
					ddlDistricts.SelectedValue = Session["intDS_ID"].ToString();
					ddlYears.SelectedValue = Session["intYR_ID"].ToString();
				}
				else
				{
					lblNoPlanMessage.Visible = true;
					plhPlan.Visible = false;
					hypPlanReport.Visible = false;
				}
			}

		}

		public bool BindPlan()
		{
			bool blnBindPlan;
			decimal slotadjustment;
			int intDS_ID = (int)Session["intDS_ID"];
			int intYR_ID = (int)Session["intYR_ID"];
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prSelectPlanInfo";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			sqlCommand.Parameters.Add(new SqlParameter("@intDS_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intDS_ID"].Value = intDS_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@intYR_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intYR_ID"].Value = intYR_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@chvDS_Name", SqlDbType.VarChar, 50));
			sqlCommand.Parameters["@chvDS_Name"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@chvYR_Desc", SqlDbType.VarChar, 9));
			sqlCommand.Parameters["@chvYR_Desc"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monPL_AllottedResolutionDollars", SqlDbType.Money));
			sqlCommand.Parameters["@monPL_AllottedResolutionDollars"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monPL_CarryoverResolutionDollars", SqlDbType.Money));
			sqlCommand.Parameters["@monPL_CarryoverResolutionDollars"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monSA_SlotAdjustment", SqlDbType.Money));
			sqlCommand.Parameters["@monSA_SlotAdjustment"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@txtPL_Notes", SqlDbType.VarChar, 8000));
			sqlCommand.Parameters["@txtPL_Notes"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monTotalTransitDollars", SqlDbType.Money));
			sqlCommand.Parameters["@monTotalTransitDollars"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monTotalSpentResolutionDollars", SqlDbType.Money));
			sqlCommand.Parameters["@monTotalSpentResolutionDollars"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monTotalSpentResolutionPlusTransitDollars", SqlDbType.Money));
			sqlCommand.Parameters["@monTotalSpentResolutionPlusTransitDollars"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@monTotalSpentContractedDollars", SqlDbType.Money));
			sqlCommand.Parameters["@monTotalSpentContractedDollars"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@bitPL_IsFinalized", SqlDbType.Bit));
			sqlCommand.Parameters["@bitPL_IsFinalized"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters.Add(new SqlParameter("@bitPL_AllowDistrictEdits", SqlDbType.Bit));
			sqlCommand.Parameters["@bitPL_AllowDistrictEdits"].Direction = ParameterDirection.Output;

			sqlConnection.Open();
			sqlCommand.ExecuteNonQuery();
			
			if (sqlCommand.Parameters["@chvDS_Name"].Value != DBNull.Value)
			{
				string strDistrict = (string)sqlCommand.Parameters["@chvDS_Name"].Value;
				string strYearDesc = (string)sqlCommand.Parameters["@chvYR_Desc"].Value;

				lblPlan.Text = strDistrict + ", " + strYearDesc;

				hypPlanReport.Text = "Print preview " + strDistrict + " " + strYearDesc + " Local Service Plan";
				hypPlanReport.Attributes.Add("onMouseOver", "status='Print preview Local Service Plan.'; return true;");
				hypPlanReport.Attributes.Add("onMouseOut", "status=''; return true;");

				hypServicesReport.Text = "Print preview descriptions of all " + strYearDesc + " Resolution Services offered";
				hypServicesReport.NavigateUrl = "~/report_popup.aspx?report=ServicesDescsOnly";
				hypServicesReport.Target = "_blank";
				//hypServicesReport.NavigateUrl = "javascript:openDialog('http://www.wesd.org/rxpress/service_descs.aspx?YR_ID=" + intYR_ID.ToString() 
				//    + "', 675, 600, '', 'core');";
				hypServicesReport.Attributes.Add("onMouseOver", "status='Print preview descriptions of Resolution Services offered.'; return true;");
				hypServicesReport.Attributes.Add("onMouseOut", "status=''; return true;");

				txtResolutionAvailable.Text = Convert.ToDecimal(sqlCommand.Parameters["@monPL_AllottedResolutionDollars"].Value).ToString("C");
				txtResolutionCarryover.Text = Convert.ToDecimal(sqlCommand.Parameters["@monPL_CarryoverResolutionDollars"].Value).ToString("C");
				lblResolutionAvailable.Text = Convert.ToDecimal(sqlCommand.Parameters["@monPL_AllottedResolutionDollars"].Value).ToString("C");
				lblResolutionCarryover.Text = Convert.ToDecimal(sqlCommand.Parameters["@monPL_CarryoverResolutionDollars"].Value).ToString("C");
				if (Convert.IsDBNull(sqlCommand.Parameters["@monSA_SlotAdjustment"].Value)) {
					btnSlotAdjustment.Visible = false; 
					lblSlotAdjustment.Visible = true;
					slotadjustment = 0;
				}
				else {
					slotadjustment = Convert.ToDecimal(sqlCommand.Parameters["@monSA_SlotAdjustment"].Value);
					btnSlotAdjustment.Text = slotadjustment.ToString("C");
					btnSlotAdjustment.Visible = true;
					lblSlotAdjustment.Visible = false;
				}
				lblResolutionTotalAvailabe.Text = (Convert.ToDecimal(sqlCommand.Parameters["@monPL_AllottedResolutionDollars"].Value) 
					+ Convert.ToDecimal(sqlCommand.Parameters["@monPL_CarryoverResolutionDollars"].Value)+ slotadjustment).ToString("C");
				lblResolutionTotalRemaining.Text = (Convert.ToDecimal(sqlCommand.Parameters["@monPL_AllottedResolutionDollars"].Value) 
					+ Convert.ToDecimal(sqlCommand.Parameters["@monPL_CarryoverResolutionDollars"].Value ) + slotadjustment
					- Convert.ToDecimal(sqlCommand.Parameters["@monTotalSpentResolutionDollars"].Value)
					- Convert.ToDecimal(sqlCommand.Parameters["@monTotalTransitDollars"].Value)).ToString("C");
				// = (string)sqlCommand.Parameters["@txtPL_Notes"].Value;
				lblTransitUsed.Text = Convert.ToDecimal(sqlCommand.Parameters["@monTotalTransitDollars"].Value).ToString("C");
				lblResolutionUsed.Text = Convert.ToDecimal(sqlCommand.Parameters["@monTotalSpentResolutionDollars"].Value).ToString("C");
				lblResolutionTotalUsed.Text = Convert.ToDecimal(sqlCommand.Parameters["@monTotalSpentResolutionPlusTransitDollars"].Value).ToString("C");
				lblContractedTotalUsed.Text = Convert.ToDecimal(sqlCommand.Parameters["@monTotalSpentContractedDollars"].Value).ToString("C");
				lblOverallTotalUsed.Text = (Convert.ToDecimal(sqlCommand.Parameters["@monTotalSpentResolutionPlusTransitDollars"].Value) 
					+ Convert.ToDecimal(sqlCommand.Parameters["@monTotalSpentContractedDollars"].Value)).ToString("C");                
				// = (bool)sqlCommand.Parameters["@bitPL_IsFinalized"].Value;
				// = (bool)sqlCommand.Parameters["@bitPL_AllowDistrictEdits"].Value;
				
				blnBindPlan = true;
			}
			else
			{
				blnBindPlan = false;
			}

			sqlConnection.Close();
			return blnBindPlan;
		}

		public void AdjustPlanDisplay()
		{
			txtResolutionAvailable.Visible = m_blnEditMode;
			txtResolutionCarryover.Visible = m_blnEditMode;
			lblResolutionAvailable.Visible = !m_blnEditMode;
			lblResolutionCarryover.Visible = !m_blnEditMode;
			lnkEditPlan.Visible = !m_blnEditMode && cprPrincipal.IsInRole("Administrator");
			lnkSavePlan.Visible = m_blnEditMode;
			lnkCancelPlan.Visible = m_blnEditMode;
			vlsFundAllotments.Visible = m_blnEditMode;
		}

		 public void BindGrid()
		{
			int intDS_ID = (int)Session["intDS_ID"]; 
			int intYR_ID = (int)Session["intYR_ID"];

			SqlDataAdapter sqlAdapter = new SqlDataAdapter("prSelectPlanDetails", sqlConnection);
			sqlAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

			SqlParameter sqlParameter;			
			sqlParameter = sqlAdapter.SelectCommand.Parameters.Add("@intPL_DS_ID", SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.Input;
			sqlParameter.Value = intDS_ID;
		
			sqlParameter = sqlAdapter.SelectCommand.Parameters.Add("@intPL_YR_ID", SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.Input;
			sqlParameter.Value = intYR_ID;

			sqlParameter = sqlAdapter.SelectCommand.Parameters.Add("@intDP_ID", SqlDbType.Int);
			sqlParameter.Direction = ParameterDirection.Input;
			sqlParameter.Value = m_strarrDepartments[tbsDeptSelector.CurrentTabIndex].Split('|')[1];	
					
			DataSet dstPlan = new DataSet();
			sqlAdapter.TableMappings.Add("Table", "PlanDetails");
			sqlAdapter.TableMappings.Add("Table1", "PreviousPlanDetails");
					
			sqlConnection.Open();
			sqlAdapter.Fill(dstPlan);
			sqlConnection.Close();
			
			dstPlan.EnforceConstraints = false;
			
			DataRelation dataRelation = dstPlan.Relations.Add("PastPlanDetails", 
				dstPlan.Tables["PlanDetails"].Columns["SV_Name"], 
				dstPlan.Tables["PreviousPlanDetails"].Columns["SV_Name"]);

			dtgPlanDetails.DataSource=dstPlan;
			dtgPlanDetails.DataMember="PlanDetails";
			dtgPlanDetails.DataBind();
		}

		public void ItemCreated(Object sender, DataGridItemEventArgs e)
		{			
			ListItemType itemType = e.Item.ItemType;
			
			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
			{
				LinkButton lnkDelete = (LinkButton)e.Item.Cells[0].FindControl("lnkDelete");
				lnkDelete.Attributes.Add("onClick", "return confirm_delete();");
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

		public void Edit_Click(Object sender, DataGridCommandEventArgs e) 
		{
			dtgPlanDetails.EditItemIndex = (int)e.Item.ItemIndex;
			BindGrid();
			m_blnEditMode = false;
			AdjustPlanDisplay();
		}

		public void Cancel_Click(Object sender, DataGridCommandEventArgs e)
		{
			dtgPlanDetails.EditItemIndex = -1;
			BindGrid();
		}

		public void Delete_Click(Object sender, DataGridCommandEventArgs e)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prUpdatePlanDetailIsDeleted";
			sqlCommand.CommandType = CommandType.StoredProcedure;
							
			sqlCommand.Parameters.Add(new SqlParameter("@intPD_ID", SqlDbType.Int));				
			sqlCommand.Parameters["@intPD_ID"].Value = dtgPlanDetails.DataKeys[(int)e.Item.ItemIndex];
				
			sqlCommand.Connection.Open();

			try
			{
				sqlCommand.ExecuteNonQuery();
				dtgPlanDetails.EditItemIndex = -1;
			}
			catch (SqlException exc)
			{
				Response.Write("ERROR: Could not update record, SQL Exception<br>");
				Response.Write(exc.Message + "<br>" + exc.StackTrace);
			}

			sqlCommand.Connection.Close();
			
			ShowPlan();
		}

		public void dtgPlanDetails_OnPageIndexChanged(Object sender, DataGridPageChangedEventArgs e)
		{
			dtgPlanDetails.CurrentPageIndex = e.NewPageIndex;
			ShowPlan();
		}

		public void ItemDataBound(Object sender, DataGridItemEventArgs e)
		{
			ListItemType itemType = e.Item.ItemType;
			int intYR_ID = (int)Session["intYR_ID"];
			int intCR_YR_ID = (int)Application["CR_YR_ID"];
			DataRowView drvPlanDetail = (DataRowView) e.Item.DataItem;
			bool blnIsCoreService = false;
			bool blnIsTransitArea = false;
			
			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
			{
				// If CostType is "Whole Cost" and UnitMeasure is "ADM" then the amount of 
				// resolution units puchased is a calculated percentage of the district's ADM
				// as compared to all districts' ADM.  In this case we do not want to treat the 
				// amount purchased in terms of units, so we do not show a unit cost
				// or any unit values.  Only the calculated cost is shown under ResolutionCost
				// and TotalCost.  Currently, we do this for non administrators only because the
				// calculation utility is not yet implemented and requires an administrator to manually
				// calculate and enter the UnitCost and number of units purchased which then effects the cost values.

				// The above has been changed to key off of whether or not the service is flagged as "Core".
				if (!cprPrincipal.IsInRole("Administrator"))
				{
					//int intCT_ID = (int)drvPlanDetail["SD_CT_ID"];
					//int intUM_ID = (int)drvPlanDetail["SD_UM_ID"];
					blnIsCoreService = (bool)drvPlanDetail["SV_IsCoreService"];					
					//bool blnIsCostADMPercentage = (intCT_ID == 2 && intUM_ID == 2);
					bool blnIsCostADMPercentage = blnIsCoreService;
					((Label)e.Item.Cells[2].FindControl("lblUnitCost")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblTransitUnits")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblResolutionUnits")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblContractedUnits")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblContractedCost")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblTotalUnits")).Visible = !blnIsCostADMPercentage;
				}
				
				LinkButton lnkDelete = (LinkButton)e.Item.Cells[2].FindControl("lnkDelete");
				lnkDelete.Attributes.Add("onClick", "return confirm_delete();");
			}
			
			if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem || itemType == ListItemType.EditItem)
			{
				if (drvPlanDetail != null)
				{
					blnIsCoreService = (bool)drvPlanDetail["SV_IsCoreService"];
					m_blnIsServiceLevelMOE = (bool)drvPlanDetail["SV_IsMOEInEffect"];
					m_blnIsPlanLevelMOE = (bool)drvPlanDetail["PD_IsMOEInEffect"];
					blnIsTransitArea = (bool)drvPlanDetail["SV_IsTransitArea"];
					
					LinkButton lnkCore = (LinkButton)e.Item.Cells[2].FindControl("lnkCore");
					if (blnIsCoreService)
					{
						lnkCore.Visible = true;	
						lnkCore.Attributes.Add("Href", "javascript:openDialog('core_definition.htm', 250, 120, '', 'core');");
						lnkCore.Attributes.Add("onMouseOver", "status='View definition of a core service.'; return true;");
						lnkCore.Attributes.Add("onMouseOut", "status=''; return true;");
						lnkCore.ToolTip = "This is a core service.  Click to view a full definition.";
					}
					else
					{
						lnkCore.Visible = false;
					}

					LinkButton lnkMOE = (LinkButton)e.Item.Cells[2].FindControl("lnkMOE");
					if (m_blnIsServiceLevelMOE || m_blnIsPlanLevelMOE)
					{
						lnkMOE.Visible = true;	
						lnkMOE.Attributes.Add("Href", "javascript:openDialog('moe_definition.htm', 250, 175, '', 'moe');");
						lnkMOE.Attributes.Add("onMouseOver", "status='View definition of a maintenance of effort service.'; return true;");
						lnkMOE.Attributes.Add("onMouseOut", "status=''; return true;");
						lnkMOE.ToolTip = "This is a maintenance-of-effort service.  Click to view a full definition.";
					}
					else
					{
						lnkMOE.Visible = false;
					}

					// Allow edit/delete access based on role and context specific permissions.
					if (itemType == ListItemType.Item || itemType == ListItemType.AlternatingItem)
					{
						if (cprPrincipal.IsInRole("District Team Member"))
						{
							((LinkButton)e.Item.Cells[0].FindControl("lnkEdit")).Visible = false;
							((LinkButton)e.Item.Cells[0].FindControl("lnkDelete")).Visible = false;
						}
						else if (cprPrincipal.IsInRole("District Primary"))
						{
							if ((intYR_ID == intCR_YR_ID) && !m_blnIsFinalized) 
							{
								((LinkButton)e.Item.Cells[0].FindControl("lnkEdit")).Visible = !blnIsCoreService;
								((LinkButton)e.Item.Cells[0].FindControl("lnkDelete")).Visible = !blnIsCoreService;
							}
							else
							{
								((LinkButton)e.Item.Cells[0].FindControl("lnkEdit")).Visible = false;
								((LinkButton)e.Item.Cells[0].FindControl("lnkDelete")).Visible = false;
							}
						}
						else if (cprPrincipal.IsInRole("ESD Department Level"))
						{
							if ((intYR_ID == intCR_YR_ID) && !m_blnIsFinalized)
							{
								((LinkButton)e.Item.Cells[0].FindControl("lnkEdit")).Visible = true;
							}
							else
							{
								((LinkButton)e.Item.Cells[0].FindControl("lnkEdit")).Visible = false;
							}

							((LinkButton)e.Item.Cells[0].FindControl("lnkDelete")).Visible = false;
						}
					}
				}
			}

			if (itemType == ListItemType.Header)
			{
				e.Item.Visible = m_blnNewRecord;
				
				// Select service chosen before auto post back.
				if (m_intSV_ID > 0)
				{
					DropDownList ddlService = ((DropDownList)e.Item.Cells[2].FindControl("ddlServiceNew"));
					for (int i=0; i<ddlService.Items.Count; i++)
					{
						if (Convert.ToInt32(ddlService.Items[i].Value) == m_intSV_ID)
						{
							ddlService.SelectedIndex = i;
							break;
						}
					}
				}

				ShowEditControls(e.Item, blnIsTransitArea);
			}

			if (itemType == ListItemType.EditItem)
			{
				DropDownList ddlServiceDetail = (DropDownList) e.Item.Cells[2].FindControl("ddlServiceDetail");
				int intSV_ID = (int)drvPlanDetail.Row["SV_ID"];
				ddlServiceDetail.DataSource = CreateServiceDetailTable(intSV_ID);
				ddlServiceDetail.DataTextField = "UM_Name";
				ddlServiceDetail.DataValueField = "SD_ID";
				ddlServiceDetail.DataBind();
				
				// If ServiceDetail chosen is the same as datasource value then pull field values from datasource.
				if (m_intSD_ID == 0 || m_intSD_ID == (int)drvPlanDetail.Row["PD_SD_ID"])
				{
					m_decUnitCost = (decimal)drvPlanDetail.Row["UnitCost"];
					m_decTransitUnits = (decimal)drvPlanDetail.Row["TransitUnits"];
					m_decResolutionUnits = (decimal)drvPlanDetail.Row["ResolutionUnits"];
					m_decResolutionCost = (decimal)drvPlanDetail.Row["ResolutionCost"];
					m_decContractedUnits = (decimal)drvPlanDetail.Row["ContractedUnits"];
					m_decContractedCost = (decimal)drvPlanDetail.Row["ContractedCost"];
					m_decTotalUnits = (decimal)drvPlanDetail.Row["TotalUnits"];
					m_decTotalCost = (decimal)drvPlanDetail.Row["TotalCost"];
					m_decResolutionDollarsUsed = (decimal)drvPlanDetail.Row["PD_ResolutionDollarsUsed"];
					m_decContractedDollarsPaid = (decimal)drvPlanDetail.Row["PD_ContractedDollarsPaid"];
					m_strUnitMeasure = drvPlanDetail["UnitOfMeasure"].ToString();
					m_blnIsPlanLevelMOE = (bool)drvPlanDetail["PD_IsMOEInEffect"];
				}
				((Label) e.Item.Cells[2].FindControl("lblUnitCostEdit")).Text = m_decUnitCost.ToString("C");
				((Label) e.Item.Cells[2].FindControl("lblTransitUnitsEdit")).Text = m_decTransitUnits.ToString("N2");
				((TextBox) e.Item.Cells[2].FindControl("txtTransitUnits")).Text = m_decTransitUnits.ToString("N4");
				((Label) e.Item.Cells[2].FindControl("lblResolutionUnitsEdit")).Text = m_decResolutionUnits.ToString("N2");
				((TextBox) e.Item.Cells[2].FindControl("txtResolutionUnits")).Text = m_decResolutionUnits.ToString("N4");
				((Label) e.Item.Cells[2].FindControl("lblResolutionCostEdit")).Text = m_decResolutionCost.ToString("C");
				((Label) e.Item.Cells[2].FindControl("lblContractedUnitsEdit")).Text = m_decContractedUnits.ToString("N2");
				((TextBox) e.Item.Cells[2].FindControl("txtContractedUnits")).Text = m_decContractedUnits.ToString("N4");
				((TextBox)e.Item.Cells[2].FindControl("txtResolutionDollarsUsed")).Text = m_decResolutionDollarsUsed.ToString("C");
				((TextBox)e.Item.Cells[2].FindControl("txtContractedDollarsPaid")).Text = m_decContractedDollarsPaid.ToString("C");
				((Label) e.Item.Cells[2].FindControl("lblContractedCostEdit")).Text = m_decContractedCost.ToString("C");
				((Label) e.Item.Cells[2].FindControl("lblTotalUnitsEdit")).Text = m_decTotalUnits.ToString("N2");
				((Label) e.Item.Cells[2].FindControl("lblTotalCostEdit")).Text = m_decTotalCost.ToString("C");
				((Label)e.Item.Cells[2].FindControl("lblResolutionDollarsUsedEdit")).Text = m_decResolutionDollarsUsed.ToString("C");
				((Label)e.Item.Cells[2].FindControl("lblContractedDollarsPaidEdit")).Text = m_decContractedDollarsPaid.ToString("C");
				ddlServiceDetail.SelectedIndex = ddlServiceDetail.Items.IndexOf(ddlServiceDetail.Items.FindByText(m_strUnitMeasure));
				CheckBox chkIsMOE = (CheckBox)e.Item.Cells[2].FindControl("chkIsMOE");
				chkIsMOE.Checked = m_blnIsPlanLevelMOE;
				if (cprPrincipal.IsInRole("Administrator") && !m_blnIsServiceLevelMOE)
				{
					chkIsMOE.Visible = true;
				}
				else
				{
					chkIsMOE.Visible = false;
				}
						
						// If CostType is "Whole Cost" and UnitMeasure is "ADM"...(same as above).
						bool blnIsCostADMPercentage = false;;
				if (!cprPrincipal.IsInRole("Administrator"))
				{
					int intCT_ID = (int)drvPlanDetail["SD_CT_ID"];
					int intUM_ID = (int)drvPlanDetail["SD_UM_ID"];
					blnIsCostADMPercentage = (intCT_ID == 2 && intUM_ID == 2);
					((Label)e.Item.Cells[2].FindControl("lblUnitCostEdit")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblTransitUnitsEdit")).Visible = !blnIsCostADMPercentage;
					((TextBox)e.Item.Cells[2].FindControl("txtTransitUnits")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblResolutionUnitsEdit")).Visible = !blnIsCostADMPercentage;
					((TextBox)e.Item.Cells[2].FindControl("txtResolutionUnits")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblContractedUnitsEdit")).Visible = !blnIsCostADMPercentage;
					((TextBox)e.Item.Cells[2].FindControl("txtContractedUnits")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblContractedCostEdit")).Visible = !blnIsCostADMPercentage;
					((Label)e.Item.Cells[2].FindControl("lblTotalUnitsEdit")).Visible = !blnIsCostADMPercentage;
				}
				// If cost is not calculated then show controls as normal.
				if (!blnIsCostADMPercentage)
				{
					ShowEditControls(e.Item, blnIsTransitArea);
				}
			}
		}

		public void Update_Click(Object sender, DataGridCommandEventArgs e)
		{
			if (Page.IsValid)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandText = "prUpdatePlanDetail";
				sqlCommand.CommandType = CommandType.StoredProcedure;
							
				sqlCommand.Parameters.Add(new SqlParameter("@intPD_ID", SqlDbType.Int));
				sqlCommand.Parameters.Add(new SqlParameter("@intPD_SD_ID", SqlDbType.Int));
				sqlCommand.Parameters.Add(new SqlParameter("@decPD_TransitUnits", SqlDbType.Decimal));
				sqlCommand.Parameters["@decPD_TransitUnits"].Precision = 10;
				sqlCommand.Parameters["@decPD_TransitUnits"].Scale = 4;
				sqlCommand.Parameters.Add(new SqlParameter("@decPD_PurchasedResolutionUnits", SqlDbType.Decimal));
				sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Precision = 10;
				sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Scale = 4;
				sqlCommand.Parameters.Add(new SqlParameter("@decPD_PurchasedContractedUnits", SqlDbType.Decimal));
				sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Precision = 10;
				sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Scale = 4;
				sqlCommand.Parameters.Add(new SqlParameter("@monPD_ResolutionDollarsUsed", SqlDbType.Money));
				sqlCommand.Parameters.Add(new SqlParameter("@monPD_ContractedDollarsPaid", SqlDbType.Money));
				sqlCommand.Parameters.Add(new SqlParameter("@txtPD_Notes", SqlDbType.Text));
				sqlCommand.Parameters.Add(new SqlParameter("@bitPD_IsMOEInEffect", SqlDbType.Bit));
				sqlCommand.Parameters.Add(new SqlParameter("@intPD_ModifiedBy_US_ID", SqlDbType.Int));
				
				sqlCommand.Parameters["@intPD_ID"].Value = dtgPlanDetails.DataKeys[(int)e.Item.ItemIndex];
				sqlCommand.Parameters["@intPD_SD_ID"].Value = Convert.ToInt32(((DropDownList)e.Item.Cells[2].FindControl("ddlServiceDetail")).SelectedValue);
				if (m_blnAllowTransitUnitsEdit)
				{
					m_strTransitUnitsValue = ((TextBox)e.Item.Cells[2].FindControl("txtTransitUnits")).Text;
				}
				else
				{
					m_strTransitUnitsValue = ((Label)e.Item.Cells[2].FindControl("lblTransitUnitsEdit")).Text;
				}
				sqlCommand.Parameters["@decPD_TransitUnits"].Value = Convert.ToDecimal(m_strTransitUnitsValue);
				sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Value = 
					Convert.ToDecimal(((TextBox)e.Item.Cells[2].FindControl("txtResolutionUnits")).Text);
				sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Value = 
					Convert.ToDecimal(((TextBox)e.Item.Cells[2].FindControl("txtContractedUnits")).Text);
				sqlCommand.Parameters["@monPD_ResolutionDollarsUsed"].Value =
					Decimal.Parse(((TextBox)e.Item.Cells[2].FindControl("txtResolutionDollarsUsed")).Text, System.Globalization.NumberStyles.Currency);
				sqlCommand.Parameters["@monPD_ContractedDollarsPaid"].Value =
					Decimal.Parse(((TextBox)e.Item.Cells[2].FindControl("txtContractedDollarsPaid")).Text, System.Globalization.NumberStyles.Currency);
				sqlCommand.Parameters["@txtPD_Notes"].Value = 
					((TextBox)e.Item.Cells[2].FindControl("txtNotes")).Text;
				sqlCommand.Parameters["@bitPD_IsMOEInEffect"].Value = ((CheckBox)e.Item.FindControl("chkIsMOE")).Checked;
				sqlCommand.Parameters["@intPD_ModifiedBy_US_ID"].Value = m_intUS_ID;
				
				sqlCommand.Connection.Open();

				try
				{
					sqlCommand.ExecuteNonQuery();
					dtgPlanDetails.EditItemIndex = -1;
				}
				catch (SqlException exc)
				{
					Response.Write("ERROR: Could not update record, SQL Exception<br>");
					Response.Write(exc.Message + "<br>" + exc.StackTrace);
				}

				sqlCommand.Connection.Close();
				
				ShowPlan();
			}
		}

		public void dtgPlanDetails_ItemCommand(object sender, DataGridCommandEventArgs e)
		{
			switch (e.CommandName)
			{
				case "SaveNew":
					if (Page.IsValid)
					{
						int intPD_ID = (int)Session["intPD_ID"];
						
						SqlCommand sqlCommand = new SqlCommand();
						sqlCommand.Connection = sqlConnection;
						
						int intDS_ID = (int)Session["intDS_ID"]; 
						int intYR_ID = (int)Session["intYR_ID"];

						// If PlanDetail already exists then update existing otherwise insert new.
						if (intPD_ID > 0)
						{
							sqlCommand.CommandText = "prUpdatePlanDetailPastDeleted";
							sqlCommand.Parameters.Add(new SqlParameter("@intPD_ID", SqlDbType.Int));
							sqlCommand.Parameters["@intPD_ID"].Value = intPD_ID;
						}
						else
						{
							sqlCommand.CommandText = "prInsertPlanDetail";
							sqlCommand.Parameters.Add(new SqlParameter("@intPL_DS_ID", SqlDbType.Int));
							sqlCommand.Parameters.Add(new SqlParameter("@intPL_YR_ID", SqlDbType.Int));

							sqlCommand.Parameters["@intPL_DS_ID"].Value = intDS_ID;
							sqlCommand.Parameters["@intPL_YR_ID"].Value = intYR_ID;
						}

						sqlCommand.CommandType = CommandType.StoredProcedure;
									
						sqlCommand.Parameters.Add(new SqlParameter("@intPD_SD_ID", SqlDbType.Int));
						sqlCommand.Parameters.Add(new SqlParameter("@decPD_TransitUnits", SqlDbType.Decimal));
						sqlCommand.Parameters["@decPD_TransitUnits"].Precision = 10;
						sqlCommand.Parameters["@decPD_TransitUnits"].Scale = 4;
						sqlCommand.Parameters.Add(new SqlParameter("@decPD_PurchasedResolutionUnits", SqlDbType.Decimal));
						sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Precision = 10;
						sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Scale = 4;
						sqlCommand.Parameters.Add(new SqlParameter("@decPD_PurchasedContractedUnits", SqlDbType.Decimal));
						sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Precision = 10;
						sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Scale = 4;
						sqlCommand.Parameters.Add(new SqlParameter("@monPD_ResolutionDollarsUsed", SqlDbType.Money));
						sqlCommand.Parameters.Add(new SqlParameter("@monPD_ContractedDollarsPaid", SqlDbType.Money));
						sqlCommand.Parameters.Add(new SqlParameter("@txtPD_Notes", SqlDbType.Text));
						sqlCommand.Parameters.Add(new SqlParameter("@intPD_ModifiedBy_US_ID", SqlDbType.Int));
						
						sqlCommand.Parameters["@intPD_SD_ID"].Value = ((DropDownList)e.Item.Cells[2].FindControl("ddlServiceDetailNew")).SelectedValue;
						if (m_blnAllowTransitUnitsEdit)
						{
							m_strTransitUnitsValue = ((TextBox)e.Item.Cells[2].FindControl("txtTransitUnitsNew")).Text;
						}
						else
						{
							m_strTransitUnitsValue = ((Label)e.Item.Cells[2].FindControl("lblTransitUnitsNew")).Text;
						}					
						sqlCommand.Parameters["@decPD_TransitUnits"].Value = Convert.ToDecimal(m_strTransitUnitsValue);
						sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Value = 
							Convert.ToDecimal(((TextBox)e.Item.Cells[2].FindControl("txtResolutionUnitsNew")).Text);
						sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Value = 
							Convert.ToDecimal(((TextBox)e.Item.Cells[2].FindControl("txtContractedUnitsNew")).Text);
						sqlCommand.Parameters["@monPD_ResolutionDollarsUsed"].Value =
							Decimal.Parse(((TextBox)e.Item.Cells[2].FindControl("txtResolutionDollarsUsedNew")).Text, System.Globalization.NumberStyles.Currency);
						sqlCommand.Parameters["@monPD_ContractedDollarsPaid"].Value =
							Decimal.Parse(((TextBox)e.Item.Cells[2].FindControl("txtContractedDollarsPaidNew")).Text, System.Globalization.NumberStyles.Currency);
						sqlCommand.Parameters["@txtPD_Notes"].Value = ((TextBox)e.Item.FindControl("txtNotesNew")).Text;
						sqlCommand.Parameters["@intPD_ModifiedBy_US_ID"].Value = m_intUS_ID;
						
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
						
						ShowPlan();
					}
					break;

				case "Expand":
					ImageButton imgbtnExpand;
					imgbtnExpand = (ImageButton)e.Item.Cells[1].FindControl("imgbtnExpand");
					
					if (imgbtnExpand.ImageUrl == "images/Plus.gif")
					{
						foreach(DataGridItem item in dtgPlanDetails.Items)
						{
							((PlaceHolder)item.Cells[3].FindControl("ChildRows")).Visible = false;
							((ImageButton)item.Cells[1].FindControl("imgbtnExpand")).ImageUrl = "images/Plus.gif";
							((ImageButton)item.Cells[1].FindControl("imgbtnExpand")).ToolTip = "Expand and show history for this service";
						}
						
						imgbtnExpand.ImageUrl = "images/Minus.gif";
						imgbtnExpand.ToolTip = "Collapse and hide history for this service";
					}
					else
					{
						imgbtnExpand.ImageUrl = "images/Plus.gif";
						imgbtnExpand.ToolTip = "Expand and show history for this service";
					}

					PlaceHolder ChildRows;
					ChildRows = (PlaceHolder)e.Item.Cells[3].FindControl("ChildRows"); 
					ChildRows.Visible = !ChildRows.Visible;
					break;
			}
		}

		public void NewCancel_Click(object sender, System.EventArgs e)
		{
			m_blnNewRecord = false;
			BindGrid();
		}

		private void ShowEditControls(DataGridItem item, bool blnIsTransitArea)
		{
			// Show appropriate controls based on authorization level.
			if (item.FindControl("lblResolutionUnitsEdit") != null)
			{
				item.Cells[2].FindControl("txtResolutionUnits").Visible = m_blnAllowPurchasedUnitsEdit && !blnIsTransitArea;
				item.Cells[2].FindControl("txtContractedUnits").Visible = m_blnAllowPurchasedUnitsEdit && !blnIsTransitArea;
				item.Cells[2].FindControl("txtTransitUnits").Visible = m_blnAllowTransitUnitsEdit;
				item.Cells[2].FindControl("lblResolutionUnitsEdit").Visible = !m_blnAllowPurchasedUnitsEdit || blnIsTransitArea;
				item.Cells[2].FindControl("lblContractedUnitsEdit").Visible = !m_blnAllowPurchasedUnitsEdit || blnIsTransitArea;
				item.Cells[2].FindControl("lblTransitUnitsEdit").Visible = !m_blnAllowTransitUnitsEdit;
				item.Cells[2].FindControl("txtResolutionDollarsUsed").Visible = cprPrincipal.IsInRole("Administrator");
				item.Cells[2].FindControl("txtContractedDollarsPaid").Visible = cprPrincipal.IsInRole("Administrator");
				item.Cells[2].FindControl("lblResolutionDollarsUsedEdit").Visible = !cprPrincipal.IsInRole("Administrator");
				item.Cells[2].FindControl("lblContractedDollarsPaidEdit").Visible = !cprPrincipal.IsInRole("Administrator");
			} 
			if (item.FindControl("lblResolutionUnitsNew") != null)
			{
				item.Cells[2].FindControl("txtTransitUnitsNew").Visible = m_blnAllowTransitUnitsEdit;
				item.Cells[2].FindControl("lblTransitUnitsNew").Visible = !m_blnAllowTransitUnitsEdit;
				item.Cells[2].FindControl("txtResolutionUnitsNew").Visible = m_blnAllowPurchasedUnitsEdit && !blnIsTransitArea;
				item.Cells[2].FindControl("lblResolutionUnitsNew").Visible = !m_blnAllowPurchasedUnitsEdit || blnIsTransitArea;
				item.Cells[2].FindControl("txtContractedUnitsNew").Visible = m_blnAllowPurchasedUnitsEdit && !blnIsTransitArea;
				item.Cells[2].FindControl("lblContractedUnitsNew").Visible = !m_blnAllowPurchasedUnitsEdit || blnIsTransitArea;
				item.Cells[2].FindControl("txtResolutionDollarsUsedNew").Visible = cprPrincipal.IsInRole("Administrator");
				item.Cells[2].FindControl("txtContractedDollarsPaidNew").Visible = cprPrincipal.IsInRole("Administrator");
				item.Cells[2].FindControl("lblResolutionDollarsUsedNew").Visible = !cprPrincipal.IsInRole("Administrator");
				item.Cells[2].FindControl("lblContractedDollarsPaidNew").Visible = !cprPrincipal.IsInRole("Administrator");
			}
		}

		protected void lnkNew_Click(object sender, System.EventArgs e)
		{
			dtgPlanDetails.EditItemIndex = -1;
			m_blnNewRecord = true;
			BindGrid();
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
			if (cprPrincipal.IsInRole("District Team Member") || cprPrincipal.IsInRole("District Primary"))
			{
				sqlCommand.CommandText = "prSelectAllOpenActiveYears";
			}
			else
			{
				sqlCommand.CommandText = "prSelectAllActiveYears";
			}
			
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

		public DataTable GetServiceTable()
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prSelectServicesRemainingByDepartment";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			sqlCommand.Parameters.Add(new SqlParameter("@intDP_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intDP_ID"].Value = m_strarrDepartments[tbsDeptSelector.CurrentTabIndex].Split('|')[1];
			
			int intDS_ID = 0;
			int intYR_ID = 0;

			if ((Session["intDS_ID"] != null && Session["intYR_ID"] != null))
			{
				intDS_ID = (int)Session["intDS_ID"]; 
				intYR_ID = (int)Session["intYR_ID"];
			}

			sqlCommand.Parameters.Add(new SqlParameter("@intDS_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intDS_ID"].Value = intDS_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@intYR_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intYR_ID"].Value = intYR_ID;
			
			DataTable dttService = new DataTable();
			DataRow dtrService;
			DataColumn dtcServiceName = new DataColumn("SV_Name");
			dttService.Columns.Add(dtcServiceName);
			DataColumn dtcServiceId = new DataColumn("SV_ID");
			dttService.Columns.Add(dtcServiceId);

			dtrService = dttService.NewRow();
			dtrService[0] = "<-- Select Service -->";
			dtrService[1] = "0";
			dttService.Rows.Add(dtrService);
			
			sqlConnection.Open();
			SqlDataReader sqlReader = sqlCommand.ExecuteReader();
			while(sqlReader.Read())
			{
				dtrService = dttService.NewRow();
				dtrService[0] = sqlReader[0];
				dtrService[1] = sqlReader[1];
				dttService.Rows.Add(dtrService);
			}
			sqlConnection.Close();
			sqlReader.Close();
			
			return dttService;
		}
		
		protected DataTable GetServiceDetailList()
		{
			// m_dttServiceDetail is a class-level DataTable
			return m_dttServiceDetail;
		}

		public DataTable CreateServiceDetailTable(int intSV_ID)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prSelectServiceUnitMeasures";
			sqlCommand.CommandType = CommandType.StoredProcedure;

			sqlCommand.Parameters.Add(new SqlParameter("@intSV_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intSV_ID"].Value = intSV_ID;
						
			DataTable dttServiceDetail = new DataTable();
			DataRow dtrServiceUnitMeasure;
			DataColumn dtcUnitMeasureName = new DataColumn("UM_Name");
			dttServiceDetail.Columns.Add(dtcUnitMeasureName);
			DataColumn dtcServiceDetailId = new DataColumn("SD_ID");
			dttServiceDetail.Columns.Add(dtcServiceDetailId);

			dtrServiceUnitMeasure = dttServiceDetail.NewRow();
			dtrServiceUnitMeasure[0] = "";
			dtrServiceUnitMeasure[1] = "0";
			dttServiceDetail.Rows.Add(dtrServiceUnitMeasure);
			
			sqlConnection.Open();
			SqlDataReader sqlReader = sqlCommand.ExecuteReader();
			while(sqlReader.Read())
			{
				dtrServiceUnitMeasure = dttServiceDetail.NewRow();
				dtrServiceUnitMeasure[0] = sqlReader[0];
				dtrServiceUnitMeasure[1] = sqlReader[1];
				dttServiceDetail.Rows.Add(dtrServiceUnitMeasure);
			}
			sqlConnection.Close();
			sqlReader.Close();
			
			return dttServiceDetail;
		}

		public void NewServiceSelected(object sender, System.EventArgs e)
		{
			DataGridItem dgiNewRow = (DataGridItem)(((Control)sender).NamingContainer);
			m_intSV_ID = Convert.ToInt32(((DropDownList)dgiNewRow.Cells[2].FindControl("ddlServiceNew")).SelectedValue);
			
			m_dttServiceDetail = CreateServiceDetailTable(m_intSV_ID);

			int intDS_ID = (int)Session["intDS_ID"]; 
			int intYR_ID = (int)Session["intYR_ID"];
			int intPD_ID = 0;
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prSelectExistingPlanDetailInfo";
			sqlCommand.CommandType = CommandType.StoredProcedure;
			
			sqlCommand.Parameters.Add(new SqlParameter("@intDS_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intDS_ID"].Value = intDS_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@intYR_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intYR_ID"].Value = intYR_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@intSV_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intSV_ID"].Value = m_intSV_ID;
			
			sqlCommand.Parameters.Add(new SqlParameter("@intPD_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intPD_ID"].Direction = ParameterDirection.Output;

			sqlCommand.Parameters.Add(new SqlParameter("@intSD_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intSD_ID"].Direction = ParameterDirection.Output;

			sqlCommand.Parameters.Add(new SqlParameter("@monSD_UnitCost", SqlDbType.Money));
			sqlCommand.Parameters["@monSD_UnitCost"].Direction = ParameterDirection.Output;

			sqlCommand.Parameters.Add(new SqlParameter("@decPD_TransitUnits", SqlDbType.Decimal));
			sqlCommand.Parameters["@decPD_TransitUnits"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters["@decPD_TransitUnits"].Precision = 8;
			sqlCommand.Parameters["@decPD_TransitUnits"].Scale = 2;

			sqlCommand.Parameters.Add(new SqlParameter("@decPD_PurchasedResolutionUnits", SqlDbType.Decimal));
			sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Precision = 8;
			sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Scale = 2;

			sqlCommand.Parameters.Add(new SqlParameter("@decPD_PurchasedContractedUnits", SqlDbType.Decimal));
			sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Direction = ParameterDirection.Output;
			sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Precision = 8;
			sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Scale = 2;

			sqlCommand.Parameters.Add(new SqlParameter("@chvPD_Notes", SqlDbType.VarChar, 8000));
			sqlCommand.Parameters["@chvPD_Notes"].Direction = ParameterDirection.Output;
			
			sqlConnection.Open();
			
			sqlCommand.ExecuteNonQuery();
			
			// If an existing PlanDetail record exists for this service then load up previous values.
			if (sqlCommand.Parameters["@intPD_ID"].Value != DBNull.Value)
			{
				intPD_ID = Convert.ToInt32(sqlCommand.Parameters["@intPD_ID"].Value);
				m_intSD_ID = Convert.ToInt32(sqlCommand.Parameters["@intSD_ID"].Value);
				m_decUnitCost = Convert.ToDecimal(sqlCommand.Parameters["@monSD_UnitCost"].Value);
				m_decTransitUnits = Convert.ToDecimal(sqlCommand.Parameters["@decPD_TransitUnits"].Value);
				m_decResolutionUnits = Convert.ToDecimal(sqlCommand.Parameters["@decPD_PurchasedResolutionUnits"].Value);
				m_decContractedUnits = Convert.ToDecimal(sqlCommand.Parameters["@decPD_PurchasedContractedUnits"].Value);
				m_strPlanDetailNotes = Convert.ToString(sqlCommand.Parameters["@chvPD_Notes"].Value);
			}
			
			sqlConnection.Close();
			
			// To do: Look at writing this information to ViewState which is already setup 
			// to serialize to disk.  Compare performance using various methods: db access, session state, view state.
			Session["intPD_ID"] = intPD_ID;
			Session["intSD_ID"] = m_intSD_ID;
			Session["m_decUnitCost"] = m_decUnitCost;
			Session["m_decTransitUnits"] = m_decTransitUnits;
			Session["m_decResolutionUnits"] = m_decResolutionUnits;
			Session["m_decContractedUnits"] = m_decContractedUnits;
			Session["m_strPlanDetailNotes"] = m_strPlanDetailNotes;
			
			m_blnNewRecord = true;
			BindGrid();
		}

		public void ServiceDetailSelected(object sender, System.EventArgs e)
		{
			DataGridItem dgiNewRow = (DataGridItem)(((Control)sender).NamingContainer);
			m_intSD_ID = Convert.ToInt32(((DropDownList)dgiNewRow.Cells[2].FindControl("ddlServiceDetail")).SelectedValue);
			m_strUnitMeasure = ((DropDownList)dgiNewRow.Cells[2].FindControl("ddlServiceDetail")).SelectedItem.Text;
			
			if (m_intSD_ID > 0)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandText = "prSelectServiceUnitCost";
				sqlCommand.CommandType = CommandType.StoredProcedure;
			
				sqlCommand.Parameters.Add(new SqlParameter("@intSD_ID", SqlDbType.Int));
				sqlCommand.Parameters["@intSD_ID"].Value = m_intSD_ID;

				sqlCommand.Parameters.Add(new SqlParameter("@monSD_UnitCost", SqlDbType.Money));
				sqlCommand.Parameters["@monSD_UnitCost"].Direction = ParameterDirection.Output;
			
				sqlConnection.Open();
			
				sqlCommand.ExecuteNonQuery();
				m_decUnitCost = Convert.ToDecimal(sqlCommand.Parameters["@monSD_UnitCost"].Value);
			
				sqlConnection.Close();

				m_decTransitUnits = 0;
				m_decResolutionUnits = 0;
				m_decResolutionCost = 0;
				m_decContractedUnits = 0;
				m_decContractedCost = 0;
				m_decTotalUnits = 0;
				m_decTotalCost = 0;
			}
			
			BindGrid();
		}

		public void NewServiceDetailSelected(object sender, System.EventArgs e)
		{
			DataGridItem dgiNewRow = (DataGridItem)(((Control)sender).NamingContainer);
			m_intSV_ID = Convert.ToInt32(((DropDownList)dgiNewRow.Cells[2].FindControl("ddlServiceNew")).SelectedValue);
			m_intSD_ID = Convert.ToInt32(((DropDownList)dgiNewRow.Cells[2].FindControl("ddlServiceDetailNew")).SelectedValue);
			
			if (m_intSD_ID > 0)
			{
				if (m_intSD_ID == (int)Session["intSD_ID"])
				{
					m_decUnitCost = Convert.ToDecimal(Session["m_decUnitCost"]);
					m_decTransitUnits = Convert.ToDecimal(Session["m_decTransitUnits"]);
					m_decResolutionUnits = Convert.ToDecimal(Session["m_decResolutionUnits"]);
					m_decContractedUnits = Convert.ToDecimal(Session["m_decContractedUnits"]);
					m_strPlanDetailNotes = (string)Session["m_strPlanDetailNotes"];
				}
				else
				{
					SqlCommand sqlCommand = new SqlCommand();
					sqlCommand.Connection = sqlConnection;
					sqlCommand.CommandText = "prSelectServiceUnitCost";
					sqlCommand.CommandType = CommandType.StoredProcedure;
			
					sqlCommand.Parameters.Add(new SqlParameter("@intSD_ID", SqlDbType.Int));
					sqlCommand.Parameters["@intSD_ID"].Value = m_intSD_ID;

					sqlCommand.Parameters.Add(new SqlParameter("@monSD_UnitCost", SqlDbType.Money));
					sqlCommand.Parameters["@monSD_UnitCost"].Direction = ParameterDirection.Output;
			
					sqlConnection.Open();
			
					sqlCommand.ExecuteNonQuery();
					m_decUnitCost = Convert.ToDecimal(sqlCommand.Parameters["@monSD_UnitCost"].Value);
			
					sqlConnection.Close();
				}
			}

			m_dttServiceDetail = CreateServiceDetailTable(m_intSV_ID);

			m_blnNewRecord = true;
			BindGrid();
		}
		
		protected int GetSelectedServiceDetail()
		{
			for(int i=0; i < m_dttServiceDetail.Rows.Count; i++)
			{
				if(Convert.ToInt32(m_dttServiceDetail.Rows[i]["SD_ID"]) == m_intSD_ID)
				{
					return i;
				}
			}

			//If there is no match, return 0
			return 0;	
		}

		public string GetUnitCost()
		{
			return m_decUnitCost.ToString("C");
		}

		public string GetTransitUnits()
		{
			return m_decTransitUnits.ToString("N4");
		}

		public string GetResolutionUnits()
		{
			return m_decResolutionUnits.ToString("N4");
		}

		public string GetContractedUnits()
		{
			return m_decContractedUnits.ToString("N4");
		}

		public string GetResolutionDollarsUsed()
		{
			return m_decResolutionDollarsUsed.ToString("C");
		}

		public string GetContractedDollarsPaid()
		{
			return m_decContractedDollarsPaid.ToString("C");
		}

		public string GetPlanDetailNotes()
		{
			return m_strPlanDetailNotes.ToString();
		}

		public string GetIsMOEInEffect()
		{
			return m_blnIsServiceLevelMOE.ToString();
		}

		public string GetIsCoreService()
		{
			return m_blnIsCoreService.ToString();
		}

		public string GetFTELabel(bool IsFTEShown)
		{
			string fteLabel = "";
			
			if (IsFTEShown) 
			{
				fteLabel = "{FTE}";
			}

			return fteLabel;
		}

		public string ConvertHoursToFTE(bool IsFTEShown, decimal Hours, decimal FTE_Hours_Equiv)
		{
			decimal d_fte = 0;
			string fte = "";
			
			if (IsFTEShown)
			{
				d_fte = Hours / FTE_Hours_Equiv;
				if (d_fte < .1m) 
				{
					d_fte = 0;
				}
				fte = d_fte.ToString("N2");
			}

			return fte;
		}
		
		protected void lnkGetPlan_Click(object sender, System.EventArgs e)
		{
			int intDS_ID = Convert.ToInt32(ddlDistricts.SelectedValue);
			string strDS_NAME = ddlDistricts.SelectedItem.Text;
			int intYR_ID;
			string strYR_NAME = ddlYears.SelectedItem.Text;
			
			//if (cprPrincipal.IsInRole("District Team Member") || cprPrincipal.IsInRole("District Primary"))
			//{
				// To do: Pull current year id from database and set it to session level variable here.
			//	intYR_ID = 1;
			//}
			//else
			//{
				intYR_ID = Convert.ToInt32(ddlYears.SelectedValue);
			//}

			if (intDS_ID > 0 && intYR_ID > 0)
			{
				Session["intDS_ID"] = intDS_ID;
				Session["intYR_ID"] = intYR_ID;
				Session["strDS_NAME"] = strDS_NAME;
				Session["strYR_NAME"] = strYR_NAME;
				dtgPlanDetails.CurrentPageIndex = 0;
				ShowPlan();
			}
			else
			{
				//Todo: Write "You must select a district and year." to screen.
			}
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
				if ((args.Value.Split('.').Length - 1) > 1)
				{
					blnIsValid = false;
					break;
				}
			}
			args.IsValid = blnIsValid;
		}

		protected void validateCurrency(object source, ServerValidateEventArgs args)
		{
			bool blnIsValid = true;
			foreach (char c in args.Value.ToCharArray())
			{
				if (!Char.IsNumber(c) && !Char.Equals(c, '$') && !Char.Equals(c, '-') && !Char.Equals(c, '.') && !Char.Equals(c, ',') && !Char.Equals(c, '(') && !Char.Equals(c, ')'))
				{
					blnIsValid = false;
					break;
				};
			}
			args.IsValid = blnIsValid;
		}

		protected void validateAvailableFundsBalance(object source, ServerValidateEventArgs args)
		{
			bool blnIsValid = true;
			if (!cprPrincipal.IsInRole("Administrator"))
			{
				DataGridItem dgiPlanDetail = (DataGridItem)(((Control)source).NamingContainer);
				// Determine the number of purchased resolution units added to the service.
				decimal decResolutionUnits = Convert.ToDecimal(args.Value) - 
					Decimal.Parse(((Label)dgiPlanDetail.Cells[2].FindControl("lblResolutionUnitsEdit")).Text, System.Globalization.NumberStyles.Currency);
				// If change to service results in a negative balance then Resolution Units is invalid.
				decimal decAvailableFundsBalance = Decimal.Parse(lblResolutionTotalRemaining.Text, System.Globalization.NumberStyles.Currency);
				decimal decUnitCost = Decimal.Parse(((Label)dgiPlanDetail.Cells[2].FindControl("lblUnitCostEdit")).Text, System.Globalization.NumberStyles.Currency);
				if ((decAvailableFundsBalance - (decResolutionUnits * decUnitCost)) < 0)
				{
					blnIsValid = false;
				}
			}
			args.IsValid = blnIsValid;
		}

		protected void validateAvailableFundsBalanceNewService(object source, ServerValidateEventArgs args)
		{
			bool blnIsValid = true;
			if (!cprPrincipal.IsInRole("Administrator"))
			{
				DataGridItem dgiPlanDetail = (DataGridItem)(((Control)source).NamingContainer);
				// Determine the number of purchased resolution units added to the service.
				decimal decResolutionUnits = Convert.ToDecimal(args.Value);
				// If new service results in a negative balance then Resolution Units is invalid.
				decimal decAvailableFundsBalance = Decimal.Parse(lblResolutionTotalRemaining.Text, System.Globalization.NumberStyles.Currency);
				decimal decUnitCost = Decimal.Parse(((Label)dgiPlanDetail.Cells[2].FindControl("lblUnitCostNew")).Text, System.Globalization.NumberStyles.Currency);
				if ((decAvailableFundsBalance - (decResolutionUnits * decUnitCost)) < 0)
				{
					blnIsValid = false;
				}
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

		protected void lnkEditPlan_Click(object sender, System.EventArgs e)
		{
			dtgPlanDetails.EditItemIndex = -1;
			BindGrid();
			m_blnEditMode = true;
			AdjustPlanDisplay();
		}

		protected void lnkCancelPlan_Click(object sender, System.EventArgs e)
		{
			m_blnEditMode = false;
			AdjustPlanDisplay();
		}

		protected void lnkSavePlan_Click(object sender, System.EventArgs e)
		{
			if (Page.IsValid)
			{
				SqlCommand sqlCommand = new SqlCommand();
				sqlCommand.Connection = sqlConnection;
				sqlCommand.CommandText = "prUpdatePlanAllotments";
				sqlCommand.CommandType = CommandType.StoredProcedure;
				
				int intDS_ID = (int)Session["intDS_ID"]; 
				int intYR_ID = (int)Session["intYR_ID"];
				
				sqlCommand.Parameters.Add(new SqlParameter("@intPL_DS_ID", SqlDbType.Int));
				sqlCommand.Parameters["@intPL_DS_ID"].Value = intDS_ID;

				sqlCommand.Parameters.Add(new SqlParameter("@intPL_YR_ID", SqlDbType.Int));
				sqlCommand.Parameters["@intPL_YR_ID"].Value = intYR_ID;

				sqlCommand.Parameters.Add(new SqlParameter("@mnyPL_AllottedResolutionDollars", SqlDbType.Money));
				sqlCommand.Parameters["@mnyPL_AllottedResolutionDollars"].Value = Decimal.Parse(txtResolutionAvailable.Text.ToString(), System.Globalization.NumberStyles.Currency);
							
				sqlCommand.Parameters.Add(new SqlParameter("@mnyPL_CarryoverResolutionDollars", SqlDbType.Money));
				sqlCommand.Parameters["@mnyPL_CarryoverResolutionDollars"].Value = Decimal.Parse(txtResolutionCarryover.Text.ToString(), System.Globalization.NumberStyles.Currency);
			
				sqlCommand.Parameters.Add(new SqlParameter("@intPL_ModifiedBy_US_ID", SqlDbType.Int));
				sqlCommand.Parameters["@intPL_ModifiedBy_US_ID"].Value = m_intUS_ID;
							
				sqlConnection.Open();
				sqlCommand.ExecuteNonQuery();
				sqlConnection.Close();

				m_blnEditMode = false;
				ShowPlan();
			}
		}

		protected void lnkFinalizePlan_Click(object sender, System.EventArgs e)
		{
			FinalizePlan(true);
			ShowPlanStatus();
		}
		
		protected void lnkUndoFinalizePlan_Click(object sender, System.EventArgs e)
		{
			FinalizePlan(false);
			ShowPlanStatus();
		}

		private void FinalizePlan(bool blnIsFinalized)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prUpdatePlanFinalized";
			sqlCommand.CommandType = CommandType.StoredProcedure;
				
			int intDS_ID = (int)Session["intDS_ID"]; 
			int intYR_ID = (int)Session["intYR_ID"];
				
			sqlCommand.Parameters.Add(new SqlParameter("@intDS_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intDS_ID"].Value = intDS_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@intYR_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intYR_ID"].Value = intYR_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@bitIsFinalized", SqlDbType.Bit));
			sqlCommand.Parameters["@bitIsFinalized"].Value = blnIsFinalized;

			sqlConnection.Open();
			sqlCommand.ExecuteNonQuery();
			sqlConnection.Close();		
		}

		private bool IsPlanFinalized(int intDS_ID, int intYR_ID)
		{
			bool blnIsFinalized;
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Connection = sqlConnection;
			sqlCommand.CommandText = "prSelectPlanFinalized";
			sqlCommand.CommandType = CommandType.StoredProcedure;
				
			sqlCommand.Parameters.Add(new SqlParameter("@intDS_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intDS_ID"].Value = intDS_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@intYR_ID", SqlDbType.Int));
			sqlCommand.Parameters["@intYR_ID"].Value = intYR_ID;

			sqlCommand.Parameters.Add(new SqlParameter("@bitIsFinalized", SqlDbType.Bit));
			sqlCommand.Parameters["@bitIsFinalized"].Direction = ParameterDirection.Output;

			sqlConnection.Open();
			sqlCommand.ExecuteNonQuery();
			blnIsFinalized = Convert.ToBoolean(sqlCommand.Parameters["@bitIsFinalized"].Value);
			sqlConnection.Close();

			return blnIsFinalized;
		}

		private void ShowPlanStatus()
		{
			int intDS_ID = 0;
			int intYR_ID = 0;

			if (Session["intDS_ID"] != null && Session["intYR_ID"] != null)
			{
				intDS_ID = (int)Session["intDS_ID"];
				intYR_ID = (int)Session["intYR_ID"];
			}			
			
			m_blnIsFinalized = IsPlanFinalized(intDS_ID, intYR_ID);
			
			lblPlanFinalized.Visible = m_blnIsFinalized;

			lnkFinalizePlan.Visible = ((cprPrincipal.IsInRole("Administrator") 
				|| cprPrincipal.IsInRole("District Primary")) && !m_blnIsFinalized);

			lnkUndoFinalizePlan.Visible = (cprPrincipal.IsInRole("Administrator") && m_blnIsFinalized);
		}
	}
}