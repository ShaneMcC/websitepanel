<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EcommerceSystemSettings.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.EcommerceSystemSettings" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="~/DesktopModules/WebsitePanel/UserControls/CollapsiblePanel.ascx" %>

<wsp:CollapsiblePanel runat="server" TargetControlID="PayMethodsPanel" 
    meta:resourcekey="PayMethodCollapse" IsCollapsed="false" />
<asp:Panel runat="server" ID="PayMethodsPanel" Height="0"  style="overflow: hidden;">
    <div class="FormBody">
	    <div><asp:HyperLink runat="server" ID="LinkCreditCard" meta:resourcekey="LinkCreditCard" /></div>
	    
	    <div class="MarginTop5Px"><asp:HyperLink runat="server" ID="Link2Checkout" meta:resourcekey="Link2Checkout" /></div>
	    
	    <div class="MarginTop5Px"><asp:HyperLink runat="server" ID="LinkPayPalAccnt" meta:resourcekey="LinkPayPalAccnt" /></div>
	    
	    <div class="MarginTop5Px"><asp:HyperLink runat="server" ID="LinkOffline" meta:resourcekey="LinkOffline" /></div>
    </div>
</asp:Panel>

<wsp:CollapsiblePanel runat="server" IsCollapsed="false" meta:ResourceKey="RegistrarsCollapse"
    TargetControlID="RegistrarsPanel" />
<asp:Panel ID="RegistrarsPanel" runat="server" Height="0" Style="overflow: hidden;">
    <div class="FormBody">
        <div><asp:HyperLink ID="LinkEnomRegistrar" runat="server" meta:resourcekey="LinkEnomRegistrar"/></div>
        
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkDirectiRegistrar" runat="server" meta:resourcekey="LinkDirectiRegistrar"/></div>
    </div>
</asp:Panel>

<wsp:CollapsiblePanel runat="server" IsCollapsed="false" meta:ResourceKey="MiscAndAutoCollapse"
    TargetControlID="MiscAndAutoPanel" />
<asp:Panel ID="MiscAndAutoPanel" runat="server" Height="0" Style="overflow: hidden;">
    <div class="FormBody">
        <div><asp:HyperLink ID="LinkProvisioningSts" runat="server" meta:resourcekey="LinkProvisioningSts"/></div>
		
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkTermsAndConds" runat="server" meta:resourcekey="LinkTermsAndConds"/></div>
        
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkWelcomeMsg" runat="server" meta:resourcekey="LinkWelcomeMsg"/></div>
    </div>
</asp:Panel>

<wsp:CollapsiblePanel runat="server" IsCollapsed="false" meta:ResourceKey="EmailNotifyCollapse"
    TargetControlID="EmailNotifyPanel" />
<asp:Panel ID="EmailNotifyPanel" runat="server" Height="0" Style="overflow: hidden;">
    <div class="FormBody">
        <div><asp:HyperLink ID="LinkNewInvoice" runat="server" meta:resourcekey="LinkNewInvoice"/></div>
        
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkPaymentReceived" runat="server" meta:resourcekey="LinkPaymentReceived"/></div>
        
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkSvcSuspend" runat="server" meta:resourcekey="LinkSvcSuspend"/></div>
        
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkSvcActivate" runat="server" meta:resourcekey="LinkSvcActivate"/></div>
        
        <div class="MarginTop5Px"><asp:HyperLink ID="LinkSvcCancel" runat="server" meta:resourcekey="LinkSvcCancel"/></div>
    </div>
</asp:Panel>