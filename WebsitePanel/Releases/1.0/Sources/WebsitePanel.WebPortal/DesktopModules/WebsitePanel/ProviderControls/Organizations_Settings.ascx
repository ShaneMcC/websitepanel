<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Organizations_Settings.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.Organizations_Settings" %>
<table width="100%"  cellspacing="0">
    <tr>
        <td class="SubHead" width="200" nowrap>
            <asp:Label runat="server" ID="lblRootOU" meta:resourcekey="lblRootOU" />
        </td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtRootOU" MaxLength="1000" Width="200px" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRootOU" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    
    <tr>
        <td class="SubHead" nowrap="true"><asp:Label runat="server" ID="lblPrimaryDomainController" meta:resourcekey="lblPrimaryDomainController" /></td>
        <td class="Normal">
            <asp:TextBox runat="server" ID="txtPrimaryDomainController" Width="200px"/>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtPrimaryDomainController" ErrorMessage="*" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="SubHead" nowrap="true"><asp:Label runat="server" ID="Label1" meta:resourcekey="lblTemporyDomainName" /></td>
        <td><asp:TextBox  runat="server" ID="txtTemporyDomainName" MaxLength="100" Width="200px" />
        <asp:RequiredFieldValidator  ControlToValidate="txtTemporyDomainName" ErrorMessage="*" />
        </td>
    </tr>
</table>