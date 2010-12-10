<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointBackupSiteCollection.ascx.cs"
	Inherits="WebsitePanel.Portal.HostedSharePointBackupSiteCollection" %>
<%@ Register Src="UserControls/FileLookup.ascx" TagName="FileLookup" TagPrefix="uc1" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
	TagPrefix="wsp" %>
	
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />

	
<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="sharepoint_sitecollections" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="SharePointSiteCollection48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="SharePoint Site Collections"></asp:Localize>
				</div>
				<div class="FormBody">
					<wsp:SimpleMessageBox id="messageBox" runat="server" />
					<table cellspacing="0" cellpadding="5" width="100%">
						<tr>
							<td class="Huge" colspan="2">
								<asp:Literal ID="litSiteCollectionName" runat="server"></asp:Literal></td>
						</tr>
						<tr>
							<td>
								&nbsp;</td>
						</tr>
						<tr>
							<td class="SubHead" valign="top">
								<asp:Label ID="lblBackupFileName" runat="server" meta:resourcekey="lblBackupFileName"
									Text="Backup File Name:"></asp:Label></td>
							<td class="normal">
								<asp:TextBox ID="txtBackupName" runat="server" CssClass="NormalTextBox" Width="200"></asp:TextBox><asp:RequiredFieldValidator
									ID="validatorUserName" runat="server" ControlToValidate="txtBackupName" CssClass="NormalBold"
									Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator></td>
						</tr>
						<tr>
							<td class="SubHead" valign="top">
								<asp:Label ID="lblBackupOptions" runat="server" meta:resourcekey="lblBackupOptions"
									Text="Backup Options:"></asp:Label></td>
							<td class="normal">
								<asp:CheckBox ID="chkZipBackup" runat="server" meta:resourcekey="chkZipBackup" Checked="True"
									Text="ZIP Backup" AutoPostBack="True" OnCheckedChanged="chkZipBackup_CheckedChanged">
								</asp:CheckBox><br />
								<br />
							</td>
						</tr>
						<tr>
							<td class="SubHead" valign="top">
								<asp:Label ID="lblBackupDestination" runat="server" meta:resourcekey="lblBackupDestination"
									Text="Backup Destination:"></asp:Label></td>
							<td class="normal">
								<asp:RadioButton ID="rbDownload" runat="server" meta:resourcekey="rbDownload" Checked="True"
									Text="Download via HTTP" GroupName="action" AutoPostBack="True" OnCheckedChanged="rbDownload_CheckedChanged">
								</asp:RadioButton><br />
								<asp:RadioButton ID="rbCopy" runat="server" meta:resourcekey="rbCopy" Text="Copy to Folder"
									GroupName="action" AutoPostBack="True" OnCheckedChanged="rbDownload_CheckedChanged">
								</asp:RadioButton><br />
								&nbsp;&nbsp;&nbsp;&nbsp;<uc1:FileLookup ID="fileLookup" runat="server" Width="300" />
							</td>
						</tr>
					</table>
					<div class="FormFooterClean">
						<asp:Button ID="btnBackup" runat="server" meta:resourcekey="btnBackup" CssClass="Button1"
							Text="Backup" OnClick="btnBackup_Click"/>
						<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1"
							CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
					</div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
