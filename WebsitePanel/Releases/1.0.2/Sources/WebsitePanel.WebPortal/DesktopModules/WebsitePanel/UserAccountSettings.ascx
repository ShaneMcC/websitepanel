<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountSettings.ascx.cs" Inherits="WebsitePanel.Portal.UserAccountSettings" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<wsp:CollapsiblePanel id="SettingsHeader" runat="server"
    TargetControlID="SettingsPanel" resourcekey="SettingsHeader" Text="Settings">
</wsp:CollapsiblePanel>
<asp:Panel ID="SettingsPanel" runat="server" CssClass="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkMailTemplates" runat="server" meta:resourcekey="lnkMailTemplates" Text="Mail Templates"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkPolicies" runat="server" meta:resourcekey="lnkPolicies" Text="Policies"></asp:HyperLink>
    </div>
</asp:Panel>