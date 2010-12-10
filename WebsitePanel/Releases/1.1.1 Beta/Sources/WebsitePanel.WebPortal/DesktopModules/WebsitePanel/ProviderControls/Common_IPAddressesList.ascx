<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Common_IPAddressesList.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.Common_IPAddressesList" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table>
    <tr>
        <td valign="top">
            <asp:ListBox ID="lbAddresses" runat="server" Width="200px" Rows="3" CssClass="NormalTextBox"></asp:ListBox></td>
        <td valign="top" width="100%">
            <asp:ImageButton ID="btnRemove" runat="server" SkinID="DeleteSmall" meta:resourcekey="btnRemove"
				OnClick="btnRemove_Click" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <uc1:SelectIPAddress ID="ipAddress" runat="server" ServerIdParam="ServerID" />
            <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add" CssClass="Button1" OnClick="btnAdd_Click" />
        </td>
    </tr>
</table>
