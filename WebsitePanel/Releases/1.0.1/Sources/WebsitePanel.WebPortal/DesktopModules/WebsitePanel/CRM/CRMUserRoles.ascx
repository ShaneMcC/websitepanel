<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CRMUserRoles.ascx.cs"
    Inherits="WebsitePanel.Portal.CRM.CRMUserRoles" %>
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
            <wsp:Menu id="menu" runat="server" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="CRMLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="FormBody">
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    <div class="FormButtonsBarClean">
                        <div class="FormButtonsBarCleanLeft">
                            <table>
                                <tr height="23">
                                    <td class="FormLabel150"><asp:Localize runat="server" ID="locDisplayName" meta:resourcekey="locDisplayName" /></td>
                                    <td class="FormLabel150"><asp:Label runat="server" ID="lblDisplayName" /></td>
                                </tr>
                                <tr height="23">
                                    <td class="FormLabel150"><asp:Localize runat="server" ID="locEmailAddress" meta:resourcekey="locEmailAddress"/></td>
                                    <td class="FormLabel150"><asp:Label runat="server" ID="lblEmailAddress" /></td>
                                </tr>
                                <tr height="23">
                                    <td class="FormLabel150"><asp:Localize runat="server" ID="locDomainName" meta:resourcekey="locDomainName"/></td>
                                    <td class="FormLabel150"><asp:Label runat="server" ID="lblDomainName" /></td>
                                </tr>
                                <tr>
                                    <td><asp:Localize runat="server" ID="locState" meta:resourcekey="locState" /></td>
                                    <td><asp:Localize runat="server" ID="locEnabled" meta:resourcekey="locEnabled" /><asp:Localize runat="server" ID="locDisabled" meta:resourcekey="locDisabled" />&nbsp;
                                        <asp:Button runat="server" Text="Activate" CssClass="Button1" ID="btnActive" 
                                            onclick="btnActive_Click" meta:resourcekey="btnActivate" /><asp:Button  CssClass="Button1" runat="server" 
                                            Text="Deactivate" ID="btnDeactivate" onclick="btnDeactivate_Click"  meta:resourcekey="btnDeactivate"/></td>
                                </tr>
                            </table>
                            <br />
                        </div>
                        
                        <div class="FormButtonsBarCleanRight">
                            
                            <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                                Width="100%"  CssSelectorClass="NormalGridView" 
                                AllowPaging="False" AllowSorting="False" DataKeyNames="RoleID" >
                                <Columns>
                                    <asp:TemplateField >
                                        <ItemStyle  HorizontalAlign="Center" ></ItemStyle>
                                        <ItemTemplate >
                                            <asp:CheckBox runat="server" ID="cbSelected" Checked=<%# Eval("IsCurrentUserRole") %> />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="gvRole" DataField="RoleName" 
                                        ItemStyle-Width="100%" />
                                    
                                </Columns>
                            </asp:GridView>
                        </div>
                        <br />
                        <asp:Button runat="server" ID="btnUpdate" Text="Update" meta:resourcekey="btnUpdate"   CssClass="Button1"  onclick="btnUpdate_Click" />
                    </div>
                    <br />
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>
