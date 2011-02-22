<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountChangePassword.ascx.cs"
    Inherits="WebsitePanel.Portal.UserAccountChangePassword" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<asp:Panel ID="PasswordPanel" runat="server" DefaultButton="cmdChangePassword">
    <div class="FormBody">
        <table cellspacing="0" cellpadding="2" width="100%">
            <tr>
                <td class="SubHead" style="width: 150px;">
                    <asp:Label ID="lblUsername2" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
                </td>
                <td class="Huge">
                    <asp:Literal ID="lblUsername" runat="server"></asp:Literal>
                </td>
            </tr>
            <tr>
                <td class="SubHead" valign="top">
                    <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label>
                </td>
                <td>
                    <wsp:PasswordControl ID="userPassword" runat="server" />
                </td>
            </tr>
            <tr id="trChangePasswordWarning" runat="server" visible="false">
                <td>
                </td>
                <td>
                    <asp:Label ID="lblChangePasswordWarning" runat="server" CssClass="ErrorText">Warning: This will end the current session.</asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <div class="FormFooter">
        <asp:Button ID="cmdChangePassword" runat="server" meta:resourcekey="cmdChangePassword"
            CssClass="Button3" Text="Change Password" OnClick="cmdChangePassword_Click" ValidationGroup="NewPassword">
        </asp:Button>
        <asp:Button ID="btnCancel" CssClass="Button1" runat="server" meta:resourcekey="btnCancel"
            CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
    </div>
</asp:Panel>
