<%@ Page language="c#" Inherits="LSPE.change_password" Codebehind="change_password.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="rxpress_5.css" type="text/css" rel="stylesheet">
	</HEAD>
	<BODY>
		<FORM id="frmChangePassword" name="frmChangePassword" method="post" runat="server">
			<DIV align="center">
				<TABLE height="30" cellSpacing="1" cellPadding="1" width="800" <%--bgColor="#269bea"--%> border="0">
					<tr>
						<td align="center" width="750" colSpan="3" height="5">&nbsp;
						</td>
					</tr>
					<tr>
						<td align="center" width="8" height="1">&nbsp;
						</td>
						<td align="center" width="864" height="1">
							<TABLE id="tblMain" cellSpacing="1" cellPadding="1" width="800" align="center" bgColor="#ffffff"
								border="0">
								<TR>
									<TD align="center" height="40"><uc1:header id="Header1" runat="server"></uc1:header></TD>
								</TR>
								<TR>
									<TD style="HEIGHT: 26px" align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
								</TR>
								<TR>
									<TD class="subheader" align="center" height="20"><%--Change Password--%></TD>
								</TR>
								<TR>
									<TD class="normaltext" style="HEIGHT: 231px" align="center" height="231">
										<TABLE id="Table1" style="WIDTH: 433px; HEIGHT: 194px" height="194" cellSpacing="1" cellPadding="1"
											width="433" border="0">
											<TR>
												<TD style="WIDTH: 162px; HEIGHT: 6px" height="6">
													<P class="normaltext" align="right">old password:</P>
												</TD>
												<TD style="WIDTH: 139px; HEIGHT: 6px" height="6">
													<P align="center"><asp:textbox id="txtOldPassword" runat="server" Width="100px" TextMode="Password"></asp:textbox></P>
												</TD>
												<TD style="HEIGHT: 6px" height="6"><asp:label id="lblIncorrectOldPassword" runat="server" Visible="False" ForeColor="Red" CssClass="normaltext">Incorrect password, please try again.</asp:label><asp:requiredfieldvalidator id="rfvOldPassword" runat="server" Width="144px" CssClass="normaltext" ControlToValidate="txtOldPassword"
														ErrorMessage="Please enter your old password."></asp:requiredfieldvalidator></TD>
											</TR>
											<TR>
												<TD style="WIDTH: 162px" height="10"></TD>
												<TD style="WIDTH: 139px" height="10"></TD>
												<TD height="10"></TD>
											</TR>
											<TR>
												<TD class="normaltext" style="WIDTH: 162px" height="10">
													<P align="right">new password:</P>
												</TD>
												<TD style="WIDTH: 139px" height="10">
													<P align="center"><asp:textbox id="txtNewPassword" runat="server" Width="100px" TextMode="Password"></asp:textbox></P>
												</TD>
												<TD height="10"><asp:requiredfieldvalidator id="rfvNewPassword" runat="server" CssClass="normaltext" ControlToValidate="txtNewPassword"
														ErrorMessage="Please enter your new password."></asp:requiredfieldvalidator></TD>
											</TR>
											<TR>
												<TD class="normaltext" style="WIDTH: 162px" height="10">
													<P align="right">confirm&nbsp;new:</P>
												</TD>
												<TD style="WIDTH: 139px" height="10">
													<P align="center"><asp:textbox id="txtConfirmNewPassword" runat="server" Width="100px" TextMode="Password"></asp:textbox></P>
												</TD>
												<TD height="10"><asp:requiredfieldvalidator id="rfvConfirmNewPassword" runat="server" CssClass="normaltext" ControlToValidate="txtConfirmNewPassword"
														ErrorMessage="Please confirm your new password."></asp:requiredfieldvalidator><BR>
												</TD>
											</TR>
											<TR>
												<TD style="WIDTH: 162px" colSpan="1" height="30" rowSpan="1"></TD>
												<TD style="WIDTH: 139px" vAlign="middle" height="30">
													<P align="center"><asp:linkbutton id="lnkChangePassword" runat="server" Width="98px" CssClass="normaltext" Height="13px" onclick="lnkChangePassword_Click">Change Password</asp:linkbutton></P>
												</TD>
												<TD vAlign="bottom" height="30"><asp:label id="lblMessage" runat="server" Visible="False" ForeColor="Red" CssClass="normaltext">Password changed successfully!</asp:label>
													<asp:CompareValidator id="cpvConfirmPassword" runat="server" CssClass="normaltext" ControlToValidate="txtConfirmNewPassword"
														ErrorMessage="New password and confirmed new password do not match." ControlToCompare="txtNewPassword"></asp:CompareValidator></TD>
											</TR>
										</TABLE>
									</TD>
								</TR>
								<TR>
									<TD align="center">
										<DIV align="center"><uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
									</TD>
								</TR>
							</TABLE>
						</td>
						<td height="1" width="935" align="center">
							&nbsp;
						</td>
					</tr>
					<tr>
						<td height="5" width="1799" align="center" colspan="3">
							&nbsp;
						</td>
					</tr>
				</TABLE>
			</DIV>
		</FORM>
	</BODY>
</HTML>
