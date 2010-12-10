<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMUsers.ascx.cs" Inherits="WebsitePanel.Portal.CRM.CRMUsers" %>
<%@ Register Src="../ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector"
    TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
    TagPrefix="wsp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="wsp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Header">
            <wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
        </div>
        <div class="Left">
            <wsp:Menu id="menu" runat="server" SelectedItem="storage_usage" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="CRMLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" Text="CRM Users"></asp:Localize>
                </div>
                <div class="FormBody">
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanLeft">
                            <asp:Button ID="btnCreateUser" runat="server" meta:resourcekey="btnCreateUser"
                            Text="Create New User" CssClass="Button1" OnClick="btnCreateUser_Click" />
                        </div>
                        <div class="FormButtonsBarCleanRight">
                            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                                <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                                    <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                                    <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
                                </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
		                            CausesValidation="false"/>
                            </asp:Panel>
                        </div>
                    </div>

				    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvUsers"  meta:resourcekey="gvUsers" CssSelectorClass="NormalGridView"
					     AllowPaging="True" AllowSorting="True" DataSourceID="odsAccountsPaged">
					    <Columns>						     						   						    
						    <asp:TemplateField HeaderText="gvUsersDisplayName" SortExpression="DisplayName">
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>							        
								    <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
								    <asp:hyperlink id="lnk1" runat="server"
									    NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString()) %>'>
									    <%# Eval("DisplayName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:BoundField HeaderText="gvUsersEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="50%" />						   
					    </Columns>
				    </asp:GridView>
					<asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True"
							
							SelectCountMethod="GetCRMUsersPagedCount"
							SelectMethod="GetCRMUsersPaged"
							SortParameterName="sortColumn"
							TypeName="WebsitePanel.Portal.CrmHelper"
							OnSelected="odsAccountsPaged_Selected">
						<SelectParameters>
							<asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />							
							<asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
				    <br />
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Users Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <wsp:QuotaViewer ID="usersQuota" runat="server" QuotaTypeId="2"   />				    				    
				</div>                                                
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>
