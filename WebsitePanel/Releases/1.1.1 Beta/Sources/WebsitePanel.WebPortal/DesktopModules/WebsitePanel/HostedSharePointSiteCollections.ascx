<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointSiteCollections.ascx.cs"
	Inherits="WebsitePanel.Portal.HostedSharePointSiteCollections" %>
<%@ Register Src="UserControls/SpaceServiceItems.ascx" TagName="SpaceServiceItems"
	TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
	TagPrefix="wsp" %>
<%@ Register Src="UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
	
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

<script type="text/javascript">

function confirmation() 
{
	if (!confirm("Are you sure you want to delete Site Collection?")) return false; else ShowProgressDialog('Deleting SharePoint site collection...');	
}
</script>
	
<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="sharepoint_sitecollections" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="SharePointSiteCollection48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="SharePoint Site Collections"></asp:Localize>
				</div>
				<div class="FormBody">
					<wsp:SimpleMessageBox id="messageBox" runat="server" />
					<div class="FormButtonsBarClean">
						<div class="FormButtonsBarCleanLeft">
							<asp:Button ID="btnCreateSiteCollection" runat="server" meta:resourcekey="btnCreateSiteCollection"
								Text="Create New Site Collection" CssClass="Button1" OnClick="btnCreateSiteCollection_Click" />
						</div>
						<div class="FormButtonsBarCleanRight">
							<asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
								<asp:Localize ID="locSearch" runat="server" meta:resourcekey="locSearch" Visible="false"></asp:Localize>
								<asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
									<asp:ListItem Value="ItemName" meta:resourcekey="ddlSearchColumnUrl">Url</asp:ListItem>
								</asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox"
									Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" runat="server" meta:resourcekey="cmdSearch"
										SkinID="SearchButton" CausesValidation="false" />
							</asp:Panel>
						</div>
					</div>
					<asp:GridView ID="gvSiteCollections" runat="server" AutoGenerateColumns="False" EnableViewState="true"
						Width="100%" EmptyDataText="gvSiteCollection" CssSelectorClass="NormalGridView" OnRowCommand="gvSiteCollections_RowCommand"
						AllowPaging="True" AllowSorting="True" DataSourceID="odsSiteCollectionsPaged">
						<Columns>
							<asp:TemplateField meta:resourcekey="gvSiteCollectionUrl" SortExpression="ItemName">
								<ItemStyle Width="50%"></ItemStyle>
								<ItemTemplate>
									<asp:HyperLink ID="lnk1" runat="server" NavigateUrl='<%# GetSiteCollectionEditUrl(Eval("Id").ToString()) %>'>
									    <%# Eval("PhysicalAddress") %>
									</asp:HyperLink>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField meta:resourcekey="gvOwnerDisplayName" DataField="OwnerName"	ItemStyle-Width="50%" />
							<asp:TemplateField>
								<ItemTemplate>
									<asp:ImageButton ID="cmdDelete" runat="server" Text="Delete" SkinID="ExchangeDelete"
										CommandName="DeleteItem" CommandArgument='<%# Eval("Id") %>' 
										OnClientClick="confirmation()"></asp:ImageButton>
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
					<asp:ObjectDataSource ID="odsSiteCollectionsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetSharePointSiteCollectionPagedCount"
						SelectMethod="GetSharePointSiteCollectionPaged" SortParameterName="sortColumn" TypeName="WebsitePanel.Portal.HostedSharePointSiteCollectionsHelper"
						OnSelected="odsSharePointSiteCollectionPaged_Selected">
						<SelectParameters>
					        <asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
							<asp:QueryStringParameter Name="organizationId" QueryStringField="ItemID" DefaultValue="0" />
							<asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
					<br />
					<asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Site Collections Created:"></asp:Localize>
					&nbsp;&nbsp;&nbsp;
					<%--<wsp:Quota ID="siteCollectionsQuota1" runat="server" QuotaName="HostedSharePoint.Sites" />--%>
					<wsp:QuotaViewer ID="siteCollectionsQuota" runat="server" QuotaTypeId="2" />
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
