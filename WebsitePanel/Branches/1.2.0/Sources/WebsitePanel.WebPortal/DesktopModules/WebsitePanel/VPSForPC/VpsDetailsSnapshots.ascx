<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsSnapshots.ascx.cs" Inherits="WebsitePanel.Portal.VPSForPC.VpsDetailsSnapshots" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/QuotaViewer.ascx" TagName="QuotaViewer" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="VpsContainer">
    <div class="Module">

	    <div class="Header">
		    <wsp:Breadcrumb id="breadcrumb" runat="server" />
	    </div>
    	
	    <div class="Left">
		    <wsp:Menu id="menu" runat="server" SelectedItem="" />
	    </div>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="Title">
				    <asp:Image ID="imgIcon" SkinID="Snapshot48" runat="server" />
				    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Snapshots" />
			    </div>
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_snapshots" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    
				    <table style="width:100%;">
				        <tr>
				            <td valign="top">
				            
                                <div class="FormButtonsBarClean">
                                    <asp:Button ID="btnTakeSnapshot" runat="server" meta:resourcekey="btnTakeSnapshot"
                                    Text="Take Snapshot" CssClass="Button1" onclick="btnTakeSnapshot_Click" />
                                </div>
                                <br />
                                
				                <asp:TreeView ID="SnapshotsTree" runat="server" 
                                    onselectednodechanged="SnapshotsTree_SelectedNodeChanged" ShowLines="True">
                                    <SelectedNodeStyle CssClass="SelectedTreeNode" />
                                    <Nodes>
                                    </Nodes>
                                    <NodeStyle CssClass="TreeNode" />
				                </asp:TreeView>
				                
				                <div id="NoSnapshotsPanel" runat="server" style="padding: 5px;">
				                    <asp:Localize ID="locNoSnapshots" runat="server" meta:resourcekey="locNoSnapshots" Text="No snapshots"></asp:Localize>
				                </div>
                                
                                <br />
				                <br />
				                <asp:Localize ID="locQuota" runat="server" meta:resourcekey="locQuota"
				                    Text="Number of Snapshots:"></asp:Localize>
				                &nbsp;&nbsp;&nbsp;
				                <wsp:QuotaViewer ID="snapshotsQuota" runat="server" QuotaTypeId="2" />
				    
				            </td>
				            <td valign="top" id="SnapshotDetailsPanel" runat="server">
				                <asp:Image ID="imgThumbnail" runat="server" Width="160" Height="120" />
				                <p>
				                    <asp:Localize ID="locCreated" runat="server" meta:resourcekey="locCreated"
				                        Text="Created:"></asp:Localize>
				                    <asp:Literal ID="litCreated" runat="server"></asp:Literal>
				                </p>
				                <ul class="ActionButtons">
				                    <li>
				                        <asp:LinkButton ID="btnApply" runat="server" CausesValidation="false" CssClass="ActionButtonApplySnapshot"
				                            meta:resourcekey="btnApply" Text="Apply" onclick="btnApply_Click"></asp:LinkButton>
				                    </li>
				                    <li>
				                        <asp:LinkButton ID="btnRename" runat="server" CausesValidation="false" CssClass="ActionButtonRename"
				                            meta:resourcekey="btnRename" Text="Rename"></asp:LinkButton>
				                    </li>
				                    <li>
				                        <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CssClass="ActionButtonDeleteSnapshot"
				                            meta:resourcekey="btnDelete" Text="Delete" onclick="btnDelete_Click"></asp:LinkButton>
				                    </li>
				                    <li>
				                        <asp:LinkButton ID="btnDeleteSubtree" runat="server" CausesValidation="false" CssClass="ActionButtonDeleteSnapshotTree"
				                            meta:resourcekey="btnDeleteSubtree" Text="Delete subtree" 
                                            onclick="btnDeleteSubtree_Click"></asp:LinkButton>
				                    </li>
				                </ul>
				            </td>
				        </tr>
				    </table>
				    
			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>

<asp:Panel ID="RenamePanel" runat="server" CssClass="Popup" style="display:none;">
	<table class="Popup-Header" cellpadding="0" cellspacing="0">
		<tr>
			<td class="Popup-HeaderLeft"></td>
			<td class="Popup-HeaderTitle">
				<asp:Localize ID="locRenameSnapshot" runat="server" Text="Rename snapshot"
				    meta:resourcekey="locRenameSnapshot"></asp:Localize>
			</td>
			<td class="Popup-HeaderRight"></td>
		</tr>
	</table>
	<div class="Popup-Content">
		<div class="Popup-Body">
			<br />
			
			<table cellspacing="10">
			    <tr>
			        <td>
			            <asp:TextBox ID="txtSnapshotName" runat="server" CssClass="NormalTextBox" Width="300"></asp:TextBox>
			            
			            <asp:RequiredFieldValidator ID="SnapshotNameValidator" runat="server" Text="*" Display="Dynamic"
                                ControlToValidate="txtSnapshotName" meta:resourcekey="SnapshotNameValidator" SetFocusOnError="true"
                                ValidationGroup="RenameSnapshot">*</asp:RequiredFieldValidator>
			        </td>
			    </tr>
			</table>
			
                                                
			<br />
		</div>
		
		<div class="FormFooter">
		    <asp:Button ID="btnRenameSnapshot" runat="server" CssClass="Button1"
		        meta:resourcekey="btnRenameSnapshot" Text="Rename" onclick="btnRenameSnapshot_Click"
                ValidationGroup="RenameSnapshot" />
		        
			<asp:Button ID="btnCancelRename" runat="server" CssClass="Button1"
			    meta:resourcekey="btnCancelRename" Text="Cancel" CausesValidation="false" />
		</div>
	</div>
</asp:Panel>

<ajaxToolkit:ModalPopupExtender ID="RenameSnapshotModal" runat="server" BehaviorID="RenameSnapshotModal"
	TargetControlID="btnRename" PopupControlID="RenamePanel"
	BackgroundCssClass="modalBackground" DropShadow="false" CancelControlID="btnCancelRename" />