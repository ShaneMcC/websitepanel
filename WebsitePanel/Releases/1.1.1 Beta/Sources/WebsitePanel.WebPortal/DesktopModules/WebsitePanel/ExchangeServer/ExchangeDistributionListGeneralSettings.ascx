<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeDistributionListGeneralSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeDistributionListGeneralSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="wsp" %>
<%@ Register Src="UserControls/DistributionListTabs.ascx" TagName="DistributionListTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
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
                    <wsp:DistributionListTabs id="tabs" runat="server" SelectedTab="dlist_settings" />
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtDisplayName" runat="server" CssClass="HugeTextBox200" ValidationGroup="CreateMailbox"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									ErrorMessage="Enter Display Name" ValidationGroup="EditList" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
						    <td></td>
						    <td>
						        <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						        <br />
						        <br />
						    </td>
						</tr>
					    <tr>
						    <td class="FormLabel150"><asp:Localize ID="locManager" runat="server" meta:resourcekey="locManager" Text="Manager:"></asp:Localize></td>
						    <td>
                                <wsp:MailboxSelector id="manager" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true"
											DistributionListsEnabled="true" />
											
								<asp:CustomValidator runat="server" 
                                     ValidationGroup="EditList"  meta:resourcekey="valManager" ID="valManager" 
                                     onservervalidate="valManager_ServerValidate" />
                            </td>
					    </tr>
					    
					    <tr><td>&nbsp;</td></tr>
						<tr>
							<td colspan="2"><asp:Localize ID="locMembers" runat="server" meta:resourcekey="locMembers" Text="Members:"></asp:Localize></td>
						</tr>
						<tr>
						    <td colspan="2">
                                <wsp:AccountsList id="members" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true"
											DistributionListsEnabled="true"  />
                            </td>
						</tr>
					    <tr><td>&nbsp;</td></tr>
						<tr>
							<td class="FormLabel150" colspan="2"><asp:Localize ID="locNotes" runat="server" meta:resourcekey="locNotes" Text="Notes:"></asp:Localize></td>
						</tr>
					    <tr>
						    <td colspan="2">
							    <asp:TextBox ID="txtNotes" runat="server" CssClass="TextBox200" Rows="4" TextMode="MultiLine"></asp:TextBox>
						    </td>
					    </tr>
					</table>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave" ValidationGroup="EditList" OnClick="btnSave_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditList" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>