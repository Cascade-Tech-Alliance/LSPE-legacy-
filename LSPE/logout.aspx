<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Page language="c#" Inherits="LSPE.logout" Codebehind="logout.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="rxpress_5.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body>
		<div align="center">
			<table border="0" <%--bgcolor="#269bea"--%> cellspacing="1" width="800" height="30" cellpadding="1">
				<tr>
					<td height="5" align="center" colspan="3" width="750">
						&nbsp;
					</td>
				</tr>
				<tr>
					<td height="1" width="8" align="center">
						&nbsp;
					</td>
					<td height="1" width="864" align="center">
						<TABLE id="tblMain" cellSpacing="1" cellPadding="1" width="800" border="0" align="center"
							bgcolor="#ffffff">
							<tr>
								<td height="1" width="750" valign="bottom" colspan="2">
									&nbsp;
								</td>
							</tr>
							<TR>
								<TD align="center">
									<uc1:header id="Header1" runat="server"></uc1:header></TD>
							</TR>
							<TR>
								<TD align="center" height="10"></TD>
							</TR>
							<TR>
								<TD height="200" align="center" class="normaltext">You are now logged out of the 
									system. Click <a class="normaltext" href="login.aspx">here</a> to log back in.</TD>
							</TR>
							<TR>
								<TD align="center">
									<DIV align="center">
										<uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
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
			</table>
		</div>
	</body>
</HTML>
