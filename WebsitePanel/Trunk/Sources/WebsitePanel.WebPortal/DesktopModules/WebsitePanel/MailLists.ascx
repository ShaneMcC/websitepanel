<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailLists.ascx.cs" Inherits="WebsitePanel.Portal.MailLists" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="WebsitePanel.Providers.Mail.MailList, WebsitePanel.Providers.Base"
    QuotaName="Mail.Lists" />