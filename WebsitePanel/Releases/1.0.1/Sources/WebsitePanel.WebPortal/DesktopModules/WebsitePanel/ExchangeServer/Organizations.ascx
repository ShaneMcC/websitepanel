<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Organizations.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.Organizations" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
        <div class="Left">
            &nbsp;
        </div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="Organization48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Organizations"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    
		            <div style="text-align:right;margin-bottom: 4px;">
						<asp:CheckBox ID="chkRecursive" runat="server" Text="Show Reseller Organizations"
								meta:resourcekey="chkRecursive" AutoPostBack="true" CssClass="Normal" />
                    </div>
				    
				    <div class="FormButtonsBarClean">
						<div class="FormButtonsBarCleanLeft">
							<asp:Button ID="btnCreate" runat="server" meta:resourcekey="btnCreate" CssClass="Button1" OnClick="btnCreate_Click" />
						</div>
                        <div class="FormButtonsBarCleanRight">
                            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                                <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox" style="vertical-align: middle;">
                                    <asp:ListItem Value="ItemName" meta:resourcekey="ddlSearchColumnItemName">OrganizationName</asp:ListItem>
                                    <asp:ListItem Value="Username" meta:resourcekey="ddlSearchColumnUsername">OwnerUsername</asp:ListItem>
                                </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100" style="vertical-align: middle;"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
		                            CausesValidation="false" style="vertical-align: middle;"/>
                            </asp:Panel>
                        </div>
				    </div>
				    
				    <asp:GridView ID="gvOrgs" runat="server" AutoGenerateColumns="False" DataSourceID="odsOrgsPaged"
					    Width="100%" meta:resourcekey="gvOrgs" CssSelectorClass="NormalGridView" OnRowCommand="gvOrgs_RowCommand"
					    AllowPaging="True" AllowSorting="True" EnableViewState="false">
					    <Columns>
							<asp:BoundField meta:resourcekey="gvOrgsID" DataField="OrganizationID" />
						    <asp:TemplateField meta:resourcekey="gvOrgsName" SortExpression="ItemName">
							    <ItemStyle Width="80%"></ItemStyle>
							    <ItemTemplate>
									<div style="padding:7px;">
										<asp:hyperlink id="lnk1" runat="server" EnableViewState="false" CssClass="NormalBold"
											NavigateUrl='<%# GetOrganizationEditUrl(Eval("ItemID").ToString()) %>'>
											<%# Eval("ItemName") %>
										</asp:hyperlink>
								    </div>
							    </ItemTemplate>
						    </asp:TemplateField>
							<asp:TemplateField SortExpression="PackageName" meta:resourcekey="gvOrgsSpace">
								<ItemStyle Wrap="False"></ItemStyle>
								<ItemTemplate>
									<asp:hyperlink id="lnk4" runat="server"
										NavigateUrl='<%# GetSpaceHomePageUrl((int)Eval("PackageID")) %>'>
										<%# Eval("PackageName") %>
									</asp:hyperlink>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField SortExpression="Username" meta:resourcekey="gvOrgsUser">
								<ItemStyle Wrap="False"></ItemStyle>
								<ItemTemplate>
									<asp:hyperlink id="lnk3" runat="server"
										NavigateUrl='<%# GetUserHomePageUrl((int)Eval("UserID")) %>'>
										<%# Eval("Username") %>
									</asp:hyperlink>
								</ItemTemplate>
								<HeaderStyle Wrap="False" />
							</asp:TemplateField>
						    <asp:TemplateField ItemStyle-Width="20px">
							    <ItemTemplate>
								    <asp:ImageButton ID="cmdDelete" runat="server" Text="Delete" SkinID="ExchangeDelete"
									    CommandName="DeleteItem" CommandArgument='<%# Eval("ItemID") %>'
									    meta:resourcekey="cmdDelete" OnClientClick="return confirm('Remove this item?');"></asp:ImageButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
					<asp:ObjectDataSource ID="odsOrgsPaged" runat="server" EnablePaging="True"
							SelectCountMethod="GetOrganizationsPagedCount"
							SelectMethod="GetOrganizationsPaged"
							SortParameterName="sortColumn"
							TypeName="WebsitePanel.Portal.OrganizationsHelper"
							OnSelected="odsOrgsPaged_Selected">
						<SelectParameters>
							<asp:QueryStringParameter Name="packageId" QueryStringField="SpaceID" DefaultValue="-1" />
							<asp:ControlParameter Name="recursive" ControlID="chkRecursive" PropertyName="Checked" DefaultValue="False" />
							<asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
				    
				    <br />
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Organizations Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <wsp:Quota ID="orgsQuota" runat="server" QuotaName="HostedSolution.Organizations" />
				    <br />
				    <br />
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>