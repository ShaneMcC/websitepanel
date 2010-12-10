<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointSites.ascx.cs" Inherits="WebsitePanel.Portal.SharePointSites" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="WebsitePanel.Providers.SharePoint.SharePointSite, WebsitePanel.Providers.Base"
    QuotaName="SharePoint.Sites" />