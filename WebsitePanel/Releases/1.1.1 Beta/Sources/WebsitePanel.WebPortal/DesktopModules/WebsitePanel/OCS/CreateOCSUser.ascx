<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateOCSUser.ascx.cs" Inherits="WebsitePanel.Portal.OCS.CreateOCSUser" %>
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
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<div id="ExchangeContainer">
    <div class="Module">
        <div class="Header">
            <wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" meta:resourcekey="breadcrumb" />
        </div>
        <div class="Left">
            <wsp:Menu id="menu" runat="server" />
        </div>
        <div class="Content">
            <div class="Center">
                <div class="Title">
                    <asp:Image ID="Image1" SkinID="OCSLogo" runat="server" />
                    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle"></asp:Localize>
                </div>
                <div class="FormBody">
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    <table id="ExistingUserTable"   runat="server" width="100%"> 					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
					        <td>                                
                                <wsp:UserSelector ID="userSelector" runat="server" IncludeMailboxes="true" />
                            </td>
					    </tr>
					    	    					    					    
					</table>
					
					<div class="FormFooterClean">
					    <asp:Button id="btnCreate" runat="server" 
					    CssClass="Button1" meta:resourcekey="btnCreate" 
					     onclick="btnCreate_Click" ></asp:Button>					    
				    </div>			
                </div>
            </div>
            <div class="Right">
                <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
            </div>
        </div>
    </div>
</div>

