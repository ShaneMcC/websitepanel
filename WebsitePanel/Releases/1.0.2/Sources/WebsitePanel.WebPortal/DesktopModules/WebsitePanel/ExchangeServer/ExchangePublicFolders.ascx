<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExchangePublicFolders.ascx.cs" Inherits="WebsitePanel.Portal.ExchangeServer.ExchangePublicFolders" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
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
					<asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Public Folders"></asp:Localize>
				</div>
				
				<div class="FormBody">
				    <wsp:SimpleMessageBox id="messageBox" runat="server" />
				    
                    <div class="FormButtonsBarClean">
                        <asp:Button ID="btnCreatePublicFolder" runat="server" meta:resourcekey="btnCreatePublicFolder"
                        Text="Create New Public Folder" CssClass="Button1" OnClick="btnCreatePublicFolder_Click" />
                    </div>
                    <br />
                    
				    <asp:TreeView ID="FoldersTree" runat="server">
				    </asp:TreeView>
				    <br />
				    <div style="text-align: center">
				        <asp:Button ID="btnDeleteFolders" runat="server" meta:resourcekey="btnDeleteFolders"
                            Text="Delete Selected Folders" CssClass="Button1" OnClick="btnDeleteFolders_Click" />
                    </div>
                    
                    <br />
				    <br />
				    <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota" Text="Total Public Folders Created:"></asp:Localize>
				    &nbsp;&nbsp;&nbsp;
				    <wsp:QuotaViewer ID="foldersQuota" runat="server" QuotaTypeId="2" />
				    
				    
				</div>
			</div>
			<div class="Right">
				<asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
			</div>
		</div>
	</div>
</div>