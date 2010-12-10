<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingAddons.ascx.cs" Inherits="WebsitePanel.Portal.HostingAddons" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Create Hosting Add-On" CssClass="Button3" OnClick="btnAddItem_Click" />
</div>
<asp:GridView id="gvAddons" runat="server" AutoGenerateColumns="False"
	DataSourceID="odsAddons" AllowPaging="True" AllowSorting="True" EmptyDataText="gvAddons"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField SortExpression="PlanName" HeaderText="gvAddonsName">
			<ItemStyle Width="100%"></ItemStyle>
			<ItemTemplate>
				<b><asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_addon", "UserID=" + Eval("UserID").ToString()) %>'>
					<%# Eval("PlanName") %>
				</asp:hyperlink></b><br />
				<%# Eval("PlanDescription") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:hyperlink id="lnkCopy" meta:resourcekey="lnkCopy" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_addon", "UserID=" + Eval("UserID").ToString(), "TargetAction=Copy") %>'>Copy</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvAddonsSpaces"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsAddons" runat="server" SelectMethod="GetRawHostingAddons"
    TypeName="WebsitePanel.Portal.HostingPlansHelper" OnSelected="odsAddons_Selected"></asp:ObjectDataSource>
