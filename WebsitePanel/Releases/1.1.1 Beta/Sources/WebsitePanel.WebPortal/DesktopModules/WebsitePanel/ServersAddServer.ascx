<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersAddServer.ascx.cs"
    Inherits="WebsitePanel.Portal.ServersAddServer" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc1" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<asp:Panel ID="ServersAddServerPanel" runat="server" DefaultButton="btnAdd">
    <div class="FormBody">
        <table>
            <tr>
                <td class="SubHead" style="width: 150px;">
                    <asp:Label ID="lblServerName" runat="server" meta:resourcekey="lblServerName"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtName" runat="server" CssClass="Huge" Width="300px">New Server</asp:TextBox>
                    <asp:RequiredFieldValidator ID="valServerName" runat="server" ErrorMessage="*" ControlToValidate="txtName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblServerUrl" runat="server" meta:resourcekey="lblServerUrl"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:TextBox ID="txtUrl" runat="server" CssClass="NormalTextBox" Width="300px">http://127.0.0.1:9003</asp:TextBox>
                    <asp:RequiredFieldValidator ID="valServerUrl" runat="server" ErrorMessage="*" ControlToValidate="txtUrl"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="SubHead" valign="top">
                    <asp:Label ID="lblServerPassword" runat="server" meta:resourcekey="lblServerPassword"></asp:Label>
                </td>
                <td class="Normal">
                    <uc1:PasswordControl id="serverPassword" runat="server">
                    </uc1:PasswordControl>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbAutoDiscovery" Checked="false" meta:resourcekey="cbAutoDiscovery" />
                </td>
            </tr>
        </table>
    </div>
    <div class="FormFooter">
        <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" Text="Add" CssClass="Button2"
            OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Adding server...');" />
        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel"
            CssClass="Button1" OnClick="btnCancel_Click" CausesValidation="False" />
    </div>
</asp:Panel>
