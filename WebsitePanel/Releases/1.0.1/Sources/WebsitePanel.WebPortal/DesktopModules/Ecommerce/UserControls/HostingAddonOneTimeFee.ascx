<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingAddonOneTimeFee.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.HostingAddonOneTimeFee" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<table cellspacing="0" cellpadding="3">
	<tr>
		<td><asp:Localize runat="server" meta:resourcekey="lclSetupFee" /></td>
		<td><asp:TextBox runat="server" Width="80px" CssClass="NormalTextBox Centered" ID="txtSetupFee" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSetupFee" Display="Dynamic"
				Text="*" meta:resourcekey="RequireFieldValidator" />
			<asp:CompareValidator runat="server" meta:resourcekey="CurrencyCompareValidator" Operator="DataTypeCheck"
				Type="Currency" Display="Dynamic" ControlToValidate="txtSetupFee" Text="*" /></td>
	</tr>
	<tr>
		<td><asp:Localize runat="server" meta:resourcekey="lclOneTimeFee" /></td>
		<td><asp:TextBox runat="server" Width="80px" CssClass="NormalTextBox Centered" ID="txtOneTimeFee" />
			<asp:RequiredFieldValidator runat="server" meta:resourcekey="RequireFieldValidator"
				ControlToValidate="txtOneTimeFee" Display="Dynamic" Text="*" />
			<asp:CompareValidator runat="server" meta:resourcekey="CurrencyCompareValidator"
				Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ControlToValidate="txtSetupFee"
				Text="*" /></td>
	</tr>
</table>