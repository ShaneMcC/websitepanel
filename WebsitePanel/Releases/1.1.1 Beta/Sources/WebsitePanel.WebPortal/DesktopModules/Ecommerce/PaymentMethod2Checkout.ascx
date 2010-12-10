<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentMethod2Checkout.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.PaymentMethod2Checkout" %>
<%@ Register TagPrefix="wsp" TagName="PluginProperties" Src="SupportedPlugins/2Checkout_Settings.ascx" %>
<div class="FormBody">
	<table>
		<tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclDisplayName" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtDisplayName" CssClass="NormalTextBox" MaxLength="50" 
					Width="150px" Text="2Checkout" />&nbsp;
				<asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtDisplayName" />
			</td>
		</tr>
	</table>
	&nbsp;
	<br />
	<wsp:PluginProperties runat="server" ID="ctlPluginProps" />
</div>
<div class="FormFooter">
	<asp:Button runat="server" ID="btnSaveSettings" meta:resourcekey="btnSaveSettings" CssClass="Button1" 
		OnClick="btnSaveSettings_Click" />&nbsp;
	<asp:Button runat="server" ID="btnDisable" CausesValidation="false" meta:resourcekey="btnDisable" CssClass="Button1" 
		OnClick="btnDisable_Click" />&nbsp;
	<asp:Button runat="server" ID="btnCancel" CausesValidation="false" meta:resourcekey="btnCancel" CssClass="Button1" 
		OnClick="btnCancel_Click" />
</div>