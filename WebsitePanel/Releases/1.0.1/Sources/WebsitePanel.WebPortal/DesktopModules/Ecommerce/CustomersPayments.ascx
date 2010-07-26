<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomersPayments.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomersPayments" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Import Namespace="WebsitePanel.Ecommerce.Portal" %>
<div>
	<asp:GridView ID="gvCustomersPayments" runat="server" meta:resourcekey="gvCustomersPayments" 
		AutoGenerateColumns="False" DataSourceID="odsCustomersPayments" DataKeyNames="PaymentId"
		CssSelectorClass="NormalGridView" AllowPaging="True" PageSize="20" OnRowCommand="gvCustomersPayments_RowCommand">
		<Columns>
			<asp:TemplateField meta:resourcekey="gvPaymentId">
				<ItemStyle Wrap="False" Width="5%" />
				<ItemTemplate>
					<%# Eval("PaymentId") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvTransactionId">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("TransactionId") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvInvoiceId">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<asp:HyperLink runat="server" NavigateUrl='<%# GetReferringInvoiceURL((int)Eval("InvoiceId")) %>'
						Text='<%# Eval("InvoiceNumber") %>' />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvTransactionAmount">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Currency") %>&nbsp;<%# Eval("Total", "{0:C}") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvPaymentMethod">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("MethodName") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvCreated">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("Created") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvProviderName">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# Eval("ProviderName") %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField meta:resourcekey="gvStatusName">
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<%# ecPanelFormatter.GetTransactionStatusName(Eval("Status")) %>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<HeaderStyle HorizontalAlign="Center" />
				<ItemStyle Wrap="False" />
				<ItemTemplate>
					<asp:Button runat="server" CssClass="Button1" CommandName="Approve" CommandArgument='<%# Container.DataItemIndex %>' Enabled='<%# GetApproveButtonEnabled(Eval("Status")) %>' meta:resourcekey="btnApprove" />&nbsp;<asp:Button 
						runat="server" CommandName="Decline" CommandArgument='<%# Container.DataItemIndex %>' CssClass="Button1" Enabled='<%# GetDeclineButtonEnabled(Eval("Status")) %>' meta:resourcekey="btnDecline" />
						&nbsp;<asp:Button runat="server" CommandName="Remove" CommandArgument='<%# Container.DataItemIndex %>' CssClass="Button1" meta:resourcekey="btnRemove" />
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

<asp:ObjectDataSource runat="server" ID="odsCustomersPayments" TypeName="WebsitePanel.Ecommerce.Portal.StorehouseHelper" 
	SelectCountMethod="GetCustomersPaymentsCount" SelectMethod="GetCustomersPaymentsPaged" EnablePaging="true" />