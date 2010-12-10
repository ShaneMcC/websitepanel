<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeCreateDistributionList.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeCreateDistributionList" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<%@ Register src="UserControls/MailboxSelector.ascx" tagname="MailboxSelector" tagprefix="wsp" %>

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
					<asp:Image ID="Image1" SkinID="ExchangeListAdd48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create Distribution List"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
					
					<table>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locDisplayName" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
							<td>
								<asp:TextBox ID="txtDisplayName" runat="server" CssClass="HugeTextBox200"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valRequireDisplayName" runat="server" meta:resourcekey="valRequireDisplayName" ControlToValidate="txtDisplayName"
									ErrorMessage="Enter Display Name" ValidationGroup="CreateList" Display="Dynamic" Text="*" SetFocusOnError="True"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td class="FormLabel150"><asp:Localize ID="locAccount" runat="server" meta:resourcekey="locAccount" Text="E-mail Address: *"></asp:Localize></td>
							<td>
                                <wsp:EmailAddress id="email" runat="server" ValidationGroup="CreateList">
                                </wsp:EmailAddress>
                            </td>
						</tr>
						
						<tr>
							<td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locManagedBy" ></asp:Localize></td>
							<td>                                
                                 <wsp:mailboxselector id="manager" runat="server"
											MailboxesEnabled="true"
											ContactsEnabled="true"
											DistributionListsEnabled="true" />											
											<asp:CustomValidator runat="server" 
                                     ValidationGroup="CreateList"  meta:resourcekey="valManager" ID="valManager" 
                                     onservervalidate="valManager_ServerValidate" />
                            </td>
						</tr>
					</table>
				    <div class="FormFooterClean">
					    <asp:Button id="btnCreate" runat="server" Text="Create Distribution List" CssClass="Button1" meta:resourcekey="btnCreate" ValidationGroup="CreateList" OnClick="btnCreate_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateList" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>