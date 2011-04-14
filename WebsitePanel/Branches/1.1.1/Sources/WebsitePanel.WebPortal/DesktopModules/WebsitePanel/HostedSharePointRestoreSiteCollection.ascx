<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointRestoreSiteCollection.ascx.cs"
	Inherits="WebsitePanel.Portal.HostedSharePointRestoreSiteCollection" %>
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
								<td class="SubHead" valign="top" nowrap width="200">
									<asp:Label ID="lblRestoreFrom" runat="server" meta:resourcekey="lblRestoreFrom" Text="Restore From:"></asp:Label></td>
								<td class="normal" width="100%">
									<table width="100%">
										<tr>
											<td class="Normal">
												<asp:RadioButton ID="radioUpload" meta:resourcekey="radioUpload" Checked="True" GroupName="media"
													Text="Uploaded File" runat="server" AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged">
												</asp:RadioButton></td>
										</tr>
										<tr>
											<td class="Normal">
												<asp:RadioButton ID="radioFile" meta:resourcekey="radioFile" GroupName="media" Text="Hosting Space File"
													runat="server" AutoPostBack="True" OnCheckedChanged="radioUpload_CheckedChanged">
												</asp:RadioButton></td>
										</tr>
										<tr>
											<td class="Normal" id="cellUploadFile" runat="server">
												<table width="100%">
													<tr>
														<td>
															<asp:FileUpload ID="uploadFile" runat="server" Width="300px" /></td>
													</tr>
													<tr>
														<td class="Small" nowrap>
															<asp:Label ID="lblAllowedFiles1" runat="server" meta:resourcekey="lblAllowedFiles"
																Text=".ZIP, .BAK files are allowed"></asp:Label></td>
													</tr>
												</table>
											</td>
										</tr>
										<tr>
											<td class="Normal" id="cellFile" runat="server">
												<table width="100%">
													<tr>
														<td>
															<uc1:FileLookup ID="fileLookup" runat="server" Width="300" IncludeFiles="true" />
														</td>
													</tr>
													<tr>
														<td class="Small" nowrap>
															<asp:Label ID="lblAllowedFiles2" runat="server" meta:resourcekey="lblAllowedFiles"
																Text=".ZIP, .BAK files are allowed"></asp:Label></td>
													</tr>
												</table>
											</td>
										</tr>
									</table>
									<br />
								</td>
							</tr>
						</table>
					<div class="FormFooterClean">
						<asp:Button ID="btnRestore" runat="server" meta:resourcekey="btnRestore" CssClass="Button1"
							Text="Restore" OnClick="btnRestore_Click" OnClientClick="ShowProgressDialog('Restoring site collection...');"/>
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
