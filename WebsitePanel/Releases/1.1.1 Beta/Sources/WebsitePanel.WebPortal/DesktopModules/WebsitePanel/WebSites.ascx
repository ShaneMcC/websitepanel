<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSites.ascx.cs" Inherits="WebsitePanel.Portal.WebSites" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddWebSite"
    CreateControlID="add_site"
    GroupName="Web"
    TypeName="WebsitePanel.Providers.Web.WebSite, WebsitePanel.Providers.Base"
    QuotaName="Web.Sites" />