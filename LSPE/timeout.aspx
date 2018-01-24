<%@ Page language="c#" Inherits="LSPE.timeout" Codebehind="timeout.aspx.cs" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
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
	</HEAD>
	<body>
		<div align="center">
			<table height="30" cellSpacing="1" cellPadding="1" width="800" <%--bgColor="#269bea"--%> border="0">
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
							<tr>
								<td vAlign="bottom" width="750" colSpan="2" height="1">&nbsp;
								</td>
							</tr>
							<TR>
								<TD style="HEIGHT: 17px" align="center"><uc1:header id="Header1" runat="server"></uc1:header></TD>
							</TR>
							<TR>
								<TD height="200" class="normaltext" align="center">Your session has timed 
									out.&nbsp; Click <a href="login.aspx" class="normaltext">here</a> to log back 
									in.</TD>
							</TR>
							<TR>
								<TD align="center">
									<DIV align="center"><uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
								</TD>
							</TR>
						</TABLE>
					</td>
					<td align="center" width="935" height="1">&nbsp;
					</td>
				</tr>
				<tr>
					<td align="center" width="1799" colSpan="3" height="5">&nbsp;
					</td>
				</tr>
			</table>
		</div>
	</body>
</HTML>
