<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccounts.ascx.cs" Inherits="WebsitePanel.Portal.MailAccounts" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="WebsitePanel.Providers.Mail.MailAccount, WebsitePanel.Providers.Base"
    QuotaName="Mail.Accounts" />