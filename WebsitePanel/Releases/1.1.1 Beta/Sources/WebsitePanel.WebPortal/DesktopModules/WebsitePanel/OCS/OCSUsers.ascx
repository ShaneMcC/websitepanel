<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OCSUsers.ascx.cs" Inherits="WebsitePanel.Portal.OCS.OCSUsers" %>
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
            <wsp:Breadcrumb id="breadcrumb" runat="server" meta:resourcekey="breadcrumb" PageName="Text.PageName" />
        </div>
        <div class="Left">
            <wsp:Menu id="menu" runat="server" SelectedItem="storage_usage" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="OCSLogo" runat="server" />
                    <asp:Localize ID="locTitle" meta:resourcekey="locTitle" runat="server" Text=""></asp:Localize>
                </div>
                <div class="FormBody">
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanLeft">
                            <asp:Button ID="btnCreateUser" runat="server" meta:resourcekey="btnCreateUser" Text="Create New User"
                                CssClass="Button1" OnClick="btnCreateUser_Click" />
                        </div>
                        <div class="FormButtonsBarCleanRight">
                            <asp:Panel ID="SearchPanel" runat="server" DefaultButton="cmdSearch">
                                <asp:DropDownList ID="ddlSearchColumn" runat="server" CssClass="NormalTextBox">
                                    <asp:ListItem Value="DisplayName" meta:resourcekey="ddlSearchColumnDisplayName">DisplayName</asp:ListItem>
                                    <asp:ListItem Value="PrimaryEmailAddress" meta:resourcekey="ddlSearchColumnEmail">Email</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtSearchValue" runat="server" CssClass="NormalTextBox" Width="100"></asp:TextBox><asp:ImageButton
                                    ID="cmdSearch" runat="server" meta:resourcekey="cmdSearch" SkinID="SearchButton"
                                    CausesValidation="false" />
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="FormButtonsBarCleanRight">
                        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                            Width="100%" CssSelectorClass="NormalGridView" DataSourceID="odsAccountsPaged"
                            meta:resourcekey="gvUsers" AllowPaging="true" AllowSorting="true" OnRowCommand="gvUsers_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="gvUsersDisplayName" meta:resourcekey="gvUsersDisplayName"
                                    SortExpression="DisplayName">
                                    <ItemStyle Width="50%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Image ID="img1" runat="server" ImageUrl='<%# GetAccountImage() %>' ImageAlign="AbsMiddle" />
                                        <asp:HyperLink ID="lnk1" runat="server" NavigateUrl='<%# GetUserEditUrl(Eval("AccountId").ToString(), Eval("InstanceId").ToString()) %>'> 
									    <%# Eval("DisplayName") %>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="gvUsersEmail" meta:resourcekey="gvUsersEmail" DataField="PrimaryEmailAddress"
                                    SortExpression="PrimaryEmailAddress" ItemStyle-Width="50%" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdDelete" runat="server" Text="Delete" SkinID="ExchangeDelete"
                                            CommandName="DeleteItem" CommandArgument='<%# Eval("InstanceID") %>' meta:resourcekey="cmdDelete"
                                            OnClientClick="return confirm('Remove this item?');"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsAccountsPaged" runat="server" EnablePaging="True" SelectCountMethod="GetOCSUsersPagedCount"
                            SelectMethod="GetOCSUsersPaged" SortParameterName="sortColumn" TypeName="WebsitePanel.Portal.OCSHelper">
                            <SelectParameters>
                                <asp:QueryStringParameter Name="itemId" QueryStringField="ItemID" DefaultValue="0" />
                                <asp:ControlParameter Name="filterColumn" ControlID="ddlSearchColumn" PropertyName="SelectedValue" />
                                <asp:ControlParameter Name="filterValue" ControlID="txtSearchValue" PropertyName="Text" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <br />
                        <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Users Created:"></asp:Localize>
                        &nbsp;&nbsp;&nbsp;
                        <wsp:QuotaViewer ID="usersQuota" runat="server" QuotaTypeId="2" />
                    </div>
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>
