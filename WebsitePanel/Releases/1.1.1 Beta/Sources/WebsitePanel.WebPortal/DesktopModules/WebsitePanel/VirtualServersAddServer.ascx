<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServersAddServer.ascx.cs" Inherits="WebsitePanel.Portal.VirtualServersAddServer" %>
<div class="FormBody">
    <table width="100%">
        <tr>
            <td class="SubHead" style="width:150px;">
                <asp:Label ID="lblServerName" runat="server" meta:resourcekey="lblServerName"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtName" runat="server" CssClass="Huge" Width="300px">New Server</asp:TextBox>
                <asp:RequiredFieldValidator ID="valRequireServerName" runat="server" ControlToValidate="txtName"
                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
        </tr>
        <tr>
            <td class="SubHead">
                <asp:Label ID="lblServerComments" runat="server" meta:resourcekey="lblServerComments"></asp:Label>
            </td>
            <td class="Normal">
                <asp:TextBox ID="txtComments" runat="server" CssClass="NormalTextBox" Width="300px" Rows="2" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button2" Text="Add Server" OnClick="btnAdd_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" OnClick="btnCancel_Click" CausesValidation="false" />
</div>