<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MySQL_EditDatabase.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.MySQL_EditDatabase" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<wsp:CollapsiblePanel id="secDataFiles" runat="server" IsCollapsed="true"
    TargetControlID="FilesPanel" meta:resourcekey="secDataFiles" Text="Database Files">
</wsp:CollapsiblePanel>
<asp:Panel ID="FilesPanel" runat="server" Height="0" style="overflow:hidden;">
    <table id="tblFiles" runat="server" width="100%" cellpadding="3">
        <tr>
            <td style="width: 150px;" class="Medium">
                <asp:Label ID="lblDataFile" runat="server" meta:resourcekey="lblDataFile" Text="Data File"></asp:Label>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table cellspacing="0" cellpadding="3">
	                <tr>
		                <td class="SubHead" nowrap><asp:Label ID="lblDataSize" runat="server" meta:resourcekey="lblSize" Text="Size, KB:"></asp:Label></td>
		                <td class="Normal"><asp:Literal id="litDataSize" Runat="server" Text="0"></asp:Literal></td>
	                </tr>
                </table>
            </td>
        </tr>
    </table> 
</asp:Panel>
<wsp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true"
    TargetControlID="MainToolsPanel" meta:resourcekey="secMainTools" Text="Maintenance Tools">
</wsp:CollapsiblePanel>
<asp:Panel ID="MainToolsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Button ID="btnBackup" runat="server" meta:resourcekey="btnBackup" CausesValidation="false" 
                    Text="Backup" CssClass="Button1" OnClick="btnBackup_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnRestore" runat="server" meta:resourcekey="btnRestore" CausesValidation="false" 
                    Text="Restore" CssClass="Button1" OnClick="btnRestore_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>

<wsp:CollapsiblePanel id="secHousekeepingTools" runat="server" IsCollapsed="true"
    TargetControlID="HousekeepingToolsPanel" meta:resourcekey="secHousekeepingTools" Text="Housekeeping Tools">
</wsp:CollapsiblePanel>
<asp:Panel ID="HousekeepingToolsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table cellpadding="10">
        <tr>
            <td>
                <asp:Button ID="btnTruncate" runat="server" meta:resourcekey="btnTruncate" CausesValidation="false" 
                    Text="Truncate Files" CssClass="Button3" OnClick="btnTruncate_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>