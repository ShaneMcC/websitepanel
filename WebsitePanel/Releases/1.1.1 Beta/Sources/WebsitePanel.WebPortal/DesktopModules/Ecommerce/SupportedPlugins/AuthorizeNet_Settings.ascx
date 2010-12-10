<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AuthorizeNet_Settings.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.SupportedPlugins.AuthorizeNet_Settings" %>
<table>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclServiceLogin" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtAccount" CssClass="NormalTextBox" Width="150px" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtAccount" Display="Dynamic" ErrorMessage="*" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclTransKey" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtTransKey" CssClass="NormalTextBox" Width="150px" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtTransKey" Display="Dynamic" ErrorMessage="*" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclMD5HashKey" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtMd5Hash" CssClass="NormalTextBox" Width="150px" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtMd5Hash" Display="Dynamic" ErrorMessage="*" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclDemoAccount" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkDemoAccount" /></td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclMerchantEmail" /></td>
		<td>
			<asp:TextBox runat="server" CssClass="NormalTextBox" ID="txtMerchantEmail" Width="150px" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtMerchantEmail" Display="Dynamic" ErrorMessage="*" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclSendConfirmation" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkSendConfirmation" /></td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lclLiveMode" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkLiveModeEnabled" /></td>
	</tr>
</table>