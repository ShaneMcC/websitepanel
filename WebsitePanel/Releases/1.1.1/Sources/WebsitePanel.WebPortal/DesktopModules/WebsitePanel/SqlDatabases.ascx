<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlDatabases.ascx.cs" Inherits="WebsitePanel.Portal.SqlDatabases" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddDatabase"
    CreateControlID="edit_item"
    GroupName="MsSQL2000"
    TypeName="WebsitePanel.Providers.Database.SqlDatabase, WebsitePanel.Providers.Base"
    QuotaName="MsSQL2000.Databases" />