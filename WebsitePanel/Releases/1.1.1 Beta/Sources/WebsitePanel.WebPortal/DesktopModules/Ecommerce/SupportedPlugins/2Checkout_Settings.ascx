<%@ Control Language="C#" AutoEventWireup="true" Codebehind="2Checkout_Settings.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.SupportedPlugins.ToCheckout_Settings" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<table>
	<tr>
		<td>
			<asp:Localize runat="server" ID="lclSecretWord" meta:resourcekey="lclSecretWord" /></td>
		<td>
			<wsp:PasswordTextBox runat="server" ID="txtSecretWord" CssClass="NormalTextBox" 
				Width="150px" /></td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" ID="lcl2COAccount" meta:resourcekey="lcl2COAccount" /></td>
		<td>
			<asp:TextBox runat="server" ID="txt2COAccount" Width="150px" /></td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" meta:resourcekey="lcl2CO_Currency" /></td>
		<td>
			<asp:DropDownList runat="server" ID="ddl2CO_Currency">
				<asp:ListItem Value="AUD">Australian Dollars</asp:ListItem>
				<asp:ListItem Value="GBP">British Pounds Sterling</asp:ListItem>
				<asp:ListItem Value="CAD">Canadian Dollars</asp:ListItem>
				<asp:ListItem Value="DKK">Danish Kroner</asp:ListItem>
				<asp:ListItem Value="EUR">Euros</asp:ListItem>
				<asp:ListItem Value="HKD">Hong Kong Dollars</asp:ListItem>
				<asp:ListItem Value="JPY">Japanese Yen</asp:ListItem>
				<asp:ListItem Value="NZD">New Zealand Dollars</asp:ListItem>
				<asp:ListItem Value="NOK">Norwegian Kroner</asp:ListItem>
				<asp:ListItem Value="SEK">Swedish Kronor</asp:ListItem>
				<asp:ListItem Value="CHF">Swiss Francs</asp:ListItem>
				<asp:ListItem Value="USD" Selected="True">US Dollars</asp:ListItem>
			</asp:DropDownList></td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" ID="lclLiveMode" meta:resourcekey="lclLiveMode" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkLiveMode" />
		</td>
	</tr>
	<tr>
		<td class="SubHead">
			<asp:Localize runat="server" ID="lclFixedCart" meta:resourcekey="lclFixedCart" /></td>
		<td>
			<asp:CheckBox runat="server" ID="chkFixedCart" /></td>
	</tr>
</table>
<p style="text-align: justify;"><asp:Localize runat="server" meta:resourcekey="lclPaymentRoutineNotes" /></p>
<p><asp:Localize runat="server" meta:resourcekey="lclPaymentRoutine" />&nbsp;<asp:Literal runat="server" ID="litPaymentRoutine" /></p>