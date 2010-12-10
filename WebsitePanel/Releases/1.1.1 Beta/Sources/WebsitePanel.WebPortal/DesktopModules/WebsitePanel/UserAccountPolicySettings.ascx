<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountPolicySettings.ascx.cs" Inherits="WebsitePanel.Portal.UserAccountPolicySettings" %>
<div class="FormBody">
    <ul class="LinksList">
        <li>
            <asp:HyperLink ID="lnkWebsitePanelPolicy" runat="server" meta:resourcekey="lnkWebsitePanelPolicy"
                    Text="WebsitePanel Policy" NavigateUrl='<%# GetSettingsLink("WebsitePanelPolicy", "SettingsWebsitePanelPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkWebPolicy" runat="server" meta:resourcekey="lnkWebPolicy"
                    Text="WEB Policy" NavigateUrl='<%# GetSettingsLink("WebPolicy", "SettingsWebPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkFtpPolicy" runat="server" meta:resourcekey="lnkFtpPolicy"
                    Text="FTP Policy" NavigateUrl='<%# GetSettingsLink("FtpPolicy", "SettingsFtpPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkMailPolicy" runat="server" meta:resourcekey="lnkMailPolicy"
                    Text="MAIL Policy" NavigateUrl='<%# GetSettingsLink("MailPolicy", "SettingsMailPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkMsSqlPolicy" runat="server" meta:resourcekey="lnkMsSqlPolicy"
                    Text="MS SQL Policy" NavigateUrl='<%# GetSettingsLink("MsSqlPolicy", "SettingsMsSqlPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkMySqlPolicy" runat="server" meta:resourcekey="lnkMySqlPolicy"
                    Text="MySQL Policy" NavigateUrl='<%# GetSettingsLink("MySqlPolicy", "SettingsMySqlPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkSharePointPolicy" runat="server" meta:resourcekey="lnkSharePointPolicy"
                    Text="SharePoint Policy" NavigateUrl='<%# GetSettingsLink("SharePointPolicy", "SettingsSharePointPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkOsPolicy" runat="server" meta:resourcekey="lnkOsPolicy"
                    Text="OperatingSystem Policy" NavigateUrl='<%# GetSettingsLink("OsPolicy", "SettingsOperatingSystemPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkExchangePolicy" runat="server" meta:resourcekey="lnkExchangePolicy"
                    Text="Exchange Server Policy" NavigateUrl='<%# GetSettingsLink("ExchangePolicy", "SettingsExchangePolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkExchangeHostedEditionPolicy" runat="server" meta:resourcekey="lnkExchangeHostedEditionPolicy"
                    Text="Exchange Hosting Mode Policy" NavigateUrl='<%# GetSettingsLink("ExchangeHostedEditionPolicy", "SettingsExchangeHostedEditionPolicy") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkVpsPolicy" runat="server" meta:resourcekey="lnkVpsPolicy"
                    Text="Virtual Private Servers Policy" NavigateUrl='<%# GetSettingsLink("VpsPolicy", "SettingsVpsPolicy") %>'></asp:HyperLink>
        </li>
    </ul>
</div>
<div class="FormFooter">
    <asp:Button id="btnCancel" CssClass="Button1" runat="server" meta:resourcekey="btnCancel" CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
</div>