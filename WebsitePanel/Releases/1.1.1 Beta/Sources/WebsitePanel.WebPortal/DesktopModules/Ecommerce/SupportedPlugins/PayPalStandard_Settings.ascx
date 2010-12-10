<%@ Control AutoEventWireup="true" Codebehind="PayPalStandard_Settings.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.SupportedPlugins.PayPalStandard_Settings" Language="C#" %>
<table>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclBusiness" /></td>
		<td>
			<asp:TextBox runat="server" ID="txtBusiness" Width="150px" /></td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclLiveMode" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkLiveMode" /></td>
	</tr>
</table>