<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomersInvoicesViewInvoice.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomersInvoicesViewInvoice" %>
<%@ Register TagPrefix="wsp" TagName="CustomerInvoice" Src="UserControls/CustomerInvoiceTemplated.ascx" %>
<%@ Register TagPrefix="wsp" TagName="SelectPaymentMethod" Src="UserControls/ChoosePaymentMethod.ascx" %>
<%@ Register TagPrefix="wsp" TagName="ManualPaymentMethod" Src="UserControls/ManualPaymentAdd.ascx" %>

<div class="FormBody">
	<wsp:CustomerInvoice ID="ctlCustomerInvoice" runat="server" />
</div>

<wsp:SelectPaymentMethod runat="server" ID="ctlSelPaymentMethod" />

<asp:PlaceHolder runat="server" ID="pnlAddManPayment">
	<div class="FormButtonsBar">
		<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclAddManualPayment" /></div>
	</div>
	<div class="FormBody">
		<wsp:ManualPaymentMethod runat="server" id="ctlManualPayment" />
	</div>
</asp:PlaceHolder>
<div class="FormFooter">
	<asp:Button runat="server" ID="btnReturn" CausesValidation="false" meta:resourcekey="btnReturn" 
		CssClass="Button1" OnClick="btnReturn_Click" />
	<asp:Button runat="server" ID="btnPayForInvoice" meta:resourcekey="btnPayForInvoice" CssClass="Button1" 
		OnClick="btnPayForInvoice_Click" />
	<asp:Button runat="server" ID="btnAddPayment" 
		meta:resourcekey="btnAddPayment" CssClass="Button1" OnClick="btnAddPayment_Click" />
	<asp:Button runat="server" ID="btnActivateInvoice" meta:resourcekey="btnActivateInvoice" CssClass="Button1" 
		OnClick="btnActivateInvoice_Click" />
	
</div>