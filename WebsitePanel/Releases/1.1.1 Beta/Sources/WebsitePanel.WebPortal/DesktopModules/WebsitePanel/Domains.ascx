<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Domains.ascx.cs" Inherits="WebsitePanel.Portal.Domains" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="wsp" %>
<%@ Register Src="UserControls/UserDetails.ascx" TagName="UserDetails" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SearchBox.ascx" TagName="SearchBox" TagPrefix="wsp" %>





<div class="FormButtonsBar">
    <div class="Left">
        <asp:Button ID="btnAddDomain" runat="server" meta:resourcekey="btnAddDomain" Text="Add Domain" CssClass="Button2" OnClick="btnAddDomain_Click" />
        &nbsp;<asp:CheckBox ID="chkRecursive" runat="server" Text="Show Nested Spaces Items" meta:resourcekey="chkRecursive"
			AutoPostBack="true" Checked="True" CssClass="Normal" />

    </div>
    <div class="Right">
        <wsp:SearchBox ID="searchBox" runat="server" />
    </div>
</div>

<asp:GridView ID="gvDomains" runat="server" AutoGenerateColumns="False" Width="100%" AllowSorting="True" DataSourceID="odsDomainsPaged"
    EmptyDataText="gvDomains"
    CssSelectorClass="NormalGridView" AllowPaging="True" OnRowCommand="gvDomains_RowCommand">
    <Columns>
        <asp:TemplateField SortExpression="DomainName" HeaderText="gvDomainsName">
            <ItemStyle Width="60%" Wrap="False"></ItemStyle>
            <ItemTemplate>
	            <b><asp:hyperlink id=lnkEdit1 runat="server" CssClass="Medium"
	                NavigateUrl='<%# GetItemEditUrl(Eval("PackageID"), Eval("DomainID")) %>'>
		            <%# Eval("DomainName")%></asp:hyperlink>
	            </b>
	            <div runat="server" class="Small" style="margin-top:2px;" visible=' <%# Eval("WebSiteName") != DBNull.Value %>'>
                    <asp:Label ID="lblWebSite" runat="server" meta:resourcekey="lblWebSite" Text="Web:"></asp:Label>
                    <b><%# Eval("WebSiteName")%></b>
	            </div>
	            <div runat="server" class="Small" style="margin-top:2px;" visible=' <%# Eval("MailDomainName") != DBNull.Value %>'>
                    <asp:Label ID="lblMailDomain" runat="server" meta:resourcekey="lblMailDomain" Text="Mail:"></asp:Label>
                    <b><%# Eval("MailDomainName")%></b>
	            </div>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="gvDomainsType">
            <ItemStyle Width="30%"></ItemStyle>
            <ItemTemplate>
	            <%# GetDomainTypeName((bool)Eval("IsSubDomain"), (bool)Eval("IsInstantAlias"), (bool)Eval("IsDomainPointer"))%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="PackageName" HeaderText="gvDomainsSpace">
            <ItemStyle Width="30%"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id="lnkEdit2" runat="server" EnableViewState="false"
	                NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		            <%# Eval("PackageName") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField SortExpression="Username" HeaderText="gvDomainsUser">
			<ItemStyle Wrap="False" />
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit3 runat="server" EnableViewState="false"
				    NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
            <HeaderStyle Wrap="False" />
		</asp:TemplateField>
        <asp:TemplateField SortExpression="ServerName" HeaderText="gvDomainsServer">
			<ItemStyle Wrap="False" />
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit4 runat="server" EnableViewState="false"
				    NavigateUrl='<%# GetItemsPageUrl("ServerID", Eval("ServerID").ToString()) %>'>
					<%# Eval("ServerName") %>
				</asp:hyperlink>
			</ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
			<ItemTemplate>
				<asp:LinkButton ID="cmdDetach" runat="server" Text="Detach"
					CommandName="Detach" CommandArgument='<%# Eval("DomainID") %>'
					meta:resourcekey="cmdDetach" OnClientClick="return confirm('Remove this item?');"></asp:LinkButton>
			</ItemTemplate>
        </asp:TemplateField>
    </Columns>
	<PagerSettings Mode="NumericFirstLast" />
</asp:GridView>
<asp:ObjectDataSource ID="odsDomainsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetDomainsPagedCount"
    SelectMethod="GetDomainsPaged" SortParameterName="sortColumn" TypeName="WebsitePanel.Portal.ServersHelper" OnSelected="odsDomainsPaged_Selected">
    <SelectParameters>
        <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
        <asp:QueryStringParameter Name="serverId" QueryStringField="ServerID" DefaultValue="0" Type="Int32" />
        <asp:ControlParameter Name="recursive" ControlID="chkRecursive" PropertyName="Checked" DefaultValue="False" />
        <asp:ControlParameter Name="filterColumn" ControlID="searchBox" PropertyName="FilterColumn" />
        <asp:ControlParameter Name="filterValue" ControlID="searchBox" PropertyName="FilterValue" />
    </SelectParameters>
</asp:ObjectDataSource>

	
<div class="GridFooter">
    <table>
        <tr>
            <td><asp:Label ID="lblDomains" runat="server" meta:resourcekey="lblDomains" Text="Domains:" CssClass="NormalBold"></asp:Label></td>
            <td><wsp:Quota ID="quotaDomains" runat="server" QuotaName="OS.Domains" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblSubDomains" runat="server" meta:resourcekey="lblSubDomains" Text="Sub-Domains:" CssClass="NormalBold"></asp:Label></td>
            <td><wsp:Quota ID="quotaSubDomains" runat="server" QuotaName="OS.SubDomains" /></td>
        </tr>
        <tr>
            <td><asp:Label ID="lblDomainPointers" runat="server" meta:resourcekey="lblDomainPointers" Text="Domain Aliases:" CssClass="NormalBold"></asp:Label></td>
            <td><wsp:Quota ID="quotaDomainPointers" runat="server" QuotaName="OS.DomainPointers" /></td>
        </tr>
    </table>
</div>

