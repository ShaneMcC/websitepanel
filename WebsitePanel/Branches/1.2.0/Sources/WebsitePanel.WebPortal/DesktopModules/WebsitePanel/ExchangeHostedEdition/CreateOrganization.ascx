<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateOrganization.ascx.cs"
	Inherits="WebsitePanel.Portal.ExchangeHostedEdition.CreateOrganization" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl"
	TagPrefix="wsp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<asp:Panel runat="server" ID="CreateExchangeOrganizationPanel" DefaultButton="createOrganization">
	<div class="FormBody">
		<wsp:SimpleMessageBox ID="messageBox" runat="server" />
		<asp:ValidationSummary ID="validationErrors" runat="server" ValidationGroup="CreateOrg"
			DisplayMode="List" ShowMessageBox="true" ShowSummary="false" />
		<table cellpadding="2">
			<tr>
				<td style="width: 150px;">
					<asp:Localize ID="locOrganizationName" runat="server" meta:resourcekey="locOrganizationName">Organization name:</asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="organizationName" runat="server" Width="200"></asp:TextBox>
					<asp:RequiredFieldValidator ID="requireOrganizationName" runat="server" meta:resourcekey="requireOrganizationName"
						ControlToValidate="organizationName" ValidationGroup="CreateOrg" Text="*" ErrorMessage="Enter organization name"
						Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="locAdministratorName" runat="server" meta:resourcekey="locAdministratorName">Administrator name:</asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="administratorName" runat="server" Width="200"></asp:TextBox>
					<asp:RequiredFieldValidator ID="requireAdministratorName" runat="server" meta:resourcekey="requireAdministratorName"
						ControlToValidate="administratorName" ValidationGroup="CreateOrg" Text="*" ErrorMessage="Enter administrator name"
						Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
				</td>
			</tr>
			<tr>
				<td>
					<asp:Localize ID="locAdministratorEmail" runat="server" meta:resourcekey="locAdministratorEmail">Administrator e-mail:</asp:Localize>
				</td>
				<td>
					<asp:TextBox ID="administratorEmail" runat="server" Width="100"></asp:TextBox>
					<asp:RequiredFieldValidator ID="requireAdministratorEmail" runat="server" meta:resourcekey="requireAdministratorEmail"
						ControlToValidate="administratorEmail" ValidationGroup="CreateOrg" Text="*" ErrorMessage="Enter administrator e-mail address"
						Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
					@
					<asp:TextBox ID="domain" runat="server" Width="200"></asp:TextBox>
					<asp:RequiredFieldValidator ID="requireDomain" runat="server" meta:resourcekey="requireDomain"
						ControlToValidate="domain" ValidationGroup="CreateOrg" Text="*" ErrorMessage="Specify organization domain"
						Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator ID="requireCorrectDomain" runat="server" ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"
						ErrorMessage="Enter correct domain name" ControlToValidate="domain" Display="Dynamic"
						meta:resourcekey="requireCorrectDomain" ValidationGroup="CreateOrg">*</asp:RegularExpressionValidator>
				</td>
			</tr>
			<tr>
				<td valign="top">
					<asp:Localize ID="locAdministratorPassword" runat="server" meta:resourcekey="locAdministratorPassword">Administrator password:</asp:Localize>
				</td>
				<td valign="top">
					<wsp:PasswordControl id="administratorPassword" runat="server" ValidationGroup="CreateOrg">
					</wsp:PasswordControl>
				</td>
			</tr>
		</table>
	</div>
	<div class="FormFooter">
		<asp:Button ID="createOrganization" runat="server" meta:resourcekey="createOrganization"
			Text="Create" CssClass="Button1" ValidationGroup="CreateOrg" OnClick="createOrganization_Click" />
	</div>
</asp:Panel>
