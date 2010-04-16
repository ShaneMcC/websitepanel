<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeersEditPeer.ascx.cs" Inherits="WebsitePanel.Portal.PeersEditPeer" %>
<%@ Register TagPrefix="dnc" TagName="UserContact" Src="UserControls/ContactDetails.ascx" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
<asp:BulletedList ID="blLog" runat="server" CssClass="ErrorText">
</asp:BulletedList>
<asp:Panel ID="pnlEdit" runat="server" DefaultButton="btnUpdate">
<div class="FormBody">
	<table id="tblAccount" runat="server" cellspacing="0" cellpadding="2" width="100%">
		<tr id="rowUsername" runat="server">
			<td class="SubHead" style="width:200px;">
				<asp:Label ID="lblUserName1" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label>
			</td>
			<td class="NormalBold">
				<asp:TextBox id="txtUsername" runat="server" CssClass="HugeTextBox"></asp:TextBox>
				<asp:RequiredFieldValidator id="usernameValidator" runat="server" ErrorMessage="*" ControlToValidate="txtUsername"
					Display="Dynamic"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr id="rowUsernameReadonly" runat="server">
			<td class="SubHead" style="width:200px;">
				<asp:Label ID="lblUserNameReadonly" runat="server" meta:resourcekey="lblUserName" Text="User name:"></asp:Label>
			</td>
			<td class="Huge">
				<asp:Label ID="lblUsername" Runat="server"></asp:Label>
			</td>
		</tr>
		<tr>
			<td class="SubHead" valign="top">
				<asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label>
			</td>
			<td class="Normal">
			    <uc2:PasswordControl ID="userPassword" runat="server" />
			</td>
		</tr>
		<tr id="rowChangePassword" runat="server">
			<td class="SubHead">
			</td>
			<td class="Normal">
				<asp:Button id="cmdChangePassword" runat="server" meta:resourcekey="cmdChangePassword" CssClass="Button3" Text="Change Password" OnClick="cmdChangePassword_Click" ValidationGroup="NewPassword"></asp:Button>
			</td>
		</tr>
		<tr>
			<td class="Normal">&nbsp;</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First name:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:TextBox id="txtFirstName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
				<asp:RequiredFieldValidator id="firstNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
					ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last name:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:TextBox id="txtLastName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
                <asp:RequiredFieldValidator id="lastNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
					ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Text="E-mail:"></asp:Label>
			</td>
			<td class="Normal">
                <uc2:EmailControl id="txtEmail" runat="server">
                </uc2:EmailControl>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblSecondaryEmail" runat="server" meta:resourcekey="lblSecondaryEmail" Text="Secondary e-mail:"></asp:Label>
			</td>
			<td class="Normal">
                <uc2:EmailControl id="txtSecondaryEmail" runat="server" RequiredEnabled="false">
                </uc2:EmailControl>
            </td>
		</tr>
		<tr>
			<td class="SubHead">
				<asp:Label ID="lblMailFormat" runat="server" meta:resourcekey="lblMailFormat" Text="Mail Format:"></asp:Label>
			</td>
			<td class="Normal">
				<asp:DropDownList ID="ddlMailFormat" runat="server"
				    CssClass="NormalTextBox" resourcekey="ddlMailFormat">
				    <asp:ListItem Value="Text">PlainText</asp:ListItem>
				    <asp:ListItem Value="HTML">HTML</asp:ListItem>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td class="SubHead">
                <asp:Label ID="lblDemoAccount" runat="server" meta:resourcekey="lblDemoAccount" Text="Demo Account:"></asp:Label>  
            </td>
			<td class="Normal">
				<asp:CheckBox id="chkDemo" runat="server" meta:resourcekey="chkDemo" Text="Yes"></asp:CheckBox>
			</td>
		</tr>
	</table>
	
    <wsp:CollapsiblePanel id="headContact" runat="server" IsCollapsed="true"
        TargetControlID="pnlContact" meta:resourcekey="secContact" Text="Contact">
    </wsp:CollapsiblePanel>
	<asp:Panel ID="pnlContact" runat="server" Height="0" style="overflow:hidden;">
	    <dnc:usercontact id="contact" runat="server"></dnc:usercontact>
	</asp:Panel>

</div>
<div class="FormFooter">
	    <asp:Button id="btnUpdate" runat="server" CssClass="Button1" Text="Add User" OnClick="btnUpdate_Click"></asp:Button>
		<asp:Button id="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
		<asp:Button id="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" CausesValidation="False" Text="Delete" OnClick="btnDelete_Click" OnClientClick="return confirm('Delete peer account?');"></asp:Button>
</div>
</asp:Panel>