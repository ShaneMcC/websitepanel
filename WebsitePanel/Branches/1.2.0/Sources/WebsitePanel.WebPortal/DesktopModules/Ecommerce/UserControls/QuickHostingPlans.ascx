<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickHostingPlans.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.QuickHostingPlans" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclChooseHostingPlan" /></div>
</div>
<div>
	<asp:Repeater runat="server" ID="rptHostingPlans">
		<HeaderTemplate>
			<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl" width="100%">
				<tr>
					<td colspan="2"><wsp:RadioGroupValidator runat="server" ID="reqRadioCheckedValidator" 
						RadioButtonsGroup="QuickHostingPlans" CssClass="QuickLabel" Display="Dynamic" 
						meta:resourcekey="lclRadioCheckedValidator" /></td>
				</tr>
		</HeaderTemplate>
		<ItemTemplate>
			<tr>
				<td class="QuickLabel Width20Pcs"><wsp:GroupRadioButton runat="server" ID="rbSelected" AutoPostBack="true" OnCheckedChanged="rbSelected_OnCheckedChanged" 
					GroupName="QuickHostingPlans" Text='<%# Eval("ProductName") %>' ControlValue='<%# Eval("ProductId") %>' /></td>
				<td class="QuickLabel"><a href="javascript:void(0);" onclick="window.open('Default.aspx?pid=ecProductDetails&ResellerId=<%# Eval("ResellerId") %>&ProductId=<%# Eval("ProductId") %>', 'view_details', 'channelmode=no,directories=no,fullscreen=no,height=450px,left=50px,location=no,menubar=no,resizable=0,scrollbars=yes,status=no,titlebar=no,menubar=no,top=50px,width=450px')"><asp:Localize runat="server" meta:resourcekey="lclPlanDetails" /></a></td>
			</tr>
		</ItemTemplate>
		<FooterTemplate>
			</table>
		</FooterTemplate>
	</asp:Repeater>
</div>