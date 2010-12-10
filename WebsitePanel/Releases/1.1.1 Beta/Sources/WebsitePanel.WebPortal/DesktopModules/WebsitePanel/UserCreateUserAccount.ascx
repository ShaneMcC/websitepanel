<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserCreateUserAccount.ascx.cs"
    Inherits="WebsitePanel.Portal.UserCreateUserAccount" %>
<%@ Register TagPrefix="dnc" TagName="UserContact" Src="UserControls/ContactDetails.ascx" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="uc2" %>
<%@ Register Src="UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="uc2" %>
<asp:BulletedList ID="blLog" runat="server" CssClass="ErrorText">
</asp:BulletedList>
<asp:Panel ID="UserCreatUserAccountPanel" runat="server" DefaultButton="btnCreate">
    <div class="FormBody">
        <table cellspacing="0" cellpadding="2">
            <tr>
                <td class="SubHead" style="width: 200px;">
                    <asp:Label ID="lblUsername1" runat="server" meta:resourcekey="lblUsername" Text="Username:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="HugeTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="usernameValidator" runat="server" ErrorMessage="*"
                        ControlToValidate="txtUsername" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="SubHead" valign="top">
                    <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" Text="Password:"></asp:Label>
                </td>
                <td class="Normal">
                    <uc2:PasswordControl ID="userPassword" runat="server" AllowGeneratePassword="true" />
                </td>
            </tr>
            <tr>
                <td class="Normal">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblFirstName" runat="server" meta:resourcekey="lblFirstName" Text="First Name:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="firstNameValidator" runat="server" ErrorMessage="*"
                        Display="Dynamic" ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblLastName" runat="server" meta:resourcekey="lblLastName" Text="Last Name:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <asp:TextBox ID="txtLastName" runat="server" CssClass="NormalTextBox"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="lastNameValidator" runat="server" ErrorMessage="*"
                        Display="Dynamic" ControlToValidate="txtLastName"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblEmail" runat="server" meta:resourcekey="lblEmail" Text="E-mail:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <uc2:EmailControl id="txtEmail" runat="server">
                    </uc2:EmailControl>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblSecondaryEmail" runat="server" meta:resourcekey="lblSecondaryEmail"
                        Text="Secondary e-mail:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <uc2:EmailControl id="txtSecondaryEmail" runat="server" RequiredEnabled="false">
                    </uc2:EmailControl>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblMailFormat" runat="server" meta:resourcekey="lblMailFormat" Text="Mail Format:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <asp:DropDownList ID="ddlMailFormat" runat="server" CssClass="NormalTextBox" resourcekey="ddlMailFormat">
                        <asp:ListItem Value="Text">PlainText</asp:ListItem>
                        <asp:ListItem Value="HTML" Selected="True">HTML</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="normal">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="SubHead" valign="top">
                    <asp:Label ID="lblRole" runat="server" meta:resourcekey="lblRole" Text="Role:"></asp:Label>
                </td>
                <td class="NormalBold" valign="top">
                    <asp:DropDownList ID="role" runat="server" resourcekey="role" AutoPostBack="true"
                        OnSelectedIndexChanged="role_SelectedIndexChanged" CssClass="NormalTextBox">
                        <asp:ListItem Value="User"></asp:ListItem>
                        <asp:ListItem Value="Reseller"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr id="rowStatus" runat="server">
                <td class="SubHead">
                    <asp:Label ID="lblStatus" runat="server" meta:resourcekey="lblStatus" Text="Status:"></asp:Label>
                </td>
                <td class="NormalBold">
                    <asp:DropDownList ID="status" runat="server" resourcekey="ddlStatus" CssClass="NormalTextBox">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="2">Suspended</asp:ListItem>
                        <asp:ListItem Value="3">Cancelled</asp:ListItem>
                        <asp:ListItem Value="4">Pending</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="SubHead">
                    <asp:Label ID="lblDemoAccount" runat="server" meta:resourcekey="lblDemoAccount" Text="Demo Account:"></asp:Label>
                </td>
                <td class="Normal">
                    <asp:CheckBox ID="chkDemo" runat="server" meta:resourcekey="chkDemo" Text="Yes">
                    </asp:CheckBox>
                </td>
            </tr>
            <tr id="rowEcommerceEnbl" runat="server">
                <td class="SubHead">
                    <asp:Localize runat="server" meta:resourcekey="lclEcommerceEnabled" />
                </td>
                <td class="Normal">
                    <asp:CheckBox runat="server" ID="chkEcommerceEnbl" Text="Yes" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="Normal">
                </td>
                <td class="NormalBold">
                    <br />
                    <asp:CheckBox ID="chkAccountLetter" runat="server" meta:resourcekey="chkAccountLetter"
                        Text="Send Account Summary Letter" Checked="False" />
                </td>
            </tr>
            <asp:Panel ID="pnlDisabledSummaryLetterHint" runat="server" Visible="false">
                <tr>
                    <td class="Normal">
                    </td>
                    <td class="Normal">
                        <asp:Label ID="lblDisabledSummaryLetterHint" runat="server" meta:resourcekey="lblDisabledSummaryLetterHint"
                            Text="To enable account summary letter please go to Mail Templates\User Account Summary Letter." />
                    </td>
                </tr>
            </asp:Panel>
        </table>
        <wsp:CollapsiblePanel id="headContact" runat="server" IsCollapsed="true" TargetControlID="pnlContact"
            meta:resourcekey="secContact" Text="Contact">
        </wsp:CollapsiblePanel>
        <asp:Panel ID="pnlContact" runat="server" Height="0" Style="overflow: hidden;">
            <dnc:usercontact id="contact" runat="server">
            </dnc:usercontact>
        </asp:Panel>
    </div>
    <div class="FormFooter">
        <asp:Button ID="btnCreate" CssClass="Button1" runat="server" Text="Create" meta:resourcekey="btnCreate"
            OnClick="btnCreate_Click"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1"
            CausesValidation="False" Text="Cancel" OnClick="btnCancel_Click"></asp:Button>
    </div>
</asp:Panel>
