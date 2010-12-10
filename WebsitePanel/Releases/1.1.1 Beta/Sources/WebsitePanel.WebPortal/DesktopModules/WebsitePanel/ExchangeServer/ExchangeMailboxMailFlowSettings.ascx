<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMailFlowSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxMailFlowSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/AcceptedSenders.ascx" TagName="AcceptedSenders" TagPrefix="wsp" %>
<%@ Register Src="UserControls/RejectedSenders.ascx" TagName="RejectedSenders" TagPrefix="wsp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
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
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Mailbox"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>
				<div class="FormBody">
                    <wsp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mailflow" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<wsp:CollapsiblePanel id="secForwarding" runat="server"
                        TargetControlID="Forwarding" meta:resourcekey="secForwarding" Text="Forwarding Address">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="Forwarding" runat="server" Height="0" style="overflow:hidden;">
						<asp:UpdatePanel ID="ForwardingUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
							
					    <table>
							<tr>
								<td colspan="2">
									<asp:CheckBox ID="chkEnabledForwarding" runat="server" meta:resourcekey="chkEnabledForwarding" Text="Enable Forwarding" AutoPostBack="true" OnCheckedChanged="chkEnabledForwarding_CheckedChanged" />
								</td>
							</tr>
						</table>
						<table id="ForwardSettingsPanel" runat="server">
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locForwardTo" runat="server" meta:resourcekey="locForwardTo" Text="Forward To:"></asp:Localize></td>
							    <td>
									<wsp:MailboxSelector id="forwardingAddress" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true"
											DistributionListsEnabled="true" />
								</td>
						    </tr>
						    <tr>
								<td></td>
								<td>
									<asp:CheckBox ID="chkDoNotDeleteOnForward" runat="server" meta:resourcekey="chkDoNotDeleteOnForward" Text="Deliver messages to both forwarding address and mailbox" />
								</td>
						    </tr>
						</table>
						
							</ContentTemplate>
						</asp:UpdatePanel>
					</asp:Panel>


					<wsp:CollapsiblePanel id="secSendOnBehalf" runat="server"
                        TargetControlID="SendOnBehalf" meta:resourcekey="secSendOnBehalf" Text="Send On Behalf">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="SendOnBehalf" runat="server" Height="0" style="overflow:hidden;">
					    <table>
							<tr>
								<td>
									<asp:Localize ID="locGrantAccess" runat="server" meta:resourcekey="locGrantAccess" Text="Grant this permission to:"></asp:Localize>
								</td>
							</tr>
							<tr>
								<td>
									<wsp:AccountsList id="accessAccounts" runat="server"
											MailboxesEnabled="true" />
								</td>
							</tr>
					    </table>
					</asp:Panel>
					
					
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
					
					
					<wsp:CollapsiblePanel id="secDeliveryOptions" runat="server"
                        TargetControlID="DeliveryOptions" meta:resourcekey="secDeliveryOptions" Text="Delivery Options">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="DeliveryOptions" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel200"><asp:Localize ID="locMaxRecipients" runat="server" meta:resourcekey="locMaxRecipients" Text="Maximum Recipients:"></asp:Localize></td>
							    <td>
									<wsp:SizeBox id="sizeMaxRecipients" runat="server" DisplayUnits="false" ValidationGroup="EditMailbox" />
								</td>
						    </tr>
						    <tr>
							    <td class="FormLabel200"><asp:Localize ID="locMaxSendingSize" runat="server" meta:resourcekey="locMaxSendingSize" Text="Maximum Sending Message Size:"></asp:Localize></td>
							    <td>
									<wsp:SizeBox id="sizeMaxSendingSize" runat="server" ValidationGroup="EditMailbox" />
								</td>
						    </tr>
						    <tr>
							    <td class="FormLabel200"><asp:Localize ID="locMaxReceivingSize" runat="server" meta:resourcekey="locMaxReceivingSize" Text="Maximum Receiving Message Size:"></asp:Localize></td>
							    <td>
									<wsp:SizeBox id="sizeMaxReceivingSize" runat="server" ValidationGroup="EditMailbox" />
								</td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<table style="width:100%;margin-top:10px;">
					    <tr>
					        <td align="center">
					            <asp:CheckBox ID="chkPmmAllowed" runat="server" meta:resourcekey="chkPmmAllowed" AutoPostBack="true" Visible="false"
					                Text="Allow these settings to be managed from Personal Mailbox Manager" OnCheckedChanged="chkPmmAllowed_CheckedChanged" />
					        </td>
					    </tr>
					</table>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave" ValidationGroup="EditMailbox" OnClick="btnSave_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>