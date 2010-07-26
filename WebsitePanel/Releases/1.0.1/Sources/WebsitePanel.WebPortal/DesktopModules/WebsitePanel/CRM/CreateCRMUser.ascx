<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateCRMUser.ascx.cs" Inherits="WebsitePanel.Portal.CRM.CreateCRMUser" %>
<%@ Register Src="../UserControls/EmailControl.ascx" TagName="EmailControl" TagPrefix="wsp" %>

<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PasswordControl.ascx" TagName="PasswordControl" TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="../ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<%@ Register src="../ExchangeServer/UserControls/UserSelector.ascx" tagname="UserSelector" tagprefix="wsp" %>

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
										  					   					    							
					<table id="ExistingUserTable"   runat="server"> 					    
					    <tr>
					        <td class="FormLabel150"><asp:Localize ID="Localize1" runat="server" meta:resourcekey="locDisplayName" Text="Display Name: *"></asp:Localize></td>
					        <td><wsp:UserSelector id="userSelector" runat="server" IncludeMailboxes="true"></wsp:UserSelector></td>
					    </tr>
					    
					    <tr>
					        <td class="FormLabel150">
					            <asp:Localize ID="Localize2" runat="server" meta:resourcekey="locBusinessUnit" Text="Business Unit:"></asp:Localize>
					        </td>
					        <td>
					            <asp:DropDownList runat="server" ID="ddlBusinessUnits" />
					        </td>
					    </tr>
					    
					</table>																			  					
					
					<div class="FormFooterClean">
					    <asp:Button id="btnCreate" runat="server" Text="Create CRM User"
					    CssClass="Button1" meta:resourcekey="btnCreate" ValidationGroup="CreateCRMUser"					    
					    OnClientClick="ShowProgressDialog('Creating CRM user...');" onclick="btnCreate_Click"></asp:Button>
					    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True" ShowSummary="False" ValidationGroup="CreateMailbox" />
				    </div>
				</div>
				
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>