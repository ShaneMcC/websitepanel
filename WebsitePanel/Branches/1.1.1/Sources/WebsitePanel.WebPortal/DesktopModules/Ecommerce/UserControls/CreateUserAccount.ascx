<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateUserAccount.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.CreateUserAccount" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>
<div class="FormBody">
	<table>
		<tr>
			<td colspan="2" class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclUserAccountInformation" /></td>
		</tr>
		<tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclUsername" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtUsername" CssClass="NormalTextBox" Width="200px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtUsername" 
					Display="Dynamic" ErrorMessage="*"/>
			</td>
		</tr>
		<tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclPassword" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtPassword" CssClass="NormalTextBox"
					TextMode="Password" Width="200px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" 
					Display="Dynamic" ErrorMessage="*"/>
			</td>
		</tr>
		<tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclConfirmPassword" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtConfirmPassword" CssClass="NormalTextBox" 
					TextMode="Password" Width="200px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmPassword" 
					Display="Dynamic" ErrorMessage="*"/>
				<asp:CompareValidator runat="server" ControlToValidate="txtConfirmPassword" 
					ControlToCompare="txtPassword" Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td colspan="2" class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclContactInformation" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclFirstName" /></td>
			<td width="100%">
				<asp:TextBox runat="server" ID="txtFirstName" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtFirstName" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclLastName" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtLastName" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtLastName" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclEmail" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtEmail" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" 
					Display="Dynamic" ErrorMessage="*" />
				<asp:RegularExpressionValidator ID="valCorrectEmail" runat="server"
					ErrorMessage="Wrong e-mail" ControlToValidate="txtEmail" Display="Dynamic" meta:resourcekey="valCorrectEmail"
					ValidationExpression="^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$"></asp:RegularExpressionValidator>
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCompany" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtCompany" Width="250px" CssClass="NormalTextBox" /></td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclAddress" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtAddress" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCity" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtCity" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclCountry" />&nbsp;</td>
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
				<asp:Localize runat="server" meta:resourcekey="lclState" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtCountryState" CssClass="NormalTextBox" Width="250px" />
				<asp:DropDownList runat="server" ID="ddlCountryStates" DataTextField="Text" DataValueField="Value" 
					CssClass="NormalTextBox" Width="200px" />
				<asp:RequiredFieldValidator runat="server" ID="reqCountryState" Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclPostalCode" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtPostalCode" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPostalCode" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclPhoneNumber" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtPhoneNumber" Width="250px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPhoneNumber" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td nowrap>
				<asp:Localize runat="server" meta:resourcekey="lclFaxNumber" />&nbsp;</td>
			<td>
				<asp:TextBox runat="server" ID="txtFaxNumber" Width="250px" CssClass="NormalTextBox" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclInstantMsngr" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtInstantMsngr" Width="250px" CssClass="NormalTextBox" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclMailFormat" /></td>
			<td>
				<asp:DropDownList ID="ddlMailFormat" runat="server" CssClass="NormalTextBox">
				    <asp:ListItem Value="HTML" meta:resourcekey="lclHtml"/>
					<asp:ListItem Value="Text" meta:resourcekey="lclPlainText"/>
				</asp:DropDownList></td>
		</tr>
		<tr>
			<td colspan="2">
				<wsp:ManualContextValidator runat="server" ID="ctxFormShieldValidator" meta:resourcekey="ctxFormShieldValidator" EnableClientScript="false" 
					OnEvaluatingContext="ctxFormShieldValidator_EvaluatingContext" Display="Dynamic" EnableViewState="false"  />
				<ajaxToolkit:NoBot id="ctlNoBot" runat="server" OnGeneratechallengeAndResponse="ctlNoBot_GenerateChallengeAndResponse" /></td>
		</tr>
	</table>
</div>