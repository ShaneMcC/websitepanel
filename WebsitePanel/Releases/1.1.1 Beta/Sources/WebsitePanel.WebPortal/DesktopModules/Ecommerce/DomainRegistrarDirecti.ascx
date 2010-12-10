<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainRegistrarDirecti.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DomainRegistrarDirecti" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>
<div class="FormBody">
	<fieldset>
		<legend>
			<asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="lblFtuNote" />
		</legend>
		<div runat="server" meta:resourcekey="lblFirsttimeUserNote" />
	</fieldset>
	
	<fieldset>
    <legend>
        <asp:Label runat="server" CssClass="NormalBold" meta:resourcekey="lblRegistrarConfig" />
    </legend>
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
					<asp:Localize runat="server" meta:resourcekey="lclParentId" /></td>
				<td>
					<asp:TextBox runat="server" ID="txtServiceParentId" Width="150px" /></td>
			</tr>
			<tr>
				<td>
					<asp:Localize runat="server" meta:resourcekey="lclLiveMode" /></td>
				<td>
					<asp:CheckBox runat="server" ID="chkLiveMode" /></td>
			</tr>
			<tr>
				<td>
					<asp:Localize runat="server" meta:resourcekey="lclSecureChannel" /></td>
				<td>
					<asp:CheckBox runat="server" ID="chkSecureChannel" /></td>
			</tr>
		</table>
	</fieldset>
</div>
<div class="FormFooter">
	<asp:Button runat="server" ID="btnSaveSettings" meta:resourcekey="btnSaveSettings" 
		CssClass="Button1" OnClick="btnSaveSettings_Click" />&nbsp;
	<asp:Button runat="server" ID="btnDisable" meta:resourcekey="btnDisable" 
		CssClass="Button1" OnClick="btnDisable_Click" />&nbsp;
	<asp:Button runat="server" ID="btnCancel" meta:resourcekey="btnCancel" 
		CssClass="Button1" OnClick="btnCancel_Click" />
</div>