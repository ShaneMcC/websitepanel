<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomersInvoices.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomersInvoices" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<div class="FormBody">
	<asp:GridView ID="gvCustomersInvoices" runat="server" meta:resourcekey="gvCustomersInvoices" 
		AutoGenerateColumns="False" DataSourceID="odsCustomersInvoices"
		CssSelectorClass="NormalGridView" AllowPaging="True" PageSize="20">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvInvoiceNumber">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# EditUrl("InvoiceId", Eval("InvoiceId").ToString(), "view_invoice", "UserId=" + PanelSecurity.SelectedUserId) %>'
						Text='<%# Eval("InvoiceNumber") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvUsername">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Username") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvInvoicePaid">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetBooleanName((bool)Eval("Paid")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCreated">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Created") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvDueDate">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("DueDate") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvSubTotal">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Currency") %>
					&nbsp;<%# Eval("SubTotal", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvTotal">
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<%# Eval("Currency") %>
					&nbsp;<%# Eval("Total", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemStyle Wrap="False" HorizontalAlign="Center" />
				<ItemTemplate>
					<asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnVoidInvoice"
						OnClick="btnVoidInvoice_Click" CommandArgument='<%# Eval("InvoiceId") %>' />
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

<asp:ObjectDataSource runat="server" ID="odsCustomersInvoices" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetCustomersInvoicesCount" SelectMethod="GetCustomersInvoicesPaged" EnablePaging="true" />