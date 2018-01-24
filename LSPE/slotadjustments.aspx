<%@ Register TagPrefix="uc1" TagName="menu" Src="UserControls/menu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="header" Src="UserControls/header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="footer" Src="UserControls/footer.ascx" %>
<%@ Page language="c#" Inherits="LSPE.slotadjustments" Codebehind="slotadjustments.aspx.cs" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>Local Service Plan EXPRESS</title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link href="rxpress_5.css" type="text/css" rel="stylesheet">
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
					<asp:Label ID="lblNotAdminNotify" runat="server" ForeColor="Red" Text="Sorry.  You don't have permission to view the content of this page." Visible="False" />
					<form id="form1" runat="server">
						<TABLE id="tblMain" cellSpacing="1" cellPadding="1" width="800" border="0" align="center"
							bgcolor="#ffffff">
							<TR>
								<TD align="center" height="40">
									<uc1:header id="Header1" runat="server"></uc1:header></TD>
							</TR>
							<TR>
								<TD align="center"><uc1:menu id="Menu1" runat="server"></uc1:menu></TD>
							</TR>
							<TR>
								<TD class="subheader" vAlign="middle" align="center" height="20"><%--Prior Year Reconciliations--%></TD>
							</TR>
							<tr><td vAlign="middle" style="height: 35px" class="normaltext">&nbsp;District:&nbsp;
								<asp:dropdownlist id="ddlDistricts" runat="server" CssClass="normaltext"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								<asp:label id="lblYear" runat="server" CssClass="normaltext">Year:</asp:label>&nbsp; 
								&nbsp;
								<asp:dropdownlist id="ddlYears" runat="server" CssClass="normaltext"></asp:dropdownlist>&nbsp;&nbsp;&nbsp;
								<asp:linkbutton id="btnGetSlotAdjustments" runat="server" CssClass="normaltext" CausesValidation="False" onclick="btnGetSlotAdjustments_Click">Get Adjustments</asp:linkbutton>&nbsp;&nbsp;&nbsp;
								<asp:LinkButton ID="btnPrintable" Text="Printable Version" runat="server" CssClass="normaltext" Visible="false"
								OnClientClick="javascript:window.open('PrintableSlotAdjustments.aspx','Page2','width=600, height=600, toolbar=no, location=no, menubar=no, status=no, scrollbars=yes, resizable=yes');return false;"  />
								<asp:label id="lblNoSAMessage" runat="server" CssClass="message" Visible="False">No adjustments can be found in the system for the specified district and year.</asp:label>&nbsp;
							    <br /><br />
							</td></tr>
							
						    <asp:PlaceHolder ID="phSAHeader" runat="Server" Visible="false">
						    <TR>
							<TD vAlign="middle" align="center">
							    <TABLE id="Table1" cellSpacing="1" cellPadding="1" width="850" bgColor="white" border="0">
								    <TR>
									    <TD class="plan" align="center" width="850" colSpan="1" height="15" rowSpan="1">
						            <asp:Label ID="lblSAHeader" runat="server" Font-Bold="true"/>
						                </TD>
						            </TR>
						        </TABLE>
						    </TR>
						    </asp:PlaceHolder>
							    
							<TR>
								<TD height="200" align="center" class="normaltext" valign="top">
		<table><tr><td>
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" CssClass="datagrid" AutoGenerateColumns="False"
        OnRowUpdating="GridView1_RowUpdating" ShowFooter="true" OnRowDeleting="GridView1_RowDeleting" OnRowDeleted="GridView1_RowDeleted"
        OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound">
            <Columns>
                <asp:TemplateField ShowHeader="False" FooterStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" >
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" CausesValidation="True" CommandName="Update"
                            Text="Update"></asp:LinkButton><br />
                        <asp:LinkButton ID="btnCancelUpdate" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="Cancel"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="Edit"></asp:LinkButton>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:LinkButton ID="btnInsert" runat="server" CssClass="normaltext" OnClick="btnInsert_Click"
                         CausesValidation="true" ValidationGroup="vgInsert">Insert</asp:LinkButton><br />
                        <asp:LinkButton ID="btnCancelInsert" runat="server" CausesValidation="False" CommandName="Cancel"
                            CssClass="normaltext">Cancel</asp:LinkButton>
                    </FooterTemplate>
                    <ControlStyle CssClass="normaltext" />
                </asp:TemplateField>
                <asp:TemplateField Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="lblSA_ID" runat="server" Text='<%# Eval("SA_ID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Service&nbsp;">
                    <FooterTemplate>
                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="dlServices" InitialValue="0"
                            CssClass="normaltext" Display="Dynamic" ErrorMessage="Service Selection Required<br>" ValidationGroup="vgInsert"></asp:RequiredFieldValidator>
                        <asp:DropDownList ID="dlServices" runat="server" CssClass="normaltext" ValidationGroup="vgInsert" 
                        DataSourceID="SqlDataSource2" DataTextField="SV_Name" DataValueField="SV_ID" OnDataBound="dlServices_DataBound">
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblService" runat="server" Text='<%# Eval("SV_Name") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                    <FooterStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;Final&nbsp;Cost">
                    <EditItemTemplate>
                        <asp:CompareValidator ID="cvTxtFinalSlotCost"  runat="server" CssClass="normaltext" ErrorMessage="Currency Value Required<br>" 
                        ControlToValidate="txtFinalSlotCost" Display="Dynamic" Type="Currency" Operator="DataTypeCheck"></asp:CompareValidator>
                        $&nbsp;<asp:TextBox ID="txtFinalSlotCost" runat="Server" Width="70px" Text='<%# Eval("SA_FinalSlotCost","{0:F2}") %>' CssClass="normaltext" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFinalSlotCost" runat="server" Text='<%# Eval("SA_FinalSlotCost","{0:C}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:RequiredFieldValidator ID="rfvTxtEmptyFinalSlotCost" runat="server" ErrorMessage="Currency Value Required<br>"
                         Display="dynamic" ValidationGroup="vgInsert" ControlToValidate="txtFinalSlotCostInsert"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvTxtFinalSlotCostInsert" runat="server" ErrorMessage="Currency Value Required<br>" ValidationGroup="vgInsert"
                        Operator="DataTypeCheck" Display="dynamic" Type="Currency" ControlToValidate="txtFinalSlotCostInsert"></asp:CompareValidator>
                        &nbsp;&nbsp;$&nbsp;<asp:TextBox ID="txtFinalSlotCostInsert" runat="server" Width="70px" CssClass="normaltext" ValidationGroup="vgInsert"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="&nbsp;Adjustment">
                    <EditItemTemplate>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" CssClass="normaltext" ErrorMessage="Currency Value Required<br>" ControlToValidate="txtAdjustment" Display="Dynamic" Type="Currency" Operator="DataTypeCheck"></asp:CompareValidator>
                        $&nbsp;<asp:TextBox Width="70px" ID="txtAdjustment" runat="server" Text='<%# Eval("SA_Adjustment","{0:F2}") %>' CssClass="normaltext" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblAdjustment" runat="server" Text='<%# Eval("SA_Adjustment", "{0:C}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                    <FooterStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Currency Value Required<br>"
                         Display="dynamic" ValidationGroup="vgInsert" ControlToValidate="txtAdjustmentInsert"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Currency Value Required<br>" ValidationGroup="vgInsert"
                        Operator="DataTypeCheck" Display="dynamic" Type="Currency" ControlToValidate="txtAdjustmentInsert"></asp:CompareValidator>
                        &nbsp;&nbsp;$&nbsp;<asp:TextBox ID="txtAdjustmentInsert" runat="server" Width="70px" CssClass="normaltext" ValidationGroup="vgInsert"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False">
                    <EditItemTemplate>
                        &nbsp;
                    </EditItemTemplate>
                    <ItemTemplate>
<%--                        <asp:Label ID="lblSA_ID" runat="server" Visible="false" Text='<%# Eval("SA_ID") %>' />--%>
                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="False" CommandName="Delete"
                            CssClass="normaltext" Text="Delete"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#ffffff" CssClass="subheader" />
            <EmptyDataRowStyle VerticalAlign="Middle" />
            <EmptyDataTemplate>
                <table width="600px">
                <tr><td colspan="4" align="left"><asp:Label ID="lblEmptyDataNotify" runat="server" Text="There are currently no adjustment records stored for this district and year.&nbsp;&nbsp;Please select a service, enter a final cost, enter an adjustment value, and click insert to store one.<br><br>" CssClass="normaltext" /></td></tr>
                <tr>
                    <td>Service</td><td align="right">Final Cost</td><td align="right">Adjustment</td><td></td>
                </tr>
                <tr><td align="left">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Service&nbsp;Selection&nbsp;Required<br>" CssClass="normaltext" ControlToValidate="dlEmptyInsert" InitialValue="0" Display="dynamic" ValidationGroup="vgEmptyInsert"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="dlEmptyInsert" runat="server" DataSourceID="SqlDataSource2" DataTextField="SV_Name" DataValueField="SV_ID" OnDataBound="dlEmptyInsert_DataBound" ValidationGroup="vgEmptyInsert" CssClass="normaltext" />
                </td><td align="right">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Currency&nbsp;Value&nbsp;Required<br>" ControlToValidate="txtFinalSlotCostEmpty" Display="dynamic" ValidationGroup="vgEmptyInsert"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Currency&nbsp;Value&nbsp;Required<br>" ControlToValidate="txtFinalSlotCostEmpty" Display="Dynamic" Type="Currency" Operator="DataTypeCheck" ValidationGroup="vgEmptyInsert"></asp:CompareValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp;$&nbsp;<asp:TextBox ID="txtFinalSlotCostEmpty" runat="server" ValidationGroup="vgEmptyInsert" CssClass="normaltext"></asp:TextBox>
                </td><td align="right">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Currency&nbsp;Value&nbsp;Required<br>" ControlToValidate="txtEmptyInsert" Display="dynamic" ValidationGroup="vgEmptyInsert"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="Currency&nbsp;Value&nbsp;Required<br>" ControlToValidate="txtEmptyInsert" Display="Dynamic" Type="Currency" Operator="DataTypeCheck" ValidationGroup="vgEmptyInsert"></asp:CompareValidator>
                    &nbsp;&nbsp;&nbsp;&nbsp;$&nbsp;<asp:TextBox ID="txtEmptyInsert" runat="server" ValidationGroup="vgEmptyInsert" CssClass="normaltext"></asp:TextBox>
                </td><td align="left">
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="btnEmptyInsert" runat="server" ValidationGroup="vgEmptyInsert" OnClick="btnEmptyInsert_Click" CausesValidation="true" CssClass="normaltext" >Insert</asp:LinkButton>
                </td></tr></table>
            </EmptyDataTemplate>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        SelectCommand="prSelectSlotAdjustments" SelectCommandType="StoredProcedure"
        UpdateCommand="prUpdateSlotAdjustment" UpdateCommandType="StoredProcedure"
        DeleteCommand="prDeleteSlotAdjustment" DeleteCommandType="StoredProcedure"
        InsertCommand="prInsertSlotAdjustment" InsertCommandType="StoredProcedure"
        >
            <SelectParameters>
                <asp:SessionParameter Name="DS_ID" SessionField="intDS_ID" Type="Int32" DefaultValue="0" />
                <asp:SessionParameter Name="YR_ID" SessionField="intYR_ID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="SA_ID" Type="int32" />
                <asp:Parameter Name="SA_FinalSlotCost" Type="Decimal" />
                <asp:Parameter Name="SA_Adjustment" Type="Decimal" />
            </UpdateParameters>
            <DeleteParameters>
                <asp:Parameter Name="SA_ID" Type="int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:SessionParameter Name="DS_ID" Type="Int32" SessionField="intDS_ID" />
                <asp:SessionParameter Name="YR_ID" Type="Int32" SessionField="intYR_ID" />
                <asp:Parameter Name="SV_ID" Type="Int32" />
                <asp:Parameter Name="SA_FinalSlotCost" Type="Decimal" />
                <asp:Parameter Name="Adjustment" Type="Decimal" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server"
        SelectCommand="prSelectUnusedSlotServices" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:SessionParameter Name="DS_ID" SessionField="intDS_ID" Type="int32" />
                <asp:SessionParameter Name="YR_ID" SessionField="intYR_ID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        </td></tr><tr>
	    <td align="right" class="normaltext">
	        <asp:Label ID="lblSATotal" runat="server" Visible="false" />
	    </td>
	</tr></table>
								</TD>
							</TR>

							<TR>
								<TD align="center">
									<DIV align="center">
										<uc1:footer id="Footer1" runat="server"></uc1:footer></DIV>
								</TD>
							</TR>
						</TABLE>
					</form>
					</td>
					<td height="1" width="800" align="center">
						&nbsp;
					</td>
				</tr>
				<tr>
					<td height="5" width="800" align="center" colspan="3">
						&nbsp;
					</td>
				</tr>
			</table>
		</div>
	</body>
</HTML>
