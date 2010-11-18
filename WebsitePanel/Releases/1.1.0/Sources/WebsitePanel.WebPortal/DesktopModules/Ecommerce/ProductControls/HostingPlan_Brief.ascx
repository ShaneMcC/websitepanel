<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlan_Brief.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.ProductControls.HostingPlan_Brief" %>
<div class="ProductBlock">
	<table cellpadding="0" cellspacing="0" border="0" width="100%">
		<tr>
			<td class="ProductInfo">
				<asp:Literal runat="server" ID="ltrProductName" />
				<asp:Repeater runat="server" ID="rptHighlights">
					<HeaderTemplate>
						<ul type="disc" class="HighLights">
					</HeaderTemplate>
					<ItemTemplate>
						<li><%# Container.DataItem %></li>
					</ItemTemplate>
					<FooterTemplate>
						</ul>
					</FooterTemplate>
				</asp:Repeater>
				<div class="PriceBlock">
					<div class="Left">
						<div class="NormalText"><asp:Localize runat="server" meta:resourcekey="lclStartingFrom" /></div>
						<asp:Literal runat="server" ID="ltrCurrencySymb" />&nbsp;
						<span class="ProductPrice"><asp:Literal runat="server" ID="ltrProductPrice" /></span>&nbsp;
					</div>
					<div class="Right"><asp:Button runat="server" ID="btnOrder" meta:resourcekey="btnOrder" 
						CssClass="Button1" OnClick="btnOrder_Click" /></div>
				</div>
			</td>
		</tr>
	</table>
</div>