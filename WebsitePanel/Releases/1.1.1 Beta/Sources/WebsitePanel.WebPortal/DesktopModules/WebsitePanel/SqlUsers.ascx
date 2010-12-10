<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlUsers.ascx.cs" Inherits="WebsitePanel.Portal.SqlUsers" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddUser"
    CreateControlID="edit_item"
    GroupName="MsSQL2000"
    TypeName="WebsitePanel.Providers.Database.SqlUser, WebsitePanel.Providers.Base"
    QuotaName="MsSQL2000.Users" />