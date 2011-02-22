<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceServiceItems.ascx.cs" Inherits="WebsitePanel.Portal.UserControls.SpaceServiceItems" %>
<%@ Register Src="Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
<%@ Register Src="ServerDetails.ascx" TagName="ServerDetails" TagPrefix="wsp" %>
<%@ Register Src="SearchBox.ascx" TagName="SearchBox" TagPrefix="wsp" %>
<div class="FormButtonsBar">
    <div class="Left">
        <asp:Button ID="btnAddItem" runat="server" Text="btnAddItem" CssClass="Button3" OnClick="btnAddItem_Click" />
        &nbsp;<asp:CheckBox ID="chkRecursive" runat="server" Text="Show Nested Space Items"
            meta:resourcekey="chkRecursive" AutoPostBack="true" Checked="True" CssClass="Normal" />
    </div>
    <div class="Right">
        <wsp:SearchBox ID="searchBox" runat="server" />
    </div>
</div>
<asp:Literal ID="litGroupName" runat="server" Visible="false"></asp:Literal>
<asp:Literal ID="litTypeName" runat="server" Visible="false"></asp:Literal>

<asp:UpdatePanel ID="ItemsPanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
	<ContentTemplate>

<asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="False" AllowSorting="True"
    DataSourceID="odsItemsPaged" EmptyDataText="gvItems" CssSelectorClass="NormalGridView"
    AllowPaging="True" OnRowCommand="gvItems_RowCommand" OnRowDataBound="gvItems_RowDataBound">
    <Columns>
        <asp:TemplateField SortExpression="ItemName" HeaderText="gvItemsName">
            <ItemStyle Width="100%"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkEdit1" runat="server" CssClass="Medium"
	                NavigateUrl='<%# GetItemEditUrl(Eval("PackageID"), Eval("ItemID")) %>'>
		            <%# Eval("ItemName")%>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvItemsView">
			<ItemStyle Wrap="false"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkView" runat="server" CssClass="Medium" Target="_blank"
	                NavigateUrl='<%# GetUrlNormalized(Eval("PackageID"), Eval("ItemID"))%>'>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="PackageName" HeaderText="gvItemsSpace">
            <ItemStyle Wrap="False"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkEdit2" runat="server"
	                NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		            <%# Eval("PackageName") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField SortExpression="Username" HeaderText="gvItemsUser">
		    <ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit3" runat="server"
				    NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
            <HeaderStyle Wrap="False" />
		</asp:TemplateField>
        <asp:TemplateField SortExpression="ServerName" HeaderText="gvItemsServer">
            <ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<asp:hyperlink id="lnkEdit4" runat="server"
				    NavigateUrl='<%# GetItemsPageUrl("ServerID", Eval("ServerID").ToString()) %>'>
					<%# Eval("ServerName") %>
				</asp:hyperlink>
			</ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
			<ItemTemplate>
				<asp:LinkButton ID="cmdDetach" runat="server" Text="Detach"
					CommandName="Detach" CommandArgument='<%# Eval("ItemID") %>'
					meta:resourcekey="cmdDetach" OnClientClick="return confirm('Remove this item?');"></asp:LinkButton>
			</ItemTemplate>
        </asp:TemplateField>
    </Columns>
	<PagerSettings Mode="NumericFirstLast" />
</asp:GridView>
<asp:ObjectDataSource ID="odsItemsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetServiceItemsPagedCount"
    SelectMethod="GetServiceItemsPaged" SortParameterName="sortColumn"
        TypeName="WebsitePanel.Portal.ServiceItemsHelper" OnSelected="odsItemsPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
        <asp:ControlParameter Name="groupName" ControlID="litGroupName" PropertyName="Text" />
        <asp:ControlParameter Name="typeName" ControlID="litTypeName" PropertyName="Text" />
        <asp:QueryStringParameter Name="serverId" QueryStringField="ServerID" DefaultValue="0" Type="Int32" />
        <asp:ControlParameter Name="recursive" ControlID="chkRecursive" PropertyName="Checked" DefaultValue="False" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox" PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>

	</ContentTemplate>
</asp:UpdatePanel>

<asp:Panel id="QuotasPanel" runat="server" CssClass="GridFooter">
	<asp:Label ID="lblQuotaName" runat="server" Text="Items" CssClass="NormalBold"></asp:Label>&nbsp;
	<wsp:Quota ID="itemsQuota" runat="server" QuotaName="Group.Items" />
</asp:Panel>