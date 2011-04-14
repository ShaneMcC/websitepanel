<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsInstantAlias.ascx.cs" Inherits="WebsitePanel.Portal.SpaceSettingsInstantAlias" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<wsp:CollapsiblePanel id="secInstantAlias" runat="server"
    TargetControlID="InstantAliasPanel" meta:resourcekey="secInstantAlias" Text="Instant Alias"/>
<asp:Panel ID="InstantAliasPanel" runat="server" Height="0" style="overflow:hidden;">
    <table>
        <tr>
            <td class="SubHead" width="150" nowrap>
                <asp:Label ID="lblInstantAlias" runat="server" meta:resourcekey="lblInstantAlias" Text="Instant Alias:"></asp:Label>
            </td>
            <td class="NormalBold">
                domain.com.&nbsp;<asp:TextBox ID="txtInstantAlias" runat="server" CssClass="NormalTextBox" Width="200px"></asp:TextBox>
            </td>
        </tr>
    </table>
</asp:Panel>