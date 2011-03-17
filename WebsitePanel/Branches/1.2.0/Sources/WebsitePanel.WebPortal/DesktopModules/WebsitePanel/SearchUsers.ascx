<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchUsers.ascx.cs" Inherits="WebsitePanel.Portal.SearchUsers" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="uc1" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="uc2" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>

<div class="FormButtonsBar">
    <asp:Panel ID="tblSearch" runat="server" DefaultButton="cmdSearch" CssClass="NormalBold">
    <asp:Label ID="lblSearch" runat="server" meta:resourcekey="lblSearch"></asp:Label>
        <asp:DropDownList ID="ddlFilterColumn" runat="server" CssClass="NormalTextBox" resourcekey="ddlFilterColumn">
            <asp:ListItem Value="Username">Username</asp:ListItem>
            <asp:ListItem Value="Email">Email</asp:ListItem>
            <asp:ListItem Value="FullName">FullName</asp:ListItem>
            <asp:ListItem Value="CompanyName">CompanyName</asp:ListItem>
        </asp:DropDownList><asp:TextBox ID="txtFilterValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" SkinID="SearchButton" meta:resourcekey="cmdSearch"
			CausesValidation="false" OnClick="cmdSearch_Click" />
    </asp:Panel>
</div>

<asp:GridView id="gvUsers" runat="server" AutoGenerateColumns="False"
	AllowPaging="True" AllowSorting="True"
	CssSelectorClass="NormalGridView"
	DataSourceID="odsUsersPaged" EnableViewState="False"
	EmptyDataText="gvUsers">
	<Columns>
		<asp:TemplateField SortExpression="Username" HeaderText="gvUsersUsername" HeaderStyle-Wrap="false">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server"
				    NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="FullName" HtmlEncode="false" SortExpression="FullName" HeaderText="gvUsersName">
		    <HeaderStyle Wrap="false" />
        </asp:BoundField>
		<asp:BoundField DataField="Email" SortExpression="Email" HeaderText="gvUsersEmail"></asp:BoundField>
		<asp:TemplateField SortExpression="RoleID" HeaderText="gvUsersRole">
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<%# PanelFormatter.GetUserRoleName((int)Eval("RoleID"))%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="CompanyName" SortExpression="CompanyName" HeaderText="gvUsersCompanyName">
		    <HeaderStyle Wrap="false" />
        </asp:BoundField>
		<asp:TemplateField SortExpression="OwnerUsername" HeaderText="gvUsersReseller" HeaderStyle-Wrap="false">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# GetUserHomePageUrl((int)Eval("OwnerID")) %>'>
					<%# Eval("OwnerUsername") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="PackagesNumber" SortExpression="PackagesNumber" HeaderText="gvUsersSpaces"></asp:BoundField>
		<asp:TemplateField SortExpression="StatusID" HeaderText="gvUsersStatus">
			<ItemStyle Wrap="False"></ItemStyle>
			<ItemTemplate>
				<%# PanelFormatter.GetAccountStatusName((int)Eval("StatusID"))%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField ItemStyle-Width="20px" ItemStyle-Wrap="false">
			<ItemTemplate><uc4:Comments id="Comments1" runat="server"
				    Comments='<%# Eval("Comments") %>'>
                </uc4:Comments></ItemTemplate>
		</asp:TemplateField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsUsersPaged" runat="server" EnablePaging="True" SelectCountMethod="GetUsersPagedRecursiveCount"
    SelectMethod="GetUsersPagedRecursive" SortParameterName="sortColumn" TypeName="WebsitePanel.Portal.UsersHelper" OnSelected="odsUsersPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="filterColumn" QueryStringField="Criteria" />
        <asp:QueryStringParameter Name="filterValue" QueryStringField="Query" />
    </SelectParameters>
</asp:ObjectDataSource>
