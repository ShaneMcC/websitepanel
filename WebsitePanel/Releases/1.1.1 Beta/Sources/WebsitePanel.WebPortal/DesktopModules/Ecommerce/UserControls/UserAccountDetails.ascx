<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAccountDetails.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.UserAccountDetails" %>
<%@ Register Src="~/DesktopModules/WebsitePanel/UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register Src="~/DesktopModules/WebsitePanel/UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>

<table cellspacing="0" cellpadding="2" width="100%">
    <tr>
	    <td class="SubHead" width="200" nowrap>
			<asp:Localize runat="server" meta:resourcekey="lclUsername" />
	    </td>
	    <td width="100%" class="NormalBold">
		    <asp:TextBox id="txtUsername" runat="server" CssClass="HugeTextBox"></asp:TextBox>
		    <asp:RequiredFieldValidator id="usernameValidator" runat="server" ErrorMessage="*" ControlToValidate="txtUsername"
			    Display="Dynamic"></asp:RequiredFieldValidator>
	    </td>
    </tr>
    <tr>
	    <td class="SubHead" width="200" nowrap valign="top">
		    <asp:Localize runat="server" meta:resourcekey="lclPassword" />
	    </td>
	    <td width="100%" class="Normal">
	        <uc2:PasswordControl ID="userPassword" runat="server" />
	    </td>
    </tr>
    <tr>
	    <td class="Normal">&nbsp;</td>
    </tr>
    <tr>
	    <td class="SubHead" width="200" nowrap>
		    <asp:Localize runat="server" meta:resourcekey="lclFirstName" />
	    </td>
	    <td width="100%" class="NormalBold">
		    <asp:TextBox id="txtFirstName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
		    <asp:RequiredFieldValidator id="firstNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
			    ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
	    </td>
    </tr>
    <tr>
	    <td class="SubHead">
		    <asp:Localize runat="server" meta:resourcekey="lclLastName" />
	    </td>
	    <td class="NormalBold">
		    <asp:TextBox id="txtLastName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
            <asp:RequiredFieldValidator id="lastNameValidator" runat="server" ErrorMessage="*" Display="Dynamic"
			    ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
	    </td>
    </tr>
    <tr>
	    <td class="SubHead">
		    <asp:Localize runat="server" meta:resourcekey="lclEmail" />
	    </td>
	    <td class="NormalBold">
            <uc2:EmailControl id="txtEmail" runat="server"/>
	    </td>
    </tr>
    <tr>
	    <td class="SubHead">
		    <asp:Localize runat="server" meta:resourcekey="lclSecondaryEmail" />
	    </td>
	    <td class="NormalBold">
            <uc2:EmailControl id="txtSecondaryEmail" runat="server" RequiredEnabled="false" />
        </td>
    </tr>
    <tr>
	    <td class="SubHead">
		    <asp:Localize runat="server" meta:resourcekey="lclMailFormat" />
	    </td>
	    <td class="NormalBold">
		    <asp:DropDownList ID="ddlMailFormat" runat="server" CssClass="NormalTextBox">
		        <asp:ListItem Value="Text" meta:resourcekey="lclPlainText" />
		        <asp:ListItem Value="HTML" meta:resourcekey="lclHTML" />
		    </asp:DropDownList>
	    </td>
    </tr>
</table>