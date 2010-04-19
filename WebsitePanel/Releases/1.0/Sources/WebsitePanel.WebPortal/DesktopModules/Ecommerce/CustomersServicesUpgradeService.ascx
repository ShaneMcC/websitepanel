<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomersServicesUpgradeService.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomersServicesUpgradeService" %>
<%@ Register TagPrefix="wsp" TagName="DomainOption" Src="UserControls/PlanDomainOption.ascx" %>
<%@ Register TagPrefix="wsp" TagName="HostingAddons" Src="UserControls/PlanHostingAddons.ascx" %>
<%@ Register TagPrefix="wsp" TagName="SelectPaymentMethod" Src="UserControls/ChoosePaymentMethod.ascx" %>

<div class="ProductInfo">
	<asp:Literal runat="server" ID="ltrProductName" />
</div>
<div>
	<wsp:DomainOption runat="server" ID="ctlPlanDomain" />

	<div class="StrongHeaderLabel"><asp:Localize runat="server" meta:resourcekey="lclChooseAddonMsg" /></div>
	<br />
	<wsp:HostingAddons runat="server" ID="ctlPlanAddons" />

	<wsp:SelectPaymentMethod runat="server" ID="ctlPaymentMethod" />
</div>
<div class="FormFooter">
	<asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnContinue" OnClick="btnContinue_Click" />
</div>