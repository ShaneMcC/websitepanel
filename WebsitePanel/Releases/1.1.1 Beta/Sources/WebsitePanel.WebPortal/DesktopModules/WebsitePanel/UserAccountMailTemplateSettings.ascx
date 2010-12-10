<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountMailTemplateSettings.ascx.cs" Inherits="WebsitePanel.Portal.UserAccountMailTemplateSettings" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormBody">
    <ul class="LinksList">
        <li>
            <asp:HyperLink ID="lnkAccountLetter" runat="server" meta:resourcekey="lnkAccountLetter"
                Text="Account Summary Letter" NavigateUrl='<%# GetSettingsLink("AccountSummaryLetter", "SettingsAccountSummaryLetter") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkPackageLetter" runat="server" meta:resourcekey="lnkPackageLetter"
                Text="Hosting Space Summary Letter" NavigateUrl='<%# GetSettingsLink("PackageSummaryLetter", "SettingsPackageSummaryLetter") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkPasswordReminder" runat="server" meta:resourcekey="lnkPasswordReminder"
                Text="Password Reminder Letter" NavigateUrl='<%# GetSettingsLink("PasswordReminderLetter", "SettingsPasswordReminderLetter") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkExchangeMailboxSetup" runat="server" meta:resourcekey="lnkExchangeMailboxSetup"
                Text="Exchange Mailbox Setup Letter" NavigateUrl='<%# GetSettingsLink("ExchangeMailboxSetupLetter", "SettingsExchangeMailboxSetupLetter") %>'></asp:HyperLink>
        </li>
         <li>
            <asp:HyperLink ID="HyperLink1" runat="server" meta:resourcekey="lnkOrganizationUserSummaryLetter"
                Text="Exchange Mailbox Setup Letter" NavigateUrl='<%# GetSettingsLink("OrganizationUserSummaryLetter", "SettingsOrganizationUserSummaryLetter") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="HyperLink2" runat="server" meta:resourcekey="lnkHostedSolutionReport"
                Text="Exchange Mailbox Setup Letter" NavigateUrl='<%# GetSettingsLink("HostedSoluitonReportSummaryLetter", "HostedSoluitonReportSummaryLetter") %>'></asp:HyperLink>
        </li>
        <li>
            <asp:HyperLink ID="lnkExchangeHostedEditionOrganizationSummary" runat="server" meta:resourcekey="lnkExchangeHostedEditionOrganizationSummary"
                Text="Exchange Hosting Mode Organization Summary" NavigateUrl='<%# GetSettingsLink("ExchangeHostedEditionOrganizationSummary", "SettingsExchangeHostedEditionOrganizationSummary") %>'></asp:HyperLink>
        </li>           
        <li>
            <asp:HyperLink ID="lnkVpsSummaryLetter" runat="server" meta:resourcekey="lnkVpsSummaryLetter"
                Text="VPS Summary Letter" NavigateUrl='<%# GetSettingsLink("VpsSummaryLetter", "SettingsVpsSummaryLetter") %>'></asp:HyperLink>
        </li> 
    </ul>
</div>
<div class="FormFooter">
    <asp:Button id="btnCancel" CssClass="Button1" runat="server" meta:resourcekey="btnCancel" CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
</div>