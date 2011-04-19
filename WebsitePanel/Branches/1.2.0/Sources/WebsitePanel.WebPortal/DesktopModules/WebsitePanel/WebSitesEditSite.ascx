<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesEditSite.ascx.cs"
	Inherits="WebsitePanel.Portal.WebSitesEditSite" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<%@ Register Src="WebSitesExtensionsControl.ascx" TagName="WebSitesExtensionsControl"
	TagPrefix="uc6" %>
<%@ Register Src="WebSitesCustomErrorsControl.ascx" TagName="WebSitesCustomErrorsControl"
	TagPrefix="uc4" %>
<%@ Register Src="WebSitesMimeTypesControl.ascx" TagName="WebSitesMimeTypesControl"
	TagPrefix="uc5" %>
<%@ Register Src="WebSitesHomeFolderControl.ascx" TagName="WebSitesHomeFolderControl"
	TagPrefix="uc1" %>
<%@ Register Src="WebSitesCustomHeadersControl.ascx" TagName="WebSitesCustomHeadersControl"
	TagPrefix="uc6" %>
<%@ Register Src="WebSitesSecuredFoldersControl.ascx" TagName="WebSitesSecuredFoldersControl"
	TagPrefix="wsp" %>
<%@ Register Src="WebSitesHeliconApeControl.ascx" TagName="WebSitesHeliconApeControl"
	TagPrefix="wsp" %>
<%@ Register Src="UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/UsernameControl.ascx" TagName="UsernameControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
	TagPrefix="wsp" %>
<%@ Register Src="UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Portal" %>
<%@ Register Src="WebsitesSSL.ascx" TagName="WebsitesSSL" TagPrefix="uc2" %>
<style type="text/css">
	.style1
	{
		width: 51px;
	}
</style>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server" />
<script type="text/javascript">

	function confirmationSITE() {
		if (!confirm("Are you sure you want to delete Web site?")) return false; else ShowProgressDialog('Deleting Web site...');
	}

	function confirmationFPSE() {
		if (!confirm("Are you sure you want to delete Frontpage account?")) return false; else ShowProgressDialog('Uninstalling Frontpage...');
	}

	function confirmationWMSVC() {
		if (!confirm("Are you sure you want to disable Remote Management?")) return false; else ShowProgressDialog('Disabling Remote Management...');
	}

	function confirmationWebDeployPublishing() {
		if (!confirm("Are you sure you want to disable Web Publishing?")) return false; else ShowProgressDialog('Disabling Web Publishing...');
	}
</script>
<asp:Panel ID="WDeployBuildPublishingProfileWizardPanel" runat="server" CssClass="PopupContainer"
	DefaultButton="PubProfileWizardOkButton" Style="display: none;">
	<wsp:PopupHeader runat="server" meta:resourcekey="WDeployBuildPublishingProfileWizard" />
	<div class="Content">
		<asp:UpdatePanel runat="server" ID="WDeployPubProfilePanel" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="MyDatabaseList" EventName="SelectedIndexChanged" />
			</Triggers>
			<ContentTemplate>
				<div class="FormBody">
					<asp:PlaceHolder runat="server" ID="ChooseDatabasePanel">
						<fieldset>
							<legend>
								<asp:Localize ID="Localize1" runat="server" Text="Database Name" /></legend>
							<div class="FormFieldDescription">
								<asp:Localize ID="Localize2" runat="server">Please choose database name...</asp:Localize>
							</div>
							<div class="FormField">
								<asp:DropDownList ID="MyDatabaseList" runat="server" DataTextField="Name" DataValueField="Id"
									AutoPostBack="true" Width="100%" OnSelectedIndexChanged="MyDatabaseList_SelectedIndexChanged">
								</asp:DropDownList>
							</div>
						</fieldset>
					</asp:PlaceHolder>
					<asp:PlaceHolder runat="server" ID="ChooseDatabaseUserPanel">
						<fieldset>
							<legend>
								<asp:Localize ID="Localize3" runat="server" Text="Database User" /></legend>
							<div class="FormFieldDescription">
								<asp:Localize ID="Localize4" runat="server">Please choose database user name...</asp:Localize>
							</div>
							<div class="FormField">
								<asp:DropDownList ID="MyDatabaseUserList" runat="server" DataTextField="Name" DataValueField="Id" Width="100%">
								</asp:DropDownList>
							</div>
						</fieldset>
					</asp:PlaceHolder>
					<asp:PlaceHolder runat="server" ID="ChooseFtpAccountPanel">
						<fieldset>
							<legend>
								<asp:Localize ID="Localize5" runat="server" Text="FTP Account" /></legend>
							<div class="FormFieldDescription">
								<asp:Localize ID="Localize6" runat="server">Please choose FTP account...</asp:Localize>
							</div>
							<div class="FormField">
								<asp:DropDownList ID="MyFtpAccountList" runat="server" DataTextField="Name" DataValueField="Id" Width="100%">
								</asp:DropDownList>
							</div>
						</fieldset>
					</asp:PlaceHolder>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
		<div class="FormFooter">
			<asp:Button ID="PubProfileWizardOkButton" runat="server" CssClass="Button1" meta:resourcekey="PubProfileWizardOkButton"
				Text="OK" ValidationGroup="WDeployBuildPublishingProfileWizard" OnClick="PubProfileWizardOkButton_Click" />
			<asp:Button ID="PubProfileWizardCancelButton" runat="server" CssClass="Button1" meta:resourcekey="PubProfileWizardCancelButton"
				ValidationGroup="WDeployBuildPublishingProfileWizard" Text="Cancel" CausesValidation="false" />
		</div>
	</div>
</asp:Panel>
<ajaxToolkit:ModalPopupExtender ID="WDeployRebuildPublishingProfileWizardModal" runat="server"
	TargetControlID="WDeployRebuildPubProfileLinkButton" PopupControlID="WDeployBuildPublishingProfileWizardPanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="PubProfileWizardCancelButton" />
<div class="FormBody">
	<wsp:SimpleMessageBox id="messageBox" runat="server" EnableViewState="false" />
	<table width="100%" cellpadding="0" cellspacing="0" border="0">
		<tr>
			<td valign="top">
				<table cellpadding="7" border="0">
					<tr>
						<td class="Big">
							<asp:HyperLink ID="lnkSiteName" runat="server" NavigateUrl="#" Target="_blank">domain.com</asp:HyperLink>
							<asp:Literal ID="litIPAddress" runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td>
							<div class="FormButtonsBar">
								<asp:Button ID="btnAddPointer" runat="server" Text="Add Pointer" CssClass="Button2"
									meta:resourcekey="btnAddPointer" OnClick="btnAddPointer_Click" />
							</div>
							<asp:GridView ID="gvPointers" runat="server" EnableViewState="True" AutoGenerateColumns="false"
								ShowHeader="false" CssSelectorClass="NormalGridView" EmptyDataText="gvPointers"
								DataKeyNames="DomainID" OnRowDeleting="gvPointers_RowDeleting">
								<Columns>
									<asp:TemplateField HeaderText="gvPointersName">
										<ItemStyle Wrap="false" Width="100%"></ItemStyle>
										<ItemTemplate>
											<asp:HyperLink ID="lnkPointer" runat="server" NavigateUrl='<%# "http://" + (string)Eval("DomainName") %>'
												Target="_blank"><%# Eval("DomainName") %></asp:HyperLink>
											<asp:ImageButton runat="server" SkinID="DeleteSmall" ID="cmdDeletePointer" CommandName='delete'
												CommandArgument='<%# Eval("DomainId") %>' meta:resourcekey="cmdDeletePointer"
												Visible='<%# !(bool)Eval("IsInstantAlias") %>' OnClientClick="return confirm('Remove pointer?');">
											</asp:ImageButton>
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</td>
					</tr>
				</table>
			</td>
			<td nowrap valign="top" align="right">
				<table class="Toolbox" cellpadding="7" width="150px">
					<tr>
						<td class="MediumBold" align="center">
							<asp:Literal ID="litStatus" runat="server"></asp:Literal>
						</td>
					</tr>
					<tr>
						<td align="center">
							<asp:ImageButton ID="cmdStart" runat="server" SkinID="StartMedium" meta:resourcekey="cmdStart"
								CommandName="Started" OnClick="cmdChangeState_Click" />
							<asp:ImageButton ID="cmdPause" runat="server" SkinID="PauseMedium" meta:resourcekey="cmdPause"
								CommandName="Paused" OnClick="cmdChangeState_Click" />
							<asp:ImageButton ID="cmdContinue" runat="server" SkinID="ContinueMedium" meta:resourcekey="cmdContinue"
								CommandName="Continuing" OnClick="cmdChangeState_Click" />
							<asp:ImageButton ID="cmdStop" runat="server" SkinID="StopMedium" meta:resourcekey="cmdStop"
								CommandName="Stopped" OnClick="cmdChangeState_Click" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td colspan="2">
				<table width="100%" cellpadding="0" cellspacing="1">
					<tr>
						<td class="Tabs">
							&nbsp;&nbsp;
							<asp:DataList ID="dlTabs" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="dlTabs_SelectedIndexChanged"
								RepeatLayout="Flow" DataKeyField="ViewId">
								<ItemStyle Wrap="False" />
								<ItemTemplate>
									<asp:LinkButton ID="cmdSelectTab" runat="server" CommandName="select" CssClass="Tab">
                                    <%# Eval("Name") %>
									</asp:LinkButton>
								</ItemTemplate>
								<SelectedItemStyle Wrap="False" />
								<SelectedItemTemplate>
									<asp:LinkButton ID="cmdSelectTab" runat="server" CommandName="select" CssClass="ActiveTab">
                                    <%# Eval("Name") %>
									</asp:LinkButton>
								</SelectedItemTemplate>
							</asp:DataList>
						</td>
					</tr>
				</table>
				<div class="FormBody">
					<asp:MultiView ID="tabs" runat="server" ActiveViewIndex="0">
						<asp:View ID="tabHomeFolder" runat="server">
							<uc1:WebSitesHomeFolderControl ID="webSitesHomeFolderControl" runat="server" />
						</asp:View>
						<asp:View ID="tabVirtualDirs" runat="server">
							<div style="width: 500px;">
								<div class="FormButtonsBar">
									<asp:Button ID="btnAddVirtualDirectory" runat="server" meta:resourcekey="btnAddVirtualDirectory"
										Text="Create Directory" CssClass="Button3" CausesValidation="false" OnClick="btnAddVirtualDirectory_Click" />
								</div>
								<asp:GridView ID="gvVirtualDirectories" runat="server" EnableViewState="True" AutoGenerateColumns="false"
									ShowHeader="true" CssSelectorClass="NormalGridView" EmptyDataText="gvVirtualDirectories">
									<Columns>
										<asp:TemplateField HeaderText="gvVirtualDirectoriesName">
											<ItemStyle Width="50%" CssClass="NormalBold"></ItemStyle>
											<ItemTemplate>
												<asp:HyperLink ID="lnkEditVDir" runat="server" NavigateUrl='<%# EditUrl("ItemID", PanelRequest.ItemID.ToString(), "edit_vdir", "VirtDir=" + Eval("Name"), "SpaceID=" + PanelSecurity.PackageId) %>'>
									        <%# Eval("Name") %>
												</asp:HyperLink>
											</ItemTemplate>
										</asp:TemplateField>
										<asp:BoundField DataField="ContentPath" HeaderText="gvVirtualDirectoriesPath">
											<ItemStyle Width="100%" />
										</asp:BoundField>
									</Columns>
								</asp:GridView>
							</div>
						</asp:View>
						<asp:View ID="tabSecuredFolders" runat="server">
							<wsp:WebSitesSecuredFoldersControl ID="webSitesSecuredFoldersControl" runat="server" />
						</asp:View>
						<asp:View ID="tabHeliconApe" runat="server">
							<wsp:WebSitesHeliconApeControl ID="webSitesHeliconApeControl" runat="server" />
						</asp:View>
						<asp:View ID="tabFrontPage" runat="server">
							<asp:Panel ID="pnlFrontPage" runat="server" Style="padding: 20;">
								<table id="tblSharePoint" runat="server">
									<tr>
										<td class="NormalBold">
											<asp:Label ID="lblSharePoint" runat="server" meta:resourcekey="lblSharePoint" Text="This web site has SharePoint Service installed and thus can't"></asp:Label>
										</td>
									</tr>
								</table>
								<asp:Literal ID="litFrontPageUnavailable" runat="server"></asp:Literal>
								<table id="tblFrontPage" cellspacing="0" cellpadding="2" width="100%" runat="server">
									<tr>
										<td class="SubHead" style="width: 150px;" height="30">
											<asp:Label ID="lblFPStatus" runat="server" meta:resourcekey="lblFPStatus" Text="FrontPage status:"></asp:Label>
										</td>
										<td class="NormalBold">
											<asp:Literal ID="litFrontPageStatus" runat="server"></asp:Literal>
										</td>
									</tr>
									<tr>
										<td class="SubHead">
											<asp:Label ID="lblFPAccount" runat="server" meta:resourcekey="lblFPAccount" Text="FrontPage User Account:"></asp:Label>
										</td>
										<td class="Normal">
											<wsp:UsernameControl ID="frontPageUsername" runat="server" ValidationGroup="FrontPage" />
										</td>
									</tr>
									<tr>
										<td class="SubHead" valign="top">
											<asp:Label ID="lblFPPassword" runat="server" meta:resourcekey="lblFPPassword" Text="Password:"></asp:Label>
										</td>
										<td>
											<wsp:PasswordControl ID="frontPagePassword" runat="server" ValidationGroup="FrontPage" />
										</td>
									</tr>
									<tr>
										<td>
										</td>
										<td class="Normal">
											<asp:Button ID="btnInstallFrontPage" runat="server" meta:resourcekey="btnInstallFrontPage"
												Text="Install" CssClass="Button1" ValidationGroup="FrontPage" OnClick="btnInstallFrontPage_Click"
												OnClientClick="ShowProgressDialog('Installing Frontpage...');" />
											<asp:Button ID="btnChangeFrontPagePassword" runat="server" meta:resourcekey="btnChangeFrontPagePassword"
												Text="Change Password" CssClass="Button2" ValidationGroup="FrontPage" OnClick="btnChangeFrontPagePassword_Click" />
											<asp:Button ID="btnUninstallFrontPage" runat="server" meta:resourcekey="btnUninstallFrontPage"
												Text="Uninstall" CssClass="Button1" CausesValidation="false" OnClick="btnUninstallFrontPage_Click"
												OnClientClick="confirmationFPSE()" />
										</td>
									</tr>
								</table>
							</asp:Panel>
						</asp:View>
						<asp:View ID="tabExtensions" runat="server">
							<uc6:WebSitesExtensionsControl ID="webSitesExtensionsControl" runat="server" />
						</asp:View>
						<asp:View ID="tabErrors" runat="server">
							<uc4:WebSitesCustomErrorsControl ID="webSitesCustomErrorsControl" runat="server" />
						</asp:View>
						<asp:View ID="tabHeaders" runat="server">
							<uc6:WebSitesCustomHeadersControl ID="webSitesCustomHeadersControl" runat="server" />
						</asp:View>
						<asp:View ID="tabWebDeployPublishing" runat="server">
							<div style="padding: 20;">
								<asp:PlaceHolder runat="server" ID="PanelWDeploySitePublishingDisabled" Visible="false">
									<div class="NormalBold">
										<asp:Localize runat="server" meta:resourcekey="WDeploySitePublishingDisabled" /></div>
									<br />
									<div class="Normal">
										<asp:Localize runat="server" meta:resourcekey="WDeploySitePublishingEnablementHint" /></div>
									<br />
								</asp:PlaceHolder>
								<asp:PlaceHolder runat="server" ID="PanelWDeployManagePublishingProfile" Visible="false">
									<div class="NormalBold">
										<asp:Localize runat="server" meta:resourcekey="WDeploySitePublishingEnabled" /></div>
									<br />
									<div class="Normal">
										<asp:Localize runat="server" meta:resourcekey="WDeployPublishingProfileUsageNotes" /></div>
									<br />
									<p>
										<asp:LinkButton runat="server" ID="WDeployDownloadPubProfileLink" Text="Download Publishing Profile for this web site"
											CommandName="DownloadProfile" OnCommand="WDeployDownloadPubProfileLink_Command" />&nbsp;/&nbsp;<asp:LinkButton
												runat="server" ID="WDeployRebuildPubProfileLinkButton" Text="Re-build Publishing Profile for this web site" /></p>
									<br />
								</asp:PlaceHolder>
								<asp:PlaceHolder runat="server" ID="PanelWDeployPublishingCredentials" Visible="false">
									<table cellpadding="4" border="0">
										<tr>
											<td class="Normal">
												<asp:Localize runat="server" meta:resourcekey="WDeployPublishingAccountLocalize" />
											</td>
											<td class="NormalBold">
												<asp:TextBox runat="server" ID="WDeployPublishingAccountTextBox" CssClass="NormalTextBox"
													ValidationGroup="WDeployPublishingGroup" MaxLength="20" />
												<asp:Literal runat="server" ID="WDeployPublishingAccountLiteral" Visible="false" />
												<asp:RequiredFieldValidator ID="WDeployPublishingAccountRequiredFieldValidator" runat="server"
													ErrorMessage="*" ValidationGroup="WDeployPublishingGroup" ControlToValidate="WDeployPublishingAccountTextBox" />
											</td>
										</tr>
										<tr>
											<td class="Normal">
												<asp:Localize runat="server" meta:resourcekey="WDeployPublishingPasswordLocalize" />
											</td>
											<td>
												<asp:TextBox runat="server" ID="WDeployPublishingPasswordTextBox" TextMode="Password"
													CssClass="NormalTextBox" ValidationGroup="WDeployPublishingGroup" />
												<asp:RequiredFieldValidator ID="WDeployPublishingPasswordRequiredFieldValidator"
													runat="server" ErrorMessage="*" ValidationGroup="WDeployPublishingGroup" ControlToValidate="WDeployPublishingPasswordTextBox" />
											</td>
										</tr>
										<tr>
											<td class="Normal">
												<asp:Localize runat="server" meta:resourcekey="WDeployPublishingConfirmPasswordLocalize" />
											</td>
											<td>
												<asp:TextBox runat="server" ID="WDeployPublishingConfirmPasswordTextBox" TextMode="Password"
													CssClass="NormalTextBox" ValidationGroup="WDeployPublishingGroup" />
												<asp:RequiredFieldValidator ID="WDeployPublishingConfirmPasswordRequiredFieldValidator"
													runat="server" ErrorMessage="*" ValidationGroup="WDeployPublishingGroup" ControlToValidate="WDeployPublishingConfirmPasswordTextBox" />
												<asp:CompareValidator ID="WDeployPublishingConfirmPasswordTextBoxCompareValidator"
													runat="server" meta:resourcekey="cvPasswordComparer" ControlToValidate="WDeployPublishingConfirmPasswordTextBox"
													ControlToCompare="WDeployPublishingPasswordTextBox" Display="Dynamic" />
											</td>
										</tr>
									</table>
									<br />
									<div>
										<asp:Button runat="server" ID="WDeployEnabePublishingButton" meta:resourcekey="WDeployEnabePublishingButton"
											CssClass="Button1" OnClick="WDeployEnabePublishingButton_Click" ValidationGroup="WDeployPublishingGroup" />&nbsp;
										<asp:Button runat="server" ID="WDeployChangePublishingPasswButton" meta:resourcekey="WDeployChangePublishingPasswButton"
											CssClass="Button1" OnClick="WDeployChangePublishingPasswButton_Click" ValidationGroup="WDeployPublishingGroup" />&nbsp;
										<asp:Button runat="server" ID="WDeployDisablePublishingButton" meta:resourcekey="WDeployDisablePublishingButton"
											CssClass="Button1" OnClick="WDeployDisablePublishingButton_Click" OnClientClick="return confirmationWebDeployPublishing();"
											ValidationGroup="WDeployPublishingGroup" CausesValidation="false" />
									</div>
								</asp:PlaceHolder>
								<asp:PlaceHolder runat="server" ID="PanelWDeployNotInstalled" Visible="false">Web Deploy
									Remote Agent is not installed... </asp:PlaceHolder>
							</div>
						</asp:View>
						<asp:View ID="tabMimes" runat="server">
							<uc5:WebSitesMimeTypesControl ID="webSitesMimeTypesControl" runat="server" />
						</asp:View>
						<asp:View ID="tabCF" runat="server">
							<asp:Literal ID="litCFUnavailable" runat="server"></asp:Literal>
							<br></br>
							<br></br>
							<table id="tableCF" runat="server">
								<tr id="rowCF" runat="server">
									<td class="style1">
										<asp:CheckBox ID="chkCfExt" runat="server" Text="" meta:resourcekey="chkEnabled" />
									</td>
									<td class="Normal">
										<asp:Label ID="lblCF" runat="server" meta:resourcekey="lblCF" Text="Enable ColdFusion Extensions."></asp:Label>
									</td>
								</tr>
								<tr id="rowVirtDir" runat="server">
									<td class="style1">
										<asp:CheckBox ID="chkVirtDir" runat="server" Text="" meta:resourcekey="chkEnabled" />
									</td>
									<td class="Normal">
										<asp:Label ID="lblVirtDir" runat="server" meta:resourcekey="lblVirtDir" Text="Create Virtual Directories for scripts and Flash remoting."></asp:Label>
									</td>
								</tr>
							</table>
						</asp:View>
						<asp:View ID="tabWebManagement" runat="server">
							<div style="padding: 20;">
								<asp:PlaceHolder runat="server" ID="pnlWmSvcSiteDisabled" Visible="false">
									<div class="NormalBold">
										<asp:Localize runat="server" meta:resourcekey="lclWmSvcSiteDisabled" /></div>
									<br />
									<div class="Normal">
										<asp:Localize runat="server" meta:resourcekey="lclEnablementHint" /></div>
									<br />
								</asp:PlaceHolder>
								<asp:PlaceHolder runat="server" ID="pnlWmSvcSiteEnabled" Visible="false">
									<div class="NormalBold">
										<asp:Localize runat="server" meta:resourcekey="lclWmSvcSiteEnabled" /></div>
									<br />
									<div class="Normal">
										<asp:Localize runat="server" ID="lclWmSvcConnectionHint" meta:resourcekey="lclWmSvcConnectionHint" /></div>
									<br />
								</asp:PlaceHolder>
								<asp:PlaceHolder runat="server" ID="pnlWmcSvcManagement">
									<table cellpadding="4" border="0">
										<tr>
											<td class="Normal">
												<asp:Localize runat="server" meta:resourcekey="lclWmSvcAccountName" />
											</td>
											<td class="NormalBold">
												<asp:TextBox runat="server" ID="txtWmSvcAccountName" CssClass="NormalTextBox" ValidationGroup="WmSvcGroup"
													MaxLength="20" />
												<asp:Literal runat="server" ID="litWmSvcAccountName" Visible="false" />
												<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ValidationGroup="WmSvcGroup"
													ControlToValidate="txtWmSvcAccountName" />
											</td>
										</tr>
										<tr>
											<td class="Normal">
												<asp:Localize runat="server" meta:resourcekey="lclWmSvcAccountPassword" />
											</td>
											<td>
												<asp:TextBox runat="server" ID="txtWmSvcAccountPassword" TextMode="Password" CssClass="NormalTextBox"
													ValidationGroup="WmSvcGroup" />
												<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ValidationGroup="WmSvcGroup"
													ControlToValidate="txtWmSvcAccountPassword" />
											</td>
										</tr>
										<tr>
											<td class="Normal">
												<asp:Localize runat="server" meta:resourcekey="lclWmSvcAccountPasswordC" />
											</td>
											<td>
												<asp:TextBox runat="server" ID="txtWmSvcAccountPasswordC" TextMode="Password" CssClass="NormalTextBox"
													ValidationGroup="WmSvcGroup" />
												<asp:RequiredFieldValidator runat="server" ErrorMessage="*" ValidationGroup="WmSvcGroup"
													ControlToValidate="txtWmSvcAccountPasswordC" />
												<asp:CompareValidator runat="server" meta:resourcekey="cvPasswordComparer" ControlToValidate="txtWmSvcAccountPasswordC"
													ControlToCompare="txtWmSvcAccountPassword" Display="Dynamic" />
											</td>
										</tr>
									</table>
									<br />
									<div>
										<asp:Button runat="server" ID="btnWmSvcSiteEnable" meta:resourcekey="btnWmSvcSiteEnable"
											CssClass="Button1" OnClick="btnWmSvcSiteEnable_Click" ValidationGroup="WmSvcGroup" />
										<asp:Button runat="server" ID="btnWmSvcChangePassw" meta:resourcekey="btnWmSvcChangePassw"
											CssClass="Button1" OnClick="btnWmSvcChangePassw_Click" ValidationGroup="WmSvcGroup" />&nbsp;
										<asp:Button runat="server" ID="btnWmSvcSiteDisable" meta:resourcekey="btnWmSvcSiteDisable"
											CssClass="Button1" OnClick="btnWmSvcSiteDisable_Click" OnClientClick="return confirmationWMSVC();"
											ValidationGroup="WmSvcGroup" CausesValidation="false" />
									</div>
								</asp:PlaceHolder>
								<asp:PlaceHolder runat="server" ID="pnlNotInstalled" Visible="false">
									<div class="NormalBold">
										<asp:Localize runat="server" meta:resourcekey="lclWmSvcNotInstalled" /></div>
								</asp:PlaceHolder>
							</div>
						</asp:View>
						<asp:View ID="SSL" runat="server">
							<uc2:WebsitesSSL ID="WebsitesSSLControl" runat="server" />
						</asp:View>
					</asp:MultiView>
				</div>
			</td>
		</tr>
	</table>
</div>
<div class="FormFooter">
	<asp:Button ID="btnUpdate" runat="server" meta:resourcekey="btnUpdate" Text="Update"
		CssClass="Button1" OnClick="btnUpdate_Click" OnClientClick="ShowProgressDialog('Updating web site...');" />
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel"
		CssClass="Button1" CausesValidation="false" OnClick="btnCancel_Click" />
	<asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" Text="Delete"
		CssClass="Button1" CausesValidation="false" OnClientClick="return confirm('Delete this web site?');"
		OnClick="btnDelete_Click" />
</div>