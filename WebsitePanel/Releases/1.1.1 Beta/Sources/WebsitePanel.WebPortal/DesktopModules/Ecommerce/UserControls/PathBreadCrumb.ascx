<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PathBreadCrumb.ascx.cs"
	Inherits="WebsitePanel.Ecommerce.Portal.UserControls.PathBreadCrumb" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<asp:DataList runat="server" HeaderStyle-VerticalAlign="Middle" ItemStyle-VerticalAlign="Middle"
	SelectedItemStyle-VerticalAlign="Middle" ID="dlPathBreadCrumb" CssClass="Normal"
	EnableViewState="false" RepeatDirection="Horizontal" RepeatLayout="Flow">
	<HeaderTemplate>
		<asp:HyperLink runat="server" CssClass="Normal" meta:resourcekey="lnkCatalogRoot"
			NavigateUrl='<%# NavigateURL("UserID", PanelSecurity.SelectedUserId.ToString()) %>' />
		<asp:Image runat="server" SkinID="PathSeparatorWhite" />
	</HeaderTemplate>
	<SeparatorTemplate>
		<asp:Image runat="server" SkinID="PathSeparatorWhite" />
	</SeparatorTemplate>
	<ItemTemplate>
		<asp:HyperLink runat="server"
			NavigateUrl='<%# NavigateURL("CategoryId", Eval("CategoryId").ToString(), "UserID=" + PanelSecurity.SelectedUserId) %>'
			Text='<%# Eval("CategoryName") %>' />
		</ItemTemplate>
	<SelectedItemTemplate>
		<asp:Label runat="server" CssClass="Normal" Text='<%# Eval("CategoryName") %>' />
	</SelectedItemTemplate>
</asp:DataList>