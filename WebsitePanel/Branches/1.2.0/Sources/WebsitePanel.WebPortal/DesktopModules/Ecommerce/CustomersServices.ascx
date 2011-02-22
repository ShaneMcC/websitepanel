<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomersServices.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomersServices" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<div class="FormBody">
	<asp:GridView ID="gvCustomersSvcs" runat="server" meta:resourcekey="gvCustomersSvcs" 
		AutoGenerateColumns="False" DataSourceID="odsCustomersSvcs" CssSelectorClass="NormalGridView" 
		AllowPaging="True" PageSize="20" OnRowCommand="gvCustomersSvcs_RowCommand">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvServiceName">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# EditUrl("ServiceId", Eval("ServiceId").ToString(), "view_svc", "UserId=" + PanelSecurity.SelectedUserId) %>'
						Text='<%# Eval("ServiceName") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvUsername">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Username") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvServiceType">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetSvcItemTypeName(Eval("TypeId")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvServiceStatus">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetServiceStatusName(Eval("Status")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCreated">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Created") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvLastModified">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Modified") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:Button runat="server" CommandName="Upgrade" CommandArgument='<%# Eval("ServiceId") %>' 
						CssClass="Button1" meta:resourcekey="btnUpgrade" Visible='<%# GetUpgradeButtonVisibility(Eval("TypeId"), Eval("Status")) %>' />&nbsp;
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
		<HeaderStyle CssClass="GridHeader" />
		<RowStyle CssClass="Normal" />
		<PagerStyle CssClass="GridPager" />
		<EmptyDataRowStyle CssClass="Normal" />
		<PagerSettings Mode="NumericFirstLast" />
	</asp:GridView>
</div>

<asp:ObjectDataSource runat="server" ID="odsCustomersSvcs" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetCustomersServicesCount" SelectMethod="GetCustomersServicesPaged" EnablePaging="true" />