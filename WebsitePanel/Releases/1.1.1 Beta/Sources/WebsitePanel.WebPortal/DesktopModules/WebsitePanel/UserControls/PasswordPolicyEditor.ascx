<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PasswordPolicyEditor.ascx.cs" Inherits="WebsitePanel.Portal.PasswordPolicyEditor" %>

<asp:UpdatePanel runat="server" ID="PasswordPolicyPanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate> 

<asp:CheckBox id="chkEnabled" runat="server" meta:resourcekey="chkEnabled"
	Text="Enable Policy" CssClass="NormalBold" AutoPostBack="true" OnCheckedChanged="chkEnabled_CheckedChanged" />
<table id="PolicyTable" runat="server" cellpadding="2">
    <tr>
        <td class="Normal" style="width:150px;"><asp:Label ID="lblMinimumLength" runat="server"
            meta:resourcekey="lblMinimumLength" Text="Minimum length:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtMinimumLength" runat="server" CssClass="NormalTextBox" Width="40px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireMinLength" runat="server" ControlToValidate="txtMinimumLength" meta:resourcekey="valRequireMinLength"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectMinLength" runat="server" ControlToValidate="txtMinimumLength" meta:resourcekey="valCorrectMinLength"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator></td>
    </tr>
    <tr>
        <td class="Normal"><asp:Label ID="lblMaximumLength" runat="server"
            meta:resourcekey="lblMaximumLength" Text="Maximum length:"></asp:Label></td>
        <td class="Normal">
            <asp:TextBox ID="txtMaximumLength" runat="server" CssClass="NormalTextBox" Width="40px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireMaxLength" runat="server" ControlToValidate="txtMaximumLength" meta:resourcekey="valRequireMaxLength"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectMaxLength" runat="server" ControlToValidate="txtMaximumLength" meta:resourcekey="valCorrectMaxLength"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
            </td>
    </tr>
    <tr>
        <td colspan="2" class="NormalBold"><asp:Label ID="lblShouldContain" runat="server"
            meta:resourcekey="lblShouldContain" Text="Password should contain at least:"></asp:Label></td>
    </tr>
    <tr>
        <td class="Normal">
            <asp:Label ID="lblMinimumUppercase" runat="server"
                meta:resourcekey="lblMinimumUppercase" Text="Uppercase letters:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtMinimumUppercase" runat="server" CssClass="NormalTextBox" Width="40px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireUppercase" runat="server" ControlToValidate="txtMinimumUppercase" meta:resourcekey="valRequireUppercase"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectUppercase" runat="server" ControlToValidate="txtMinimumUppercase" meta:resourcekey="valCorrectUppercase"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
            </td>
    </tr>
    <tr>
        <td class="Normal">
            <asp:Label ID="lblMinimumNumbers" runat="server"
                meta:resourcekey="lblMinimumNumbers" Text="Numbers:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtMinimumNumbers" runat="server" CssClass="NormalTextBox" Width="40px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireNumbers" runat="server" ControlToValidate="txtMinimumNumbers" meta:resourcekey="valRequireNumbers"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectNumbers" runat="server" ControlToValidate="txtMinimumNumbers" meta:resourcekey="valCorrectNumbers"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
            </td>
    </tr>
    <tr>
        <td class="Normal">
            <asp:Label ID="lblMinimumSymbols" runat="server"
                meta:resourcekey="lblMinimumSymbols" Text="Non-alphanumeric symbols:"></asp:Label>
        </td>
        <td class="Normal"><asp:TextBox ID="txtMinimumSymbols" runat="server" CssClass="NormalTextBox" Width="40px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="valRequireSymbols" runat="server" ControlToValidate="txtMinimumSymbols" meta:resourcekey="valRequireSymbols"
                ErrorMessage="*" ValidationGroup="SettingsEditor" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="valCorrectSymbols" runat="server" ControlToValidate="txtMinimumSymbols" meta:resourcekey="valCorrectSymbols"
                Display="Dynamic" ErrorMessage="*" ValidationExpression="\d{1,3}" ValidationGroup="SettingsEditor"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr id="rowEqualUsername" runat="server" visible="false">
        <td class="Normal" colspan="2">
            <asp:CheckBox id="chkNotEqualUsername" runat="server" meta:resourcekey="chkNotEqualUsername" Text="Should not be equal to username" />
        </td>
    </tr>
</table>

	</ContentTemplate>
</asp:UpdatePanel>