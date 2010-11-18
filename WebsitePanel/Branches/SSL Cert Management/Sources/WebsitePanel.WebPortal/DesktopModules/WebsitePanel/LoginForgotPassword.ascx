<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginForgotPassword.ascx.cs" Inherits="WebsitePanel.Portal.LoginForgotPassword" %>
<div class="FormBody">
	<br/>
	<br/>
	<table cellpadding="3" cellspacing="0" style="width:300px">
		<tr>
			<td class="SubHead" nowrap align="right"><asp:Label ID="lblUsername" runat="server" meta:resourcekey="lblUsername" Text="User name:"></asp:Label></td>
			<td class="Normal">
				<asp:TextBox id="txtUsername" runat="server" CssClass="NormalTextBox" Width="150px"></asp:TextBox>
				<asp:RequiredFieldValidator id="usernameValidator" runat="server" ControlToValidate="txtUsername" ErrorMessage="*"
					CssClass="NormalBold"></asp:RequiredFieldValidator></td>
		</tr>
	</table>
	<br/>
	<br />
</div>
<div class="FormFooter">
	<table cellpadding="3" cellspacing="0" style="width:100%;">
		<tr>
			<td align="left">
				<asp:Button id="btnSend" runat="server" Text="Send" meta:resourcekey="btnSend" CssClass="Button1" OnClick="btnSend_Click" />
			</td>
			<td align="right">
				<asp:LinkButton id="cmdBack" runat="server" meta:resourcekey="cmdBack" CssClass="CommandButton" CausesValidation="False" OnClick="cmdBack_Click">Back to login page</asp:LinkButton>
			</td>
		</tr>
	</table>
</div>