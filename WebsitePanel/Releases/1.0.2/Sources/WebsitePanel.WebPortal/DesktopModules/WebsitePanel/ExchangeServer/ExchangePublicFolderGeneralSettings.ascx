<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangePublicFolderGeneralSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangePublicFolderGeneralSettings" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="wsp" %>
<%@ Register Src="UserControls/PublicFolderTabs.ascx" TagName="PublicFolderTabs" TagPrefix="wsp" %>
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
			<wsp:Menu id="menu" runat="server" SelectedItem="public_folders" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangePublicFolder48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Edit Distribution List"></asp:Localize>
					-
					<asp:Literal ID="litDisplayName" runat="server" Text="John Smith" />
                </div>
				<div class="FormBody">
                    <wsp:PublicFolderTabs id="tabs" runat="server" SelectedTab="public_folder_settings" />
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locName" runat="server" meta:resourcekey="locName" Text="Folder Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtName" runat="server" CssClass="HugeTextBox200"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valDisplayName" runat="server" meta:resourcekey="valRequireName" ControlToValidate="txtName"
									ErrorMessage="Enter Folder Name" ValidationGroup="EditPublicFolder" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>

						<tr>
						    <td></td>
							<td>
							    <br />
							    <asp:Button ID="btnMailEnable" runat="server" Text="Mail Enable Folder" meta:resourcekey="btnMailEnable" CssClass="Button1" CausesValidation="false" OnClick="btnMailEnable_Click" />
							    <asp:Button ID="btnMailDisable" runat="server" Text="Mail Disable Folder" meta:resourcekey="btnMailDisable" CssClass="Button1" CausesValidation="false" OnClick="btnMailDisable_Click" />
							</td>
						</tr>
					    <tr><td>&nbsp;</td></tr>
						<tr>
							<td colspan="2"><asp:Localize ID="locAuthors" runat="server" meta:resourcekey="locAuthors" Text="Authors:"></asp:Localize></td>
						</tr>
						<tr>
						    <td colspan="2">
                                <wsp:AccountsList id="authors" runat="server"
										MailboxesEnabled="true" />
                            </td>
						</tr>
					    <tr><td>&nbsp;</td></tr>
						<tr>
						    <td>
						        <br />
						        <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						    </td>
						</tr>
					</table>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave" ValidationGroup="EditPublicFolder" OnClick="btnSave_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditPublicFolder" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>