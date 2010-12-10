<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainName_ServiceDetails.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.ProductControls.DomainName_ServiceDetails" %>
<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclServiceName" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrServiceName" /></td>
		</tr>
	<asp:PlaceHolder runat="server" ID="pnlUsername">
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclUsername" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrUsername" /></td>
		</tr>
	</asp:PlaceHolder>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclServiceStatus" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrServiceStatus" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclServiceType" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrServiceTypeName" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclSvcCreated" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrSvcCreated" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclSvcCycleName" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrSvcCycleName" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclSvcCyclePeriod" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrSvcCyclePeriod" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclSvcSetupFee" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrSvcSetupFee" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclSvcRecurringFee" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrSvcRecurringFee" /></td>
		</tr>
	<asp:PlaceHolder runat="server" ID="pnlDomainOrder">
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclSvcProviderName" /></td>
			<td>
				<asp:Literal runat="server" ID="ltrSvcProviderName" /></td>
		</tr>
	</asp:PlaceHolder>
	</table>
</div>

<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclServiceHistory" /></div>
</div>
<div>
	<asp:GridView ID="gvServiceHistory" runat="server" meta:resourcekey="gvServiceHistory" 
		AutoGenerateColumns="False" CssSelectorClass="NormalGridView" AllowPaging="False">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvCycleName">
				<ItemStyle Width="40%"></ItemStyle>
				<ItemTemplate>
					<%# Eval("CycleName") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvBillingPeriod">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# String.Concat(Eval("PeriodLength"), " ", Eval("BillingPeriod"), "(s)") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvSetupFee">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Currency") %>&nbsp;<%# Eval("SetupFee", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvRecurringFee">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Currency") %>&nbsp;<%# Eval("RecurringFee", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvStartDate">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("StartDate") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvEndDate">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("EndDate") %>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
		<HeaderStyle CssClass="GridHeader" HorizontalAlign="Left" />
		<RowStyle CssClass="Normal" />
		<PagerStyle CssClass="GridPager" />
		<EmptyDataRowStyle CssClass="Normal" />
		<PagerSettings Mode="NumericFirstLast" />
	</asp:GridView>
</div>