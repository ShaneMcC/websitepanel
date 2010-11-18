<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewProductDetails.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.ViewProductDetails" %>
<%@ Register TagPrefix="wsp" TagName="ViewQuotas" Src="UserControls/HostingPlanQuotas.ascx" %>
<%@ Register TagPrefix="wsp" TagName="PlanHighlights" Src="ProductControls/HostingPlan_Highlights.ascx" %>
<div class="FormBody">
	<div class="StrongHeaderLabel"><asp:Literal runat="server" ID="litPlanName" /></div>
	<p class="InventoryDescription">
		<asp:Literal runat="server" ID="litProductDesc" />
	</p>
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclHighlightsLabel" /></div>
	<wsp:PlanHighlights runat="server" ID="ctlPlanHighlights" />
</div>

<wsp:ViewQuotas runat="server" id="ctlPlanQuotas" />
