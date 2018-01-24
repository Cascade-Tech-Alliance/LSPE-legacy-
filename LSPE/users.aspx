<%@ Reference Control="~/UserControls/TabStrip.ascx" %>
<%@ Import Namespace="System.Data" %>
<%@ Page language="c#" Inherits="LSPE.users" Codebehind="users.aspx.cs" MaintainScrollPositionOnPostBack = "true" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="TabStrip" Src="UserControls/TabStrip.ascx" %>
<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="rxpress_5.css" type="text/css" rel="stylesheet">
		<%--<script language="JavaScript" src="rxpress.js"></script>--%>
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
		<form id="frmUsers" method="post" runat="server">
			<div align="center">
				<table height="30" cellSpacing="1" cellPadding="1" width="800" <%--bgColor="#269bea"--%> border="0">
					<TBODY>
						<tr>
							<td align="center" width="750" colSpan="3" height="5">&nbsp;
							</td>
						</tr>
						<tr>
							<td align="center" width="8" height="1">&nbsp;
							</td>
							<td align="center" width="864" height="1">
								<TABLE id="tblMain" cellSpacing="1" cellPadding="1" width="850" align="center" bgColor="#ffffff"
									border="0">
									<TBODY>
										<TR>
											<TD align="center" height="40"><uc1:header id="Header1" runat="server"></uc1:header></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 26px" align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
										</TR>
										<TR>
											<TD class="subheader" align="center" height="20"><%--Maintain Users--%></TD>
										</TR>
										<TR>
											<TD align="center">
												<TABLE cellSpacing="1" cellPadding="1" width="850" border="0">
													<TBODY>
														<TR>
														</TR>
														<TR>
															<TD><uc1:tabstrip id="tbsUserTypeSelector" runat="server" CssClass="tabstrip_normal" OnSelectionChanged="SelectionChanged"></uc1:tabstrip></TD>
														</TR>
														<TR>
															<TD>
																<TABLE cellSpacing="1" cellPadding="1" width="800" border="0">
																	<TR>
																		<TD width="52">
																			<TABLE cellSpacing="1" cellPadding="1" width="52" border="0">
																				<TR>
																					<TD><asp:linkbutton id="lnkNew" runat="server" ToolTip="add new user" onclick="lnkNew_Click">
																							<img border="0" src="images/new.gif" width="16" height="16"></asp:linkbutton></TD>
																				</TR>
																			</TABLE>
																		</TD>
																		<TD width="750">
																			<TABLE class="users" cellSpacing="1" cellPadding="1" width="750" border="0">
																				<TR>
																					<TD align="left" width="125"><STRONG>Last Name</STRONG></TD>
																					<TD align="left" width="125"><STRONG>First Name</STRONG></TD>
																					<TD align="left" width="225"><STRONG>Email</STRONG></TD>
																					<TD align="left" width="175"><STRONG>Username</STRONG></TD>
																					<TD align="left" width="100"><STRONG>Password</STRONG></TD>
																				</TR>
																			</TABLE>
																		</TD>
																	</TR>
																</TABLE>
															</TD>
														</TR>
														<TR>
															<TD><asp:datagrid id="dtgUsers" runat="server" CssClass="datagrid" OnDeleteCommand="Delete_Click"
																	OnItemCommand="dtgUsers_ItemCommand" OnItemDataBound="ItemDataBound" OnUpdateCommand="Update_Click"
																	OnCancelCommand="Cancel_Click" OnEditCommand="Edit_Click" AutoGenerateColumns="False" OnItemCreated="ItemCreated"
																	GridLines="None" DataKeyField="US_ID">
																	<Columns>
																		<asp:TemplateColumn>
																			<HeaderTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																					<TR>
																						<TD>
																							<asp:LinkButton id="lnkSaveNew" runat="server" ToolTip="save new user" CommandName="SaveNew">
																								<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																						<TD>
																							<asp:LinkButton id="lnkCancelNew" runat="server" ToolTip="cancel new user" CommandName="Cancel"
																								CausesValidation="false">
																								<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																					</TR>
																				</TABLE>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																					<TR>
																						<TD>
																							<asp:LinkButton id="lnkEdit" runat="server" ToolTip="edit user" CommandName="Edit" CausesValidation="false">
																								<img border="0" src="images/edit.gif" width="16" height="16"></asp:LinkButton></TD>
																						<TD>
																							<asp:LinkButton id="lnkDelete" runat="server" ToolTip="delete user" CommandName="Delete" CausesValidation="false">
																								<img border="0" src="images/delete.gif" width="16" height="16"></asp:LinkButton></TD>
																					</TR>
																				</TABLE>
																			</ItemTemplate>
																			<EditItemTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																					<TR>
																						<TD>
																							<asp:LinkButton id="lnkUpdate" runat="server" ToolTip="save user" CommandName="Update">
																								<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																						<TD>
																							<asp:LinkButton id="lnkCancel" runat="server" ToolTip="cancel editing user" CommandName="Cancel"
																								CausesValidation="false">
																								<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																					</TR>
																				</TABLE>
																			</EditItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<HeaderTemplate>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<TABLE cellSpacing="1" cellPadding="1" border="0" width="13">
																					<TR>
																						<TD>
																							<asp:ImageButton id="imgbtnExpand" CommandName="Expand" ImageUrl="images/Plus.gif" runat="server"
																								ToolTip="Expand and show organization level access."></asp:ImageButton>
																						</TD>
																					</TR>
																				</TABLE>
																			</ItemTemplate>
																		</asp:TemplateColumn>
																		<asp:TemplateColumn>
																			<HeaderTemplate>
																				<TABLE class="users" cellSpacing="1" cellPadding="1" width="750" border="0">
																					<TR>
																						<TD align="left" width="125">
																							<asp:TextBox id="txtLastNameNew" runat="server" CssClass="input_name" ToolTip="Enter user's last name."></asp:TextBox></TD>
																						<TD align="left" width="125">
																							<asp:TextBox id="txtFirstNameNew" runat="server" CssClass="input_name" ToolTip="Enter user's first name."></asp:TextBox></TD>
																						<TD align="left" width="225">
																							<asp:TextBox id="txtEmailNew" runat="server" CssClass="input_email" ToolTip="Enter user's email address."></asp:TextBox></TD>
																						<TD align="left" width="175">
																							<asp:TextBox id="txtUsernameNew" runat="server" CssClass="input_username" ToolTip="Enter user's username."></asp:TextBox></TD>
																						<TD align="left" width="100">
																							<asp:TextBox id="txtPasswordNew" runat="server" CssClass="input_password" TextMode="Password"></asp:TextBox></TD>
																					</TR>
																					<TR>
																						<TD align="left" width="475" colSpan="3"></TD>
																						<TD align="right" width="175">Re-enter Password:&nbsp;</TD>
																						<TD align="left" width="100">
																							<asp:TextBox id="txtPasswordReenterNew" runat="server" CssClass="input_password" TextMode="Password"></asp:TextBox></TD>
																					</TR>
																				</TABLE>
																			</HeaderTemplate>
																			<ItemTemplate>
																				<TABLE class="users" cellSpacing="1" cellPadding="1" width="750" border="0">
																					<TR>
																						<TD align="left" width="125">
																							<asp:Label id=lblLastName runat="server" text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>'>
																							</asp:Label></TD>
																						<TD align="left" width="125">
																							<asp:Label id=lblFirstName runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>'>
																							</asp:Label></TD>
																						<TD align="left" width="225">
																							<asp:Label id=lblEmail runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'>
																							</asp:Label></TD>
																						<TD align="left" width="175">
																							<asp:Label id=lblUsername runat="server" text='<%# DataBinder.Eval(Container.DataItem, "Username") %>'>
																							</asp:Label></TD>
																						<TD align="left" width="100">**********</TD>
																					</TR>
																				</TABLE>
																			</ItemTemplate>
																			<EditItemTemplate>
																				<TABLE class="users" cellSpacing="1" cellPadding="1" width="750" border="0">
																					<TR>
																						<TD align="left" width="125">
																							<asp:TextBox id=txtLastName runat="server" CssClass="input_name" text='<%# DataBinder.Eval(Container.DataItem, "LastName") %>'>
																							</asp:TextBox>
																							<asp:RequiredFieldValidator id="rfvLastName" runat="server" ErrorMessage="Last Name is required." ControlToValidate="txtLastName"
																								Display="None"></asp:RequiredFieldValidator></TD>
																						<TD align="left" width="125">
																							<asp:TextBox id=txtFirstName runat="server" CssClass="input_name" text='<%# DataBinder.Eval(Container.DataItem, "FirstName") %>'>
																							</asp:TextBox>
																							<asp:RequiredFieldValidator id="rfvFirstName" runat="server" ErrorMessage="First Name is required." ControlToValidate="txtFirstName"
																								Display="None"></asp:RequiredFieldValidator></TD>
																						<TD align="left" width="225">
																							<asp:TextBox id=txtEmail runat="server" CssClass="input_email" text='<%# DataBinder.Eval(Container.DataItem, "Email") %>'>
																							</asp:TextBox>
																							<asp:RequiredFieldValidator id="rfvEmail" runat="server" ErrorMessage="Email is required." ControlToValidate="txtEmail"
																								Display="None"></asp:RequiredFieldValidator>
																							<asp:RegularExpressionValidator id="revEmail" runat="server" Width="66px" ErrorMessage="Email format is invalid."
																								ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" Display="None"></asp:RegularExpressionValidator></TD>
																						<TD align="left" width="175">
																							<asp:TextBox id=txtUsername runat="server" CssClass="input_username" text='<%# DataBinder.Eval(Container.DataItem, "Username") %>'>
																							</asp:TextBox>
																							<asp:RequiredFieldValidator id="rfvUsername" runat="server" ErrorMessage="Username is required." ControlToValidate="txtUsername"
																								Display="None"></asp:RequiredFieldValidator></TD>
																						<TD align="left" width="100">
																							<asp:TextBox id="txtPassword" runat="server" CssClass="input_password" TextMode="Password"></asp:TextBox>
																							<asp:RequiredFieldValidator id="rfvPassword" runat="server" ErrorMessage="Password is a required field." ControlToValidate="txtPassword"
																								Display="None"></asp:RequiredFieldValidator>
																							<asp:CompareValidator id="cpvPassword" runat="server" ErrorMessage="Password and Re-entered Password do not match."
																								ControlToValidate="txtPassword" Display="None" ControlToCompare="txtPasswordReenter"></asp:CompareValidator></TD>
																					<TR>
																						<TD colSpan="4">
																							<TABLE style="WIDTH: 646px; HEIGHT: 28px" cellSpacing="0" cellPadding="0" width="646" border="0">
																								<TR>
																									<TD align="left" width="550">
																										<asp:ValidationSummary id="vlsUser" runat="server" DisplayMode="SingleParagraph" HeaderText="Errors:"></asp:ValidationSummary></TD>
																									<TD align="right" width="150">Re-enter Password:&nbsp;</TD>
																								</TR>
																							</TABLE>
																						</TD>
																						<TD align="left" width="100">
																							<asp:TextBox id="txtPasswordReenter" runat="server" CssClass="input_password" TextMode="Password"></asp:TextBox></TD>
																					</TR>
														</TR>
												</TABLE>
												</EditItemTemplate> </asp:TemplateColumn>
												<asp:TemplateColumn>
													<ItemTemplate>
														<asp:PlaceHolder ID="plhOrganizationUsers" Visible="false" Runat="server">
															<tr>
																<td colspan="2" width="50">
																</td>
																<td width="600">
																	<table cellSpacing="1" cellPadding="1" border="0" width="250">
																		<TR>
																			<TD width="37">
																				<asp:LinkButton id="lnkNewOrganizationUser" OnClick="lnkNewOrganizationUser_Click" runat="server">
																					<img border="0" src="images/new.gif" width="16" height="16"></asp:LinkButton></TD>
																			<td>
																				<TABLE class="users" cellSpacing="1" cellPadding="1" width="200" border="0">
																					<TR>
																						<TD align="left" width="200"><STRONG>Organizations</STRONG></TD>
																					</TR>
																				</TABLE>
																			</td>
																		</TR>
																	</table>
																	<asp:DataGrid 
																		id="dtgOrganizationUsers"
																		DataSource='<%# ((DataRowView)Container.DataItem).CreateChildView("OrganizationUsers") %>'
																		runat="server"
																		OnDeleteCommand="DeleteOrganizationUser_Click" OnItemCommand="dtgOrganizationUsers_ItemCommand" DataKeyField="ORU_ID" OnUpdateCommand="UpdateOrganizationUser_Click" OnCancelCommand="CancelOrganizationUser_Click" OnEditCommand="EditOrganizationUser_Click" AutoGenerateColumns="False" OnItemCreated="OrganizationUsersItemCreated" OnItemDataBound="OrganizationUsersItemDataBound"
																		GridLines="None">
																		<Columns>
																			<asp:TemplateColumn>
																				<HeaderTemplate>
																					<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																						<TR>
																							<TD>
																								<asp:LinkButton id="lnkSaveNewOrganizationUser" runat="server" CommandName="SaveNew">
																									<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																							<TD>
																								<asp:LinkButton id="lnkCancelNewOrganizationUser" runat="server" CommandName="Cancel" CausesValidation="false">
																									<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																						</TR>
																					</TABLE>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																						<TR>
																							<TD>
																								<asp:LinkButton id="lnkEditOrganizationUser" runat="server" CommandName="Edit" CausesValidation="false">
																									<img border="0" src="images/edit.gif" width="16" height="16"></asp:LinkButton></TD>
																							<TD>
																								<asp:LinkButton id="lnkDeleteOrganizationUser" runat="server" CommandName="Delete" CausesValidation="false">
																									<img border="0" src="images/delete.gif" width="16" height="16"></asp:LinkButton></TD>
																						</TR>
																					</TABLE>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<TABLE cellSpacing="1" cellPadding="1" width="30" border="0">
																						<TR>
																							<TD>
																								<asp:LinkButton id="lnkSaveOrganizationUser" runat="server" CommandName="Update">
																									<img border="0" src="images/save.gif" width="16" height="16"></asp:LinkButton></TD>
																							<TD>
																								<asp:LinkButton id="lnkCancelOrganizationUser" runat="server" CommandName="Cancel" CausesValidation="false">
																									<img border="0" src="images/cancel.gif" width="16" height="16"></asp:LinkButton></TD>
																						</TR>
																					</TABLE>
																				</EditItemTemplate>
																			</asp:TemplateColumn>
																			<asp:TemplateColumn>
																				<HeaderTemplate>
																					<TABLE class="users" cellSpacing="1" cellPadding="1" width="200" border="0">
																						<TR>
																							<TD align="left" width="200">
																								<asp:DropDownList id="ddlOrganizationNew" CssClass="normaltext" runat="server" DataSource='<%# GetOrganizationList() %>' DataTextField="OR_Name" DataValueField="OR_ID">
																								</asp:DropDownList></TD>
																						</TR>
																					</TABLE>
																				</HeaderTemplate>
																				<ItemTemplate>
																					<TABLE class="users" cellSpacing="1" cellPadding="1" width="200" border="0">
																						<TR>
																							<TD align="left" width="200">
																								<asp:Label id="lblOrganization" runat="server" text='<%# DataBinder.Eval(Container.DataItem, "OR_Name") %>'>
																								</asp:Label></TD>
																						</TR>
																					</TABLE>
																				</ItemTemplate>
																				<EditItemTemplate>
																					<TABLE class="users" cellSpacing="1" cellPadding="1" width="200" border="0">
																						<TR>
																							<TD align="left" width="200">
																								<asp:DropDownList id="ddlOrganization" CssClass="normaltext" runat="server" DataSource='<%# GetOrganizationList() %>' DataTextField="OR_Name" DataValueField="OR_ID" SelectedIndex='<%# GetSelectedOrganization((string)DataBinder.Eval(Container.DataItem, "OR_Name")) %>'>
																								</asp:DropDownList></TD>
																						</TR>
																					</TABLE>
																				</EditItemTemplate>
																			</asp:TemplateColumn>
																		</Columns>
																	</asp:DataGrid>
			</div>
			</TD> </TR> </asp:placeholder> </ItemTemplate> </asp:TemplateColumn> </Columns> 
			</asp:datagrid></TD></TR></TBODY></TABLE></TD></TR>
			<TR>
				<TD align="center">
					<DIV align="center"><uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
				</TD>
			</TR>
			</TBODY></TABLE></TD>
			<td align="center" width="935" height="1">&nbsp;
			</td>
			</TR>
			<tr>
				<td align="center" width="1799" colSpan="3" height="5">&nbsp;
				</td>
			</tr>
			</TBODY></TABLE></DIV></form>
	</body>
</HTML>
