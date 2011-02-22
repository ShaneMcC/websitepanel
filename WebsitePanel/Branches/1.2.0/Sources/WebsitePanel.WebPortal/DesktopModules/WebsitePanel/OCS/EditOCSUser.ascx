<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditOCSUser.ascx.cs" Inherits="WebsitePanel.Portal.OCS.EditOCSUser" %>
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
            <wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName"  meta:resourcekey="breadcrumb" />
        </div>
        <div class="Left">
            <wsp:Menu id="menu" runat="server" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="OCSLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                    -
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>
                <div class="FormBody">
                    
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <table>
                        <tr>
                            <td>
                                <asp:Localize runat="server" ID="locDisplayName" meta:resourcekey="locDisplayName" />
                            </td>
                            <td class="Huge">
                                <asp:Label runat="server" ID="lblDisplayName" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Localize runat="server" ID="locEmail" meta:resourcekey="locEmail"/>
                            </td>
                            <td class="Huge">
                                <asp:Label runat="server" ID="lblSIP" />
                            </td>
                        </tr>
                    </table>
                    
                    <wsp:CollapsiblePanel id="secFedaration" runat="server"
                        TargetControlID="pnlFedaration" meta:resourcekey="secFedaration" Text="Company Information"/>
                    
                    <asp:Panel runat="server" ID="pnlFedaration" >
                        <asp:CheckBox runat="server" ID="cbEnableFederation" meta:resourcekey="cbEnableFederation"/><br/>
                        <asp:CheckBox runat="server" ID="cbEnablePublicConnectivity" meta:resourcekey="cbEnablePublicConnectivity"/>                        
                    
                    </asp:Panel>
                    <br />
                    
                    <wsp:CollapsiblePanel id="secArchiving" runat="server"
                        TargetControlID="pnlArchiving" meta:resourcekey="secArchiving" Text="Company Information"/>
                    
                    <asp:Panel runat="server" ID="pnlArchiving" >
                        <asp:CheckBox runat="server" ID="cbArchiveInternal" meta:resourcekey="cbArchiveInternal"/><br/>
                        <asp:CheckBox runat="server" ID="cbArchiveFederation" meta:resourcekey="cbArchiveFederation"/>                                            
                    </asp:Panel>
                    <br />
                    
                    <wsp:CollapsiblePanel id="secPresence" runat="server"
                        TargetControlID="pnlPresence" meta:resourcekey="secPresence" Text="Company Information"/>
                    
                    <asp:Panel runat="server" ID="pnlPresence" >
                        <asp:CheckBox runat="server" ID="cbEnablePresence" meta:resourcekey="cbEnablePresence"/><br/>
                        <asp:Localize runat="server" ID="locNote" meta:resourcekey="locNote"/>                        
                    </asp:Panel>
                    
                    
                        
					<div class="FormFooterClean">
					 <asp:Button runat="server" ID="btnSave" meta:resourcekey="btnSave"  
                        CssClass="Button1" onclick="btnSave_Click"  />					 					                                                
				    </div>			
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>
