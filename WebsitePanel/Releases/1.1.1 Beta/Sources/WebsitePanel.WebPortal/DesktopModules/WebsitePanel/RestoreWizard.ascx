<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RestoreWizard.ascx.cs" Inherits="WebsitePanel.Portal.RestoreWizard" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="wsp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <div class="Huge">
        <asp:Literal ID="litRestoreType" runat="server"></asp:Literal>
    </div>
    <br />
    <table cellpadding="3" cellspacing="0">
        <tr>
            <td class="SubHead" style="width:200px">
                <asp:Label ID="lblBackupLocation" runat="server" meta:resourcekey="lblBackupLocation" Text="Backup Location:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="NormalTextBox" resourcekey="ddlLocation" AutoPostBack="True"
                    OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                    <asp:ListItem Value="1">SpaceFolder</asp:ListItem>
                    <asp:ListItem Value="2">ServerFolder</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <asp:Panel ID="SpaceFolderPanel" runat="server">
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td class="SubHead" style="width:200px">
                    <asp:Label ID="lblSpace" runat="server" meta:resourcekey="lblSpace" Text="Space:"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlSpace" runat="server" CssClass="NormalTextBox" AutoPostBack="True"
                        OnSelectedIndexChanged="ddlSpace_SelectedIndexChanged">
                    </asp:DropDownList>&nbsp;
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblSpaceFile" runat="server" meta:resourcekey="lblSpaceFile" Text="File:"></asp:Label>
                </td>
                <td>
                    <wsp:FileLookup id="spaceFile" runat="server" ValidationGroup="Backup" IncludeFiles="true">
                    </wsp:FileLookup>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="ServerFolderPanel" runat="server">
        <table cellpadding="3" cellspacing="0">
            <tr>
                <td class="SubHead" style="width:200px">
                    <asp:Label ID="lblServerPath" runat="server" meta:resourcekey="lblServerPath" Text="Path:"></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtServerPath" runat="server" CssClass="NormalTextBox" Width="400px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="valRequireServerPath" runat="server" Display="Dynamic" ControlToValidate="txtServerPath"
                        ErrorMessage="*" ValidationGroup="Backup" meta:resourcekey="valRequireServerPath"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<div class="FormFooter">
    <asp:Button ID="btnRestore" runat="server" meta:resourcekey="btnRestore" CssClass="Button1" Text="Restore" OnClick="btnRestore_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
</div>