<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HostedSharePointStorageUsage.ascx.cs" Inherits="WebsitePanel.Portal.HostedSharePointStorageUsage" %>
<%@ Register Src="ExchangeServer/UserControls/Breadcrumb.ascx" TagName="Breadcrumb"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport"
    TagPrefix="wsp" %>
<%@ Register Src="UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel"
    TagPrefix="wsp" %>
<%@ Register Src="ExchangeServer/UserControls/SizeBox.ascx" TagName="SizeBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="wsp" %>

<%@ Register Src="ExchangeServer/UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>

<div id="ExchangeContainer">
	<div class="Module">
		<div class="Header">
			<wsp:Breadcrumb id="breadcrumb" runat="server" PageName="Text.PageName" />
		</div>
		<div class="Left">
			<wsp:Menu id="menu" runat="server" SelectedItem="storage_limits" />
            </div>
		<div class="Content">
			<div class="Center">
				<div class="Title">
					<asp:Image ID="Image1" SkinID="ExchangeStorageConfig48" runat="server" />
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Storage Usage"></asp:Localize>
				</div>
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
					<wsp:CollapsiblePanel id="secSiteCollectionsReport" runat="server"
                        TargetControlID="siteCollectionsReport" meta:resourcekey="secSiteCollectionsReport" Text="Site Collections">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="siteCollectionsReport" runat="server" Height="0" style="overflow:hidden;">
				        <asp:GridView ID="gvStorageUsage" runat="server" AutoGenerateColumns="False" meta:resourcekey="gvStorageUsage"
					        Width="100%" EmptyDataText="gvSiteCollections" CssSelectorClass="NormalGridView">
					        <Columns>
						        <asp:BoundField meta:resourcekey="gvSiteCollectionName" DataField="Url" />
						        <asp:BoundField meta:resourcekey="gvSiteCollectionSize" DataField="DiskSpace" />						        
					        </Columns>
				        </asp:GridView>
				        <br />
			            <table cellpadding="2">
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalboxItems" runat="server" meta:resourcekey="locTotalMailboxItems" ></asp:Localize></td>
					            <td><asp:Label ID="lblTotalItems" runat="server" CssClass="NormalBold">177</asp:Label></td>
					        </tr>
					        <tr>
					            <td class="FormLabel150"><asp:Localize ID="locTotalMailboxesSize" runat="server" meta:resourcekey="locTotalMailboxesSize" ></asp:Localize></td>
					            <td><asp:Label ID="lblTotalSize" runat="server" CssClass="NormalBold">100</asp:Label></td>
					        </tr>
				        </table>
				        <br />
				    </asp:Panel>                   										                    								    
				
				
				<div class="FormFooterClean">
					    <asp:Button id="btnRecalculateDiscSpace" runat="server" Text="Recalculate Disk Space" CssClass="Button1" meta:resourcekey="btnRecalculateDiscSpace" OnClick="btnRecalculateDiscSpace_Click" ></asp:Button>						
				    </div>
				</div>
				
			</div>
			
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>