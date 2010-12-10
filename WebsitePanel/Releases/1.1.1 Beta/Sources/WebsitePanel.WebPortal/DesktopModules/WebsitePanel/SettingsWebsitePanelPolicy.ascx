<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsWebsitePanelPolicy.ascx.cs" Inherits="WebsitePanel.Portal.SettingsWebsitePanelPolicy" %>
<%@ Register Src="UserControls/UsernamePolicyEditor.ascx" TagName="UsernamePolicyEditor" TagPrefix="uc2" %>
<%@ Register Src="UserControls/PasswordPolicyEditor.ascx" TagName="PasswordPolicyEditor" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<wsp:CollapsiblePanel id="secPanelSettings" runat="server"
    TargetControlID="PanelSettingsPanel" meta:resourcekey="secPanelSettings" Text="WebsitePanel Settings">
</wsp:CollapsiblePanel>
<asp:Panel ID="PanelSettingsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblLogoImage" runat="server" meta:resourcekey="lblLogoImage" Text="Logo Image:"></asp:Label>
            </td>
            <td class="Normal" width="400px">
                <asp:TextBox ID="txtLogoImageURL" runat="server" Width="100%" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblDemoMessage" runat="server" meta:resourcekey="lblDemoMessage" Text="Demo Message:"></asp:Label>
            </td>
            <td class="Normal" width="400px">
                <asp:TextBox ID="txtDemoMessage" runat="server" Rows="5" TextMode="MultiLine" Width="100%" CssClass="NormalTextBox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td class="SubHead" width="150" valign="top" nowrap>
                <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Account Password:"></asp:Label>
            </td>
            <td class="Normal">
                <uc1:PasswordPolicyEditor id="passwordPolicy" runat="server">
                </uc1:PasswordPolicyEditor>
            </td>
        </tr>
    </table>
</asp:Panel>