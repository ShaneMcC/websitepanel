<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickHostingAddon.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.QuickHostingAddon" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl">
	<tr class="FormSectionHeader">
		<th class="FormButtonsBar Width100Pcs" style="text-align: left;"><asp:CheckBox runat="server" ID="chkSelected" 
			AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkSelected_CheckedChanged" /><asp:Literal runat="server" ID="ltrAddonName" /></th>
		<th class="FormButtonsBar Centered Width15Pcs" nowrap>&nbsp;<asp:Localize runat="server" Visible="false" ID="lclAddonPrice" meta:resourcekey="lclAddonPrice" /></th>
		<th class="FormButtonsBar Centered Width15Pcs" nowrap>&nbsp;<asp:Localize runat="server" Visible="false" ID="lclSetupFee" meta:resourcekey="lclSetupFee" /></th>
	</tr>

<asp:PlaceHolder runat="server" ID="pnlRecurring" Visible="false">
	<asp:Repeater runat="server" ID="rptAddonCycles">
		<HeaderTemplate>
			<tr>
				<td colspan="3"><wsp:RadioGroupValidator runat="server" CssClass="QuickLabel" RadioButtonsGroup="<%# GroupName %>" 
					meta:resourcekey="valQuickAddonCycles" Display="Dynamic" EnableClientScript="true" /></td>
			</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td class="QuickLabel"><wsp:GroupRadioButton runat="server" ID="rbSelected" 
					GroupName="<%# GroupName %>" Text='<%# Eval("CycleName") %>' ControlValue='<%# Eval("CycleId") %>' /></td>
				<td class="QuickLabel Centered"><%# EcommerceSettings.CurrencyCodeISO %>&nbsp;<%# Eval("RecurringFee", "{0:C}") %></td>
				<td class="QuickLabel Centered"><%# EcommerceSettings.CurrencyCodeISO %>&nbsp;<%# Eval("SetupFee", "{0:C}") %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="pnlOneTimeFee" Visible="false">
	<tr>
		<td class="QuickLabel"><asp:Localize runat="server" meta:resourcekey="lclOneTimeFee" /></td>
		<td class="QuickLabel"><%= EcommerceSettings.CurrencyCodeISO %>&nbsp;<asp:Literal 
			runat="server" ID="ltrAddonPrice" /></td>
		<td class="QuickLabel"><%= EcommerceSettings.CurrencyCodeISO %>&nbsp;<asp:Literal 
			runat="server" ID="ltrSetupFee" /></td>
	</tr>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="pnlQuantity" Visible="false">
	<tr>
		<td colspan="3" class="QuickLabel"><asp:Localize runat="server" meta:resourcekey="lclQuantity" />&nbsp;<asp:TextBox 
			runat="server" ID="txtQuantity" CssClass="Normal Centered" Width="30px" Text="1" />&nbsp;<asp:RequiredFieldValidator 
			runat="server" ControlToValidate="txtQuantity" Text="*" Display="Dynamic" /></td>
	</tr>
</asp:PlaceHolder>

</table>