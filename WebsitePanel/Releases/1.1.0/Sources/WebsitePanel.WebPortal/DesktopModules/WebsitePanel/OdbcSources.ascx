<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OdbcSources.ascx.cs" Inherits="WebsitePanel.Portal.OdbcSources" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    GroupName="OS"
    TypeName="WebsitePanel.Providers.OS.SystemDSN, WebsitePanel.Providers.Base"
    QuotaName="OS.ODBC" />