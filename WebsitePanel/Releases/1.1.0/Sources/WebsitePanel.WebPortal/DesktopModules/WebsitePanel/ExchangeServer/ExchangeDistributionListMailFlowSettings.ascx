<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDistributionListMailFlowSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeDistributionListMailFlowSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/DistributionListTabs.ascx" TagName="DistributionListTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/AcceptedSenders.ascx" TagName="AcceptedSenders" TagPrefix="wsp" %>
<%@ Register Src="UserControls/RejectedSenders.ascx" TagName="RejectedSenders" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

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
                    <wsp:DistributionListTabs id="tabs" runat="server" SelectedTab="dlist_mailflow" />					
					<wsp:SimpleMessageBox id="messageBox" runat="server" />
					
					<wsp:CollapsiblePanel id="secAcceptMessagesFrom" runat="server"
                        TargetControlID="AcceptMessagesFrom" meta:resourcekey="secAcceptMessagesFrom" Text="Accept Messages From">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="AcceptMessagesFrom" runat="server" Height="0" style="overflow:hidden;">
					    <wsp:AcceptedSenders id="acceptAccounts" runat="server" />
					    <asp:CheckBox ID="chkSendersAuthenticated" runat="server" meta:resourcekey="chkSendersAuthenticated" Text="Require that all senders are authenticated" />
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secRejectMessagesFrom" runat="server"
                        TargetControlID="RejectMessagesFrom" meta:resourcekey="secRejectMessagesFrom" Text="Reject Messages From">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="RejectMessagesFrom" runat="server" Height="0" style="overflow:hidden;">
					    <wsp:RejectedSenders id="rejectAccounts" runat="server" />
					</asp:Panel>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave" ValidationGroup="EditMailbox" OnClick="btnSave_Click"></asp:Button>
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>