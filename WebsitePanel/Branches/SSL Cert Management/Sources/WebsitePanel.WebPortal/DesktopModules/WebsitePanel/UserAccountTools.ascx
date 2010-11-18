<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountTools.ascx.cs" Inherits="WebsitePanel.Portal.UserAccountTools" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<wsp:CollapsiblePanel id="ToolsHeader" runat="server"
    TargetControlID="ToolsPanel" resourcekey="ToolsHeader" Text="Account Tools">
</wsp:CollapsiblePanel>
<asp:Panel ID="ToolsPanel" runat="server" CssClass="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkBackup" runat="server" meta:resourcekey="lnkBackup" Text="Backup"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkRestore" runat="server" meta:resourcekey="lnkRestore" Text="Restore"></asp:HyperLink>
    </div>
</asp:Panel>