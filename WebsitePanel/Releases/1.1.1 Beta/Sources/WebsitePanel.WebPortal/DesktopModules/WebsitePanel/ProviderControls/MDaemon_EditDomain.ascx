<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MDaemon_EditDomain.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.MDaemon_EditDomain" %>
<table width="100%">
    <tr>
        <td class="SubHead" width="200" nowrap><asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal" width="100%">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblAbuse" runat="server" meta:resourcekey="lblAbuse" Text="Abuse Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAbuseAccount" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList></td>
    </tr>
</table>