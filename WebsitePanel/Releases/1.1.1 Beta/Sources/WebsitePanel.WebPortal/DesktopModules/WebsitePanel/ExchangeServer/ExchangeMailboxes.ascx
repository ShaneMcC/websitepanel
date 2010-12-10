<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxes.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxes" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="mailboxes" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeMailbox48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Mailboxes"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanLeft">
                            <asp:Button ID="btnCreateMailbox" runat="server" meta:resourcekey="btnCreateMailbox"
                            Text="Create New Mailbox" CssClass="Button1" OnClick="btnCreateMailbox_Click" />
                        </div>
                        <div class="FormButtonsBarCleanRight">
                            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                                <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                                    <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                                    <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
                                    <asp:ListItem Value="AccountName" meta:resourcekey="ddlSearchColumnAccountName">AccountName</asp:ListItem>
                                    
                                </asp:DropDownList><asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton ID="cmdSearch" Runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
		                            CausesValidation="false"/>
                            </asp:Panel>
                        </div>
                    </div>

				    <asp:GridView ID="gvMailboxes" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvMailboxes" CssSelectorClass="NormalGridView"
					    OnRowCommand="gvMailboxes_RowCommand" AllowPaging="True" AllowSorting="True"
					    DataSourceID="odsAccountsPaged">
					    <Columns>
						    <asp:TemplateField HeaderText="gvMailboxesDisplayName" SortExpression="DisplayName">
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>
							        <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage((int)Eval("AccountType")) %>' ImageAlign="AbsMiddle" />
								    <asp:hyperlink id="lnk1" runat="server"
									    NavigateUrl='<%# GetMailboxEditUrl(Eval("AccountId").ToString()) %>'>
									    <%# Eval("DisplayName") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:BoundField HeaderText="gvMailboxesEmail" DataField="PrimaryEmailAddress" SortExpression="PrimaryEmailAddress" ItemStyle-Width="50%" />
						    <asp:TemplateField>
							    <ItemTemplate>
								    <asp:ImageButton ID="cmdDelete" runat="server" Text="Delete" SkinID="ExchangeDelete"
									    CommandName="DeleteItem" CommandArgument='<%# Eval("AccountId") %>'
									    meta:resourcekey="cmdDelete" OnClientClick="return confirm('Remove this item?');"></asp:ImageButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
					<asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True"
							SelectCountMethod="GetExchangeAccountsPagedCount"
							SelectMethod="GetExchangeAccountsPaged"
							SortParameterName="sortColumn"
							TypeName="WebsitePanel.Portal.ExchangeHelper"
							OnSelected="odsAccountsPaged_Selected">
						<SelectParameters>
							<asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
							<asp:Parameter Name="accountTypes" DefaultValue="1,5,6" />
							<asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
							<asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
						</SelectParameters>
					</asp:ObjectDataSource>
				    <br />
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Mailboxes Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <wsp:QuotaViewer ID="mailboxesQuota" runat="server" QuotaTypeId="2" />
				    
				    
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>