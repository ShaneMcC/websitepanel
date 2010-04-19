<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Common_ActiveDirectoryIntegration.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.Common_ActiveDirectoryIntegration" %>
<table>
    <tr>
        <td class="Normal">
            <asp:Label ID="lblUsersOU" runat="server" meta:resourcekey="lblUsersOU" Text="Users OU (Optional):"></asp:Label>
        </td>
        <td><asp:TextBox Runat="server" ID="txtUsersOU" CssClass="NormalTextBox"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="Normal">
            <asp:Label ID="lblGroupsOU" runat="server" meta:resourcekey="lblGroupsOU" Text="Groups OU (Optional):"></asp:Label>
        </td>
        <td><asp:TextBox Runat="server" ID="txtGroupsOU" CssClass="NormalTextBox"></asp:TextBox></td>
    </tr>
</table>