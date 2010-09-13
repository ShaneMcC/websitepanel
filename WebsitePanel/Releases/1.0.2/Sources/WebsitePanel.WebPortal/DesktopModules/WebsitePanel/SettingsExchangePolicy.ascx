<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsExchangePolicy.ascx.cs" Inherits="WebsitePanel.Portal.SettingsExchangePolicy" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<wsp:CollapsiblePanel id="secMailboxPassword" runat="server"
    TargetControlID="MailboxPasswordPanel" meta:resourcekey="secMailboxPassword" Text="Mailbox Password Policy"/>
<asp:Panel ID="MailboxPasswordPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" nowrap>
                
            </td>
            <td>
                <wsp:PasswordPolicyEditor id="mailboxPasswordPolicy" runat="server" />
            </td>
        </tr>
    </table>
</asp:Panel>