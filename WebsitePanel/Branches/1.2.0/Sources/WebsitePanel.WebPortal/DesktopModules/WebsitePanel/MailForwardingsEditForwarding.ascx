<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailForwardingsEditForwarding.ascx.cs"
	Inherits="WebsitePanel.Portal.MailForwardingsEditForwarding" %>
<%@ Register TagPrefix="dnc" TagName="EmailAddress" Src="MailEditAddress.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<script type="text/javascript">

	function confirmation() {
		if (!confirm("Are you sure you want to delete this Mail Alias?")) return false; else ShowProgressDialog('Deleting Mail Alias...');
	}
</script>
<div class="FormBody">
	<dnc:EmailAddress id="emailAddress" runat="server">
	</dnc:EmailAddress>
	<table cellspacing="0" cellpadding="3" width="100%">
		<tr>
			<td class="SubHead" style="width: 150px;">
				<asp:Label ID="lblForwardsToEmail" runat="server" meta:resourcekey="lblForwardsToEmail"
					Text="Forwards to e-mail:"></asp:Label>
			</td>
			<td class="normal">
				<asp:TextBox ID="txtForwardTo" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
				<asp:RequiredFieldValidator ID="valtxtForwardTo" runat="server" ErrorMessage="*" meta:resourcekey="valRequireEmail" 
					ControlToValidate="txtForwardTo" Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
	</table>
	<asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>
<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" meta:resourcekey="btnSave" CssClass="Button1"
		Text="Save" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Saving Mail Alias...');" />
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1"
		CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
	<asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1"
		CausesValidation="false" Text="Delete" OnClientClick="return confirmation();"
		OnClick="btnDelete_Click" />
</div>
