<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeAddDomainName.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeAddDomainName" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="domains" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeDomainNameAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Add Domain"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locDomainName" runat="server" meta:resourcekey="locDomainName" Text="Domain Name:"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtDomainName" runat="server" CssClass="HugeTextBox200"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireDomainName" runat="server" meta:resourcekey="valRequireDomainName" ControlToValidate="txtDomainName"
									ErrorMessage="Enter Domain Name" ValidationGroup="CreateDomain" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
								<asp:RegularExpressionValidator id="valRequireCorrectDomain" runat="server" ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"
									ErrorMessage='Please, enter correct domain name in the form "mydomain.com" or "sub.mydomain.com"' ControlToValidate="txtDomainName"
									Display="Dynamic" meta:resourcekey="valRequireCorrectDomain" ValidationGroup="CreateDomain">*</asp:RegularExpressionValidator>
							</td>
						</tr>
					</table>
					<br />
				    <div class="FormFooterClean">
					    <asp:Button id="btnAdd" runat="server" Text="Add Domain" CssClass="Button1" meta:resourcekey="btnAdd" ValidationGroup="CreateDomain" OnClick="btnAdd_Click" OnClientClick="ShowProgressDialog('Creating Domain...');"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateDomain" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>