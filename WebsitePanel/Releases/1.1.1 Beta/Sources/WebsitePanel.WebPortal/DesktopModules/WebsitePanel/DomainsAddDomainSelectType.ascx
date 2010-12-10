<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainsAddDomainSelectType.ascx.cs" Inherits="WebsitePanel.Portal.DomainsAddDomainSelectType" EnableViewState="false" %>

<div class="FormBody">

    <p>
        <asp:Localize ID="IntroPar" runat="server" meta:resourcekey="IntroPar" />
    </p>
    
    <p>
        <b><asp:HyperLink ID="DomainLink" runat="server" meta:resourcekey="DomainLink">Domain</asp:HyperLink></b><br />
        <asp:Localize ID="DomainDescription" runat="server" meta:resourcekey="DomainDescription" /><br /><br />
    </p>
    
    <p>
        <b><asp:HyperLink ID="SubDomainLink" runat="server" meta:resourcekey="SubDomainLink">Sub-domain</asp:HyperLink></b><br />
        <asp:Localize ID="SubDomainDescription" runat="server" meta:resourcekey="SubDomainDescription" /><br /><br />
    </p>
    
    <p id="ProviderSubDomainPanel" runat="server">
        <b><asp:HyperLink ID="ProviderSubDomainLink" runat="server" meta:resourcekey="ProviderSubDomainLink">Provider Sub-domain</asp:HyperLink></b><br />
        <asp:Localize ID="ProviderSubDomainDescription" runat="server" meta:resourcekey="ProviderSubDomainDescription" /><br /><br />
    </p>
    
    <p>
        <b><asp:HyperLink ID="DomainPointerLink" runat="server" meta:resourcekey="DomainPointerLink">Domain Alias</asp:HyperLink></b><br />
        <asp:Localize ID="DomainPointerDescription" runat="server" meta:resourcekey="DomainPointerDescription" />
    </p>

</div>

<div class="FormFooter">
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
</div>