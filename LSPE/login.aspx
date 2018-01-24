<%@ Page language="c#" Inherits="LSPE.login" Codebehind="login.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS - Login</title>
		<LINK href="rxpress_3.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body onload="document.getElementById('txtUserName').focus();">
		<p>&nbsp;</p>
		<form id="frmLogin" name="frmLogin" method="post" runat="server">
			<div align="center">
				<table cellSpacing="1" cellPadding="1" width="800" <%--bgColor="#269bea"--%> border="0">
					<tr>
						<td align="center" width="800" colSpan="3" height="5">&nbsp;
						</td>
					</tr>
					<tr>
						<td align="center" width="8" height="1" rowSpan="2">&nbsp;
						</td>
						<td align="center" width="858" height="1" bgColor="white">
							<table height="404" cellSpacing="3" cellPadding="4" width="800" bgColor="#ffffff" border="0">
								<TR>
									<TD align="center" width="800" colSpan="2" height="40">
										<uc1:header id="Header1" runat="server"></uc1:header></TD>
								</TR>
								<TR>
									<TD vAlign="bottom" align="left" width="250" height="40"><DIV align="center">
											<TABLE id="Table2" height="238" cellSpacing="1" width="250" border="0">
												<TR>
													<TD width="123" height="25"><FONT face="Verdana">username</FONT></TD>
													<TD width="150" height="25"><INPUT id="txtUserName" type="text" size="20" name="txtUserName" runat="server"></TD>
												</TR>
												<TR>
													<TD width="123" height="25"><FONT face="Verdana">password </FONT>
													</TD>
													<TD width="150" height="25"><INPUT id="txtPassword" type="password" size="20" name="txtPassword" runat="server"></TD>
												</TR>
												<TR>
												</TR>
												<TR>
												</TR>
												<TR>
													<TD width="110" colSpan="2" height="31">
														<TABLE id="Table1" style="WIDTH: 254px; HEIGHT: 28px" cellSpacing="1" cellPadding="1" width="254"
															border="0">
															<TR>
																<TD align="center" width="243">
																	<asp:label id="lblLoginError" runat="server" Width="197px" Font-Names="Verdana" ForeColor="Red"
																		Font-Size="Smaller">Login failed, please try again.</asp:label></TD>
																<TD>
																	<P align="right">
																		<asp:button id="btnLogin" runat="server" Text="Login" onclick="btnLogin_Click"></asp:button></P>
																</TD>
															</TR>
                                                            <tr>
                                                                <td colspan="2" height="75" valign="bottom">
                                                                    <font size="2" face='Verdana'>Trouble logging in? Forgot your password?  Contact April Felguth at 503-385-4694.</font>
                                                                </td>
                                                            </tr>
														</TABLE>
													</TD>
												</TR>
												<TR>
													<TD width="123" height="9"></TD>
													<TD vAlign="middle" width="150" height="9">
														<P align="right">&nbsp;</P>
													</TD>
												</TR>
												<TR>
												</TR>
												<TR>
													<TD width="300" colSpan="2" height="48">
														<P align="center">&nbsp;</P>
													</TD>
												</TR>
											</TABLE>
										</DIV>
									</TD>
									<TD vAlign="bottom" align="right" ><IMG src="images/NewDayAtWESD.jpg"></TD>
								</TR>
								<center>
									<center></center>
							</table>
							</CENTER></td>
						<td align="center" width="858" height="1">&nbsp;
						</td>
					</tr>
					<TR>
						<TD width="858" height="5"></TD>
					</TR>
					<tr>
						<td width="858" height="5">
						&nbsp;
						<td width="858" height="5">&nbsp;</td>
					</tr>
				</table>
			</div>
		</form>
		<p>&nbsp;</p>
		<p>&nbsp;</p>
	</body>
</HTML>
