<%@ Reference Control="~/UserControls/TabStrip.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TabStrip" Src="UserControls/TabStrip.ascx" %>
<%@ language="c#" Inherits="LSPE.plans" Codebehind="plans.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="rxpress_5.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="rxpress.js" type="text/javascript"></script>
		<script src="https://code.jquery.com/jquery-3.2.1.min.js" type="text/javascript"></script>
		<script type="text/javascript" language="javascript">
			$(function() {
				$("input[type=text], input[type=number], textarea").focus(function () {
					this.select();
				});
			});
		</script>
	</HEAD>
	<body>
		<form id="frmPlan" method="post" runat="server">
			<div align="center">
				<TABLE height="30" cellSpacing="1" cellPadding="1" width="960" <%--bgColor="#269bea"--%> border="0">
					<TBODY>
						<TR>
							<TD align="center" width="750" colSpan="3" height="5">&nbsp;
							</TD>
						</TR>
						<TR>
							<TD align="center" width="8" height="1">&nbsp;
							</TD>
							<TD align="center" width="864" height="1">
								<TABLE id="tblMain" cellSpacing="1" cellPadding="1" width="800" align="center" bgColor="#ffffff"
									border="0">
									<TBODY>
										<TR>
											<TD align="center" height="40"><uc1:header id="Header1" runat="server"></uc1:header></TD>
										</TR>
										<TR>
											<TD align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
										</TR>
										<TR>
											<TD class="subheader" vAlign="middle" align="center" height="20"><%--Plans--%></TD>
										</TR>
										<TR>
											<TD vAlign="middle" align="center" style="height: 35px">
												<P class="normaltext" align="left">&nbsp;District:&nbsp;
													<asp:dropdownlist id="ddlDistricts" runat="server" CssClass="normaltext"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
													<asp:label id="lblYear" runat="server" CssClass="normaltext">Year:</asp:label>&nbsp; 
													&nbsp;
													<asp:dropdownlist id="ddlYears" runat="server" CssClass="normaltext"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
													<asp:linkbutton id="lnkGetPlan" runat="server" CssClass="normaltext" CausesValidation="False" onclick="lnkGetPlan_Click">Get Plan</asp:linkbutton>&nbsp;<asp:label id="lblNoPlanMessage" runat="server" CssClass="message" Visible="False">A plan cannot be found in the system for the specified district and year.</asp:label>&nbsp;</P>
											</TD>
										</TR>
										<asp:placeholder id="plhPlan" Visible="false" Runat="server">
											<TR>
												<TD vAlign="middle" align="left">
													<asp:hyperlink id="hypPlanReport" runat="server" CssClass="normaltext" ToolTip="Print preview Resolution Plan."></asp:hyperlink>&nbsp;
													<FONT class="normaltext">(requires <A class="normaltext" href="http://www.adobe.com/products/acrobat/readstep2.html" target="_blank">
															Adobe Reader</A> be installed on your computer)</FONT>
												</TD>
											</TR>
											<TR>
												<TD vAlign="middle" align="center">
													<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="1000" bgColor="white" border="0">
														<TR>
															<TD class="plan" align="center" width="850" colSpan="1" height="15" rowSpan="1">
																<asp:label id="lblPlan" runat="server" Font-Bold="True">Label</asp:label></TD>
														</TR>
														<TR>
															<TD align="left" width="850" height="15">
																<asp:LinkButton id="lnkFinalizePlan" runat="server" CssClass="normaltext" Font-Bold="True" ForeColor="Red" onclick="lnkFinalizePlan_Click">Finalize Plan</asp:LinkButton>
																<asp:Label id="lblPlanFinalized" runat="server" CssClass="normaltext" Font-Bold="True" ForeColor="Red">PLAN FINALIZED</asp:Label>&nbsp;
																<asp:LinkButton id="lnkUndoFinalizePlan" runat="server" CssClass="normaltext" Font-Bold="True" onclick="lnkUndoFinalizePlan_Click">Undo Finalize Plan</asp:LinkButton></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR>
												<TD align="center">
													<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="1000" border="0">
														<TR>
															<TD vAlign="middle" align="center" width="40">
																<TABLE id="Table4" cellSpacing="1" cellPadding="1" width="52" border="0">
																	<TR>
																		<TD vAlign="middle">
																			<TABLE id="Table3" cellSpacing="1" cellPadding="1" width="30" border="0">
																				<TR>
																					<TD>
																						<asp:linkbutton id="lnkSavePlan" runat="server" Visible="False" ToolTip="Save edit's to plan's available funds" onclick="lnkSavePlan_Click">
																							<img border="0" src="images/save.gif" width="16" height="16"></asp:linkbutton><BR>
																						<asp:linkbutton id="lnkEditPlan" runat="server" ToolTip="Edit plan's available funds." onclick="lnkEditPlan_Click">
																							<img border="0" src="images/edit.gif" width="16" height="16"></asp:linkbutton></TD>
																					<TD>
																						<asp:linkbutton id="lnkCancelPlan" runat="server" CausesValidation="False" Visible="False" ToolTip="Cancel editing plan's available funds" onclick="lnkCancelPlan_Click">
																							<img border="0" src="images/cancel.gif" width="16" height="16"></asp:linkbutton><BR>
																					</TD>
																				</TR>
																			</TABLE>
																		</TD>
																	</TR>
																</TABLE>
															</TD>
															<TD width="810">
																<TABLE id="tblPlan" cellSpacing="1" cellPadding="1" width="800" align="left">
																	<TR>
																		<TD class="resolution" align="center" width="517" colSpan="8" height="14">Resolution 
																			Funds
																		</TD>
																		<TD class="contracted" align="right" width="193" height="14">Contracted Funds
																		</TD>
																		<TD style="WIDTH: 145px" align="right" width="145" bgColor="#ffffff" height="14" rowSpan="2">Overall<BR>
																			Total Used
																		</TD>
																	</TR>
																	<TR>
																		<TD class="resolution" align="right" width="82" height="14">Allocation</TD>
																		<TD class="resolution" align="right" width="87" height="14">Prior Year<BR>Carryover
																		</TD>
																		<td class="resolution" align="right" width="91" height="14">Prior Year<BR>Reconciliation</td>
																		<TD class="resolution" align="right" width="91" height="14">Total<BR>
																			Allocation
																		</TD>
																		<TD class="resolution" align="right" width="91" height="14">Transit<BR>
																			Approved
																		</TD>
																		<TD class="resolution" align="right" width="91" height="14">Resolution<BR>
																			Funds Committed
																		</TD>
																		<TD class="resolution" align="right" width="91" height="14">Total<BR>
																			Used
																		</TD>
																		<TD class="resolution" align="right" width="91" height="14">Total<BR>
																			Remaining
																		</TD>
																		<TD class="contracted" align="right" width="193" height="14">Total Used
																		</TD>
																	</TR>
																	<TR>
																		<TD class="resolution" align="right" width="82" height="1">
																			<asp:textbox id="txtResolutionAvailable" runat="server" CssClass="input_allotments"></asp:textbox>
																			<asp:requiredfieldvalidator id="rfvResolutionAvailable" runat="server" Display="None" ControlToValidate="txtResolutionAvailable"
																				ErrorMessage="Resolution Funds Available cannot be left blank."></asp:requiredfieldvalidator>
																			<asp:customvalidator id="csvResolutionFundsAvailable" runat="server" Display="None" ControlToValidate="txtResolutionAvailable"
																				ErrorMessage="Resolution Funds Available must be a dollar amount." OnServerValidate="validateCurrency"></asp:customvalidator>
																			<asp:label id="lblResolutionAvailable" runat="server" Visible="False">Label</asp:label></TD>
																		<TD class="resolution" align="right" width="87" height="1">
																			<asp:textbox id="txtResolutionCarryover" runat="server" CssClass="input_allotments"></asp:textbox>
																			<asp:requiredfieldvalidator id="rfvResolutionCarryover" runat="server" Display="None" ControlToValidate="txtResolutionCarryover"
																				ErrorMessage="Resolution Funds Carryover is a required field."></asp:requiredfieldvalidator>
																			<asp:customvalidator id="csvResolutionCarryover" runat="server" Display="None" ControlToValidate="txtResolutionCarryover"
																				ErrorMessage="Resolution Funds Carryover must be a dollar amount." OnServerValidate="validateCurrency"></asp:customvalidator>
																			<asp:label id="lblResolutionCarryover" runat="server" Visible="False">Label</asp:label></TD>
																		<TD class="resolution" align="right" width="91" height="1">
																			<asp:LinkButton ID="btnSlotAdjustment" runat="server" OnClientClick="javascript:window.open('PrintableSlotAdjustments.aspx','Page2','width=600, height=600, toolbar=no, location=no, menubar=no, status=no, scrollbars=yes, resizable=yes');return false;" Text="" CssClass="normaltext" /><%--<asp:Label ID="btnSlotAdjustment" runat="server" Text="" CssClass="normaltext" />--%>
																			<asp:Label ID="lblSlotAdjustment" runat="Server" Text="$0.00" Visible="False" />
																		</TD>
																		<TD class="resolution" align="right" width="91" height="1">
																			<asp:label id="lblResolutionTotalAvailabe" runat="server">Label</asp:label></TD>
																		<TD class="resolution" align="right" width="91" height="1">
																			<asp:label id="lblTransitUsed" runat="server">Label</asp:label></TD>	
																		<TD class="resolution" align="right" width="91" height="1">
																			<asp:label id="lblResolutionUsed" runat="server">Label</asp:label></TD>	
																		<TD class="resolution" align="right" width="91" height="1">
																			<asp:label id="lblResolutionTotalUsed" runat="server">Label</asp:label></TD>
																		<TD class="resolution" align="right" width="91" height="1">
																			<asp:label id="lblResolutionTotalRemaining" runat="server">Label</asp:label></TD>
																		<TD class="contracted" align="right" width="193" height="1">
																			<asp:label id="lblContractedTotalUsed" runat="server">Label</asp:label></TD>
																		<TD align="right" width="145" bgColor="#ffffff" height="1">
																			<asp:label id="lblOverallTotalUsed" runat="server">Label</asp:label></TD>
																	</TR>
																</TABLE>
															</TD>
														</TR>
														<TR>
															<TD vAlign="middle" align="center"></TD>
															<TD vAlign="top" width="810">
																<asp:validationsummary id="vlsFundAllotments" runat="server" CssClass="normaltext" HeaderText="Errors:  "
																	DisplayMode="SingleParagraph" Width="490px"></asp:validationsummary></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
											<TR>
												<TD align="center">
													<TABLE cellSpacing="1" cellPadding="1" width="850" border="0">
														<TR>
															<TD>
																<DIV class="normaltext" id="Div1" runat="server">
																	<asp:hyperlink id="hypServicesReport" runat="server" CssClass="normaltext" ToolTip="Print preview descriptions of Resolution Services offered."></asp:hyperlink></DIV>
															</TD>
														</TR>
														<TR>
															<TD>
																<uc1:tabstrip id="tbsDeptSelector" runat="server" CssClass="tabstrip_small" OnSelectionChanged="SelectionChanged"></uc1:tabstrip></TD>
														</TR>
														<TR>
															<TD>
																<TABLE cellSpacing="1" cellPadding="1" width="970" border="0">
																	<TR>
																		<TD width = "100%" colspan="2" class="normaltextimportant">
																			Click the "Add Service" link below to select from a list of available services not currently on your plan.  If you remove a service from your plan you can add it back and the information previously saved will be restored.
																		</TD>
																	</TR>
																	<TR>
																		<TD width="50">
																			<TABLE cellSpacing="1" cellPadding="1" width="52" border="0">
																				<TR>
																					<TD>
																						<asp:linkbutton id="lnkNew" runat="server" ToolTip="Click to select from a list of available services not currently on your plan." Font-Size="8pt" onclick="lnkNew_Click">Add Service</asp:linkbutton></TD>
																				</TR>
																			</TABLE>
																		</TD>
																		<TD width="970">
																			<TABLE class="plan_details" cellSpacing="1" cellPadding="1" width="970" border="0">
																				<TR>
																					<TD align="left" width="200"><STRONG>Service&nbsp;&nbsp;&nbsp; </STRONG>
																					</TD>
																					<TD align="center" width="45"><STRONG>Unit<BR>
																							Meas.</STRONG></TD>
																					<TD align="right" width="75"><STRONG>Unit<BR>
																							Cost</STRONG></TD>
																					<TD class="transit" align="right" width="55"><STRONG>Transit<BR>
																							Units</STRONG></TD>
																					<TD class="resolution" align="right" width="55"><STRONG>Resolut.<BR>
																							Units</STRONG></TD>
																					<TD class="resolution" align="right" width="75"><STRONG>Resolut.<BR>
																							Cost</STRONG></TD>
																					<TD class="contracted" align="right" width="55"><STRONG>Contract.<BR>
																							Units</STRONG></TD>
																					<TD class="contracted" align="right" width="75"><STRONG>Contract.<BR>
																							Cost</STRONG></TD>
																					<TD align="right" width="65"><STRONG>Total<BR>
																							Units</STRONG></TD>
																					<TD align="right" width="80"><STRONG>Total<BR>
																							Cost</STRONG></TD>
																					<TD align="right" width="80"><STRONG>Resolut.<BR>
																							Used</STRONG></TD>
																					<TD align="right" width="80"><STRONG>Contract.<BR>
																							Paid</STRONG></TD>				
																				</TR>
																			</TABLE>
																		</TD>
																	</TR>
																</TABLE>
															</TD>
														</TR>
														<TR>
															<TD vAlign="top" height="180">
																<asp:datagrid id="dtgPlanDetails" runat="server" CssClass="datagrid" PageSize="5" AllowPaging="True"
																	OnDeleteCommand="Delete_Click" OnItemCommand="dtgPlanDetails_ItemCommand" DataKeyField="PD_ID"
																	OnUpdateCommand="Update_Click" OnCancelCommand="Cancel_Click" OnEditCommand="Edit_Click" AutoGenerateColumns="False"
																	OnItemCreated="ItemCreated" OnItemDataBound="ItemDataBound" GridLines="None" OnPageIndexChanged="dtgPlanDetails_OnPageIndexChanged">
																	<Columns>
																		<asp:TemplateColumn>
																			<HeaderTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" width="39" border="0">
																					<TR>
																						<TD>
																							<asp:LinkButton id="lnkSaveNew" runat="server" CommandName="SaveNew" ToolTip="Save and add new service to plan">
																								<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																						<TD>
																							<asp:LinkButton id="lnkCancelNew" runat="server" CommandName="Cancel" CausesValidation="false" ToolTip="Cancel saving and adding new service to plan">
																								<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																					</TR>
																				</TABLE>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" width="39" border="0">
																					<TR>
																						<TD>
																							<asp:LinkButton id="lnkEdit" runat="server" CommandName="Edit" CausesValidation="false" ToolTip="Edit service">
																								<img border="0" src="images/edit.gif" width="16" height="16"></asp:LinkButton></TD>
																						<TD>
																							<asp:LinkButton id="lnkDelete" runat="server" CommandName="Delete" CausesValidation="false" ToolTip="Remove service from plan">
																								<img border="0" src="images/delete.gif" width="16" height="16"></asp:LinkButton></TD>
																					</TR>
																				</TABLE>
																			</ItemTemplate>
																			<EditItemTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" width="39" border="0">
																					<TR>
																						<TD>
																							<asp:LinkButton id="lnkUpdate" runat="server" CommandName="Update" ToolTip="Save edits to service">
																								<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																						<TD>
																							<asp:LinkButton id="lnkCancel" runat="server" CommandName="Cancel" CausesValidation="false" ToolTip="Cancel editing service">
																								<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																					</TR>
																				</TABLE>
																			</EditItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<HeaderTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" border="0" width="13">
																					<TR>
																						<TD>
																							<asp:ImageButton Visible="False" id="imgbtnExpandNew" CommandName="ExpandNew" ImageUrl="images/Plus.gif"
																								runat="server" ToolTip="Expand and show history for this service"></asp:ImageButton>
																						</TD>
																					</TR>
																				</TABLE>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" border="0" width="13">
																					<TR>
																						<TD>
																							<asp:ImageButton id="imgbtnExpand" CommandName="Expand" ImageUrl="images/Plus.gif" runat="server"
																								ToolTip="Expand and show history for this service"></asp:ImageButton>
																						</TD>
																					</TR>
																				</TABLE>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<HeaderTemplate>
																				<TABLE class="plan_details" cellSpacing="1" cellPadding="1" width="970" border="0">
																					<TR>
																						<TD align="left" width="200">
																							<asp:DropDownList id=ddlServiceNew runat="server" CssClass="select_plan_detail_service" OnSelectedIndexChanged="NewServiceSelected" DataValueField="SV_ID" DataTextField="SV_Name" DataSource="<%# GetServiceTable() %>" AutoPostBack="True">
																							</asp:DropDownList></TD>
																						<TD align="left" width="45">
																							<asp:DropDownList id=ddlServiceDetailNew runat="server" CssClass="select_plan_detail_u_of_m" OnSelectedIndexChanged="NewServiceDetailSelected" DataValueField="SD_ID" DataTextField="UM_Name" DataSource="<%# GetServiceDetailList() %>" AutoPostBack="True" SelectedIndex="<%# GetSelectedServiceDetail() %>">
																							</asp:DropDownList></TD>
																						<TD align="right" width="65">
																							<asp:Label id=lblUnitCostNew runat="server" text="<%# GetUnitCost() %>">
																							</asp:Label></TD>
																						<TD class="transit" align="right" width="55">
																							<asp:Label id=lblTransitUnitsNew runat="server" text="<%# GetTransitUnits() %>">
																							</asp:Label>
																							<asp:TextBox id=txtTransitUnitsNew runat="server" CssClass="input_units" text="<%# GetTransitUnits() %>">
																							</asp:TextBox>
																							<asp:CustomValidator id="csvTransitUnitsNew" runat="server" ErrorMessage="Transit Units must be a numeric value."
																								ControlToValidate="txtTransitUnitsNew" Display="None" OnServerValidate="validateNumeric" ClientValidationFunction="validateNumeric"></asp:CustomValidator>
																							<asp:RequiredFieldValidator id="rfvTransitUnitsNew" runat="server" ErrorMessage="Transit Units is a required field."
																								ControlToValidate="txtTransitUnitsNew" Display="None"></asp:RequiredFieldValidator></TD>
																						<TD class="resolution" align="right" width="55">
																							<asp:TextBox id=txtResolutionUnitsNew runat="server" CssClass="input_units" text="<%# GetResolutionUnits() %>">
																							</asp:TextBox>
																							<asp:Label id=lblResolutionUnitsNew runat="server" text="<%# GetResolutionUnits() %>">
																							</asp:Label>
																							<asp:CustomValidator id="csvResolutionUnitsNew" runat="server" ErrorMessage="Resolution Units must be a numeric value."
																								ControlToValidate="txtResolutionUnitsNew" Display="None" OnServerValidate="validateNumeric" ClientValidationFunction="validateNumeric"></asp:CustomValidator>
																							<asp:CustomValidator id="csvResolutionUnitsNewAvailableBalance" runat="server" ErrorMessage="Resolution Units value would result in a negative Resolution Funds Remaining balance."
																								ControlToValidate="txtResolutionUnitsNew" Display="None" OnServerValidate="validateAvailableFundsBalanceNewService"></asp:CustomValidator>
																							<asp:RequiredFieldValidator id="rfvResolutionUnitsNew" runat="server" ErrorMessage="Resolution Units is a required field."
																								ControlToValidate="txtResolutionUnitsNew" Display="None"></asp:RequiredFieldValidator></TD>
																						<TD class="resolution" align="right" width="75">
																							<asp:Label id="lblResolutionCostNew" runat="server"></asp:Label></TD>
																						<TD class="contracted" align="right" width="55">
																							<asp:TextBox id=txtContractedUnitsNew runat="server" CssClass="input_units" text="<%# GetContractedUnits() %>">
																							</asp:TextBox>
																							<asp:Label id=lblContractedUnitsNew runat="server" text="<%# GetContractedUnits() %>">
																							</asp:Label>
																							<asp:CustomValidator id="csvContractedUnitsNew" runat="server" ErrorMessage="Contracted Units must be a numeric value."
																								ControlToValidate="txtContractedUnitsNew" Display="None" OnServerValidate="validateNumeric" ClientValidationFunction="validateNumeric"></asp:CustomValidator>
																							<asp:RequiredFieldValidator id="rfvContractedUnitsNew" runat="server" ErrorMessage="Contracted Units is a required field."
																								ControlToValidate="txtContractedUnitsNew" Display="None"></asp:RequiredFieldValidator></TD>
																						<TD class="contracted" align="right" width="75">
																							<asp:Label id="lblContractedCostNew" runat="server"></asp:Label></TD>
																						<TD align="right" width="65">
																							<asp:Label id="lblTotalUnitsNew" runat="server"></asp:Label></TD>
																						<TD align="right" width="80">
																							<asp:Label id="lblTotalCostNew" runat="server"></asp:Label></TD>
																						<TD align="right" width="80">
																							<asp:Label id="lblResolutionDollarsUsedNew" runat="server" text="<%# GetResolutionDollarsUsed() %>"></asp:Label>
																							<asp:TextBox id="txtResolutionDollarsUsedNew" runat="server" CssClass="input_unit_cost" text="<%# GetResolutionDollarsUsed() %>"></asp:TextBox></TD>
																						<TD align="right" width="80">
																							<asp:Label id="lblContractedDollarsPaidNew" runat="server" text="<%# GetContractedDollarsPaid() %>"></asp:Label>
																							<asp:TextBox id="txtContractedDollarsPaidNew" runat="server" CssClass="input_unit_cost" text="<%# GetContractedDollarsPaid() %>"></asp:TextBox></TD>		
																					</TR>
																					<TR>
																						<TD width="955" colSpan="12">
																							<asp:ValidationSummary id="vlsPlanDetailNew" runat="server" DisplayMode="SingleParagraph" HeaderText="Errors:  "
																								Height="13px"></asp:ValidationSummary>
																							<asp:TextBox id=txtNotesNew runat="server" CssClass="input_notes" text="<%# GetPlanDetailNotes() %>" TextMode="MultiLine">
																							</asp:TextBox></TD>
																					</TR>
																				</TABLE>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<TABLE class="plan_details" cellSpacing="1" cellPadding="1" width="970" border="0">
																					<TR>
																						<TD class="service" align="left" width="200">
																							<asp:Label id=lblService runat="server" text='<%# DataBinder.Eval(Container.DataItem, "SV_Name") %>'>
																							</asp:Label>
																							<asp:LinkButton id="lnkCore" runat="server" CssClass="normal_text_small" CausesValidation="false">
																										Core</asp:LinkButton>
																							<asp:LinkButton id="lnkMOE" runat="server" CssClass="normal_text_small" CausesValidation="false">
																										MOE</asp:LinkButton></TD>
																						<TD align="center" width="45" valign="top">
																							<asp:Label id=lblUnitOfMeasure runat="server" text='<%# DataBinder.Eval(Container.DataItem, "UnitOfMeasure") %>'>
																							</asp:Label><br>
																							<asp:Label id="lblUnitOfMeasureFTE" runat="server" text='<%# GetFTELabel((bool)DataBinder.Eval(Container.DataItem, "SV_IsFTEShown")) %>'>
																							</asp:Label></TD>
																						<TD align="right" width="75" valign="top">
																							<asp:Label id=lblUnitCost runat="server" text='<%# DataBinder.Eval(Container.DataItem, "UnitCost", "{0:C}") %>'>
																							</asp:Label></TD>
																						<TD class="transit" align="right" width="55" valign="top">
																							<asp:Label id=lblTransitUnits runat="server" text='<%# DataBinder.Eval(Container.DataItem, "TransitUnits", "{0:N2}") %>'>
																							</asp:Label><br>
																							<asp:Label id="lblTransitUnitsFTE" runat="server" text='<%# ConvertHoursToFTE((bool)DataBinder.Eval(Container.DataItem, "SV_IsFTEShown"), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "TransitUnits")), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "SV_FTE_Hour_Equiv"))) %>'>
																							</asp:Label></TD>
																						<TD class="resolution" align="right" width="55" valign="top">
																							<asp:Label id=lblResolutionUnits runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ResolutionUnits", "{0:N4}") %>'>
																							</asp:Label><br>
																							<asp:Label id="lblResolutionUnitsFTE" runat="server" text='<%# ConvertHoursToFTE((bool)DataBinder.Eval(Container.DataItem, "SV_IsFTEShown"), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ResolutionUnits")), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "SV_FTE_Hour_Equiv"))) %>'>
																							</asp:Label></TD>
																						<TD class="resolution" align="right" width="75" valign="top">
																							<asp:Label id=lblResolutionCost runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ResolutionCost", "{0:C}") %>'>
																							</asp:Label></TD>
																						<TD class="contracted" align="right" width="55" valign="top">
																							<asp:Label id=lblContractedUnits runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ContractedUnits", "{0:N4}") %>'>
																							</asp:Label><br>
																							<asp:Label id="lblContractedUnitsFTE" runat="server" text='<%# ConvertHoursToFTE((bool)DataBinder.Eval(Container.DataItem, "SV_IsFTEShown"), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "ContractedUnits")), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "SV_FTE_Hour_Equiv"))) %>'>
																							</asp:Label></TD>
																						<TD class="contracted" align="right" width="75" valign="top">
																							<asp:Label id=lblContractedCost runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ContractedCost", "{0:C}") %>'>
																							</asp:Label></TD>
																						<TD align="right" width="65" valign="top">
																							<asp:Label id=lblTotalUnits runat="server" text='<%# DataBinder.Eval(Container.DataItem, "TotalUnits", "{0:N4}") %>'>
																							</asp:Label><br>
																							<asp:Label id="lblTotalUnitsFTE" runat="server" text='<%# ConvertHoursToFTE((bool)DataBinder.Eval(Container.DataItem, "SV_IsFTEShown"), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "TotalUnits")), Convert.ToDecimal(DataBinder.Eval(Container.DataItem, "SV_FTE_Hour_Equiv"))) %>'>
																							</asp:Label></TD>
																						<TD align="right" width="80" valign="top">
																							<asp:Label id=lblTotalCost runat="server" text='<%# DataBinder.Eval(Container.DataItem, "TotalCost", "{0:C}") %>'>
																							</asp:Label></TD>
																						<TD align="right" width="80" valign="top">
																							<asp:Label id=lblResolutionDollarsUsed runat="server" text='<%# DataBinder.Eval(Container.DataItem, "PD_ResolutionDollarsUsed", "{0:C}") %>'>
																							</asp:Label></TD>
																						<TD align="right" width="80" valign="top">
																							<asp:Label id=lblContractedDollarsPaid runat="server" text='<%# DataBinder.Eval(Container.DataItem, "PD_ContractedDollarsPaid", "{0:C}") %>'>
																							</asp:Label></TD>		
																					</TR>
																					<TR>
																						<TD width="795" colSpan="12">
																							<asp:Label id=lblNotes runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Notes") %>'>
																							</asp:Label></TD>
																					</TR>
																				</TABLE>
																			</ItemTemplate>
																			<EditItemTemplate>
																				<TABLE class="plan_details" cellSpacing="1" cellPadding="1" width="970" border="0">
																					<TR>
																						<TD class="service" align="left" width="200">
																							<asp:Label id=lblServiceEdit runat="server" text='<%# DataBinder.Eval(Container.DataItem, "SV_Name") %>'>
																							</asp:Label>
																							<asp:LinkButton id="lnkCore" runat="server" CssClass="normal_text_small" CausesValidation="false">
																										Core</asp:LinkButton>
																							<asp:LinkButton id="lnkMOE" runat="server" CssClass="normal_text_small" CausesValidation="false">
																										MOE</asp:LinkButton>
																							&nbsp;&nbsp;
																							<asp:CheckBox ID="chkIsMOE" Runat="server" Text="MOE"></asp:CheckBox>
																						</TD>
																						<TD align="center" width="45">
																							<asp:DropDownList id="ddlServiceDetail" runat="server" CssClass="select_plan_detail_u_of_m" OnSelectedIndexChanged="ServiceDetailSelected"
																								AutoPostBack="True"></asp:DropDownList></TD>
																						<TD align="right" width="65">
																							<asp:Label id="lblUnitCostEdit" runat="server"></asp:Label></TD>
																						<TD class="transit" align="right" width="55">
																							<asp:Label id="lblTransitUnitsEdit" runat="server"></asp:Label>
																							<asp:TextBox id="txtTransitUnits" runat="server" CssClass="input_units"></asp:TextBox>
																							<asp:CustomValidator id="csvTransitUnits" runat="server" ErrorMessage="Transit Units must be a numeric value."
																								ControlToValidate="txtTransitUnits" Display="None" OnServerValidate="validateNumeric" ClientValidationFunction="validateNumeric"></asp:CustomValidator>
																							<asp:RequiredFieldValidator id="rfvTransitUnits" runat="server" ErrorMessage="Transit Units is a required field."
																								ControlToValidate="txtTransitUnits" Display="None"></asp:RequiredFieldValidator></TD>
																						<TD class="resolution" align="right" width="55">
																							<asp:TextBox id="txtResolutionUnits" runat="server" CssClass="input_units"></asp:TextBox>
																							<asp:Label id="lblResolutionUnitsEdit" runat="server"></asp:Label>
																							<asp:CustomValidator id="csvResolutionUnits" runat="server" ErrorMessage="Resolution Units must be a numeric value."
																								ControlToValidate="txtResolutionUnits" Display="None" OnServerValidate="validateNumeric" ClientValidationFunction="validateNumeric"></asp:CustomValidator>
																							<asp:CustomValidator id="csvResolutionUnitsAvailableBalance" runat="server" ErrorMessage="Resolution Units value would result in a negative Resolution Funds Remaining balance."
																								ControlToValidate="txtResolutionUnits" Display="None" OnServerValidate="validateAvailableFundsBalance"></asp:CustomValidator>
																							<asp:RequiredFieldValidator id="rfvResolutionUnits" runat="server" ErrorMessage="Resolution Units is a required field."
																								ControlToValidate="txtResolutionUnits" Display="None"></asp:RequiredFieldValidator></TD>
																						<TD class="resolution" align="right" width="75">
																							<asp:Label id="lblResolutionCostEdit" runat="server"></asp:Label></TD>
																						<TD class="contracted" align="right" width="55">
																							<asp:TextBox id="txtContractedUnits" runat="server" CssClass="input_units"></asp:TextBox>
																							<asp:Label id="lblContractedUnitsEdit" runat="server"></asp:Label>
																							<asp:CustomValidator id="csvContractedUnits" runat="server" ErrorMessage="Contracted Units must be a numeric value."
																								ControlToValidate="txtContractedUnits" Display="None" OnServerValidate="validateNumeric" ClientValidationFunction="validateNumeric"></asp:CustomValidator>
																							<asp:RequiredFieldValidator id="rfvContractedUnits" runat="server" ErrorMessage="Contracted Units is a required field."
																								ControlToValidate="txtContractedUnits" Display="None"></asp:RequiredFieldValidator></TD>
																						<TD class="contracted" align="right" width="75">
																							<asp:Label id="lblContractedCostEdit" runat="server"></asp:Label></TD>
																						<TD align="right" width="65">
																							<asp:Label id="lblTotalUnitsEdit" runat="server"></asp:Label></TD>
																						<TD align="right" width="80">
																							<asp:Label id="lblTotalCostEdit" runat="server"></asp:Label></TD>
																						<TD align="right" width="80">
																							<asp:TextBox id="txtResolutionDollarsUsed" runat="server" CssClass="input_unit_cost"></asp:TextBox>
																							<asp:Label id="lblResolutionDollarsUsedEdit" runat="server"></asp:Label></TD>																							
																						<TD align="right" width="80">
																							<asp:TextBox id="txtContractedDollarsPaid" runat="server" CssClass="input_unit_cost"></asp:TextBox>
																							<asp:Label id="lblContractedDollarsPaidEdit" runat="server"></asp:Label></TD>			
																								
																					</TR>
																					<TR>
																						<TD width="955" colSpan="12">
																							<asp:ValidationSummary id="vlsPlanDetail" runat="server" DisplayMode="SingleParagraph" HeaderText="Errors:  "></asp:ValidationSummary>
																							<asp:TextBox id=txtNotes runat="server" CssClass="input_notes" text='<%# DataBinder.Eval(Container.DataItem, "Notes") %>' TextMode="MultiLine">
																							</asp:TextBox></TD>
																					</TR>
																				</TABLE>
																			</EditItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<ItemTemplate>
																				<asp:PlaceHolder ID="ChildRows" Visible="false" Runat="server">
																					<tr>
																						<td colspan="2" width="50">
																						</td>
																						<td width="800">
																							<asp:DataGrid id="dtgPreviousPlanDetails" DataSource='<%# ((DataRowView)Container.DataItem).CreateChildView("PastPlanDetails") %>' runat="server" AutoGenerateColumns="False" GridLines="None">
																								<Columns>
																									<asp:TemplateColumn>
																										<HeaderTemplate></HeaderTemplate>
																										<ItemTemplate>
																											<TABLE class="plan_details" cellSpacing="1" cellPadding="1" width="800" border="0">
																												<TR>
																													<TD align="right" width="200">
																														<asp:Label id="Label1" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FiscalYear") %>'>
																														</asp:Label></TD>
																													<TD align="center" width="45">
																														<asp:Label id="Label2" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "UnitOfMeasure") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="65">
																														<asp:Label id="Label3" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "UnitCost", "{0:C}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="55">
																														<asp:Label id="Label4" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "TransitUnits", "{0:N2}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="55" class="resolution">
																														<asp:Label id="Label5" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ResolutionUnits", "{0:N2}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="75" class="resolution">
																														<asp:Label id="Label6" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ResolutionCost", "{0:C}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="55" class="contracted">
																														<asp:Label id="Label7" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ContractedUnits", "{0:N2}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="75" class="contracted">
																														<asp:Label id="Label8" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "ContractedCost", "{0:C}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="65">
																														<asp:Label id="Label9" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "TotalUnits", "{0:N2}") %>'>
																														</asp:Label></TD>
																													<TD align="right" width="80">
																														<asp:Label id="Label10" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "TotalCost", "{0:C}") %>'>
																														</asp:Label></TD>
																												</TR>
																											</TABLE>
																										</ItemTemplate>
																									</asp:TemplateColumn>
																								</Columns>
																							</asp:DataGrid>
																						</td>
																					</tr>
																				</asp:PlaceHolder>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																	</Columns>
																	<PagerStyle Mode="NumericPages"></PagerStyle>
																</asp:datagrid></TD>
														</TR>
													</TABLE>
												</TD>
											</TR>
										</asp:placeholder>
										<TR>
											<TD align="center" height="10">
												<DIV align="center"><uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
											</TD>
										</TR>
									</TBODY>
								</TABLE>
								<DIV align="center">&nbsp;</DIV>
							</TD>
							<td></td>
						</TR>
						<tr>
							<td colSpan="3"></td>
						</tr>
					</TBODY>
				</TABLE>
			</div>
		</form>
	</body>
</HTML>
