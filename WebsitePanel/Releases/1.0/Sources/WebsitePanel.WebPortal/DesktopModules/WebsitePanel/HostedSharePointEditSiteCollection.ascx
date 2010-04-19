<%@ Control Language="C#" AutoEventWireup="true" Codebehind="HostedSharePointEditSiteCollection.ascx.cs"
	Inherits="WebsitePanel.Portal.HostedSharePointEditSiteCollection" %>
<%@ Register Src="ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<%@ Register Src="UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
	TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/DomainSelector.ascx" TagName="DomainSelector" TagPrefix="wsp" %>	
<%@ Register Src="ExchangeServer/UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="wsp" %>	


<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
	TagPrefix="wsp" %>

<%@ Register src="UserControls/QuotaEditor.ascx" tagname="QuotaEditor" tagprefix="uc1" %>

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
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="SharePoint Site Collection"></asp:Localize>
                    </div>
				<div class="FormBody">
					<wsp:SimpleMessageBox id="localMessageBox" runat="server">
                    </wsp:SimpleMessageBox>					
					<table id="tblEditItem" runat="server" cellspacing="0" cellpadding="5" width="100%">
						<tr>
							<td class="SubHead" nowrap width="200">
								<asp:Label ID="lblSiteCollectionUrl" runat="server" meta:resourcekey="lblSiteCollectionUrl"
									Text="Url:"></asp:Label>
							</td>
							<td width="100%" class="NormalBold">
								<wsp:DomainSelector id="domain" runat="server" ShowAt="false"/>								
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionOwner" runat="server" meta:resourcekey="lblSiteCollectionOwner"
									Text="Owner:"></asp:Label>
							</td>
							<td class="Normal">
								<wsp:UserSelector id="userSelector" IncludeMailboxes="true" runat="server"/>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionLocaleID" runat="server" meta:resourcekey="lblSiteCollectionLocaleID"
									Text="Locale ID:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:DropDownList ID="ddlLocaleID" runat="server" DataTextField="DisplayName" DataValueField="LCID" ></asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblMaxStorage" runat="server" meta:resourcekey="lblMaxStorage"></asp:Label>
							</td>
							<td class="Normal">																
                                <uc1:QuotaEditor ID="maxStorage" runat="server"  Width="200px" CssClass="NormalTextBox" QuotaTypeId="2" />
                                
                                </td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblWarningStorage" runat="server" meta:resourcekey="lblWarningStorage"></asp:Label>
							</td>
							<td class="Normal">																
                                <uc1:QuotaEditor ID="warningStorage" runat="server" Width="200px" QuotaTypeId="2" CssClass="NormalTextBox"/>                                
                                </td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblTitle" runat="server" meta:resourcekey="lblTitle" Text="Title:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:TextBox ID="txtTitle" runat="server" Width="200px" CssClass="NormalTextBox" ></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireTitle" runat="server" ErrorMessage="*"
									ControlToValidate="txtTitle" ValidationGroup="CreateSiteCollection"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblDescription" runat="server" meta:resourcekey="lblDescription" Text="Description:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:TextBox ID="txtDescription" runat="server" Width="200px" CssClass="NormalTextBox"
									TextMode="MultiLine" Rows="5"></asp:TextBox>
								<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*"
									ControlToValidate="txtDescription" ValidationGroup="CreateSiteCollection"></asp:RequiredFieldValidator>
							</td>
						</tr>
					</table>
					<table id="tblViewItem" runat="server" cellspacing="0" cellpadding="5" width="100%">
						<tr>
							<td class="SubHead" nowrap width="200">
								<asp:Label ID="lblSiteCollectionUrl2" runat="server" meta:resourcekey="lblSiteCollectionUrl"
									Text="Url:"></asp:Label>
							</td>
							<td width="100%" class="NormalBold">
								<span class="Huge">
								<asp:HyperLink runat="server" ID="lnkUrl" />								</span>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionOwner2" runat="server" meta:resourcekey="lblSiteCollectionOwner"
									Text="Owner:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litSiteCollectionOwner" runat="server"></asp:Literal>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblSiteCollectionLocaleID2" runat="server" meta:resourcekey="lblSiteCollectionLocaleID"
									Text="Locale ID:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litLocaleID" runat="server"></asp:Literal>
							</td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblMaxStorageView" runat="server" meta:resourcekey="lblMaxStorage"
									Text="Limit site storage to a maximum of:"></asp:Label>
							</td>
							<td class="Normal">								
								<uc1:QuotaEditor ID="editMaxStorage" runat="server" Width="200px" QuotaTypeId="2" CssClass="NormalTextBox"/>                                
								
							</td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblWarningStorageView" runat="server" meta:resourcekey="lblWarningStorage"
									Text="Send warning E-mail when site storage reaches:"></asp:Label>
							</td>
							<td class="Normal">								
								<uc1:QuotaEditor ID="editWarningStorage" runat="server" Width="200px" QuotaTypeId="2" CssClass="NormalTextBox"/>                                
							</td>
						</tr>
						
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblTitle2" runat="server" meta:resourcekey="lblTitle" Text="Title:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litTitle" runat="server"></asp:Literal>
							</td>
						</tr>
						<tr>
							<td class="SubHead">
								<asp:Label ID="lblDescription2" runat="server" meta:resourcekey="lblDescription"
									Text="Description:"></asp:Label>
							</td>
							<td class="Normal">
								<asp:Literal ID="litDescription" runat="server"></asp:Literal>
							</td>
						</tr>
					</table>
					<table width="100%">
						<tr>
							<td>
								<wsp:CollapsiblePanel id="secMainTools" runat="server" IsCollapsed="true" TargetControlID="ToolsPanel"
									meta:resourcekey="secMainTools" Text="SharePoint Site Collection Tools">
								</wsp:CollapsiblePanel>
								<asp:Panel ID="ToolsPanel" runat="server" Height="0" Style="overflow: hidden;">
									<table id="tblMaintenance" runat="server" cellpadding="10">
										<tr>
											<td>
												<asp:Button ID="btnBackup" runat="server" meta:resourcekey="btnBackup" CausesValidation="false"
													Text="Backup Site Collection" CssClass="Button3" OnClick="btnBackup_Click" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:Button ID="btnRestore" runat="server" meta:resourcekey="btnRestore" CausesValidation="false"
													Text="Restore Site Collection" CssClass="Button3" OnClick="btnRestore_Click" />
											</td>
										</tr>
									</table>
								</asp:Panel>
							</td>
						</tr>
					</table>
					<div class="FormFooterClean">
						<asp:Button ID="btnUpdate" runat="server"  CssClass="Button1"
							Text="Update" OnClick="btnUpdate_Click"  ValidationGroup="CreateSiteCollection" />
						<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1"
							CausesValidation="false" Text="Cancel" OnClick="btnCancel_Click" />
						<asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1"
							CausesValidation="false" Text="Delete" OnClientClick="return confirm('Delete Site?');"
							OnClick="btnDelete_Click" />
					</div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>
