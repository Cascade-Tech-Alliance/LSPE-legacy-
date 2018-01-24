<%@ Control Language="c#" ClassName="TabStrip" Inherits="LSPE.UserControls.TabStrip" Codebehind="TabStrip.ascx.cs" %>
<asp:Repeater runat="server" id="rptTabStrip" OnItemCommand="ItemCommand" OnItemCreated="ItemCreated">
	<HeaderTemplate>
	</HeaderTemplate>
	<ItemTemplate>
		<asp:Button runat="server" id="btnTab" CssClass='<%# CssClass %>' BorderWidth="1px" BorderStyle="solid" BorderColor='<%# SetBorderColor(Container) %>' text='<%# Container.DataItem %>' backcolor='<%# SetBackColor(Container) %>' forecolor='<%# SetForeColor(Container) %>' />
	</ItemTemplate>
	<FooterTemplate>
	</FooterTemplate>
</asp:Repeater>
