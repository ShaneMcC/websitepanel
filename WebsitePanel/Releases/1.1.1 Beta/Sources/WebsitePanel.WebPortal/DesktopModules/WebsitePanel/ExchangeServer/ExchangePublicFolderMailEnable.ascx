<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangePublicFolderMailEnable.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangePublicFolderMailEnable" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="wsp" %>
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
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Create Public Folder"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
						
					<table>
						<tr id="EmailRow" runat="server">
							<td class="FormLabel150"><asp:Localize ID="locEmail" runat="server" meta:resourcekey="locEmail" Text="E-mail Address: *"></asp:Localize></td>
							<td>
                                <wsp:EmailAddress id="email" runat="server" ValidationGroup="CreateFolder">
                                </wsp:EmailAddress>
                            </td>
						</tr>
					</table>
					
				    <div class="FormFooterClean">
					    <asp:Button id="btnCreate" runat="server" Text="Create Public Folder" CssClass="Button1" meta:resourcekey="btnCreate" ValidationGroup="CreateFolder" OnClick="btnCreate_Click"></asp:Button>
					    <asp:Button id="btnCancel" runat="server" Text="Cancel" CssClass="Button1" meta:resourcekey="btnCancel" CausesValidation="false" OnClick="btnCancel_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateFolder" />
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>