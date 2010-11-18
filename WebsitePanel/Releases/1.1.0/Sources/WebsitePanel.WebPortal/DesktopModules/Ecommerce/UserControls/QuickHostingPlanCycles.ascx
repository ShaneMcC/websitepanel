<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickHostingPlanCycles.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.QuickHostingPlanCycles" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>
<asp:Repeater runat="server" ID="rptPlanCycles">
	<HeaderTemplate>
		<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl">
			<tr class="FormSectionHeader">
				<th class="FormButtonsBar Width100Pcs" style="text-align: left;"><asp:Localize runat="server" meta:resourcekey="lclModuleHeader" /></th>
				<th class="FormButtonsBar" nowrap><asp:Localize runat="server" meta:resourcekey="lclRecurringFee" /></th>
				<th class="FormButtonsBar" nowrap><asp:Localize runat="server" meta:resourcekey="lclSetupFee" /></th>
			</tr>
			<tr>
				<td colspan="3"><wsp:RadioGroupValidator runat="server" ID="reqCycleCheckedValidator" 
					CssClass="QuickLabel" RadioButtonsGroup="QuickPlanCycles" Display="Dynamic" 
					meta:resourcekey="lclCycleChecked" /></td>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr>
			<td class="QuickLabel"><wsp:GroupRadioButton runat="server" GroupName="QuickPlanCycles" ID="rbSelected" 
				Text='<%# Eval("CycleName") %>' ControlValue='<%# Eval("CycleId") %>' /></td>
			<td class="QuickLabel Centered"><%# EcommerceSettings.CurrencyCodeISO %>&nbsp;<%# Eval("RecurringFee", "{0:C}") %></td>
			<td class="QuickLabel Centered"><%# EcommerceSettings.CurrencyCodeISO %>&nbsp;<%# Eval("SetupFee", "{0:C}") %></td>
		</tr>
	</ItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:Repeater>