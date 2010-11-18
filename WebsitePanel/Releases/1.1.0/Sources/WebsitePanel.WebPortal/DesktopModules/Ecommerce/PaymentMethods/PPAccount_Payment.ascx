<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PPAccount_Payment.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.PaymentMethods.PPAccount_Payment" %>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclOrderShippingInfo" /></div>
</div>
<div class="FormBody">
	<table>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locFirstName" /></td>
			<td width="100%"><asp:Literal runat="server" ID="litFirstName" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locLastName" /></td>
			<td>
				<asp:Literal runat="server" ID="litLastName" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locEmail" /></td>
			<td>
				<asp:Literal runat="server" ID="litEmail" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCompanyName" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrCompany" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locAddress" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litAddress" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locPostalCode" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litPostalCode" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locCity" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litCity" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locCountry" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litCountry" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locCountryState" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litState" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locPhoneNumber" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litPhoneNumber" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locFaxNumber" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="litFaxNumber" /></td>
		</tr>
	</table>
</div>