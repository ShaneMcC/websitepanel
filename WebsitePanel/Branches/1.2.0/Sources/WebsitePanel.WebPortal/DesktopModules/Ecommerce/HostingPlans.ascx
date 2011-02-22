<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlans.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.HostingPlans" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddHostingPlan" runat="server" meta:resourcekey="btnAddHostingPlan" 
		CssClass="Button3" OnClick="btnAddHostingPlan_Click" />
</div>
<div>
	<asp:GridView ID="gvHostingPlans" runat="server" meta:resourcekey="gvHostingPlans" 
		AutoGenerateColumns="False" DataSourceID="odsHostingPlans"
		CssSelectorClass="NormalGridView" AllowPaging="True" PageSize="20">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvHostingPlan">
				<ItemStyle Width="40%"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("ProductId", Eval("ProductId").ToString(), "edit_hostingplan", "UserID="+ PanelSecurity.SelectedUserId) %>'>
						<%# Eval("ProductName")%>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvProductSku">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("ProductSku") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvStatus">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Enabled") %>
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

<asp:ObjectDataSource runat="server" ID="odsHostingPlans" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetHostingPlansCount" SelectMethod="GetHostingPlansPaged" EnablePaging="true" />