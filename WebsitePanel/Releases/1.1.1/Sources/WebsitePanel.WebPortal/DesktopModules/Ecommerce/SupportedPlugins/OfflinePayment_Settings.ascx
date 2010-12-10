<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OfflinePayment_Settings.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.SupportedPlugins.OfflinePayment_Settings" %>
<table>
	<tr>
		<td><asp:Localize runat="server" meta:resourcekey="lclTranNumberFormat" /></td>
		<td><asp:TextBox runat="server" ID="txtTranNumberFormat" CssClass="NormalTextBox" Width="150px" /></td>
	</tr>
	<tr>
		<td><asp:Localize runat="server" meta:resourcekey="lclAutoApprove" /></td>
		<td><asp:CheckBox runat="server" ID="chkAutoApprove" /></td>
	</tr>
</table>