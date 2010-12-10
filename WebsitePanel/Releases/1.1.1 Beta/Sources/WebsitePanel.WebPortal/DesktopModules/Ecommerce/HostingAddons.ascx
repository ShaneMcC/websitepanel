<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostingAddons.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.HostingAddons" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddAddon" runat="server" meta:resourcekey="btnAddAddon" 
		CssClass="Button3" OnClick="btnAddAddon_Click" />
</div>
<div>
	<asp:GridView ID="gvHostingAddons" meta:resourcekey="gvHostingAddons" runat="server" 
		AutoGenerateColumns="False" DataSourceID="odsHostingAddons" PageSize="20" 
		AllowPaging="True" CssSelectorClass="NormalGridView">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvAddonName">
				<ItemStyle Width="60%" />
				<ItemTemplate>
					<asp:HyperLink ID="lnkEdit" runat="server" 
						NavigateUrl='<%# EditUrl("ProductId", Eval("ProductId").ToString(), "edit_hostingaddon", "UserID="+ PanelSecurity.SelectedUserId) %>'
						Text='<%# Eval("ProductName")%>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvAddonSku">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("ProductSku") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvAddonStatus">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Enabled") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvAddonCreated">
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

<asp:ObjectDataSource runat="server" ID="odsHostingAddons" EnablePaging="true" 
	TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetHostingAddonsCount" SelectMethod="GetHostingAddonsPaged" />
