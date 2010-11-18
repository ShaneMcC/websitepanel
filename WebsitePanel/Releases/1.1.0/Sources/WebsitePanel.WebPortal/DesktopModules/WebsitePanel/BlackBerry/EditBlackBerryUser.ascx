<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditBlackBerryUser.ascx.cs" Inherits="WebsitePanel.Portal.BlackBerry.EditBlackBerryUser" %>
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
<%@ Register src="../ExchangeServer/UserControls/MailboxSelector.ascx" tagname="MailboxSelector" tagprefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
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
                    <asp:Image ID="Image1" SkinID="BlackBerryUsersLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="FormBody">
                    
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />

                    <wsp:CollapsiblePanel id="secPassword" runat="server"
                        TargetControlID="pnlSetPassword" meta:resourcekey="secPassowrd">
                    </wsp:CollapsiblePanel>
                    
                    <asp:Panel runat="server" ID="pnlSetPassword">
                    <asp:UpdatePanel runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td><asp:RadioButton runat="server" ID="rbSpecifyPassword" OnCheckedChanged="rbSpecifyPassword_OnCheckedChanged" Checked="true" AutoPostBack="true" meta:resourcekey="rbSpecifyPassword" GroupName="Password"/></td>
                        </tr>
                        <tr>
                            <td><asp:RadioButton OnCheckedChanged="rbGeneratePassword_OnCheckedChanged"  AutoPostBack="true" runat="server" ID="rbGeneratePassword" meta:resourcekey="rbGeneratePassword" GroupName="Password" /></td>
                        </tr>
                    </table>
                    
                    <table runat="server" id="tblPassword" visible="true">
                        <tr>
                            <td class="FormLabel150"><asp:Localize runat="server" ID="locPassword" meta:resourcekey="locPassword"></asp:Localize></td>
                            <td><asp:TextBox runat="server" ID="txtPassword" CssClass="TextBox200" TextMode="Password" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator> 
                            </td>
                        </tr>
                        <tr>
                            <td  class="FormLabel150"><asp:Localize runat="server" ID="locTime" meta:resourcekey="locTime"/></td>
                            <td><asp:TextBox runat="server" ID="txtTime" CssClass="TextBox200" Text="48" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtTime" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RangeValidator runa="server" ControlToValidate="txtTime"  ErrorMessage="*" Type="Integer" MinimumValue="1" MaximumValue="720" />
                                
                            </td>
                        </tr>
                    </table>                        
                        </ContentTemplate>
                    </asp:UpdatePanel>                    
                    <br />
                    <asp:Button  runat="server" ID="btnSetPassword" meta:resourcekey="btnSetPassword" 
                            CssClass="Button1" onclick="btnSetPassword_Click"/>                    
                    </asp:Panel>
                    <br />                    
                   
                    
                    
                    <asp:GridView runat="server" ID="dvStats" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%"  CssSelectorClass="NormalGridView" ShowHeader="true" ShowFooter="false">
                        <Columns>
                            <asp:BoundField DataField="Name" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Value" />
                        </Columns>
                    </asp:GridView>
                        
					<div class="FormFooterClean">
					 <asp:Button runat="server" ID="btnDelete" meta:resourcekey="btnDelete"  
                        CssClass="Button1" onclick="btnDelete_Click"  />
                        
                        <asp:Button runat="server" ID="btnDeleteData" meta:resourcekey="btnDeleteData"  
                        CssClass="Button1" onclick="btnDeleteData_Click"  />
				    </div>			
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>