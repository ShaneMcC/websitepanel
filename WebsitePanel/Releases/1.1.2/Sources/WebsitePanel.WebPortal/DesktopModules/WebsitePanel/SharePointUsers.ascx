<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointUsers.ascx.cs" Inherits="WebsitePanel.Portal.SharePointUsers" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="WebsitePanel.Providers.OS.SystemUser, WebsitePanel.Providers.Base"
    QuotaName="SharePoint.Users" />