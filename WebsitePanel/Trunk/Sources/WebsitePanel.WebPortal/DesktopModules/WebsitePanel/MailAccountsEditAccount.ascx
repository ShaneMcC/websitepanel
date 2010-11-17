<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailAccountsEditAccount.ascx.cs"
	Inherits="WebsitePanel.Portal.MailAccountsEditAccount" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc1" %>
<%@ Register TagPrefix="dnc" TagName="MailEditAddress" Src="MailEditAddress.ascx" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<script type="text/javascript">

	function confirmation() {
		if (!confirm("Are you sure you want to delete this Mail Account?")) return false; else ShowProgressDialog('Deleting Mail Account...');
	}
</script>
<div class="FormBody">
	<dnc:MailEditAddress id="mailEditAddress" runat="server">
	</dnc:MailEditAddress>
	<table cellspacing="0" cellpadding="3" width="100%">
		<tr>
			<td class="SubHead" style="width: 150px;" valign="top">
				<asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label>
			</td>
			<td class="normalbold" valign="top">
				<uc1:PasswordControl ID="passwordControl" runat="server" />
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label runat="server" ID="lblMailboxSizeLimit" meta:resourcekey="lblMailboxSizeLimit" />
			</td>
			<td class="SubHead">
				<asp:Label runat="server" ID="lblMaxMailboxSizeLimit" />
				<asp:TextBox runat="server" CssClass="NormalTextBox" ID="txtMailBoxSizeLimit" Width="80px" Text="0" />
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtMailBoxSizeLimit"
					ErrorMessage="*" Display="Dynamic" />
				<asp:CompareValidator runat="server" ID="CompareValidator1" ControlToValidate="txtMailBoxSizeLimit" 
					Type="Integer" Operator="GreaterThanEqual" Display="Dynamic" ValueToCompare="0" meta:resourcekey="CompareValidator1" />
				<asp:RangeValidator runat="server" ID="MaxMailboxSizeLimitValidator" ControlToValidate="txtMailBoxSizeLimit" 
					Type="Integer" MinimumValue="0" MaximumValue="0" Display="Dynamic" meta:resourcekey="MaxMailboxSizeLimitValidator" />
			</td>
		</tr>
	</table>
	<br />
	<asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>
<div class="FormFooter">
	<asp:Button ID="btnSave" runat="server" CssClass="Button1" meta:resourcekey="btnSave"
		Text="Save" OnClick="btnSave_Click" OnClientClick="ShowProgressDialog('Saving Mail Account...');" />
	<asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel"
		CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
	<asp:Button ID="btnDelete" runat="server" CssClass="Button1" meta:resourcekey="btnDelete"
		CausesValidation="false" Text="Delete" OnClientClick="return confirmation();"
		OnClick="btnDelete_Click" />
</div>
