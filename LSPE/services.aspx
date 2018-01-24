<%@ Reference Control="~/UserControls/TabStrip.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TabStrip" Src="UserControls/TabStrip.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Text.RegularExpressions" %>
<%@ Page language="c#" Inherits="LSPE.services" Codebehind="services.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="rxpress_5.css" type="text/css" rel="stylesheet">
		<script language="JavaScript" src="rxpress.js"></script>
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
				<TABLE height="30" cellSpacing="1" cellPadding="1" width="800" <%--bgColor="#269bea"--%> border="0">
					<TBODY>
						<TR>
							<TD align="center" width="750" colSpan="3" height="5">&nbsp;
							</TD>
						</TR>
						<TR>
							<TD align="center" width="8" height="1">&nbsp;
							</TD>
							<TD align="center" width="850" height="1">
								<TABLE id="tblMain" cellSpacing="1" cellPadding="1" width="800" align="center" bgColor="#ffffff"
									border="0">
									<TR>
										<TD align="center" height="40"><uc1:header id="Header1" runat="server"></uc1:header></TD>
									</TR>
									<TR>
										<TD align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
									</TR>
									<TR>
										<TD vAlign="middle" align="center" height="20">
											<P class="subheader"><%--Services--%></P>
										</TD>
									</TR>
									<TR>
										<TD vAlign="middle" align="center" height="10">
											<P align="right"><asp:HyperLink id="hypServicesReport" runat="server" CssClass="normaltext">View Printer Friendly Report</asp:HyperLink></P>
										</TD>
									</TR>
									<TR>
									</TR>
									<TR>
									</TR>
									<TR>
									</TR>
									<TR>
									</TR>
									<TR>
									</TR>
									<TR>
									</TR>
									<TR>
										<TD align="center">
											<TABLE cellSpacing="1" cellPadding="1" width="850" border="0">
												<TR>
												</TR>
												<TR>
													<TD><uc1:tabstrip id="tbsDeptSelector" runat="server" CssClass="tabstrip_normal" OnSelectionChanged="SelectionChanged"></uc1:tabstrip></TD>
												</TR>
												<TR>
													<TD>
														<TABLE cellSpacing="1" cellPadding="1" width="850" border="0">
															<TR>
																<TD width="50">
																	<TABLE cellSpacing="1" cellPadding="1" width="52" border="0">
																		<TR>
																			<TD><asp:linkbutton id="lnkNew" runat="server" onclick="lnkNew_Click">
																					<img border="0" src="images/new.gif" width="16" height="16"></asp:linkbutton></TD>
																		</TR>
																	</TABLE>
																</TD>
																<TD width="800">
																	<TABLE class="services" cellSpacing="1" cellPadding="1" width="800" border="0">
																		<TR>
																			<TD align="left" width="750"><STRONG>Service Type</STRONG></TD>
																		</TR>
																	</TABLE>
																</TD>
															</TR>
														</TABLE>
													</TD>
												</TR>
												<TR>
													<TD><asp:datagrid id="dtgServiceTypes" runat="server" CssClass="datagrid" AllowPaging="True" PageSize="5"
															GridLines="None" OnPageIndexChanged="dtgServiceTypes_OnPageIndexChanged" OnItemDataBound="ItemDataBound"
															OnItemCreated="ItemCreated" AutoGenerateColumns="False" OnEditCommand="Edit_Click" OnCancelCommand="Cancel_Click"
															OnUpdateCommand="Update_Click" DataKeyField="ST_ID" OnItemCommand="dtgServiceTypes_ItemCommand"
															OnDeleteCommand="Delete_Click">
															<Columns>
																<asp:TemplateColumn>
																	<HeaderTemplate>
																		<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																			<TR>
																				<TD>
																					<asp:LinkButton id="lnkSaveNew" runat="server" CommandName="SaveNew">
																						<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																				<TD>
																					<asp:LinkButton id="lnkCancelNew" runat="server" CommandName="Cancel" CausesValidation="false">
																						<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																			</TR>
																		</TABLE>
																	</HeaderTemplate>
																	<ItemTemplate>
																		<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																			<TR>
																				<TD>
																					<asp:LinkButton id="lnkEdit" runat="server" CommandName="Edit" CausesValidation="false">
																						<img border="0" src="images/edit.gif" width="16" height="16"></asp:LinkButton></TD>
																				<TD>
																					<asp:LinkButton id="lnkDelete" runat="server" CommandName="Delete" CausesValidation="false">
																						<img border="0" src="images/delete.gif" width="16" height="16"></asp:LinkButton></TD>
																			</TR>
																		</TABLE>
																	</ItemTemplate>
																	<EditItemTemplate>
																		<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																			<TR>
																				<TD>
																					<asp:LinkButton id="lnkUpdate" runat="server" CommandName="Update">
																						<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																				<TD>
																					<asp:LinkButton id="lnkCancel" runat="server" CommandName="Cancel" CausesValidation="false">
																						<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																			</TR>
																		</TABLE>
																	</EditItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn>
																	<ItemTemplate>
																		<TABLE cellSpacing="1" cellPadding="1" border="0">
																			<TR>
																				<TD>
																					<asp:ImageButton id="imgbtnExpand" runat="server" ImageUrl="images/Plus.gif" CommandName="Expand"></asp:ImageButton></TD>
																			</TR>
																		</TABLE>
																	</ItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn>
																	<HeaderTemplate>
																		<TABLE class="services" cellSpacing="1" cellPadding="1" width="800" border="0">
																			<TR>
																				<TD vAlign="top" align="left" width="750">
																					<asp:TextBox id=txtServiceTypeNew runat="server" CssClass="input_service" text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
																					</asp:TextBox><BR>
																					<asp:TextBox id=txtServiceTypeDescNew runat="server" CssClass="input_service_type_desc" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' TextMode="MultiLine">
																					</asp:TextBox>
																					<asp:RequiredFieldValidator id="rfvServiceTypeNew" runat="server" Display="None" ControlToValidate="txtServiceTypeNew"
																						ErrorMessage="Service Tyep is a required field."></asp:RequiredFieldValidator></TD>
																			</TR>
																			<TR>
																				<TD width="795">
																					<asp:RequiredFieldValidator id="rfvServiceDescNew" runat="server" Display="None" ControlToValidate="txtServiceTypeDescNew"
																						ErrorMessage="Description is a required field."></asp:RequiredFieldValidator>
																					<asp:ValidationSummary id="vlsServiceTypeNew" runat="server" Height="13px" HeaderText="Errors:  " DisplayMode="SingleParagraph"></asp:ValidationSummary></TD>
																			</TR>
																		</TABLE>
																	</HeaderTemplate>
																	<ItemTemplate>
																		<TABLE class="services" cellSpacing="1" cellPadding="1" width="800" border="0">
																			<TR>
																				<TD vAlign="top" align="left" width="750">
																					<asp:Label id=lblServiceType runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' Font-Bold>
																					</asp:Label><BR>
																					<asp:Label id=lblServiceTypeDesc runat="server" text='<%# Regex.Replace((string)DataBinder.Eval(Container.DataItem, "Description"), System.Environment.NewLine, "<br>") %>'>
																					</asp:Label></TD>
																			</TR>
																		</TABLE>
																	</ItemTemplate>
																	<EditItemTemplate>
																		<TABLE class="services" cellSpacing="1" cellPadding="1" width="800" border="0">
																			<TR>
																				<TD vAlign="top" align="left" width="750">
																					<asp:TextBox id=txtServiceType runat="server" CssClass="input_service" text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
																					</asp:TextBox>
																					<asp:RequiredFieldValidator id="rfvServiceType" runat="server" Display="None" ControlToValidate="txtServiceType"
																						ErrorMessage="Service Type is a required field."></asp:RequiredFieldValidator><BR>
																					<asp:TextBox id=txtServiceTypeDesc runat="server" CssClass="input_service_type_desc" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>' TextMode="MultiLine">
																					</asp:TextBox></TD>
																			</TR>
																			<TR>
																				<TD width="795">
																					<asp:RequiredFieldValidator id="rfvServiceTypeDesc" runat="server" Display="None" ControlToValidate="txtServiceTypeDesc"
																						ErrorMessage="Description is a required field."></asp:RequiredFieldValidator>
																					<asp:ValidationSummary id="vlsServiceType" runat="server" Height="13px" HeaderText="Errors:  " DisplayMode="SingleParagraph"></asp:ValidationSummary></TD>
																			</TR>
																		</TABLE>
																	</EditItemTemplate>
																</asp:TemplateColumn>
																<asp:TemplateColumn>
																	<ItemTemplate>
																		<asp:PlaceHolder id="ChildRows" Runat="server" Visible="false">
																			<TR>
																				<TD width="50" colspan="2"></TD>
																				<TD width="800">
																					<TABLE cellSpacing="1" cellPadding="1" width="800" border="0">
																						<TR>
																							<TD width="73">
																								<asp:LinkButton id="lnkNewService" onclick="lnkNewService_Click" runat="server">
																									<img border="0" src="images/new.gif" width="16" height="16"></asp:LinkButton></TD>
																							<TD>
																								<TABLE class="services" cellSpacing="1" cellPadding="1" width="743" border="0">
																									<TR>
																										<TD align="left" width="250"><STRONG>Service</STRONG></TD>
																										<TD align="center" width="200"><STRONG>Account</STRONG></TD>
																										<TD align="center" width="50"><STRONG>MOE</STRONG></TD>
																										<TD align="center" width="50"><STRONG>Core</STRONG></TD>
																										<TD align="center" width="50"><STRONG>FTE</STRONG></TD>
																										<TD align="center" width="143"><STRONG>Year</STRONG></TD>
																									</TR>
																								</TABLE>
																							</TD>
																						</TR>
																					</TABLE>
																					<asp:DataGrid id=dtgServices runat="server" OnDeleteCommand="DeleteService_Click" OnItemCommand="dtgServices_ItemCommand" DataKeyField="SV_ID" OnUpdateCommand="UpdateService_Click" OnCancelCommand="CancelService_Click" OnEditCommand="EditService_Click" AutoGenerateColumns="False" OnItemCreated="ServiceItemCreated" OnItemDataBound="ServiceItemDataBound" GridLines="None" DataSource='<%# ((DataRowView)Container.DataItem).CreateChildView("RelatedServices") %>'>
																						<Columns>
																							<asp:TemplateColumn>
																								<HeaderTemplate>
																									<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																										<TR>
																											<TD>
																												<asp:LinkButton id="lnkSaveNewService" runat="server" CommandName="SaveNew">
																													<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																											<TD>
																												<asp:LinkButton id="lnkCancelNewService" runat="server" CommandName="Cancel" CausesValidation="false">
																													<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																										</TR>
																									</TABLE>
																								</HeaderTemplate>
																								<ItemTemplate>
																									<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																										<TR>
																											<TD>
																												<asp:LinkButton id="lnkEditService" runat="server" CommandName="Edit" CausesValidation="false">
																													<img border="0" src="images/edit.gif" width="16" height="16"></asp:LinkButton></TD>
																											<TD>
																												<asp:LinkButton id="lnkDeleteService" runat="server" CommandName="Delete" CausesValidation="false">
																													<img border="0" src="images/delete.gif" width="16" height="16"></asp:LinkButton></TD>
																										</TR>
																									</TABLE>
																								</ItemTemplate>
																								<EditItemTemplate>
																									<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																										<TR>
																											<TD>
																												<asp:LinkButton id="lnkSaveService" runat="server" CommandName="Update">
																													<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																											<TD>
																												<asp:LinkButton id="lnkCancelEditService" runat="server" CommandName="Cancel" CausesValidation="false">
																													<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																										</TR>
																									</TABLE>
																								</EditItemTemplate>
																							</asp:TemplateColumn>
																							<asp:TemplateColumn>
																								<HeaderTemplate></HeaderTemplate>
																								<ItemTemplate>
																									<TABLE width="5" cellSpacing="1" cellPadding="1" border="0">
																										<TR>
																											<TD width="5">
																												<asp:ImageButton id="imgbtnExpandServiceDetails" CommandName="Expand" ImageUrl="images/Plus.gif"
																													runat="server"></asp:ImageButton>
																											</TD>
																										</TR>
																									</TABLE>
																								</ItemTemplate>
																							</asp:TemplateColumn>
																							<asp:TemplateColumn>
																								<HeaderTemplate>
																									<TABLE class="services" cellSpacing="1" cellPadding="1" width="743" border="0">
																										<TR>
																											<TD align="left" class="service" width="250">
																												<asp:Textbox id="txtServiceNew" CssClass="input_service" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
																												</asp:Textbox></TD>
																											<TD align="center" width="200">
																												<asp:Textbox id="txtAccountNew" CssClass="input_account" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
																												</asp:Textbox></TD>
																											<TD align="center" width="75">
																												<asp:Checkbox id="chkMOENew" runat="server" checked="False"></asp:Checkbox></TD>
																											<TD align="center" width="75">
																												<asp:Checkbox id="chkCoreNew" runat="server" checked="False"></asp:Checkbox></TD>
																											<TD align="center" width="75">
																												<asp:Checkbox id="chkFTENew" runat="server" checked="False"></asp:Checkbox></TD>
																											<TD align="center" width="143">
																												<asp:DropDownList id="ddlYearNew" runat="server" DataSource='<%# GetYearList() %>' DataTextField="YR_Desc" DataValueField="YR_ID">
																												</asp:DropDownList></TD>
																										</TR>
																										<TR>
																											<TD width="605" colSpan="5">
																												<asp:Textbox id="txtServiceDescNew" CssClass="input_service_desc" TextMode="MultiLine" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
																												</asp:Textbox>
																											</TD>
																										</TR>
																									</TABLE>
																								</HeaderTemplate>
																								<ItemTemplate>
																									<TABLE class="services" cellSpacing="1" cellPadding="1" width="743" border="0">
																										<TR>
																											<TD align="left" class="service" width="250">
																												<asp:Label id="lblService" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
																												</asp:Label></TD>
																											<TD align="center" width="200">
																												<asp:Label id="lblAccount" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
																												</asp:Label></TD>
																											<TD align="center" width="50">
																												<asp:Label id="lblMOE" runat="server" text='<%# GetYesNo((bool)DataBinder.Eval(Container.DataItem, "MOE")) %>'>
																												</asp:Label></TD>
																											<TD align="center" width="50">
																												<asp:Label id="lblCore" runat="server" text='<%# GetYesNo((bool)DataBinder.Eval(Container.DataItem, "Core")) %>'>
																												</asp:Label></TD>
																											<TD align="center" width="50">
																												<asp:Label id="lblFTE" runat="server" text='<%# GetYesNo((bool)DataBinder.Eval(Container.DataItem, "SV_IsFTEShown")) %>'>
																												</asp:Label></TD>
																											<TD align="center" width="143">
																												<asp:Label id="lblYear" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Year") %>'>
																												</asp:Label></TD>
																										</TR>
																										<TR>
																											<TD width="605" colSpan="6">
																												<asp:Label id="lblServiceDescription" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
																												</asp:Label>
																											</TD>
																										</TR>
																									</TABLE>
																								</ItemTemplate>
																								<EditItemTemplate>
																									<TABLE class="services" cellSpacing="1" cellPadding="1" width="743" border="0">
																										<TR>
																											<TD align="left" class="service" width="250">
																												<asp:Textbox id="txtService" CssClass="input_service" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Name") %>'>
																												</asp:Textbox></TD>
																											<TD align="center" width="200">
																												<asp:Textbox id="txtAccount" CssClass="input_account" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Account") %>'>
																												</asp:Textbox></TD>
																											<TD align="center" width="50">
																												<asp:Checkbox id="chkMOE" runat="server" checked='<%# DataBinder.Eval(Container.DataItem, "MOE") %>'>
																												</asp:Checkbox></TD>
																											<TD align="center" width="50">
																												<asp:Checkbox id="chkCore" runat="server" checked='<%# DataBinder.Eval(Container.DataItem, "Core") %>'>
																												</asp:Checkbox></TD>
																											<TD align="center" width="50">
																												<asp:Checkbox id="chkFTE" runat="server" checked='<%# DataBinder.Eval(Container.DataItem, "SV_IsFTEShown") %>'>
																												</asp:Checkbox></TD>
																											<TD align="center" width="143">
																												<asp:DropDownList id="ddlYear" runat="server" DataSource='<%# GetYearList() %>' DataTextField="YR_Desc" DataValueField="YR_ID" SelectedIndex='<%# GetSelectedYear((string)DataBinder.Eval(Container.DataItem, "Year")) %>'>
																												</asp:DropDownList></TD>
																										</TR>
																										<TR>
																											<TD width="605" colSpan="6">
																												<asp:Textbox id="txtServiceDesc" CssClass="input_service_desc" TextMode="MultiLine" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Description") %>'>
																												</asp:Textbox>
																											</TD>
																										</TR>
																									</TABLE>
																								</EditItemTemplate>
																							</asp:TemplateColumn>
																							<asp:TemplateColumn>
																								<ItemTemplate>
																									<asp:PlaceHolder ID="plhServiceDetails" Visible="false" Runat="server">
																										<tr>
																											<td colspan="2" width="50">
																											</td>
																											<td width="250">
																												<table cellSpacing="1" cellPadding="1" border="0" width="250">
																													<TR>
																														<TD width="37">
																															<asp:LinkButton id="lnkNewServiceDetail" OnClick="lnkNewServiceDetail_Click" runat="server">
																																<img border="0" src="images/new.gif" width="16" height="16"></asp:LinkButton></TD>
																														<td>
																															<TABLE class="services" cellSpacing="1" cellPadding="1" width="200" border="0">
																																<TR>
																																	<TD align="center" width="100"><STRONG>Unit Measure</STRONG></TD>
																																	<TD align="right" width="100"><STRONG>Unit Cost</STRONG></TD>
																																</TR>
																															</TABLE>
																														</td>
																													</TR>
																												</table>
																												<asp:DataGrid id="dtgServiceDetails" DataSource='<%# ((DataRowView)Container.DataItem).CreateChildView("RelatedServiceDetails") %>' runat="server" OnDeleteCommand="DeleteServiceDetail_Click" OnItemCommand="dtgServiceDetails_ItemCommand" DataKeyField="SD_ID" OnUpdateCommand="UpdateServiceDetail_Click" OnCancelCommand="CancelServiceDetail_Click" OnEditCommand="EditServiceDetail_Click" AutoGenerateColumns="False" OnItemCreated="ServiceDetailItemCreated" OnItemDataBound="ServiceDetailItemDataBound" GridLines="None">
																													<Columns>
																														<asp:TemplateColumn>
																															<HeaderTemplate>
																																<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																																	<TR>
																																		<TD>
																																			<asp:LinkButton id="Linkbutton2" runat="server" CommandName="SaveNew">
																																				<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																																		<TD>
																																			<asp:LinkButton id="Linkbutton3" runat="server" CommandName="Cancel" CausesValidation="false">
																																				<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																																	</TR>
																																</TABLE>
																															</HeaderTemplate>
																															<ItemTemplate>
																																<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																																	<TR>
																																		<TD>
																																			<asp:LinkButton id="Linkbutton4" runat="server" CommandName="Edit" CausesValidation="false">
																																				<img border="0" src="images/edit.gif" width="16" height="16"></asp:LinkButton></TD>
																																		<TD>
																																			<asp:LinkButton id="lnkDeleteServiceDetail" runat="server" CommandName="Delete" CausesValidation="false">
																																				<img border="0" src="images/delete.gif" width="16" height="16"></asp:LinkButton></TD>
																																	</TR>
																																</TABLE>
																															</ItemTemplate>
																															<EditItemTemplate>
																																<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																																	<TR>
																																		<TD>
																																			<asp:LinkButton id="Linkbutton6" runat="server" CommandName="Update">
																																				<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																																		<TD>
																																			<asp:LinkButton id="Linkbutton7" runat="server" CommandName="Cancel" CausesValidation="false">
																																				<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																																	</TR>
																																</TABLE>
																															</EditItemTemplate>
																														</asp:TemplateColumn>
																														<asp:TemplateColumn>
																															<HeaderTemplate>
																																<TABLE class="services" cellSpacing="1" cellPadding="1" width="200" border="0">
																																	<TR>
																																		<TD align="center" width="100">
																																			<asp:DropDownList id="ddlUnitMeasureNew" CssClass="normaltext" runat="server" DataSource='<%# GetUnitMeasureList() %>' DataTextField="UM_Name" DataValueField="UM_ID">
																																			</asp:DropDownList></TD>
																																		<TD align="center" width="100">
																																			<asp:TextBox id="txtUnitCostNew" CssClass="input_unit_cost" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "SD_UnitCost") %>'>
																																			</asp:TextBox></TD>
																																	</TR>
																																</TABLE>
																															</HeaderTemplate>
																															<ItemTemplate>
																																<TABLE class="services" cellSpacing="1" cellPadding="1" width="200" border="0">
																																	<TR>
																																		<TD align="center" width="100">
																																			<asp:Label id="lblUnitMeasure" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "UM_Name") %>'>
																																			</asp:Label></TD>
																																		<TD align="right" width="100">
																																			<asp:Label id="lblUnitCost" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "SD_UnitCost", "{0:C}") %>'>
																																			</asp:Label></TD>
																																</TABLE>
																															</ItemTemplate>
																															<EditItemTemplate>
																																<TABLE class="services" cellSpacing="1" cellPadding="1" width="200" border="0">
																																	<TR>
																																		<TD align="center" width="100">
																																			<asp:DropDownList id="ddlUnitMeasure" CssClass="normaltext" runat="server" DataSource='<%# GetUnitMeasureList() %>' DataTextField="UM_Name" DataValueField="UM_ID" SelectedIndex='<%# GetSelectedUnitMeasure((string)DataBinder.Eval(Container.DataItem, "UM_Name")) %>'>
																																			</asp:DropDownList></TD>
																																		<TD align="center" width="100">
																																			<asp:TextBox id="txtUnitCost" CssClass="input_unit_cost" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "SD_UnitCost", "{0:C}") %>'>
																																			</asp:TextBox></TD>
																																	</TR>
																																</TABLE>
																															</EditItemTemplate>
																														</asp:TemplateColumn>
																													</Columns>
																												</asp:DataGrid>
																											</td>
																										</tr>
																									</asp:PlaceHolder>
																								</ItemTemplate>
																							</asp:TemplateColumn>
																						</Columns>
																					</asp:DataGrid></TD>
																			</TR>
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
									<TR>
										<TD align="center" height="10">
											<DIV align="center"><uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
										</TD>
									</TR>
								</TABLE>
							</TD>
							<td></td>
						</TR>
						<tr>
							<td colSpan="3" height="15"></td>
						</tr>
					</TBODY>
				</TABLE>
			</div>
		</form>
	</body>
</HTML>
