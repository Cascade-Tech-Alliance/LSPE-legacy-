<%@ Import Namespace="System.Text.RegularExpressions" %>
<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Page language="c#" Inherits="LSPE.faq" Codebehind="faq.aspx.cs" %>
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
	</HEAD>
	<body>
		<form id="frmFAQs" method="post" runat="server">
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
									<TD align="center" colSpan="1" height="20" rowSpan="1"><P class="subheader" align="center"><%--Frequently 
											Asked Questions--%></P>
									</TD>
								</TR>
								<TR>
									<TD class="normaltext" vAlign="top" align="center" height="200">
										<TABLE cellSpacing="1" cellPadding="1" width="850" border="0">
											<TR>
												<TD><asp:linkbutton id="lnkNew" runat="server" onclick="lnkNew_Click">
														<img border="0" src="images/new.gif" width="16" height="16"></asp:linkbutton></TD>
											</TR>
											<TR>
												<TD><asp:datagrid id="dtgFAQs" runat="server" CssClass="faq" AutoGenerateColumns="False" GridLines="None"
														OnItemCreated="ItemCreated" OnEditCommand="Edit_Click" OnCancelCommand="Cancel_Click" OnUpdateCommand="Update_Click"
														DataKeyField="FAQ_ID" OnItemCommand="dtgFAQs_ItemCommand" OnDeleteCommand="Delete_Click">
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
																<HeaderTemplate>
																	<TABLE class="faq" cellSpacing="1" cellPadding="1" width="820" border="0">
																		<TR>
																			<TD class="faq_question">
																				<asp:TextBox id="txtQuestionNew" runat="server" TextMode="MultiLine" CssClass="input_faq"></asp:TextBox></TD>
																		</TR>
																		<TR>
																			<TD>
																				<asp:TextBox id="txtAnswerNew" runat="server" TextMode="MultiLine" CssClass="input_faq"></asp:TextBox></TD>
																		</TR>
																	</TABLE>
																</HeaderTemplate>
																<ItemTemplate>
																	<TABLE class="faq" cellSpacing="1" cellPadding="1" width="820" border="0">
																		<TR>
																			<TD class="faq_question">
																				<asp:Label id=lblQuestion runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FAQ_Question") %>'>
																				</asp:Label></TD>
																		</TR>
																		<TR>
																			<TD>
																				<asp:Label id=lblAnswer runat="server" text='<%# Regex.Replace((string)DataBinder.Eval(Container.DataItem, "FAQ_Answer"), "\r\n", "<br>") %>'>
																				</asp:Label></TD>
																		</TR>
																	</TABLE>
																</ItemTemplate>
																<EditItemTemplate>
																	<TABLE class="faq" cellSpacing="1" cellPadding="1" width="820" border="0">
																		<TR>
																			<TD class="faq_question">
																				<asp:TextBox id=txtQuestion runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FAQ_Question") %>' TextMode="MultiLine" CssClass="input_faq">
																				</asp:TextBox></TD>
																		</TR>
																		<TR>
																			<TD>
																				<asp:TextBox id=txtAnswer runat="server" text='<%# DataBinder.Eval(Container.DataItem, "FAQ_Answer") %>' TextMode="MultiLine" CssClass="input_faq">
																				</asp:TextBox></TD>
																		</TR>
																	</TABLE>
																</EditItemTemplate>
															</asp:TemplateColumn>
														</Columns>
													</asp:datagrid></TD>
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
						<td align="center" width="1799" colSpan="3" height="5">&nbsp;
						</td>
					</tr>
				</table>
			</div>
		</form>
	</body>
</HTML>
