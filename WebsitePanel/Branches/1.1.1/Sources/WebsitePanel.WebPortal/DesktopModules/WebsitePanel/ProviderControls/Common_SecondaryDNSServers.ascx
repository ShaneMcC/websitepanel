<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Common_SecondaryDNSServers.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.Common_SecondaryDNSServers" %>
<%@ Register Src="../UserControls/SelectIPAddress.ascx" TagName="SelectIPAddress" TagPrefix="uc1" %>
<table>
    <tr>
        <td valign="top">
            <asp:ListBox ID="lbServices" runat="server" Width="200px" Rows="3" CssClass="NormalTextBox"></asp:ListBox></td>
        <td valign="top">
            <asp:ImageButton ID="btnRemove" runat="server" SkinID="DeleteSmall"
				meta:resourcekey="btnRemove" OnClick="btnRemove_Click" /></td>
    </tr>
    <tr>
        <td>
            <asp:DropDownList ID="ddlService" runat="server" CssClass="NormalTextBox" Width="100%">
            </asp:DropDownList></td>
        <td><asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button1" OnClick="btnAdd_Click" /></td>
    </tr>
</table>