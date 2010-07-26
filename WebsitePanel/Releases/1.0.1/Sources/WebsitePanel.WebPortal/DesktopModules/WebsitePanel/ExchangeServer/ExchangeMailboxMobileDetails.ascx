<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMobileDetails.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxMobileDetails" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>


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
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>
				<div class="FormBody">
                    <wsp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mobile" />
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                   <asp:Button runat="server" ID="btnWipeAllData"  
                        meta:resourcekey="btnWipeAllData"  CssClass="Button1" 
                        onclick="btnWipeAllData_Click" />
				    <asp:Button runat="server" ID="btnCancel"  meta:resourcekey="btnCancel"  
                        CssClass="Button1" onclick="btnCancel_Click"/>
					<table>					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="loc" meta:resourcekey="locStatus"></asp:Localize></td>
					        <td><asp:Label runat="server" ID="lblStatus" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize1" meta:resourcekey="locDeviceModel"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceModel" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize2" meta:resourcekey="locDeviceType"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceType" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize3" meta:resourcekey="locFirstSyncTime"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblFirstSyncTime" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize4" meta:resourcekey="locDeviceWipeRequestTime"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceWipeRequestTime" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize5" meta:resourcekey="locDeviceAcnowledgeTime"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceAcnowledgeTime" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize6" meta:resourcekey="locLastSync"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblLastSync" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize7" meta:resourcekey="locLastUpdate"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblLastUpdate" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize10" meta:resourcekey="locLastPing"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblLastPing" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize11" meta:resourcekey="locDeviceFriendlyName"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceFriendlyName" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize12" meta:resourcekey="locDeviceId"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceId" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize13" meta:resourcekey="locDeviceUserAgent"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceUserAgent" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize14" meta:resourcekey="locDeviceOS"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceOS" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize15" meta:resourcekey="locDeviceOSLanguage"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceOSLanguage" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize16" meta:resourcekey="locDeviceIMEA"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDeviceIMEA" /></td>
					    </tr>
					    <tr>
					        <td class="FormLabel150"><asp:Localize runat="server" ID="Localize17" meta:resourcekey="locDevicePassword"></asp:Localize> </td>
					        <td><asp:Label runat="server" ID="lblDevicePassword" /></td>
					    </tr>
					</table>
				    <div class="FormFooterClean">
					   <asp:Button runat="server" ID="btnBack" onclick="btnBack_Click"  meta:resourcekey="btnBack"  CssClass="Button1" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>