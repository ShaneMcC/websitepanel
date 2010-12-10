<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeCreateMailbox.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeCreateMailbox" %>
<%@ Register Src="../UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/UserSelector.ascx" TagName="UserSelector" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
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
					<asp:Image ID="Image1" SkinID="ExchangeMailboxAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create Mailbox"></asp:Localize>
				</div>
				
				<div class="FormBody" width="100%">
				    
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
					<asp:UpdatePanel ID="UpdatePanel1" 
                             UpdateMode="Conditional"
                             runat="server">
                     <ContentTemplate>

					    <table width="100%">
					    <tr>
					        <td width="100%">
					            <asp:Literal id="TopComments" runat="server" meta:resourcekey="TopComments" />
					        </td>
					    </tr>
					    <tr>
					        <td width="100%">
					            <asp:RadioButton runat="server" ID="rbtnCreateNewMailbox" meta:resourcekey="rbtnCreateNewMailbox" AutoPostBack="true" Checked="true" GroupName="CreateMailboxGoup" OnCheckedChanged="rbtnCreateNewMailbox_CheckedChanged" />					            					            
					        </td>
					    </tr>
					    <tr>
					        <td width="100%" style="border-bottom: solid 1px #DFDFDF;  ">
					            <asp:RadioButton runat="server" ID="rbtnUserExistingUser" meta:resourcekey="rbtnUserExistingUser" AutoPostBack="true"  GroupName="CreateMailboxGoup" OnCheckedChanged="rbtnUserExistingUser_CheckedChanged"  />					            
    			            </td>
					    </tr>					  
					   </table>
					   
					    
						<table id="NewUserTable"  runat="server">
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtDisplayName" runat="server" CssClass="HugeTextBox200"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									ErrorMessage="Enter Display Name" ValidationGroup="CreateMailbox" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locAccount" runat="server" meta:resourcekey="locAccount" Text="E-mail Address: *"></asp:Localize></td>
							<td>
                                <wsp:EmailAddress id="email" runat="server" ValidationGroup="CreateMailbox">
                                </wsp:EmailAddress>
                            </td>
						</tr>
						<tr>
							<td class="FormLabel150" valign="top"><asp:Localize ID="locPassword" runat="server" meta:resourcekey="locPassword" Text="Password: *"></asp:Localize></td>
							<td>
                                <wsp:PasswordControl id="password" runat="server" ValidationGroup="CreateMailbox" AllowGeneratePassword="true">
                                </wsp:PasswordControl>
                            </td>
						</tr>
						<tr>
							<td class="FormLabel150" valign="top"><asp:Localize ID="locMailboxType" runat="server" meta:resourcekey="locMailboxType" Text="Choose mailbox type:"></asp:Localize></td>
							<td>
							    <asp:RadioButtonList ID="rbMailboxType" runat="server">
							        <asp:ListItem Value="1" Selected="true" meta:resourcekey="UserMailbox">User mailbox</asp:ListItem>
							        <asp:ListItem Value="5" meta:resourcekey="RoomMailbox">Room mailbox</asp:ListItem>
							        <asp:ListItem Value="6" meta:resourcekey="EquipmentMailbox">Equipment mailbox</asp:ListItem>
							    </asp:RadioButtonList>
                            </td>
						</tr>
					</table>					
					<table id="ExistingUserTable" visible="false"  runat="server"> 					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
					        <td><uc1:UserSelector id="userSelector" runat="server"></uc1:UserSelector></td>
					    </tr>
					</table>
					</ContentTemplate>
					</asp:UpdatePanel>
					
				
					    <table>
					        <tr>						        
						        <td class="FormLabel150">						        
						            <asp:CheckBox ID="chkSendInstructions"  runat="server" meta:resourcekey="chkSendInstructions" Text="Send Setup Instructions" Checked="true" />
						        </td>
						        <td><wsp:EmailControl id="sendInstructionEmail" runat="server" RequiredEnabled="true" ValidationGroup="CreateMailbox"></wsp:EmailControl></td>
						        						        
						    </tr>						    					
					    </table>
					
					
					
					<div class="FormFooterClean">
					    <asp:Button id="btnCreate" runat="server" Text="Create Mailbox"
					    CssClass="Button1" meta:resourcekey="btnCreate" ValidationGroup="CreateMailbox"
					    OnClick="btnCreate_Click"
					    OnClientClick="ShowProgressDialog('Creating mailbox...');"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateMailbox" />
				    </div>
				</div>
				
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>