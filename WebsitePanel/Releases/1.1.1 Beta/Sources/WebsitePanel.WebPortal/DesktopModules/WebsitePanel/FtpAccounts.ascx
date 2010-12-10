<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FtpAccounts.ascx.cs" Inherits="WebsitePanel.Portal.FtpAccounts" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="FTP"
    TypeName="WebsitePanel.Providers.FTP.FtpAccount, WebsitePanel.Providers.Base"
    QuotaName="FTP.Accounts" />