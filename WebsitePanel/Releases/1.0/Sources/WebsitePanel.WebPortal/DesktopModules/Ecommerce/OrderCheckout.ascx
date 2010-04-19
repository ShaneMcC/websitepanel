<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderCheckout.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.OrderCheckout" %>
<%@ Register TagPrefix="wsp" TagName="CustomerInvoice" Src="UserControls/CustomerInvoiceTemplated.ascx" %>

<div class="FormBody">
	<wsp:CustomerInvoice ID="ctlCustomerInvoice" runat="server" />
</div>

<asp:PlaceHolder runat="server" ID="phPaymentMethod" />

<div class="FormFooter">
	<asp:Button runat="server" ID="btnComplete" meta:resourcekey="btnComplete" 
		CssClass="Button1" OnClick="btnComplete_Click" />&nbsp;
	<asp:Button runat="server" ID="btnProceed" meta:resourcekey="btnProceed" CausesValidation="false" 
		Visible="false" CssClass="Button1" OnClientClick="ProceedToCheckout();" />
</div>
