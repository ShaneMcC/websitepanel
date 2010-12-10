<%@ Control Language="C#" AutoEventWireup="true" Codebehind="Categories.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.Categories" %>
<%@ Register TagPrefix="wsp" TagName="BreadCrumb" Src="UserControls/PathBreadCrumb.ascx" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar" style="height: 25px;">
	<div class="Left">
		<asp:Button ID="btnAddCategory" runat="server" meta:resourcekey="btnAddCategory" 
			CssClass="Button3" OnClick="btnAddCategory_Click" />
	</div>
	<div class="Right">
		<wsp:BreadCrumb runat="server" ID="bcNavigatePath" />
	</div>
</div>
<div>
	<asp:GridView ID="gvCategories" DataSourceID="odsCategories" runat="server" 
	AutoGenerateColumns="False" Width="100%" meta:resourcekey="gvCategories"
	AllowPaging="True" PageSize="20" CssSelectorClass="NormalGridView">
		<Columns>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink runat="server" 
						NavigateUrl='<%# EditUrl("CategoryId", Eval("CategoryId").ToString(), Keys.EditItem, "UserID=" + PanelSecurity.SelectedUserId) %>'>
							<asp:Image ID="Image1" runat="server" SkinID="EditSmall" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCategoryName">
				<ItemStyle Wrap="false" />
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# NavigateURL("CategoryId", Eval("CategoryId").ToString(), "UserID=" + PanelSecurity.SelectedUserId) %>'>
						<%# Eval("CategoryName")%>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCategorySku">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("CategorySku") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCategoryCreated">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Created") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvLastModified">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetLastModified(Eval("Modified")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvShortDescription">
				<ItemStyle Wrap="True" Width="40%" />
				<ItemTemplate>
					<%# Eval("ShortDescription") %>
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

<asp:ObjectDataSource runat="server" ID="odsCategories" 
	SelectCountMethod="GetCategoriesCount" EnablePaging="true" 
	SelectMethod="GetCategoriesPaged" TypeName="WebsitePanel.Ecommerce.Portal.CategoryHelper" />
