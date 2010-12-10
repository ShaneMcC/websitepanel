<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlan_Highlights.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.ProductControls.HostingPlan_Highlights" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<asp:Repeater runat="server" ID="rptProductHighlights">
	<HeaderTemplate>
		<ul>
	</HeaderTemplate>
	<ItemTemplate>
		<li class="QuickLabel"><%# Container.DataItem %></li>
	</ItemTemplate>
	<FooterTemplate>
		<asp:PlaceHolder runat="server" Visible='<%# ShowMoreDetails %>'>
			<li class="QuickLabel"><a href="javascript:void(0)" onclick='window.open("Default.aspx?pid=ecProductDetails&ResellerId=<%# ecPanelRequest.ResellerId %>&ProductId=<%# ecPanelRequest.ProductId %>", "view_details", "channelmode=no,directories=no,fullscreen=no,height=450px,left=50px,location=no,menubar=no,resizable=0,scrollbars=yes,status=no,titlebar=no,menubar=no,top=50px,width=450px")'>
				<asp:Localize runat="server" meta:resourcekey="lnkMoreDetails" /></a></li>
		</asp:PlaceHolder>
		</ul>
	</FooterTemplate>
</asp:Repeater>