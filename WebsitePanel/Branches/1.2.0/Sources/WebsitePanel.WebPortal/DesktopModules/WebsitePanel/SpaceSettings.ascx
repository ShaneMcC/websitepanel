<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettings.ascx.cs" Inherits="WebsitePanel.Portal.SpaceSettings" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<wsp:CollapsiblePanel id="SettingsHeader" runat="server"
    TargetControlID="SettingsPanel" meta:resourcekey="SettingsHeader" Text="Settings">
</wsp:CollapsiblePanel>
<asp:Panel ID="SettingsPanel" runat="server">
    <div class="Normal">
        <div class="ToolLink">
            <asp:HyperLink ID="lnkNameServers" runat="server" meta:resourcekey="lnkNameServers" Text="Name Servers"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkInstantAlias" runat="server" meta:resourcekey="lnkInstantAlias" Text="Instant Alias"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkSharedSSL" runat="server" meta:resourcekey="lnkSharedSSL" Text="Shared SSL Sites"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkPackagesFolder" runat="server" meta:resourcekey="lnkPackagesFolder" Text="Child Spaces Location"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkDnsRecords" runat="server" meta:resourcekey="lnkDnsRecords" Text="Global DNS Records"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkExchangeServer" runat="server" meta:resourcekey="lnkExchangeServer" Text="Exchange Server"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkExchangeHostedEdition" runat="server" meta:resourcekey="lnkExchangeHostedEdition" Text="Exchange Hosting Mode"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkVps" runat="server" meta:resourcekey="lnkVps" Text="Virtual Private Servers"></asp:HyperLink>
        </div>
        <div class="ToolLink">
            <asp:HyperLink ID="lnkVpsForPC" runat="server" meta:resourcekey="lnkVpsForPC" Text="Virtual Private Servers for Private Cloud"></asp:HyperLink>
        </div>
    </div>
</asp:Panel>