<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Statistics.ascx.cs" Inherits="WebsitePanel.Portal.Statistics" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems" TagPrefix="wsp" %>

<wsp:SpaceServiceItems ID="itemsList" runat="server"
    CreateButtonText="btnAddItem"
    CreateControlID="edit_item"
    ViewLinkText="ViewStatistics"
    GroupName="Statistics"
    TypeName="WebsitePanel.Providers.Statistics.StatsSite, WebsitePanel.Providers.Base"
    QuotaName="Stats.Sites" />