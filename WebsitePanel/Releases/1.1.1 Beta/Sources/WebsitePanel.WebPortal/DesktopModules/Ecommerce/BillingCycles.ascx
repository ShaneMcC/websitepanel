<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingCycles.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.BillingCycles" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnCreateCycle" runat="server" meta:resourcekey="btnCreateCycle" 
		CssClass="Button3" OnClick="btnCreateCycle_Click" />
</div>
<div>
	<asp:GridView ID="gvBillingCycles" runat="server" meta:resourcekey="gvBillingCycles" 
		AutoGenerateColumns="False" DataSourceID="odsBillingCycles"
		CssSelectorClass="NormalGridView" AllowPaging="True" PageSize="20">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvCycleName">
				<ItemStyle Width="40%"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("CycleId", Eval("CycleId").ToString(), "edit_billingcycle", "UserID="+ PanelSecurity.SelectedUserId) %>'>
						<%# Eval("CycleName") %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvBillingPeriod">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("BillingPeriod") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvPeriodLength">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("PeriodLength") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCreated">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Created") %>
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

<asp:ObjectDataSource runat="server" ID="odsBillingCycles" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetBillingCyclesCount" SelectMethod="GetBillingCyclesPaged" EnablePaging="true" />