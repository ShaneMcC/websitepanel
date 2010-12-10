<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreditCard_Payment.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.PaymentMethods.CreditCard_Payment" %>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclCreditCardInfo" /></div>
</div>
<div class="FormBody">
	<table cellpadding="4" cellspacing="0" border="0" width="100%">
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locCardNumber" meta:resourcekey="locCardNumber" /></td>
			<td width="100%">
				<asp:TextBox runat="server" ID="txtCreditCard" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCreditCard" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locCardCode" meta:resourcekey="locCardCode" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtVerificationCode" Width="100px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtVerificationCode" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locCardType" meta:resourcekey="locCardType" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlCardTypes" AutoPostBack="true" 
					OnSelectedIndexChanged="ddlCardTypes_SelectedIndexChanged" CssClass="NormalTextBox" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locExpireDate" meta:resourcekey="locExpireDate" /></td>
			<td nowrap>
				<asp:DropDownList runat="server" ID="ddlExpMonth" CssClass="NormalTextBox">
					<asp:ListItem Text="01" Value="01" />
					<asp:ListItem Text="02" Value="02" />
					<asp:ListItem Text="03" Value="03" />
					<asp:ListItem Text="04" Value="04" />
					<asp:ListItem Text="05" Value="05" />
					<asp:ListItem Text="06" Value="06" />
					<asp:ListItem Text="07" Value="07" />
					<asp:ListItem Text="08" Value="08" />
					<asp:ListItem Text="09" Value="09" />
					<asp:ListItem Text="10" Value="10" />
					<asp:ListItem Text="11" Value="11" />
					<asp:ListItem Text="12" Value="12" />
				</asp:DropDownList>&nbsp;/&nbsp;
				<asp:DropDownList runat="server" ID="ddlExpYear" CssClass="NormalTextBox" />
			</td>
		</tr>
	<asp:PlaceHolder runat="server" ID="phCardExt" Visible="false">
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="locIssueNumber" /></td>
			<td nowrap>
				<asp:TextBox runat="server" ID="txtIssueNumber" Width="150px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtIssueNumber" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="locStartDate" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlStartMonth" CssClass="NormalTextBox">
					<asp:ListItem Text="01" Value="01" />
					<asp:ListItem Text="02" Value="02" />
					<asp:ListItem Text="03" Value="03" />
					<asp:ListItem Text="04" Value="04" />
					<asp:ListItem Text="05" Value="05" />
					<asp:ListItem Text="06" Value="06" />
					<asp:ListItem Text="07" Value="07" />
					<asp:ListItem Text="08" Value="08" />
					<asp:ListItem Text="09" Value="09" />
					<asp:ListItem Text="10" Value="10" />
					<asp:ListItem Text="11" Value="11" />
					<asp:ListItem Text="12" Value="12" />
				</asp:DropDownList>&nbsp;/&nbsp;
				<asp:DropDownList runat="server" ID="ddlStartYear" CssClass="NormalTextBox" />
			</td>
		</tr>
	</asp:PlaceHolder>
	</table>
	<br />
	<asp:CheckBox runat="server" ID="chkSaveDetails" TextAlign="Right" meta:resourcekey="chkSaveDetails" />
</div>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclOrderShippingInfo" /></div>
</div>
<div class="FormBody">
	<table>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locFirstName" meta:resourcekey="locFirstName" /></td>
			<td width="100%">
				<asp:TextBox runat="server" ID="txtFirstName" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locLastName" meta:resourcekey="locLastName" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtLastName" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locEmail" meta:resourcekey="locEmail" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtEmail" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locCompany" meta:resourcekey="locCompany" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtCompany" Width="250px" CssClass="NormalTextBox" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locAddress" meta:resourcekey="locAddress" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtAddress" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locCity" meta:resourcekey="locCity" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtCity" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locCountry" meta:resourcekey="locCountry" />&nbsp;</td>
			<td>
				<asp:DropDownList runat="server" ID="ddlCountry" Width="255px" CssClass="NormalTextBox" AutoPostBack="true" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged">
					<asp:ListItem Value="" meta:resourcekey="lclCountriesLabel" />	
				</asp:DropDownList>
				<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCountry" 
					Display="Dynamic" ErrorMessage="*" CssClass="NormalTextBox" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locState" meta:resourcekey="locState" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtCountryState" Width="250px" />
				<asp:DropDownList runat="server" ID="ddlCountryStates" DataTextField="Text" DataValueField="Value" 
					CssClass="NormalTextBox" Width="200px" />
				<asp:RequiredFieldValidator runat="server" ID="reqCountryState" Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locPostalCode" meta:resourcekey="locPostalCode" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtPostalCode" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPostalCode" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locPhoneNumber" meta:resourcekey="locPhoneNumber" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtPhoneNumber" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhoneNumber" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" ID="locFaxNumber" meta:resourcekey="locFaxNumber" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtFaxNumber" Width="250px" CssClass="NormalTextBox" /></td>
		</tr>
	</table>
</div>