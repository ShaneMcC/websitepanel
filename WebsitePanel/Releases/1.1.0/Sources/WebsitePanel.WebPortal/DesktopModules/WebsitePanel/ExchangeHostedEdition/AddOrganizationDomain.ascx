<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddOrganizationDomain.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeHostedEdition.AddOrganizationDomain" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <wsp:SimpleMessageBox ID="messageBox" runat="server" />

    <asp:ValidationSummary ID="validationErrors" runat="server" ValidationGroup="AddDomain" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" />
    <table>
        <tr>
            <td><asp:Localize ID="locDomain" runat="server" meta:resourcekey="locDomain">Domain:</asp:Localize></td>
            <td>
                <asp:TextBox ID="domain" runat="server" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requireDomain" runat="server" meta:resourcekey="requireDomain" ControlToValidate="domain" ValidationGroup="AddDomain"
                    Text="*" ErrorMessage="Specify organization domain" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator id="requireCorrectDomain" runat="server" ValidationExpression="^([a-zA-Z0-9]([a-zA-Z0-9\-]{0,61}[a-zA-Z0-9])?\.){1,10}[a-zA-Z]{2,6}$"
					ErrorMessage="Enter correct domain name" ControlToValidate="domain"
					Display="Dynamic" meta:resourcekey="requireCorrectDomain" ValidationGroup="AddDomain">*</asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="update" runat="server" meta:resourcekey="update" Text="Update" 
        CssClass="Button1" onclick="update_Click" ValidationGroup="AddDomain" />
    <asp:Button ID="cancel" runat="server" meta:resourcekey="cancel" Text="Cancel" 
        CssClass="Button1" CausesValidation="false" onclick="cancel_Click" />
</div>