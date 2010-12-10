<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrganizationCatchAll.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeHostedEdition.UpdateOrganizationCatchAll" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <wsp:SimpleMessageBox ID="messageBox" runat="server" />

    <asp:ValidationSummary ID="validationErrors" runat="server" ValidationGroup="CatchAll" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" />
    <table>
        <tr>
            <td colspan="2">
                <asp:RadioButtonList ID="catchAllMode" runat="server" AutoPostBack="true" 
                    onselectedindexchanged="catchAllMode_SelectedIndexChanged">
                    <asp:ListItem Value="Disabled" meta:resourcekey="catchAllModeDisabled" Text="Disabled"></asp:ListItem>
                    <asp:ListItem Value="Enabled" meta:resourcekey="catchAllModeEnabled" Text="Enabled" Selected="True"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
            </td>
        </tr>
        <tr id="rawCatchAllAddress" runat="server">
            <td><asp:Localize ID="locCatchAllAddress" runat="server" meta:resourcekey="locCatchAllAddress">Catch-all mailbox:</asp:Localize></td>
            <td>
                <asp:TextBox ID="catchAllAddress" runat="server" Width="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requireCatchAllAddress" runat="server" meta:resourcekey="requireCatchAllAddress" ControlToValidate="catchAllAddress" ValidationGroup="CatchAll"
                    Text="*" ErrorMessage="Enter catch-all e-mail address" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="requireCorrectEmail" runat="server"
	                ErrorMessage="Enter valid e-mail address" ControlToValidate="catchAllAddress" Display="Dynamic" ValidationGroup="CatchAll"
	                meta:resourcekey="requireCorrectEmail" ValidationExpression="^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+$" SetFocusOnError="True"></asp:RegularExpressionValidator>
                @
                <asp:DropDownList ID="domains" runat="server" DataValueField="Name" DataTextField="Name"></asp:DropDownList>
                <asp:RequiredFieldValidator ID="requireDomain" runat="server" meta:resourcekey="requireDomain" ControlToValidate="domains" ValidationGroup="CatchAll"
                    Text="*" ErrorMessage="Select domain" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
        </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="update" runat="server" meta:resourcekey="update" Text="Update" 
        CssClass="Button1" onclick="update_Click" ValidationGroup="CatchAll" />
    <asp:Button ID="cancel" runat="server" meta:resourcekey="cancel" Text="Cancel" 
        CssClass="Button1" CausesValidation="false" onclick="cancel_Click" />
</div>