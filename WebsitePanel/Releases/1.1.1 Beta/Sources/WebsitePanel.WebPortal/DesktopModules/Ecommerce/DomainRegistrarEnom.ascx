<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainRegistrarEnom.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DomainRegistrarEnom" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<div class="FormBody">
	<table>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclServiceAccount" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtServiceUsername" Width="150px" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclServicePassword" /></td>
			<td>
				<wsp:PasswordTextBox runat="server" ID="txtServicePassword" Width="150px" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclLiveMode" /></td>
			<td>
				<asp:CheckBox runat="server" ID="chkLiveMode" /></td>
		</tr>
	</table>
</div>

<div class="FormFooter">
	<asp:Button runat="server" ID="btnSaveSettings" meta:resourcekey="btnSaveSettings" 
		CssClass="Button1" OnClick="btnSaveSettings_Click" />&nbsp;
	<asp:Button runat="server" ID="btnDisable" meta:resourcekey="btnDisable" 
		CssClass="Button1" OnClick="btnDisable_Click" />&nbsp;
	<asp:Button runat="server" meta:resourcekey="btnCancel" CssClass="Button1" 
		CausesValidation="false" OnClick="btnCancel_Click" />
</div>