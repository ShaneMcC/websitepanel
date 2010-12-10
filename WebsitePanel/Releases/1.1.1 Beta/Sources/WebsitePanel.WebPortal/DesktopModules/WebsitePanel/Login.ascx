<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebsitePanel.Portal.Login" %>
<div class="FormBody">
	<table cellpadding="3" cellspacing="0">
		<tr>
			<td class="LoginLabel" align="right" style="width:100px;"><asp:Label ID="lblUserName" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label></td>
			<td class="Normal" align="left">
				<asp:TextBox id="txtUsername" runat="server" CssClass="LoginTextBox" Width="300px"></asp:TextBox>
				<asp:RequiredFieldValidator id="usernameValidator" runat="server" CssClass="NormalBold" ErrorMessage="*" ControlToValidate="txtUsername"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="LoginLabel" align="right" nowrap><asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label></td>
			<td class="Normal" align="left" valign="middle">
				<asp:TextBox id="txtPassword" runat="server" CssClass="LoginTextBox" Width="150px" TextMode="Password"></asp:TextBox>
				<asp:RequiredFieldValidator id="passwordValidator" runat="server" CssClass="NormalBold" ErrorMessage="*" ControlToValidate="txtPassword"></asp:RequiredFieldValidator>
				&nbsp;<asp:LinkButton id="cmdForgotPassword" runat="server"
					CssClass="CommandButton" CausesValidation="False" OnClick="cmdForgotPassword_Click"
					meta:resourcekey="cmdForgotPassword" Text="Forgot your password">
				</asp:LinkButton>
			</td>	
		</tr>
		<tr>
			<td class="SubHead" nowrap></td>
			<td class="Normal" align="left">
				<asp:CheckBox id="chkRemember" runat="server" meta:resourcekey="chkRemember" Text="Remember me on this computer"></asp:CheckBox></td>
		</tr>
		<tr>
			<td class="SubHead" nowrap></td>
			<td align="left">
				<asp:Button id="btnLogin" runat="server" meta:resourcekey="btnLogin" Text="Login" CssClass="LoginButton" OnClick="btnLogin_Click" />
			</td>
		</tr>
		<tr>
			<td><br /><br /><br /></td>
		</tr>
		<tr>
			<td class="SubHead" align="right"><asp:Label ID="lblLanguage" runat="server" meta:resourcekey="lblLanguage" Text="Preferred Language:"></asp:Label></td>
			<td class="Normal" align="left">
				<asp:DropDownList ID="ddlLanguage" runat="server" CssClass="NormalTextBox" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged"></asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td class="SubHead" align="right"><asp:Label ID="lblTheme" runat="server" meta:resourcekey="lblTheme" Text="Theme:"></asp:Label></td>
			<td class="Normal" align="left">
				<asp:DropDownList ID="ddlTheme" runat="server" CssClass="NormalTextBox" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlTheme_SelectedIndexChanged"></asp:DropDownList>
			</td>
		</tr>
	</table>
	<br />
</div>