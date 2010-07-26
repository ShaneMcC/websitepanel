<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailForwardings.ascx.cs" Inherits="WebsitePanel.Portal.MailForwardings" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="WebsitePanel.Providers.Mail.MailAlias, WebsitePanel.Providers.Base"
    QuotaName="Mail.Forwardings" />