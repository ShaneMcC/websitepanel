<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostingPlans.ascx.cs" Inherits="WebsitePanel.Portal.HostingPlans" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="uc3" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Create Hosting Plan" CssClass="Button3" OnClick="btnAddItem_Click" />
</div>
<asp:GridView id="gvPlans" runat="server" AutoGenerateColumns="False"
	DataSourceID="odsPlans" AllowPaging="True" AllowSorting="True" EmptyDataText="gvPlans"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField SortExpression="PlanName" HeaderText="gvPlansName">
			<ItemStyle Width="100%"></ItemStyle>
			<ItemTemplate>
				<b><asp:hyperlink id="lnkEdit" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_plan", "UserID=" + Eval("UserID").ToString()) %>'>
					<%# Eval("PlanName") %>
				</asp:hyperlink></b><br />
				<%# Eval("PlanDescription") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
				<asp:hyperlink id="lnkCopy" meta:resourcekey="lnkCopy" runat="server" NavigateUrl='<%# EditUrl("PlanID", Eval("PlanID").ToString(), "edit_plan", "UserID=" + Eval("UserID").ToString(), "TargetAction=Copy") %>'>Copy</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
        <asp:TemplateField SortExpression="ServerName" HeaderText="gvPlansServer"
                HeaderStyle-Wrap="false">
            <ItemStyle Width="30%" Wrap="false"></ItemStyle>
            <ItemTemplate>
		         <uc3:ServerDetails ID="serverDetails" runat="server"
		            ServerID='<%# Eval("ServerID") %>'
		            ServerName='<%# Eval("ServerName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvPlansSpaces"></asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsPlans" runat="server" SelectMethod="GetRawHostingPlans"
TypeName="WebsitePanel.Portal.HostingPlansHelper" OnSelected="odsPlans_Selected"></asp:ObjectDataSource>
