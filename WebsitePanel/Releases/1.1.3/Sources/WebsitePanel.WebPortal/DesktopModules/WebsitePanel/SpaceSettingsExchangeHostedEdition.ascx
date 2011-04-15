<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceSettingsExchangeHostedEdition.ascx.cs" Inherits="WebsitePanel.Portal.SpaceSettingsExchangeHostedEdition" %>

<fieldset>
    <legend><asp:Localize ID="locDomainTemplates" runat="server" meta:resourcekey="locDomainTemplates">Domain Templates</asp:Localize></legend>
    <table>
        <tr>
            <td style="width:150px;"><asp:Localize ID="locTemporaryDomain" runat="server" meta:resourcekey="locTemporaryDomain">Temporary domain:</asp:Localize></td>
            <td>organization_domain.<asp:TextBox ID="temporaryDomain" runat="server" Width="200"></asp:TextBox></td>
        </tr>
        <tr>
            <td valign="top"><asp:Localize ID="locEcpURL" runat="server" meta:resourcekey="locEcpDomain">ECP URL template:</asp:Localize></td>
            <td valign="top">
                <asp:TextBox ID="ecpURL" runat="server" Width="300"></asp:TextBox><br />
                <asp:Localize ID="locEcpURLDescr" runat="server" meta:resourcekey="locEcpURLDescr">You can use [DOMAIN_NAME] variable for organization default domain.</asp:Localize>
            </td>
        </tr>
    </table>
</fieldset>