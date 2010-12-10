<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrganizationServicePlan.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeHostedEdition.UpdateOrganizationServicePlan" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div class="FormBody">
    <wsp:SimpleMessageBox ID="messageBox" runat="server" />

    <asp:ValidationSummary ID="validationErrors" runat="server" ValidationGroup="ServicePlan" DisplayMode="List" ShowMessageBox="true" ShowSummary="false" />
    <fieldset>
        <legend><asp:Localize ID="locCurrentServicePlan" runat="server" meta:resourcekey="locCurrentServicePlan" Text="Current Service Plan"></asp:Localize></legend>
        <table>
            <tr>
                <td class="Label"><asp:Localize ID="locCurrentService" runat="server" meta:resourcekey="locService" Text="Service:"></asp:Localize></td>
                <td><asp:Literal ID="currentServiceName" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locCurrentProgramID" runat="server" meta:resourcekey="locProgramID" Text="Program ID:"></asp:Localize></td>
                <td><asp:Literal ID="currentProgramID" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locCurrentOfferID" runat="server" meta:resourcekey="locOfferID" Text="Offer ID:"></asp:Localize></td>
                <td><asp:Literal ID="currentOfferID" runat="server"></asp:Literal></td>
            </tr>
        </table>
    </fieldset>

    <fieldset>
        <legend><asp:Localize ID="locNewServicePlan" runat="server" meta:resourcekey="locNewServicePlan" Text="New Service Plan"></asp:Localize></legend>
        <table>
            <tr>
                <td class="Label"><asp:Localize ID="locNewService" runat="server" meta:resourcekey="locService" Text="Service:"></asp:Localize></td>
                <td>
                    <asp:DropDownList ID="services" runat="server" DataValueField="ServiceId" 
                        DataTextField="FullServiceName" AutoPostBack="True" 
                        onselectedindexchanged="services_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="requireServices" runat="server" meta:resourcekey="requireServices" ControlToValidate="services" ValidationGroup="ServicePlan"
                        Text="*" ErrorMessage="Select new service" Display="Dynamic" SetFocusOnError="true"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locNewProgramID" runat="server" meta:resourcekey="locProgramID" Text="Program ID:"></asp:Localize></td>
                <td><asp:Literal ID="newProgramID" runat="server"></asp:Literal></td>
            </tr>
            <tr>
                <td class="Label"><asp:Localize ID="locNewOfferID" runat="server" meta:resourcekey="locOfferID" Text="Offer ID:"></asp:Localize></td>
                <td><asp:Literal ID="newOfferID" runat="server"></asp:Literal></td>
            </tr>
        </table>
    </fieldset>
</div>
<div class="FormFooter">
    <asp:Button ID="apply" runat="server" meta:resourcekey="apply" Text="Apply" 
        CssClass="Button1" onclick="apply_Click" ValidationGroup="ServicePlan" />
    <asp:Button ID="cancel" runat="server" meta:resourcekey="cancel" Text="Cancel" 
        CssClass="Button1" CausesValidation="false" onclick="cancel_Click" />
</div>