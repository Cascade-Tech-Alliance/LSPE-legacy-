<%@ Page language="c#" Inherits="LSPE.reports" Codebehind="reports.aspx.cs" %>
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
		<form id="frmReports" method="post" runat="server">
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
								<TR>
									<TD align="center" height="40"><uc1:header id="Header1" runat="server"></uc1:header></TD>
								</TR>
								<TR>
									<TD align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
								</TR>
								<TR>
									<TD class="subheader" align="center" height="20"><%--Reports--%></TD>
								</TR>
								<TR>
									<TD align="center" height="10">
										<P align="left"><asp:label id="lblMessage" runat="server" Visible="False" CssClass="normaltext"></asp:label></P>
									</TD>
								</TR>
								<TR>
									<TD vAlign="top" align="center" height="50">
									    <P><asp:hyperlink id="hypPlanReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=PlanHistorical"
												Target="_blank">Resolution Plans</asp:hyperlink></P>
										<P><asp:hyperlink id="hypPlanSummaryReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=PlanSummary" 
												Target="_blank">Resolution Plan Summaries</asp:hyperlink></P>
										<P><asp:hyperlink id="hypServicesReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=Services"
												Target="_blank">Resolution Services</asp:hyperlink></P>
										<P><asp:hyperlink id="hypServicesDescsOnlyReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=ServicesDescsOnly"
												Target="_blank">Resolution Service Descriptions</asp:hyperlink></P>
										<P><asp:hyperlink id="hypProgramsReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=Programs"
												Target="_blank">Resolution Programs</asp:hyperlink></P>
										<P><asp:hyperlink id="hypProgramTotals" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=ProgramsListed"
												Target="_blank"> Resolution Program Totals</asp:hyperlink></P>
										<P><asp:hyperlink id="hypProgramTotalsNoTransit" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=ProgramsListedNoTransit"
												Target="_blank"> Resolution Program Totals (Excluding Transit)</asp:hyperlink></P>		
										<P><asp:hyperlink id="hypProgramSummariesReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=ProgramSummaries"
												Target="_blank"> Resolution Programs/Coordinator Report</asp:hyperlink></P>
										<P><asp:hyperlink id="hypRateComparisons" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=ServiceRateComparison"
												Target="_blank">Service Rate Comparison</asp:hyperlink></P>
										<p><asp:HyperLink ID="hypSlotAdjustmentsReport" runat="server" CssClass="normaltext" NavigateUrl="~/report_popup.aspx?report=SlotAdjustmentsByService"
										        Target="_blank">Prior Year Reconciliation By Service Report</asp:HyperLink></p>										
										<FONT face="Times New Roman" color="#0000ff" size="3"></FONT>
									</TD>
								</TR>
								<TR>
									<TD align="center">
										<DIV align="center"><uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
									</TD>
								</TR>
							</TABLE>
						</td>
						<td align="center" height="1">&nbsp;
						</td>
					</tr>
					<tr>
						<td align="center" width="1799" colSpan="3" height="5">
							<div id="divReportPlaceholder" runat="server"></div>
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
