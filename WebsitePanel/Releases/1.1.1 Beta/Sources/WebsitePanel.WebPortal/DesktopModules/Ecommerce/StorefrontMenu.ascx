<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StorefrontMenu.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.StorefrontMenu" %>
<div class="MenuHeader">
    <asp:Localize runat="server" meta:resourcekey="lclStorefrontCatalog" />
</div>
<div class="Menu">
	<asp:Menu runat="server" ID="catTopMenu" Orientation="Vertical"
        EnableViewState="false" CssSelectorClass="LeftMenu">
	</asp:Menu>
</div>