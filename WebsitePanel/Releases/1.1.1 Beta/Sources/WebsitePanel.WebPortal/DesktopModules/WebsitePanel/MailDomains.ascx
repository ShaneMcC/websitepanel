<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailDomains.ascx.cs" Inherits="WebsitePanel.Portal.MailDomains" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    ShowCreateButton="False"
    ShowQuota="False"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="WebsitePanel.Providers.Mail.MailDomain, WebsitePanel.Providers.Base"
    QuotaName="Mail.Domains" />