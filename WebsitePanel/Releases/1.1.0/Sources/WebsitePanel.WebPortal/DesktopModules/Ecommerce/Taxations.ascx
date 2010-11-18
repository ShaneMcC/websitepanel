<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Taxations.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.Taxations" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddTaxation" runat="server" meta:resourcekey="btnAddTaxation" 
		CssClass="Button3" OnClick="btnAddTaxation_Click" />
</div>
<div>
	<asp:GridView ID="gvTaxations" runat="server" meta:resourcekey="gvTaxations" 
		AutoGenerateColumns="False" DataSourceID="odsTaxations"
		CssSelectorClass="NormalGridView" AllowPaging="True" PageSize="20">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvDescription">
				<ItemStyle Width="40%"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("TaxationId", Eval("TaxationId").ToString(), "edit_tax", "UserID="+ PanelSecurity.SelectedUserId) %>'>
						<%# Eval("Description")%>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvTaxStatus">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetTaxationStatus((bool)Eval("Active")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCountry">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Country") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvState">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("State") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvTaxType">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetTaxationType(Eval("Type")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvTaxAmount">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetTaxationFormat(Eval("Type"), Eval("Amount")) %>
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

<asp:ObjectDataSource runat="server" ID="odsTaxations" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetTaxationsCount" SelectMethod="GetTaxationsPaged" EnablePaging="true" />