<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlEditUser.ascx.cs" Inherits="WebsitePanel.Portal.SqlEditUser" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc3" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>


<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this User?")) return false; else ShowProgressDialog('Deleting User...');
}
</script>

<div class="FormBody">
    <table cellSpacing="0" cellPadding="3" width="100%">
        <tr>
            <td class="SubHead" style="width: 150px;"><asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label></td>
            <td class="NormalBold">
                <uc2:UsernameControl ID="usernameControl" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="SubHead" valign="top"><asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label></td>
            <td valign="top">
                <uc3:PasswordControl ID="passwordControl" runat="server" />
	        </td>
        </tr>
    </table>
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
    <br />
    
    <wsp:CollapsiblePanel id="secUsers" runat="server"
        TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Databases">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
        <table id="tblDatabases" runat="server" cellSpacing="0" cellPadding="3" width="100%">
            <tr>
                <td colspan="2">
	                <asp:CheckBoxList id="dlDatabases" runat="server" CellPadding="3" RepeatColumns="2" CssClass="NormalBold"
                        DataTextField="Name" DataValueField="Name"></asp:CheckBoxList>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>

<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1" Text="Save" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Saving User...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" CausesValidation="false" 
        Text="Delete" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
</div>