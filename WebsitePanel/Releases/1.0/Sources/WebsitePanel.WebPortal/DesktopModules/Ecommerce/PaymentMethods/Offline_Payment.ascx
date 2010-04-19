<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Offline_Payment.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.PaymentMethods.Offline_Payment" %>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclOrderShippingInfo" /></div>
</div>
<div class="FormBody">
	<table>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclFirstName" /></td>
			<td width="100%"><asp:Literal runat="server" ID="ltrFirstName" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclLastName" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrLastName" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclEmailAddress" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrEmail" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCompanyName" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrCompany" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclAddress" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrAddress" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclPostalCode" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrPostalCode" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCity" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrCity" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCountry" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrCountry" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclState" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrState" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclPhoneNumber" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrPhoneNumber" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclFaxNumber" />&nbsp;</td>
			<td>
				<asp:Literal runat="server" ID="ltrFaxNumber" /></td>
		</tr>
	</table>
</div>