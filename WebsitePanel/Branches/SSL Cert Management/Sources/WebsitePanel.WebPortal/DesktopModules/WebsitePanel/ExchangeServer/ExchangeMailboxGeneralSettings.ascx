<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxGeneralSettings.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxGeneralSettings" %>
<%@ Register Src="UserControls/CountrySelector.ascx" TagName="CountrySelector" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
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
                    <wsp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_settings" />
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtDisplayName" runat="server" CssClass="HugeTextBox200"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									ErrorMessage="Enter Display Name" ValidationGroup="EditMailbox" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150" valign="top"><asp:Localize ID="locPassword" runat="server" meta:resourcekey="locPassword" Text="Password:"></asp:Localize></td>
							<td>
                                <wsp:PasswordControl id="password" runat="server" ValidationGroup="EditMailbox">
                                </wsp:PasswordControl>
                            </td>
						</tr>
						<tr>
						    <td></td>
						    <td>
						        <asp:CheckBox ID="chkHideAddressBook" runat="server" meta:resourcekey="chkHideAddressBook" Text="Hide from Address Book" />
						    </td>
						</tr>
						<tr>
						    <td></td>
						    <td>
						        <asp:CheckBox ID="chkDisable" runat="server" meta:resourcekey="chkDisable" Text="Disable Mailbox" />
						        <br />
						        <br />
						    </td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locFirstName" runat="server" meta:resourcekey="locFirstName" Text="First Name:"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtFirstName" runat="server" CssClass="TextBox100"></asp:TextBox>
								&nbsp;
								<asp:Localize ID="locInitials" runat="server" meta:resourcekey="locInitials" Text="Initials:" />
								<asp:TextBox ID="txtInitials" runat="server"  MaxLength="6" CssClass="TextBox100"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locLastName" runat="server" meta:resourcekey="locLastName" Text="Last Name:"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtLastName" runat="server" CssClass="TextBox100"></asp:TextBox>
							</td>
						</tr>
					</table>
					
					<wsp:CollapsiblePanel id="secCompanyInfo" runat="server"
                        TargetControlID="CompanyInfo" meta:resourcekey="secCompanyInfo" Text="Company Information">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="CompanyInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locJobTitle" runat="server" meta:resourcekey="locJobTitle" Text="Job Title:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtJobTitle" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locCompany" runat="server" meta:resourcekey="locCompany" Text="Company:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtCompany" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locDepartment" runat="server" meta:resourcekey="locDepartment" Text="Department:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtDepartment" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locOffice" runat="server" meta:resourcekey="locOffice" Text="Office:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtOffice" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locManager" runat="server" meta:resourcekey="locManager" Text="Manager:"></asp:Localize></td>
							    <td>
                                    <wsp:MailboxSelector id="manager" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true" />
                                </td>
						    </tr>
					    </table>
					</asp:Panel>
					
					
					<wsp:CollapsiblePanel id="secContactInfo" runat="server"
                        TargetControlID="ContactInfo" meta:resourcekey="secContactInfo" Text="Contact Information">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="ContactInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locBusinessPhone" runat="server" meta:resourcekey="locBusinessPhone" Text="Business Phone:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtBusinessPhone" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locFax" runat="server" meta:resourcekey="locFax" Text="Fax:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtFax" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locHomePhone" runat="server" meta:resourcekey="locHomePhone" Text="Home Phone:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtHomePhone" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locMobilePhone" runat="server" meta:resourcekey="locMobilePhone" Text="Mobile Phone:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtMobilePhone" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locPager" runat="server" meta:resourcekey="locPager" Text="Pager:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtPager" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locWebPage" runat="server" meta:resourcekey="locWebPage" Text="Web Page:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtWebPage" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<wsp:CollapsiblePanel id="secAddressInfo" runat="server"
                        TargetControlID="AddressInfo" meta:resourcekey="secAddressInfo" Text="Address">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="AddressInfo" runat="server" Height="0" style="overflow:hidden;">
					    <table>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locAddress" runat="server" meta:resourcekey="locAddress" Text="Street Address:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtAddress" runat="server" CssClass="TextBox200" Rows="2" TextMode="MultiLine"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locCity" runat="server" meta:resourcekey="locCity" Text="City:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtCity" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locState" runat="server" meta:resourcekey="locState" Text="State/Province:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtState" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locZip" runat="server" meta:resourcekey="locZip" Text="Zip/Postal Code:"></asp:Localize></td>
							    <td>
								    <asp:TextBox ID="txtZip" runat="server" CssClass="TextBox200"></asp:TextBox>
							    </td>
						    </tr>
						    <tr>
							    <td class="FormLabel150"><asp:Localize ID="locCountry" runat="server" meta:resourcekey="locCountry" Text="Country/Region:"></asp:Localize></td>
							    <td>
									<wsp:CountrySelector id="country" runat="server">
									</wsp:CountrySelector>
								</td>
						    </tr>
					    </table>
					</asp:Panel>
					
					<table>
					    <tr>
						    <td class="FormLabel150"><asp:Localize ID="locNotes" runat="server" meta:resourcekey="locNotes" Text="Notes:"></asp:Localize></td>
						    <td>
							    <asp:TextBox ID="txtNotes" runat="server" CssClass="TextBox200" Rows="4" TextMode="MultiLine"></asp:TextBox>
						    </td>
					    </tr>
					</table>
					
					<table style="width:100%;margin-top:10px;">
					    <tr>
					        <td align="center">
					            <asp:CheckBox ID="chkPmmAllowed" Visible="false" runat="server" meta:resourcekey="chkPmmAllowed" AutoPostBack="true"
					                Text="Allow these settings to be managed from Personal Mailbox Manager" OnCheckedChanged="chkPmmAllowed_CheckedChanged" />
					        </td>
					    </tr>
					</table>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1"
							meta:resourcekey="btnSave" ValidationGroup="EditMailbox" OnClick="btnSave_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="EditMailbox" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>