<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailNotificationEditor.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.EmailNotificationEditor" %>
<table style="width: 100%;">
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclFromEmail" /></td>
		<td>
			<asp:TextBox ID="txtFromEmail" runat="server" Width="500px" CssClass="NormalTextBox" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtFromEmail" Display="Dynamic" ErrorMessage="*" /></td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclCcEmail" /></td>
		<td>
			<asp:TextBox ID="txtCcEmail" runat="server" Width="500px" CssClass="NormalTextBox" /></td>
	</tr>
	<tr>
		<td>
			<asp:Localize runat="server" meta:resourcekey="lclSubject" /></td>
		<td>
			<asp:TextBox ID="txtSubject" runat="server" Width="500px" CssClass="NormalTextBox" />
			<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSubject" Display="Dynamic" ErrorMessage="*" /></td>
	</tr>
	<tr>
		<td valign="top" colspan="2">
			<div class="FormRow"><asp:Localize runat="server" meta:resourcekey="lclHtmlBody" /></div>
			<asp:RequiredFieldValidator runat="server" meta:resourcekey="valHtmlBody" 
			    ControlToValidate="txtHtmlBody" Display="Dynamic" />
			<asp:TextBox ID="txtHtmlBody" runat="server" Rows="20" TextMode="MultiLine" Width="600px" CssClass="NormalTextBox" /></td>
	</tr>
	<tr>
		<td valign="top" colspan="2">
			<div class="FormRow"><asp:Localize runat="server" meta:resourcekey="lclTextBody" /></div>
			<asp:RequiredFieldValidator runat="server" meta:resourcekey="valTextBody" 
			    ControlToValidate="txtTextBody" Display="Dynamic" />
			<asp:TextBox ID="txtTextBody" runat="server" Rows="20" TextMode="MultiLine" Width="600px" CssClass="NormalTextBox" /></td>
	</tr>
</table>