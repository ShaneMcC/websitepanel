<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainNames.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DomainNames" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnCreateDomain" runat="server" meta:resourcekey="btnCreateDomain" 
		CssClass="Button3" OnClick="btnCreateDomain_Click" />
</div>
<div>
	<asp:GridView ID="gvDomainsProducts" runat="server" meta:resourcekey="gvDomainsProducts" 
		AutoGenerateColumns="False" DataSourceID="odsTopLevelDomains"
		CssSelectorClass="NormalGridView" AllowPaging="True" PageSize="20">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvTopLevelDomain">
				<ItemStyle Width="40%"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("ProductId", Eval("ProductId").ToString(), "edit_tld", "UserID="+ PanelSecurity.SelectedUserId) %>'>
						<%# Eval("ProductName") %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvProductSku">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("ProductSku") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvDomainRegistrar">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("DisplayName") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvDomainStatus">
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

<asp:ObjectDataSource runat="server" ID="odsTopLevelDomains" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetTopLevelDomainsCount" SelectMethod="GetTopLevelDomainsPaged" EnablePaging="true" />