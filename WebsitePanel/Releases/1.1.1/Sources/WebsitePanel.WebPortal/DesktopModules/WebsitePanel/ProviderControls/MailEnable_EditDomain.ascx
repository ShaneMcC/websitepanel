<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEnable_EditDomain.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.MailEnable_EditDomain" %>
<table width="100%">
    <tr>
        <td class="SubHead" style="width:150px;"><asp:Label ID="lblCatchAll" runat="server" meta:resourcekey="lblCatchAll" Text="Catch-All Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlCatchAllAccount" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblPostmaster" runat="server" meta:resourcekey="lblPostmaster" Text="Postmaster Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlPostmasterAccount" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblAbuse" runat="server" meta:resourcekey="lblAbuse" Text="Abuse Account:"></asp:Label></td>
        <td class="Normal">
            <asp:DropDownList ID="ddlAbuseAccount" runat="server" CssClass="NormalTextBox">
            </asp:DropDownList></td>
    </tr>
</table>