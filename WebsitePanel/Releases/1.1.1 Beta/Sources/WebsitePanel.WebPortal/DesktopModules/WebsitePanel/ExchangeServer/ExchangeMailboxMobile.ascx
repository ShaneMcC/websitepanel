<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangeMailboxMobile.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangeMailboxMobile" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/MailboxTabs.ascx" TagName="MailboxTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>


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
                    <wsp:MailboxTabs id="tabs" runat="server" SelectedTab="mailbox_mobile" />
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
                    <asp:GridView ID="gvMobile" runat="server" AutoGenerateColumns="False" EnableViewState="true"
					    Width="100%" EmptyDataText="gvUsers" CssSelectorClass="NormalGridView"
					    AllowSorting="False" onrowcommand="gvMobile_RowCommand" 
                        onrowdatabound="gvMobile_RowDataBound" meta:resourcekey="gvMobile" 
                        onrowdeleting="gvMobile_RowDeleting" onrowediting="gvMobile_RowEditing">
					    <Columns>						     						   						    
						    <asp:TemplateField HeaderText=""  SortExpression="DeviceUserAgent" meta:resourcekey="deviceUserAgentColumn" >
							    <ItemStyle Width="50%"></ItemStyle>
							    <ItemTemplate>							       
								    <asp:hyperlink id="lnkDeviceUserAgent" runat="server" NavigateUrl='<%# GetEditUrl(Eval("Id").ToString()) %>' >
									    <%# Eval("DeviceUserAgent") %>
								    </asp:hyperlink>
							    </ItemTemplate>
						    </asp:TemplateField>
						    <asp:BoundField HeaderText="" DataField="DeviceType" ItemStyle-Width="50%" meta:resourcekey="deviceTypeColumn" />
						    	   
						    <asp:TemplateField HeaderText="" SortExpression="LastSuccessSync" meta:resourcekey="lastSyncTimeColumn" >
							    <ItemStyle Width="50%" Wrap="false"></ItemStyle>
							    <ItemTemplate>							       
								    <asp:Label runat="server" ID="lblLastSyncTime" Text='<%# Eval("LastSuccessSync") %>' />								    
							    </ItemTemplate>
						    </asp:TemplateField>
						    
						   <asp:TemplateField HeaderText="" SortExpression="Status"  meta:resourcekey="deviceStatus" >
							    <ItemStyle Width="50%"  Wrap="false"></ItemStyle>
							    <ItemTemplate>							       
								     <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status") %>' />								    
							    </ItemTemplate>
						    </asp:TemplateField>
						    
						    <asp:TemplateField>
							    <ItemTemplate>
								    <asp:ImageButton ID="cmdDelete" runat="server" Text="Delete" SkinID="ExchangeDelete"
									    CommandName="Delete"  CommandArgument='<%# Eval("ID") %>'
									    meta:resourcekey="cmdDelete" OnClientClick="return confirm('Remove this item?');"></asp:ImageButton>
							    </ItemTemplate>
						    </asp:TemplateField>
					    </Columns>
				    </asp:GridView>
					
				    <div class="FormFooterClean">
					   
				    </div>
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="HSFormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>