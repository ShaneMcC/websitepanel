<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChoosePaymentMethod.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.ChoosePaymentMethod" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>
<asp:PlaceHolder runat="server" ID="pnlControlHdr">
	<div class="FormButtonsBar">
		<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclChooseMethod" /></div>
	</div>
</asp:PlaceHolder>
<div>
	<asp:Repeater runat="server" ID="rptPaymentMethods">
		<HeaderTemplate>
			<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl">
				<tr>
					<td><wsp:RadioGroupValidator runat="server" ID="reqPaymentMethod" RadioButtonsGroup="QuickPayMethods" 
						CssClass="QuickLabel" Display="Dynamic" meta:resourcekey="reqPaymentMethod" /></td>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td class="QuickLabel"><wsp:GroupRadioButton runat="server" GroupName="QuickPayMethods" ID="rbChecked" Text='<%# Eval("DisplayName") %>' 
					ControlValue='<%# Eval("MethodName") %>' CssClass="Normal" /></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
</div>