<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AddonProducts.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.UserControls.AddonProducts" %>
<asp:DataList runat="server" ID="dlProductAddons" DataKeyField="ProductID" 
	RepeatDirection="Vertical" RepeatColumns="3" RepeatLayout="Table" 
	CellPadding="2" CellSpacing="1" Width="100%">
	<ItemStyle Width="33%" Wrap="false" CssClass="FormRow" />
	<ItemTemplate>
		<asp:CheckBox ID="chkSelected" runat="server" />&nbsp;<%# Eval("ProductName") %>
	</ItemTemplate>
</asp:DataList>