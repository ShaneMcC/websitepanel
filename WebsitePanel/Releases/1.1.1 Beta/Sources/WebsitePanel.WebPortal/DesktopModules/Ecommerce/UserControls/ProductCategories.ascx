<%@ Control Language="C#" AutoEventWireup="true" Codebehind="ProductCategories.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.ProductCategories" %>
<asp:DataList runat="server" ID="_dlCategories" GridLines="None" 
	CellPadding="2" CellSpacing="1" RepeatDirection="Horizontal" 
	RepeatLayout="Table" RepeatColumns="3" DataKeyField="CategoryID">
	<ItemStyle CssClass="FormRow" Wrap="false" Width="25%" />
	<ItemTemplate>
		<asp:CheckBox ID="chkSelected" runat="server" />&nbsp;<%# Eval("CategoryName") %>
	</ItemTemplate>
</asp:DataList>