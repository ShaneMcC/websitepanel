<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailAddress.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.UserControls.EmailAddress" %>
<%@ Register Src="DomainSelector.ascx" TagName="DomainSelector" TagPrefix="wsp" %>
<asp:TextBox ID="txtAccount" runat="server" CssClass="TextBox100" MaxLength="64"></asp:TextBox>
<wsp:DomainSelector id="domain" runat="server">
</wsp:DomainSelector>
<asp:RequiredFieldValidator ID="valRequireAccount" runat="server" meta:resourcekey="valRequireAccount" ControlToValidate="txtAccount"
	ErrorMessage="Enter E-mail address" ValidationGroup="CreateMailbox" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="valRequireCorrectEmail" runat="server"
	ErrorMessage="Enter valid e-mail address" ControlToValidate="txtAccount" Display="Dynamic"
	meta:resourcekey="valRequireCorrectEmail" ValidationExpression="^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+$" SetFocusOnError="True"></asp:RegularExpressionValidator>