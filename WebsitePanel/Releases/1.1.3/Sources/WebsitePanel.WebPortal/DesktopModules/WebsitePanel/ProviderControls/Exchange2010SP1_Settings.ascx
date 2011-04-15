<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Exchange2010SP1_Settings.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.Exchange2010SP1_Settings" %>

<fieldset>
    <legend><asp:Localize ID="locOrganizationTemplate" runat="server" meta:resourcekey="locOrganizationTemplate">Organization Template</asp:Localize></legend>
    <table>
        <tr>
            <td style="width:150px;"><asp:Localize ID="locProgramID" runat="server" meta:resourcekey="locProgramID">Program ID:</asp:Localize></td>
            <td>
                <asp:TextBox ID="programID" runat="server" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requireProgramID" runat="server" ControlToValidate="programID" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td><asp:Localize ID="locOfferID" runat="server" meta:resourcekey="locOfferID">Offer ID:</asp:Localize></td>
            <td>
                <asp:TextBox ID="offerID" runat="server" Width="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requireOfferID" runat="server" ControlToValidate="offerID" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <%-- 
        <tr>
            <td><asp:Localize ID="locLocation" runat="server" meta:resourcekey="locLocation">Location:</asp:Localize></td>
            <td>
                <asp:TextBox ID="location" runat="server" Width="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="requireLocation" runat="server" ControlToValidate="location" Display="Dynamic" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                <asp:RangeValidator ID="rangeLocation" runat="server" 
                    meta:resourcekey="rangeLocation" ControlToValidate="location" Display="Dynamic"
                    Text="Must be a number between 1000 and 2000" MinimumValue="1000" 
                    MaximumValue="2000" SetFocusOnError="true" Type="Integer"></asp:RangeValidator>
            </td>
        </tr>
        --%>
    </table>
</fieldset>

<%-- 
<fieldset>
    <legend><asp:Localize ID="locCatchAll" runat="server" meta:resourcekey="locCatchAll">Catch-All</asp:Localize></legend>
    <table>
        <tr>
            <td style="width:150px;"><asp:Localize ID="locCatchAllPath" runat="server" meta:resourcekey="locCatchAllPath">Catch-all configuration path:</asp:Localize></td>
            <td><asp:TextBox ID="catchAllPath" runat="server" Width="300"></asp:TextBox></td>
        </tr>
    </table>
</fieldset>
--%>

<fieldset>
    <legend><asp:Localize ID="locDomainTemplates" runat="server" meta:resourcekey="locDomainTemplates">Domain Templates</asp:Localize></legend>
    <table>
        <tr>
            <td style="width:150px;"><asp:Localize ID="locTemporaryDomain" runat="server" meta:resourcekey="locTemporaryDomain">Temporary domain:</asp:Localize></td>
            <td>organization_domain.<asp:TextBox ID="temporaryDomain" runat="server" Width="200"></asp:TextBox></td>
        </tr>
        <tr>
            <td valign="top"><asp:Localize ID="locEcpURL" runat="server" meta:resourcekey="locEcpDomain">Exchange Control Panel URL:</asp:Localize></td>
            <td valign="top">
                <asp:TextBox ID="ecpURL" runat="server" Width="300"></asp:TextBox><br />
                <asp:Localize ID="locEcpURLDescr" runat="server" meta:resourcekey="locEcpURLDescr">You can use [DOMAIN_NAME] variable for organization default domain.</asp:Localize>
            </td>
        </tr>
    </table>
</fieldset>