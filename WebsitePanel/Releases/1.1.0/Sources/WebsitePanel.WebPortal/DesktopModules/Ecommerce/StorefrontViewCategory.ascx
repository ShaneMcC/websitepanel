<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StorefrontViewCategory.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.StorefrontViewCategory" %>
<%@ Register TagPrefix="wsp" TagName="BriefHostingPlan" Src="ProductControls/HostingPlan_Brief.ascx" %>
<asp:DataList runat="server" ID="dlCategoryProducts" GridLines="None" CellPadding="0" CellSpacing="0" 
	RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" meta:resourcekey="dlCategoryProducts">
	<ItemStyle VerticalAlign="Top" />
	<SeparatorTemplate>
		<div class="WhiteSpacer">&nbsp;</div>
	</SeparatorTemplate>
	<ItemTemplate>
		<wsp:BriefHostingPlan runat="server" ProductInfo='<%# Container.DataItem %>' />
		<br />
	</ItemTemplate>
</asp:DataList>