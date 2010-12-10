<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CatalogBreadCrumb.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.SkinControls.CatalogBreadCrumb" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<div id="Breadcrumb">
	<div id="Path">
		<asp:HyperLink runat="server" CssClass="Normal" id="lnkCatalogMain" Text="Catalog"/>
		<asp:Repeater runat="server" ID="repCatalogPath">
			<HeaderTemplate>
				<asp:Image runat="server" SkinID="PathSeparatorWhite" />
			</HeaderTemplate>
			<SeparatorTemplate>
				<asp:Image runat="server" SkinID="PathSeparatorWhite" />
			</SeparatorTemplate>
			<ItemTemplate>
				<asp:HyperLink runat="server" CssClass="Normal" 
					NavigateUrl='<%# NavigatePageURL("ecViewCategory", "ResellerId", ecPanelRequest.ResellerId.ToString(), "CategoryId=" + Eval("CategoryId").ToString()) %>' 
					Text='<%# Eval("CategoryName") %>' />
			</ItemTemplate>
		</asp:Repeater>
		<p></p>
	</div>
</div>