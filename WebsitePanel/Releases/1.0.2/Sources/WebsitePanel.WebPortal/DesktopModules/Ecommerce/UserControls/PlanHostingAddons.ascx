<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlanHostingAddons.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.PlanHostingAddons" %>
<%@ Register TagPrefix="wsp" TagName="QuickHostingAddon" Src="QuickHostingAddon.ascx" %>

<asp:Repeater runat="server" ID="rptHostingAddons">
	<ItemTemplate>
		<wsp:QuickHostingAddon runat="server" ID="ctlQuickAddon" AddonInfo='<%# Container.DataItem %>' />
	</ItemTemplate>
</asp:Repeater>