<%@ Page Language="C#" AutoEventWireup="true" Inherits="LSPE.PrintableSlotAdjustments" Codebehind="PrintableSlotAdjustments.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Prior Year Reconciliations</title>
    <LINK href="rxpress_3.css" type="text/css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <table width="92%"><tr>
        <td class="header" align="left"><asp:Label ID="lblTitle" runat="server" Text="Prior Year Reconciliations"></asp:Label><br /><br /></td>
        <td align="right" valign="top">
            <asp:LinkButton ID="btnPrint" runat="Server" Visible="true" OnClientClick="javascript:window.print();return false;" Text="print" />
        </td></tr></table>
        <table width="92%">
        <tr>
            <td width="50%" align="left" class="subheader"><asp:Label ID="lblDistrict" runat="server"></asp:Label>&nbsp;</td>
            <td align="right" class="subheader"><%--<asp:Label ID="lblYear" runat="server"></asp:Label>--%></td>
        </tr>
        <tr><td colspan="2">
        <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" Width="100%" CssClass="datagrid"
        AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound">
            <Columns>
                <asp:TemplateField HeaderText="Service">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("SV_Name") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Final&nbsp;Cost">
                    <ItemTemplate>
                        <asp:Label ID="lblFinalSlotCost" runat="server" Text='<%# Bind("SA_FinalSlotCost","{0:C}") %>' />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="right" />
                    <HeaderStyle HorizontalAlign="right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Adjustment">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("SA_Adjustment", "{0:C}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                    <HeaderStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
            <HeaderStyle BackColor="#E2D5C2" CssClass="subheader" />
        </asp:GridView>
        </td></tr>
        <tr><td colspan="2" align="right" class="normaltext"><asp:Label ID="lblSATotal" runat="server" /></td></tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
        SelectCommand="prSelectSlotAdjustments" SelectCommandType="StoredProcedure"
        >
            <SelectParameters>
                <asp:SessionParameter Name="DS_ID" SessionField="intDS_ID" Type="Int32" DefaultValue="0" />
                <asp:SessionParameter Name="YR_ID" SessionField="intYR_ID" Type="Int32" DefaultValue="0" />
            </SelectParameters>
        </asp:SqlDataSource>
        <%--<asp:SqlDataSource ID="SqlDataSource2" runat="server"
        SelectCommand="SELECT DS_Name FROM District WHERE DS_ID = @DS_ID"
        >
            <SelectParameters>
                <asp:SessionParameter Name="DS_ID" SessionField="intDS_ID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource3" runat="server"></asp:SqlDataSource>--%>
    </div>
    </form>
</body>
</html>
