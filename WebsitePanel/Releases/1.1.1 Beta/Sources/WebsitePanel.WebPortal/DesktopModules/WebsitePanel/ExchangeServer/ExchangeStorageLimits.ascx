<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeStorageLimits.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeStorageLimits" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
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
			<wsp:Menu id="menu" runat="server" SelectedItem="storage_limits" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeStorageConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Mailboxes"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
					<wsp:CollapsiblePanel id="secStorageLimits" runat="server"
                        TargetControlID="StorageLimits" meta:resourcekey="secStorageLimits" Text="Storage Settings">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="StorageLimits" runat="server" Height="0" style="overflow:hidden;">
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
					
					
					<wsp:CollapsiblePanel id="secDeletionSettings" runat="server"
                        TargetControlID="DeletionSettings" meta:resourcekey="secDeletionSettings" Text="Deletion Settings">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="DeletionSettings" runat="server" Height="0" style="overflow:hidden;">
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
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave"
							 OnClientClick="ShowProgressDialog('Updating settings...');" ValidationGroup="EditMailbox" OnClick="btnSave_Click"></asp:Button>
						<asp:Button id="btnSaveApply" runat="server" Text="Save and Apply to All Mailboxes" CssClass="Button1" meta:resourcekey="btnSaveApply"
							 OnClientClick="ShowProgressDialog('Updating settings...');" ValidationGroup="EditMailbox" OnClick="btnSaveApply_Click"></asp:Button>
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