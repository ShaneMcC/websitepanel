<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointUsersEditUser.ascx.cs" Inherits="WebsitePanel.Portal.SharePointUsersEditUser" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="FormBody">
    <table cellSpacing="0" cellPadding="5" width="100%">
        <tr>
            <td class="SubHead" noWrap width="200"><asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User Name:"></asp:Label></td>
            <td class="NormalBold" width="100%">
                <uc3:UsernameControl ID="usernameControl" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" valign="top">
                <asp:Label ID="lblUserPassword" runat="server" meta:resourcekey="lblUserPassword" Text="User Password:"></asp:Label></td>
            <td class="Normal" valign="top">
                <uc2:PasswordControl ID="passwordControl" runat="server" />
            </td>
        </tr>
    </table>
    
    <wsp:CollapsiblePanel id="secGroups" runat="server"
        TargetControlID="GroupsPanel" meta:resourcekey="secGroups" Text="Member Of">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="GroupsPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblGroups" runat="server" cellSpacing="0" cellPadding="3" width="100%">
            <tr>
                <td colspan="2">
	                <asp:checkboxlist id="dlGroups" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" DataTextField="Name"
		                DataValueField="Name" Runat="server"></asp:checkboxlist>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" CausesValidation="false" 
        Text="Delete" OnClientClick="return confirm('Delete this user?');" OnClick="btnDelete_Click" />
</div>