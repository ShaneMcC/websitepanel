<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostingAddonsEditAddon.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.HostingAddonsEditAddon" %>
<%@ Register TagPrefix="wsp" TagName="HostingPlanCycles" Src="UserControls/HostingPlanBillingCycles.ascx" %>
<%@ Register TagPrefix="wsp" TagName="OneTimeFee" Src="UserControls/HostingAddonOneTimeFee.ascx" %>
<%@ Register TagPrefix="wsp" TagName="AssignedProducts" Src="UserControls/AddonProducts.ascx" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclHostingAddon" meta:resourcekey="lclHostingAddon" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlHostingPlans" DataTextField="PlanName" DataValueField="PlanID" />
				<asp:TextBox runat="server" ID="txtAddonName" Width="250px" CssClass="NormalTextBox" Visible="false" />
				<asp:RequiredFieldValidator runat="server" ID="reqAddonName" ErrorMessage="*" Display="Dynamic" 
					ControlToValidate="ddlHostingPlans" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclProductSku" meta:resourcekey="lclProductSku" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtProductSku" CssClass="NormalTextBox" Width="250px" />
				<asp:RequiredFieldValidator runat="server" ErrorMessage="*" Display="Dynamic" 
					ControlToValidate="txtProductSku" />
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
				<asp:Localize runat="server" meta:resourcekey="lclDummyAddon" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblDummyAddon" AutoPostBack="true" 
					RepeatDirection="Horizontal" OnSelectedIndexChanged="rblDummyAddon_SelectedIndexChanged">
					<asp:ListItem Value="True" Text="Yes" />
					<asp:ListItem Value="False" Text="No" Selected="True" />
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclRecurringAddon" meta:resourcekey="lclRecurringAddon" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblRecurringAddon" AutoPostBack="true" 
					RepeatDirection="Horizontal" OnSelectedIndexChanged="rblRecurringAddon_SelectedIndexChanged">
					<asp:ListItem Value="True" Text="Yes" Selected="True" />
					<asp:ListItem Value="False" Text="No" />
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclShowQuantity" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblShowQuantity" RepeatDirection="Horizontal">
					<asp:ListItem Value="True" Text="Yes" Selected="True" />
					<asp:ListItem Value="False" Text="No" />
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclAddonStatus" meta:resourcekey="lclAddonStatus" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblAddonStatus" RepeatDirection="Horizontal">
					<asp:ListItem Value="True" Text="Active" Selected="True" />
					<asp:ListItem Value="False" Text="Disabled" />
				</asp:RadioButtonList>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<asp:Localize runat="server" ID="lclHostingAddonDesc" meta:resourcekey="lclHostingAddonDesc" />
				<br />
				<asp:TextBox runat="server" ID="txtHostingAddonDesc" CssClass="NormalTextBox" TextMode="MultiLine" Rows="10" Columns="80" />
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
	<wsp:OneTimeFee runat="server" ID="ctlOneTimeFee" Visible="false" />
</div>

<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclAssignedCategories" /></div>
</div>
<div class="FormBody">
	<wsp:AssignedProducts runat="server" ID="ctlAssignedProds" />
</div>

<div class="FormFooter">
	<asp:Button id="btnSaveAddon" runat="server" meta:resourcekey="btnSaveAddon" 
		CssClass="Button1" OnClick="btnSaveAddon_Click" />&nbsp;
	<asp:Button id="btnDeleteAddon" runat="server" meta:resourcekey="btnDeleteAddon" 
		CssClass="Button1" OnClick="btnDeleteAddon_Click" />&nbsp;
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" 
		CssClass="Button1" CausesValidation="False" OnClick="btnCancel_Click" />
</div>