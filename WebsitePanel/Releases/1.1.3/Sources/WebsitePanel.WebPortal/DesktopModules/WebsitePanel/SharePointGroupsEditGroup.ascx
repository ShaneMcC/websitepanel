<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointGroupsEditGroup.ascx.cs" Inherits="WebsitePanel.Portal.SharePointGroupsEditGroup" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="FormBody">
    <table cellSpacing="0" cellPadding="5" width="100%">
        <tr>
            <td class="SubHead" noWrap width="200"><asp:Label ID="lblGroupName" runat="server" meta:resourcekey="lblGroupName" Text="Group name:"></asp:Label></td>
            <td class="NormalBold" width="100%">
                <uc2:UsernameControl ID="usernameControl" runat="server" />
            </td>
        </tr>
    </table>
    
    <wsp:CollapsiblePanel id="secUsers" runat="server"
        TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Members">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblUsers" runat="server" cellSpacing="0" cellPadding="3" width="100%">
            <tr>
                <td colspan="2">
	                <asp:checkboxlist id="dlUsers" CellPadding="3" RepeatColumns="2" CssClass="NormalBold" Runat="server"
		                DataValueField="Name" DataTextField="Name"></asp:checkboxlist>
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