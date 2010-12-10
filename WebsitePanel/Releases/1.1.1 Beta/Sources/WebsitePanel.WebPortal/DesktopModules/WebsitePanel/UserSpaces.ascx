<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserSpaces.ascx.cs" Inherits="WebsitePanel.Portal.UserSpaces" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="uc3" %>
<%@ Register Src="UserControls/Comments.ascx" TagName="Comments" TagPrefix="uc4" %>


<asp:Panel id="ButtonsPanel" runat="server" class="FormButtonsBar">
	<asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Create Hosting Space" CssClass="Button3" OnClick="btnAddItem_Click" />
</asp:Panel>



<asp:Panel ID="UserPackagesPanel" runat="server" Visible="false">
    <asp:Repeater ID="PackagesList" runat="server" EnableViewState="false">
        <ItemTemplate>
            <div class="IconsBlock">
                <div class="IconsTitle">
                    <asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		                <%# Eval("PackageName") %>
	                </asp:hyperlink>
                </div>
                <div>
                    <asp:DataList ID="PackageIcons" runat="server" DataSource='<%# GetIconsDataSource((int)Eval("PackageID")) %>'
                        CellSpacing="1" RepeatColumns="5" RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <asp:Panel ID="IconPanel" runat="server" CssClass="Icon">
                                <asp:HyperLink ID="imgLink" runat="server" NavigateUrl='<%# Eval("NavigateURL") %>'><asp:Image ID="imgIcon" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' /></asp:HyperLink>
                                <br />
                                <asp:HyperLink ID="lnkIcon" runat="server" NavigateUrl='<%# Eval("NavigateURL") %>'><%# Eval("Text") %></asp:HyperLink>
                            </asp:Panel>
                            <asp:Panel ID="IconMenu" runat="server" CssClass="IconMenu" Visible='<%# IsIconMenuVisible(Eval("ChildItems")) %>'>
                                <ul>
                                    <asp:Repeater ID="MenuItems" runat="server" DataSource='<%# GetIconMenuItems(Eval("ChildItems")) %>'>
                                        <ItemTemplate>
                                            <li><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("NavigateURL") %>'><%# Eval("Text") %></asp:HyperLink></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </asp:Panel>
                            <ajaxToolkit:HoverMenuExtender TargetControlID="IconPanel" PopupControlID="IconMenu" runat="server"
                                PopupPosition="Right" HoverCssClass="Icon Hover"></ajaxToolkit:HoverMenuExtender>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <asp:Panel ID="EmptyPackagesList" runat="server" Visible="false" CssClass="FormBody">
        <asp:Literal ID="litEmptyList" runat="server" EnableViewState="false"></asp:Literal>
    </asp:Panel>
</asp:Panel>



<asp:UpdatePanel runat="server" ID="ResellerPackagesPanel" Visible="false">
    <ContentTemplate>
<asp:GridView ID="gvPackages" runat="server" AutoGenerateColumns="False"
    EmptyDataText="gvPackages" CssSelectorClass="NormalGridView"
    AllowSorting="True" DataSourceID="odsPackages">
    <Columns>
        <asp:TemplateField SortExpression="PackageName" HeaderText="gvPackagesName">
            <ItemStyle Width="40%"></ItemStyle>
            <ItemTemplate>
	            <asp:hyperlink id=lnkEdit runat="server" CssClass="Medium" NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
		            <%# Eval("PackageName") %>
	            </asp:hyperlink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField SortExpression="ServerName" HeaderText="gvPackagesServer">
            <ItemStyle Width="30%"></ItemStyle>
            <ItemTemplate>
		         <uc3:ServerDetails ID="serverDetails" runat="server"
		            ServerID='<%# Eval("ServerID") %>'
		            ServerName='<%# Eval("ServerName") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField SortExpression="PurchaseDate" DataField="PurchaseDate" HeaderText="gvPackagesCreationDate" DataFormatString="{0:d}" >
            <ItemStyle Wrap="False" />
            <HeaderStyle Wrap="False" />
        </asp:BoundField>
        <asp:TemplateField SortExpression="StatusID" HeaderText="gvPackagesStatus">
            <ItemTemplate>
		         <%# PanelFormatter.GetPackageStatusName((int)Eval("StatusID"))%>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:TemplateField ItemStyle-Width="20px" ItemStyle-Wrap="false">
			<ItemTemplate><uc4:Comments id="Comments1" runat="server"
				    Comments='<%# Eval("Comments") %>'>
                </uc4:Comments></ItemTemplate>
		</asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsPackages" runat="server" SelectMethod="GetMyPackages"
    TypeName="WebsitePanel.Portal.PackagesHelper" OnSelected="odsPackages_Selected"></asp:ObjectDataSource>
    </ContentTemplate>
</asp:UpdatePanel>
