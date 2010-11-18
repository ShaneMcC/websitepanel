<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MySQL_EditDatabase.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.MySQL_EditDatabase" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<wsp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true"
    TargetControlID="MainToolsPanel" meta:resourcekey="secMainTools" Text="Maintenance Tools">
</wsp:CollapsiblePanel>
<asp:Panel ID="MainToolsPanel" runat="server" Height="0" style="overflow:hidden;">
    <table ellpadding="10">
        <tr>
            <td>
                <asp:Button ID="btnBackup" runat="server" meta:resourcekey="btnBackup" CausesValidation="false" 
                    Text="Backup" CssClass="Button1" OnClick="btnBackup_Click" />&nbsp;&nbsp;
            </td>
        </tr>
    </table>
</asp:Panel>