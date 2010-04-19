<%@ Control Language="C#" AutoEventWireup="true" Codebehind="PayPalPro_Settings.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.SupportedPlugins.PayPalPro_Settings" %>
<table>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclServiceAccount" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtUsername" CssClass="NormalTextBox" Width="150px" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" Display="Dynamic" ErrorMessage="*" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclServicePassword" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtPassword" CssClass="NormalTextBox" TextMode="Password" Width="150px" />
			<asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtPassword" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclServiceSignature" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtSignature" CssClass="NormalTextBox" Width="150px" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSignature" Display="Dynamic" ErrorMessage="*" />
		</td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclLiveMode" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkLiveMode" /></td>
	</tr>
</table>