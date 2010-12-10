<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchSpaces.ascx.cs" Inherits="WebsitePanel.Portal.SearchSpaces" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>


<div class="FormButtonsBar">
    <asp:Panel ID="tblSearch" runat="server" DefaultButton="cmdSearch" CssClass="NormalBold">
    <asp:Label ID="lblSearch" runat="server" meta:resourcekey="lblSearch"></asp:Label>
        <asp:DropDownList ID="ddlItemType" runat="server" CssClass="NormalTextBox">
        </asp:DropDownList><asp:TextBox ID="txtFilterValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" SkinID="SearchButton" meta:resourcekey="cmdSearch"
			CausesValidation="false" OnClick="cmdSearch_Click" />
    </asp:Panel>
</div>

<asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvPackages" CssSelectorClass="NormalGridView"
    AllowSorting="True" DataSourceID="odsItemsPaged" AllowPaging="True">
    <Columns>
        <asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="gvPackagesItemName" ></asp:BoundField>
        <asp:TemplateField SortExpression="PackageName" HeaderText="gvPackagesName">
            <ItemTemplate>
	            <asp:hyperlink id=lnkSpace runat="server" NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		            <%# Eval("PackageName") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="Username" HeaderText="gvPackagesUsername">
            <ItemTemplate>
	            <asp:hyperlink id=lnkUser runat="server" NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
		            <%# Eval("Username") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsItemsPaged" runat="server" EnablePaging="True" SelectCountMethod="SearchServiceItemsPagedCount"
    SelectMethod="SearchServiceItemsPaged" SortParameterName="sortColumn" TypeName="WebsitePanel.Portal.PackagesHelper"
    OnSelected="odsPackagesPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="itemTypeId" QueryStringField="ItemTypeID" Type="int32" />
        <asp:QueryStringParameter Name="filterValue" QueryStringField="Query" />
    </SelectParameters>
</asp:ObjectDataSource>