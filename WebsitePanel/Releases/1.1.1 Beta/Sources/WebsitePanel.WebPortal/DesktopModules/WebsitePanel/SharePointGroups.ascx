<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointGroups.ascx.cs" Inherits="WebsitePanel.Portal.SharePointGroups" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="SharePoint"
    TypeName="WebsitePanel.Providers.OS.SystemGroup, WebsitePanel.Providers.Base"
    QuotaName="SharePoint.Groups" />