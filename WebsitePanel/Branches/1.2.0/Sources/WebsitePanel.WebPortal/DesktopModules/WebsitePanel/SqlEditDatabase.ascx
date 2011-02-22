<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SqlEditDatabase.ascx.cs" Inherits="WebsitePanel.Portal.SqlEditDatabase" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>


<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete this Database?")) return false; else ShowProgressDialog('Deleting Database...');
}
</script>

<div class="FormBody">
	<table cellspacing="0" cellpadding="3" width="100%">
		<tr>
			<td class="SubHead" style="width: 150px;"><asp:Label ID="lblDatabaseName" runat="server" meta:resourcekey="lblDatabaseName" Text="Database name:"></asp:Label></td>
			<td class="NormalBold">
                <uc2:UsernameControl ID="usernameControl" runat="server" />
			</td>
		</tr>
	</table>
	<br />
	
    <wsp:CollapsiblePanel id="secUsers" runat="server"
        TargetControlID="UsersPanel" meta:resourcekey="secUsers" Text="Database Users">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="UsersPanel" runat="server" Height="0" style="overflow:hidden;">
	    <table cellspacing="0" cellpadding="3" width="100%">
		    <tr>
			    <td colspan="2">
				    <asp:CheckBoxList ID="dlUsers" runat="server" CssClass="NormalBold" DataTextField="Name" DataValueField="Name"
					    RepeatColumns="2" CellPadding="3"></asp:CheckBoxList>
			    </td>
		    </tr>
	    </table>
	</asp:Panel>
	
    <asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>

</div>
<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" Text="Save" CssClass="Button1" OnClick="btnSave_Click" OnClientClick = "ShowProgressDialog('Saving Database...');"/>
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CausesValidation="false" 
        Text="Cancel" CssClass="Button1" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CausesValidation="false" 
        Text="Delete" CssClass="Button1" OnClientClick="return confirmation();" OnClick="btnDelete_Click" />
</div>