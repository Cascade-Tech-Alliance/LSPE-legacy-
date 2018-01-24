<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Page language="c#" Inherits="LSPE._default" Codebehind="default.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta content="Microsoft FrontPage 4.0" name="GENERATOR">
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
							<TR>
								<TD align="center" height="40"><uc1:header id="Header1" runat="server"></uc1:header></TD>
							</TR>
							<TR>
								<TD align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
							</TR>
							<TR>
								<TD class="subheader" align="center" height="20"><%--Home--%></TD>
							</TR>
							<TR>
								<TD align="center" height="100">
									<TABLE class="normaltext id=" cellSpacing="1" width="850" border="0" Table1cellPadding="1">
										<TR>
											<TD align="center"></TD>
											<TD align="center" bgColor="#ffffff">&nbsp;</TD>
											<TD align="center" bgColor="#ffffff">&nbsp;</TD>
										</TR>
										<TR>
											<TD align="center" width="500" vAlign="top">
												<P align="left">
													<!-- Welcome to the Local Service Plan Express, WESD’s interactive program for your district’s 2016-2017 Local Service Plan.   Your plan will be open March 18th through April 30th, allowing you to enter your service choices for the upcoming year.  
													<br><br> -->
													At WESD we are proud of our role in the education community. We take very seriously our responsibility to provide an array of services to our partners that is responsive to their needs, high in quality and affordably priced.
													<br><br>		
													WESD provides approximately 45 services related to Special Education, Technology, School Improvement and Administrative Services that school districts may purchase from our agency.  We are a student-centered organization serving 21 Oregon school districts with a student population of over 82,000 students (K-12).  
													<br><br>
													In a recent survey, 97% of school district respondents were either satisfied or very satisfied with the services they received from our staff.  This is the third year in a row our service rating was 95% or higher.  In addition, 99% of our school districts were either satisfied or very satisfied with our level of customer service.  
													Simply put, our employees do an amazing job of supporting the school districts in our region.  Their skills and commitment produce life changing services for the children and students of our community!  
											</P>
											<IMG src="images/mission_statement.jpg" width="200" align="left">
											</TD>
											<TD vAlign="top" width="10" bgColor="#ffffff"></TD>
											<TD vAlign="top" bgColor="#ffffff" class="normal_text_small">
												<table cellSpacing="0" cellPadding="0" width="100%" border="0">
													<tr>
														<td width="100%" height="15">
															<%--<TABLE id="Table1" cellSpacing="1" cellPadding="2" width="100%" border="0" bgColor="#e2d5c2">
																<TR>
																	<td width="100%" class="normal_text_small" bgColor="white">Click <A class="normaltext" href="LSP Ex Instructions.pdf" target="_blank">
																			here</A> to download instructions for using Local Service Plan EXPRESS.</td>
																</TR>
															</TABLE>--%>
														</td>
													<tr>
														<td width="100%" height="15"></td>
													</tr>
													<TR>
														<TD width="100%" height="37">
															<TABLE id="Table1" cellSpacing="1" cellPadding="2" width="100%" border="0" bgColor="#e2d5c2">
																<TR>
																	<TD width="100%" class="normal_text_small" bgColor="white">Click <A class="normaltext" href="LSPE Adjustment Request Form.pdf" target="_blank">
																			here</A>&nbsp;for an LSP Adjustment Request Form.</TD>
																</TR>
															</TABLE>
														</TD>
													</TR	
													<tr>
														<td width="100%" height="15"></td>
													</tr>
													<TR>
														<TD width="100%" height="37">
															<%--<TABLE id="Table1" cellSpacing="1" cellPadding="2" width="100%" border="0" bgColor="#e2d5c2">
																<TR>
																	<TD width="100%" class="normal_text_small" bgColor="white">Visit our <A class="normaltext" href="faq.aspx">
																			FAQ</A>&nbsp;page to get answers to frequently asked questions.</TD>
																</TR>
															</TABLE>--%>
														</TD>
													</TR>
													<tr>
														<td width="100%" height="15"></td>
													</tr>
												</table>
												<table cellSpacing="1" cellPadding="2" width="100%" border="0" bgColor="#e2d5c2">
													<tr>
														<td width="100%" class="normal_text_small" bgColor="white">Please contact the <A class="normaltext" href="mailto:rxpress@wesd.org">
																webmaster</A> to report any problems with using this application, or call 
															the Help Desk phone number at (503) 385-4849.</td>
													</tr>
												</table>
												<p>&nbsp;</p>
											</TD>
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
					<td align="center" width="935" height="1">&nbsp;
					</td>
				</tr>
				<tr>
					<td align="center" width="850" colSpan="3" height="5">&nbsp;
					</td>
				</tr>
			</table>
		</div>
	</body>
</HTML>
