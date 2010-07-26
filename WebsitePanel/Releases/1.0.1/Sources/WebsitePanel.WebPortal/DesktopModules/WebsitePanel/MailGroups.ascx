<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailGroups.ascx.cs" Inherits="WebsitePanel.Portal.MailGroups" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddAccount"
    CreateControlID="edit_item"
    GroupName="Mail"
    TypeName="WebsitePanel.Providers.Mail.MailGroup, WebsitePanel.Providers.Base"
    QuotaName="Mail.Groups" />