<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeActiveSyncSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeActiveSyncSettings" %>
<%@ Register Src="UserControls/HoursBox.ascx" TagName="HoursBox" TagPrefix="wsp" %>
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
			<wsp:Menu id="menu" runat="server" SelectedItem="activesync_policy" />
		</div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeActiveSyncConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="ActiveSync Mailbox Policy"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
				    <table>
					    <tr>
						    <td class="Normal" colspan="2">
						        <asp:CheckBox ID="chkAllowNonProvisionable" runat="server"
						            meta:resourcekey="chkAllowNonProvisionable" Text="Allow Non-provisionable devices" />
						    </td>
						</tr>
						<tr>
						    <td nowrap><asp:Label meta:resourcekey="lblRefreshInterval" runat="server" ID="lblRefreshInterval" /></td>
						    <td><wsp:HoursBox id="hoursRefreshInterval" runat="server"  ValidationGroup="EditMailbox">
                                </wsp:HoursBox>
				            </td>
						</tr>
					</table>
                    
					<wsp:CollapsiblePanel id="secApplication" runat="server"
                        TargetControlID="ApplicationPanel" meta:resourcekey="secApplication" Text="Application">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ApplicationPanel" runat="server" Height="0" style="overflow:hidden;">
                        <table>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkAllowAttachments" runat="server"
						                meta:resourcekey="chkAllowAttachments" Text="Allow attachments to be downloaded to device" />
						        </td>
						    </tr>
						    <tr>
							    <td class="FormLabel200" align="right">
							        <asp:Localize ID="locMaxAttachmentSize" runat="server"
							            meta:resourcekey="locMaxAttachmentSize" Text="Maximum attachment size:"></asp:Localize></td>
							    <td>
									<wsp:SizeBox id="sizeMaxAttachmentSize" runat="server" ValidationGroup="EditMailbox" />
								</td>
						    </tr>
						</table>
						<br />
				    </asp:Panel>
				    
				    
					<wsp:CollapsiblePanel id="secWSS" runat="server"
                        TargetControlID="WSSPanel" meta:resourcekey="secWSS" Text="WSS/UNC">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="WSSPanel" runat="server" Height="0" style="overflow:hidden;">
				        <table>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkWindowsFileShares" runat="server"
						                meta:resourcekey="chkWindowsFileShares" Text="Windows File Shares" />
						        </td>
						    </tr>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkWindowsSharePoint" runat="server"
						                meta:resourcekey="chkWindowsSharePoint" Text="Windows SharePoint Services" />
						        </td>
						    </tr>
					    </table>
					    <br />
				    </asp:Panel>
				    
				    
					<wsp:CollapsiblePanel id="secPassword" runat="server"
                        TargetControlID="PasswordPanel" meta:resourcekey="secPassword" Text="Password">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="PasswordPanel" runat="server" Height="0" style="overflow:hidden;">
                    
						<asp:UpdatePanel ID="PasswordUpdatePanel" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
							<ContentTemplate>
							
				        <table>
					        <tr>
						        <td class="Normal" colspan="2">
						            <asp:CheckBox ID="chkRequirePasword" runat="server"
						                meta:resourcekey="chkRequirePasword" Text="Require password" AutoPostBack="true" OnCheckedChanged="chkRequirePasword_CheckedChanged" />
						        </td>
						    </tr>
					        <tr id="PasswordSettingsRow" runat="server">
						        <td class="Normal" colspan="2" style="padding-left: 20px;">

				                    <table>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkRequireAlphaNumeric" runat="server"
						                            meta:resourcekey="chkRequireAlphaNumeric" Text="Require alphnumeric password" />
						                    </td>
						                </tr>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkEnablePasswordRecovery" runat="server"
						                            meta:resourcekey="chkEnablePasswordRecovery" Text="Enable Password Recovery" />
						                    </td>
						                </tr>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkRequireEncryption" runat="server"
						                            meta:resourcekey="chkRequireEncryption" Text="Require encryption on device" />
						                    </td>
						                </tr>
					                    <tr>
						                    <td class="Normal" colspan="2">
						                        <asp:CheckBox ID="chkAllowSimplePassword" runat="server"
						                            meta:resourcekey="chkAllowSimplePassword" Text="Allow simple password" />
						                    </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locNumberAttempts" runat="server"
							                        meta:resourcekey="locNumberAttempts" Text="Number of failed attempts allowed:"></asp:Localize></td>
							                <td>
									            <wsp:SizeBox id="sizeNumberAttempts" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false" />
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locMinimumPasswordLength" runat="server"
							                        meta:resourcekey="locMinimumPasswordLength" Text="Minimum password length:"></asp:Localize></td>
							                <td>
									            <wsp:SizeBox id="sizeMinimumPasswordLength" runat="server" ValidationGroup="EditMailbox"
									                DisplayUnits="false" EmptyValue="0" />
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locTimeReenter" runat="server"
							                        meta:resourcekey="locTimeReenter" Text="Time without user input before password must be re-entered:"></asp:Localize></td>
							                <td>
									            <wsp:SizeBox id="sizeTimeReenter" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false" />
									            <asp:Localize ID="locMinutes" runat="server"
							                        meta:resourcekey="locMinutes" Text="minutes"></asp:Localize>
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locPasswordExpiration" runat="server"
							                        meta:resourcekey="locPasswordExpiration" Text="Password expiration:"></asp:Localize></td>
							                <td>
									            <wsp:SizeBox id="sizePasswordExpiration" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false" />
									            <asp:Localize ID="locDays" runat="server"
							                        meta:resourcekey="locDays" Text="days"></asp:Localize>
								            </td>
						                </tr>
						                <tr>
							                <td class="FormLabel200">
							                    <asp:Localize ID="locPasswordHistory" runat="server"
							                        meta:resourcekey="locPasswordHistory" Text="Enforce password history:"></asp:Localize></td>
							                <td>
									            <wsp:SizeBox id="sizePasswordHistory" runat="server" ValidationGroup="EditMailbox" DisplayUnits="false" RequireValidatorEnabled="true" />
								            </td>
						                </tr>
					                </table>

						        </td>
						    </tr>
					    </table>
					    
					        </ContentTemplate>
					    </asp:UpdatePanel>
					    <br />
				    </asp:Panel>
				    
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave"
							 OnClientClick="ShowProgressDialog('Updating settings...');" ValidationGroup="EditMailbox" OnClick="btnSave_Click"></asp:Button>
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