<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginUserAccount.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.LoginUserAccount" %>
<div class="FormBody">
	<table cellpadding="3" cellspacing="0">
		<tr>
			<td class="LoginLabel" align="right" style="width:100px;"><asp:Localize runat="server" meta:resourcekey="lclUserName"/></td>
			<td class="Normal" align="left">
				<asp:TextBox id="txtUsername" runat="server" CssClass="LoginTextBox" Width="300px"></asp:TextBox>
				<asp:RequiredFieldValidator id="usernameValidator" runat="server" CssClass="NormalBold" ErrorMessage="*" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="LoginLabel" align="right" nowrap><asp:Localize runat="server" meta:resourcekey="lclPassword"/></td>
			<td class="Normal" align="left" valign="middle">
				<asp:TextBox id="txtPassword" runat="server" CssClass="LoginTextBox" Width="150px" TextMode="Password"></asp:TextBox>
				<asp:RequiredFieldValidator id="passwordValidator" runat="server" CssClass="NormalBold" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator></td>	
		</tr>
	</table>
</div>