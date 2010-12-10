<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlanQuotas.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.HostingPlanQuotas" %>
<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl QuickLabel">
<asp:PlaceHolder runat="server" ID="OS" Visible="false">
	<tr>
		<th colspan="2">
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclOsResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="OS_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="DNS" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclDNSResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="DNS_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="Web" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclWebResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="Web_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
	
	<asp:Repeater runat="server" ID="FTP_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="Sharepoint" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclSharepointResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="Sharepoint_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="Mail" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclMailResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="Mail_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="MsSQL2000" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclMSSQL2000Resources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="MsSQL2000_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="MsSQL2005" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclMSSQL2005Resources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="MsSQL2005_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="MsSQL2008" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize ID="Localize3" runat="server" meta:resourcekey="lclMSSQL2008Resources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="MsSQL2008_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>


<asp:PlaceHolder runat="server" ID="MySQL4" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclMySQL4Resources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="MySQL4_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="MySQL5" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclMySQL5Resources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="MySQL5_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="Statistics" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclStatisticsResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="Statistics_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="HostedSolution" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize ID="Localize2" runat="server" meta:resourcekey="lclHostedSolutionResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="HostedSolution_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>


<asp:PlaceHolder runat="server" ID="Exchange2007" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="lclExchange2007Resources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="Exchange2007_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="HostedSharePoint" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize ID="Localize6" runat="server" meta:resourcekey="lclHostedSharePointResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="HostedSharePoint_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>


<asp:PlaceHolder runat="server" ID="BlackBerry" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize ID="Localize4" runat="server" meta:resourcekey="lclBlackBerryResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="BlackBerry_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="HostedCRM" Visible="false">
	<tr>
		<th colspan="2">
			<br />
			<div class="FormButtonsBar">
				<div class="FormSectionHeader"><asp:Localize ID="Localize5" runat="server" meta:resourcekey="lclHostedCRMResources" /></div>
			</div>
		</th>
	</tr>
	<asp:Repeater runat="server" ID="HostedCRM_Quotas">
		<ItemTemplate>
			<tr>
				<td class="Width20Pcs" style="white-space: nowrap;"><strong><%# GetSharedLocalizedString("Quota." + GetQuotaItemName((string)Container.DataItem)) %>:</strong></td>
				<td><%# GetQuotaItemAllocatedValue((string)Container.DataItem) %></td>
			</tr>
		</ItemTemplate>
	</asp:Repeater>
</asp:PlaceHolder>

</table>
