<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlansAddPlan.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.HostingPlansAddPlan" %>
<%@ Register TagPrefix="wsp" TagName="HostingPlanCycles" Src="UserControls/HostingPlanBillingCycles.ascx" %>
<%@ Register TagPrefix="wsp" TagName="HostingPlanHighlights" Src="UserControls/ProductHighlights.ascx" %>
<%@ Register TagPrefix="wsp" TagName="AssignedCategories" Src="UserControls/ProductCategories.ascx" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclHostingPlan" meta:resourcekey="lclHostingPlan" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlHostingPlans" DataTextField="PlanName" DataValueField="PlanID" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHostingPlans" Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclInitialStatus" meta:resourcekey="lclInitialStatus" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlInitialStatus">
					<asp:ListItem Value="1" meta:resourcekey="lclActive"/>
					<asp:ListItem Value="2" meta:resourcekey="lclSuspended"/>
					<asp:ListItem Value="3" meta:resourcekey="lclCancelled"/>
					<asp:ListItem Value="4" meta:resourcekey="lclPending"/>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclDomainOption" meta:resourcekey="lclDomainOption" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlDomainOption">
					<asp:ListItem Value="2" meta:resourcekey="lclOptional"/>
					<asp:ListItem Value="1" meta:resourcekey="lclRequired"/>
					<asp:ListItem Value="3" meta:resourcekey="lclHide"/>
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclProductSku" meta:resourcekey="lclProductSku" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtProductSku" CssClass="NormalTextBox" Width="250px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtProductSku" Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr runat="server" visible="false">
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclTaxInclusive" /></td>
			<td>
				<asp:CheckBox runat="server" id="chkTaxInclusive" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclPlanIntendsFor" meta:resourcekey="lclPlanIntendsFor" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblPlanIntendsFor" RepeatDirection="Horizontal">
					<asp:ListItem Value="3" Text="Customer" Selected="True" />
					<asp:ListItem Value="2" Text="Reseller" />
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclPlanStatus" meta:resourcekey="lclPlanStatus" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblPlanStatus" RepeatDirection="Horizontal">
					<asp:ListItem Value="True" Text="Active" Selected="True" />
					<asp:ListItem Value="False" Text="Disabled" />
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:Localize runat="server" ID="lclHostingPlanDesc" meta:resourcekey="lclHostingPlanDesc" />
				<br />
				<asp:TextBox runat="server" ID="txtHostingPlanDesc" CssClass="NormalTextBox" TextMode="MultiLine" Rows="10" Columns="80" />
			</td>
		</tr>
	</table>
</div>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclBillingCycles" /></div>
</div>
<div class="FormBody">
	<wsp:ManualContextValidator runat="server" ID="ctxValBillingCycles" CssClass="QuickLabel" Display="Dynamic" EnableViewState="false"
		OnEvaluatingContext="ctxValBillingCycles_EvaluatingContext" EnableClientScript="false" meta:resourcekey="ctxValBillingCycles" />
	<wsp:HostingPlanCycles runat="server" ID="ctlPlanCycles" />
</div>

<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclPlanHighlights" /></div>
</div>
<div class="FormBody">
	<wsp:HostingPlanHighlights runat="server" ID="ctlPlanHighlights" />
</div>

<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclAssignedCategories" /></div>
</div>
<div class="FormBody">
	<wsp:AssignedCategories runat="server" ID="ctlAssignedCats" />
</div>

<div class="FormFooter">
	<asp:Button id="btnCreatePlan" runat="server" meta:resourcekey="btnCreatePlan" 
		CssClass="Button1" OnClick="btnCreatePlan_Click" />&nbsp;
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" 
		CssClass="Button1" CausesValidation="False" OnClick="btnCancel_Click" />
</div>