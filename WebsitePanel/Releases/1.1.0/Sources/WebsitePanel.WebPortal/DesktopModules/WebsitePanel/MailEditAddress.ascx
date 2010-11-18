<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailEditAddress.ascx.cs" Inherits="WebsitePanel.Portal.MailEditAddress" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="uc2" %>
<%@ Register TagPrefix="dnc" TagName="SelectDomain" Src="DomainsSelectDomainControl.ascx" %>
<table cellspacing="0" cellpadding="3" width="100%">
	<tr>
		<td class="SubHead" style="width:150px;">
		    <asp:Label ID="lblEmailAddress" runat="server" meta:resourcekey="lblEmailAddress" Text="E-mail address:"></asp:Label>
		</td>
		<td id="EditEmailPanel" runat="server">
		    <uc2:UsernameControl ID="txtName" runat="server" width="120px" />
		    &nbsp;@&nbsp;
		    <dnc:SelectDomain id="domainsSelectDomainControl" runat="server" HideDomainPointers="false"></dnc:SelectDomain>
		</td>
		<td id="DisplayEmailPanel" runat="server">
		    <asp:Label ID="litName" Runat="server" Visible="False" CssClass="Huge"></asp:Label>
		</td>
	</tr>
</table>
