<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDistributionListPermissions.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeDistributionListPermissions" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/DistributionListTabs.ascx" TagName="DistributionListTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/AcceptedSenders.ascx" TagName="AcceptedSenders" TagPrefix="wsp" %>
<%@ Register Src="UserControls/RejectedSenders.ascx" TagName="RejectedSenders" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<%@ Register src="UserControls/AccountsList.ascx" tagname="AccountsList" tagprefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="dlists" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeList48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Distribution List"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>
				<div class="FormBody">
                    <wsp:DistributionListTabs id="tabs" runat="server" SelectedTab="dlist_permissions" />					
					<wsp:SimpleMessageBox id="messageBox" runat="server" />
					
					<wsp:CollapsiblePanel id="secSendOnBehalf" runat="server"
                        TargetControlID="SendOnBehalf" meta:resourcekey="secSendOnBehalf" >
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="SendOnBehalf" runat="server" Height="0" style="overflow:hidden;">
					    <asp:Label runat="server" ID="lblGrandPermissions" meta:resourcekey="lblGrandPermissions" /><br /><br />
					    <wsp:AccountsList id="sendBehalfList" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true" ></wsp:AccountsList>
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secSendAs" runat="server"
                        TargetControlID="SendAs" meta:resourcekey="secSendAs" >
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="SendAs" runat="server" Height="0" style="overflow:hidden;">
					    <asp:Label runat="server" ID="Label1" meta:resourcekey="lblGrandPermissions" /><br /><br />
					    <wsp:AccountsList id="sendAsList" runat="server"
					MailboxesEnabled="true" EnableMailboxOnly = "true" ></wsp:AccountsList>
					</asp:Panel>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" 
                            meta:resourcekey="btnSave" ValidationGroup="EditMailbox" 
                            onclick="btnSave_Click"></asp:Button>
				        
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>