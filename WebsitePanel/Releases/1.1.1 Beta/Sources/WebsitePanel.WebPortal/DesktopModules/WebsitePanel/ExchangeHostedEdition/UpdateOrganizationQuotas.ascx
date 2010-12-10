<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrganizationQuotas.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeHostedEdition.UpdateOrganizationQuotas" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <wsp:SimpleMessageBox ID="messageBox" runat="server" />

    <asp:ValidationSummary ID="validationErrors" runat="server" ValidationGroup="OrgQuotas" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" />
    <table>
        <tr>
            <td class="Label"><asp:Localize ID="locMailboxes" runat="server" meta:resourcekey="locMailboxes" Text="Mailboxes:"></asp:Localize></td>
            <td>
                <asp:TextBox ID="mailboxes" runat="server" Width="50"></asp:TextBox>
                <asp:Literal ID="maxMailboxes" runat="server">(max 0)</asp:Literal>
                <asp:RequiredFieldValidator ID="requireMailboxes" runat="server" meta:resourcekey="requireMailboxes" ControlToValidate="mailboxes" ValidationGroup="OrgQuotas"
                    Text="*" ErrorMessage="Enter mailboxes quota" Display="Dynamic" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rangeMailboxes" runat="server" meta:resourcekey="rangeMailboxes" ControlToValidate="mailboxes" ValidationGroup="OrgQuotas"
                    Text="!" ErrorMessage="Specify correct number" Display="Dynamic" SetFocusOnError="true" MinimumValue="0" MaximumValue="1000000" Type="Integer"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td class="Label"><asp:Localize ID="locContacts" runat="server" meta:resourcekey="locContacts" Text="Contacts:"></asp:Localize></td>
            <td>
                <asp:TextBox ID="contacts" runat="server" Width="50"></asp:TextBox>
                <asp:Literal ID="maxContacts" runat="server">(max 0)</asp:Literal>
                <asp:RequiredFieldValidator ID="requireContacts" runat="server" meta:resourcekey="requireContacts" ControlToValidate="contacts" ValidationGroup="OrgQuotas"
                    Text="*" ErrorMessage="Enter contacts quota" Display="Dynamic" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rangeContacts" runat="server" meta:resourcekey="rangeContacts" ControlToValidate="contacts" ValidationGroup="OrgQuotas"
                    Text="!" ErrorMessage="Specify correct number" Display="Dynamic" SetFocusOnError="true" MinimumValue="0" MaximumValue="1000000" Type="Integer"></asp:RangeValidator>
            </td>
        </tr>
        <tr>
            <td class="Label"><asp:Localize ID="locDistributionLists" runat="server" meta:resourcekey="locDistributionLists" Text="Distribution Lists:"></asp:Localize></td>
            <td>
                <asp:TextBox ID="distributionLists" runat="server" Width="50"></asp:TextBox>
                <asp:Literal ID="maxDistributionLists" runat="server">(max 0)</asp:Literal>
                <asp:RequiredFieldValidator ID="requireDistributionLists" runat="server" meta:resourcekey="requireDistributionLists" ControlToValidate="distributionLists" ValidationGroup="OrgQuotas"
                    Text="*" ErrorMessage="Enter distribution lists quota" Display="Dynamic" SetFocusOnError="true" Enabled="false"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rangeDistributionLists" runat="server" meta:resourcekey="rangeDistributionLists" ControlToValidate="distributionLists" ValidationGroup="OrgQuotas"
                    Text="!" ErrorMessage="Specify correct number" Display="Dynamic" SetFocusOnError="true" MinimumValue="0" MaximumValue="1000000" Type="Integer"></asp:RangeValidator>
            </td>
        </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="update" runat="server" meta:resourcekey="update" Text="Update" 
        CssClass="Button1" onclick="update_Click" ValidationGroup="OrgQuotas" />
    <asp:Button ID="cancel" runat="server" meta:resourcekey="cancel" Text="Cancel" 
        CssClass="Button1" CausesValidation="false" onclick="cancel_Click" />
</div>