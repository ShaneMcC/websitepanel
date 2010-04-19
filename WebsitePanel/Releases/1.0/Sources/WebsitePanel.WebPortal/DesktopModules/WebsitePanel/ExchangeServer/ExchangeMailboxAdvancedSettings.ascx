<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxAdvancedSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxAdvancedSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/DaysBox.ascx" TagName="DaysBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
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
                    <wsp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_advanced" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />

					<wsp:CollapsiblePanel id="secMailboxFeatures" runat="server"
                        TargetControlID="MailboxFeatures" meta:resourcekey="secMailboxFeatures" Text="Mailbox Features">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="MailboxFeatures" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkPOP3" runat="server" meta:resourcekey="chkPOP3" Text="POP3"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkIMAP" runat="server" meta:resourcekey="chkIMAP" Text="IMAP"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkOWA" runat="server" meta:resourcekey="chkOWA" Text="OWA/HTTP"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkMAPI" runat="server" meta:resourcekey="chkMAPI" Text="MAPI"></asp:CheckBox>
							    </td>
						    </tr>
						    <tr>
							    <td>
								    <asp:CheckBox ID="chkActiveSync" runat="server" meta:resourcekey="chkActiveSync" Text="ActiveSync"></asp:CheckBox>
							    </td>
						    </tr>
						</table>
						<br />
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secStatistics" runat="server"
                        TargetControlID="Statistics" meta:resourcekey="secStatistics" Text="Storage Statistics">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="Statistics" runat="server" Height="0" style="overflow:hidden;">
					    <table cellpadding="4">
							<tr>
							    <td class="FormLabel150"><asp:Localize ID="locTotalItems" runat="server" meta:resourcekey="locTotalItems" Text="Total Items:"></asp:Localize></td>
							    <td>
								    <asp:Label ID="lblTotalItems" runat="server" CssClass="NormalBold">177</asp:Label>
							    </td>
							</tr>
							<tr>
							    <td class="FormLabel150"><asp:Localize ID="locTotalSize" runat="server" meta:resourcekey="locTotalSize" Text="Total Size (MB):"></asp:Localize></td>
							    <td>
								    <asp:Label ID="lblTotalSize" runat="server" CssClass="NormalBold">16</asp:Label>
							    </td>
							</tr>
							<tr>
							    <td class="FormLabel150"><asp:Localize ID="locLastLogon" runat="server" meta:resourcekey="locLastLogon" Text="Last Logon:"></asp:Localize></td>
							    <td>
								    <asp:Label ID="lblLastLogon" runat="server" CssClass="NormalBold"></asp:Label>
							    </td>
							</tr>
							<tr>
							    <td class="FormLabel150"><asp:Localize ID="locLastLogoff" runat="server" meta:resourcekey="locLastLogoff" Text="Last Logoff:"></asp:Localize></td>
							    <td>
								    <asp:Label ID="lblLastLogoff" runat="server" CssClass="NormalBold"></asp:Label>
							    </td>
							</tr>
						</table>
						<br />
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secStorageQuotas" runat="server"
                        TargetControlID="StorageQuotas" meta:resourcekey="secStorageQuotas" Text="Storage Quotas">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="StorageQuotas" runat="server" Height="0" style="overflow:hidden;">
						<table>
							<tr>
								<td class="FormLabel200" colspan="2"><asp:Localize ID="locWhenSizeExceeds" runat="server" meta:resourcekey="locWhenSizeExceeds" Text="When the mailbox size exceeds the indicated amount:"></asp:Localize></td>
							</tr>
							<tr>
								<td class="FormLabel200" align="right"><asp:Localize ID="locIssueWarning" runat="server" meta:resourcekey="locIssueWarning" Text="Issue warning at:"></asp:Localize></td>
								<td>
									<wsp:SizeBox id="sizeIssueWarning" runat="server" ValidationGroup="EditMailbox" />
								</td>
							</tr>
							<tr>
								<td class="FormLabel200" align="right"><asp:Localize ID="locProhibitSend" runat="server" meta:resourcekey="locProhibitSend" Text="Prohibit send at:"></asp:Localize></td>
								<td>
									<wsp:SizeBox id="sizeProhibitSend" runat="server" ValidationGroup="EditMailbox" />
								</td>
							</tr>
							<tr>
								<td class="FormLabel200" align="right"><asp:Localize ID="locProhibitSendReceive" runat="server" meta:resourcekey="locProhibitSendReceive" Text="Prohibit send and receive at:"></asp:Localize></td>
								<td>
									<wsp:SizeBox id="sizeProhibitSendReceive" runat="server" ValidationGroup="EditMailbox" />
								</td>
							</tr>
						</table>
						<br />
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secDeleteRetention" runat="server"
                        TargetControlID="DeleteRetention" meta:resourcekey="secDeleteRetention" Text="Delete Item Retention">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="DeleteRetention" runat="server" Height="0" style="overflow:hidden;">
						<table>
							<tr>
								<td class="FormLabel200" align="right"><asp:Localize ID="locKeepDeletedItems" runat="server" meta:resourcekey="locKeepDeletedItems" Text="Keep deleted items for:"></asp:Localize></td>
								<td>
									<wsp:DaysBox id="daysKeepDeletedItems" runat="server" ValidationGroup="EditMailbox" />
								</td>
							</tr>
						</table>
						<br />
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secDomainUser" runat="server"
                        TargetControlID="DomainUser" meta:resourcekey="secDomainUser" Text="Domain User Name">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="DomainUser" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel200" align="right">
									
								</td>
							    <td>
									<asp:TextBox ID="txtAccountName" runat="server" CssClass="TextBox200" ReadOnly="true" Text="Username"></asp:TextBox>
								</td>
						    </tr>
					    </table>
					    <br />
					</asp:Panel>
					
					<table style="width:100%;margin-top:10px;">
					    <tr>
					        <td align="center">
					            <asp:CheckBox ID="chkPmmAllowed"  Visible="false" runat="server" meta:resourcekey="chkPmmAllowed" AutoPostBack="true"
					                Text="Allow these settings to be managed from Personal Mailbox Manager" OnCheckedChanged="chkPmmAllowed_CheckedChanged" />
					        </td>
					    </tr>
					</table>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave" ValidationGroup="EditMailbox" OnClick="btnSave_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
							ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>