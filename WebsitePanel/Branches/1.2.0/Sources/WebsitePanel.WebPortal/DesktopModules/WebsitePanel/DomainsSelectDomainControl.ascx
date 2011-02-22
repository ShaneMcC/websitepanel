<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsSelectDomainControl.ascx.cs" Inherits="WebsitePanel.Portal.DomainsSelectDomainControl" %>
<asp:DropDownList id="ddlDomains" runat="server" CssClass="NormalTextBox" DataTextField="DomainName" DataValueField="DomainID" style="vertical-align:middle;"></asp:DropDownList>
<asp:RequiredFieldValidator id="valRequireDomain" runat="server" ErrorMessage="Select domain"
	ControlToValidate="ddlDomains" Display="Dynamic" meta:resourcekey="valRequireDomain"></asp:RequiredFieldValidator>
