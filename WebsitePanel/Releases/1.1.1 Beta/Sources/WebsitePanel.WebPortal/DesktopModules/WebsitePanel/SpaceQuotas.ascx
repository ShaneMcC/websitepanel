<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceQuotas.ascx.cs" Inherits="WebsitePanel.Portal.SpaceQuotas" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>

<div class="FormBody">
<table cellpadding="3">
    <tr ID="pnlDiskspace" runat="server">
        <td class="SubHead" nowrap><asp:Label runat="server" meta:resourcekey="lblDiskspace" Text="Diskspace, MB:"/></td>
        <td class="Normal"><wsp:Quota ID="quotaDiskspace" runat="server" QuotaName="OS.Diskspace" DisplayGauge="True" />&nbsp;&nbsp;(<asp:HyperLink
				ID="lnkViewDiskspaceDetails" runat="server" Target="_blank" meta:resourcekey="GoToReportQuickLink" />)</td>
    </tr>
    <tr ID="pnlBandwidth" runat="server">
        <td class="SubHead" nowrap><asp:Label runat="server" meta:resourcekey="lblBandwidth" Text="Bandwidth, MB:"/></td>
        <td class="Normal"><wsp:Quota ID="quotaBandwidth" runat="server" QuotaName="OS.Bandwidth" 
			DisplayGauge="True" />&nbsp;&nbsp;(<asp:HyperLink ID="lnkViewBandwidthDetails" runat="server"
				Target="_blank" meta:resourcekey="GoToReportQuickLink" />)</td>
    </tr>
    <tr ID="pnlDomains" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblDomains" runat="server" meta:resourcekey="lblDomains" Text="Domains:"></asp:Label></td>
        <td class="Normal"><wsp:Quota ID="quotaDomains" runat="server" QuotaName="OS.Domains" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlSubDomains" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblSubDomains" runat="server" meta:resourcekey="lblSubDomains" Text="Sub-Domains:"></asp:Label></td>
        <td class="Normal"><wsp:Quota ID="quotaSubDomains" runat="server" QuotaName="OS.SubDomains" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlDomainPointers" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblDomainPointers" runat="server" meta:resourcekey="lblDomainPointers" Text="Domain Pointers:"></asp:Label></td>
        <td class="Normal"><wsp:Quota ID="quotaDomainPointers" runat="server" QuotaName="OS.DomainPointers" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlWebSites" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblWebSites" runat="server" meta:resourcekey="lblWebSites" Text="Web Sites:"></asp:Label></td>
        <td class="Normal"><wsp:Quota ID="quotaWebSites" runat="server" QuotaName="Web.Sites" DisplayGauge="True" /></td>
    </tr>
    <tr ID="pnlFtpAccounts" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblFtpAccounts" runat="server" meta:resourcekey="lblFtpAccounts" Text="FTP Accounts:"></asp:Label></td>
        <td class="Normal"><wsp:Quota ID="quotaFtpAccounts" runat="server" QuotaName="FTP.Accounts" DisplayGauge="True"/></td>
    </tr>
    <tr ID="pnlMailAccounts" runat="server">
        <td class="SubHead" nowrap><asp:Label ID="lblMailAccounts" runat="server" meta:resourcekey="lblMailAccounts" Text="Mail Accounts:"></asp:Label></td>
        <td class="Normal"><wsp:Quota ID="quotaMailAccounts" runat="server" QuotaName="Mail.Accounts" DisplayGauge="True" /></td>
    </tr>
</table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnViewQuotas" runat="server" CssClass="Button1" meta:resourcekey="btnViewQuotas" Text="View All Quotas" OnClick="btnViewQuotas_Click" />
</div>

