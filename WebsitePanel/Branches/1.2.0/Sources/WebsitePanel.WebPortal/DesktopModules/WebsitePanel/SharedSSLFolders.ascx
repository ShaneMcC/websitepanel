<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharedSSLFolders.ascx.cs" Inherits="WebsitePanel.Portal.SharedSSLFolders" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="add"
    GroupName="Web"
    TypeName="WebsitePanel.Providers.Web.SharedSSLFolder, WebsitePanel.Providers.Base"
    QuotaName="Web.SharedSSL" />