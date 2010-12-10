<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxPermissions.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxPermissions" %>
<%@ Register Src="UserControls/AccountsList.ascx" TagName="AccountsList" TagPrefix="uc2" %>
<%@ Register Src="UserControls/MailboxSelector.ascx" TagName="MailboxSelector" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/EmailAddress.ascx" TagName="EmailAddress" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="wsp" %>
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
                    <wsp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_permissions" />
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />                    					
					<wsp:CollapsiblePanel id="secSendAsPermission" runat="server"
                        TargetControlID="panelSendAsPermission" meta:resourcekey="secSendAsPermission" Text="Send As Permission">
                    </wsp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelSendAsPermission">
                        <asp:Label runat="server" ID="lblSendAsPermission" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="sendAsPermission" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true"  >
                        </uc2:AccountsList>                                           
                    </asp:Panel>
                                        
                    <wsp:CollapsiblePanel id="secFullAccessPermission" runat="server"
                        TargetControlID="panelFullAccessPermission" meta:resourcekey="secFullAccessPermission" Text="Full Access Permission">
                    </wsp:CollapsiblePanel>		
                    
                    <asp:Panel runat="server" ID="panelFullAccessPermission">
                        <asp:Label runat="server" ID="Label1" meta:resourcekey="grandPermission" /><br /><br />
                        <uc2:AccountsList id="fullAccessPermission" runat="server" MailboxesEnabled="true" EnableMailboxOnly = "true">
                        </uc2:AccountsList>                                            
                    </asp:Panel>
                    <div class="FormFooterClean">
					    <asp:Button id="btnSave" runat="server" Text="Save Changes" CssClass="Button1" meta:resourcekey="btnSave" ValidationGroup="EditMailbox" OnClick="btnSave_Click" ></asp:Button>					    
			        </div>
				</div>										
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>									
		</div>		
	</div>
</div>