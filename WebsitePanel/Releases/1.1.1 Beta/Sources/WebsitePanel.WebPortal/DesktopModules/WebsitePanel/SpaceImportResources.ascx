<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceImportResources.ascx.cs" Inherits="WebsitePanel.Portal.SpaceImportResources" %>
<%@ Register Src="UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>
<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<div class="FormBody">
	<asp:treeview runat="server" id="tree" populatenodesfromclient="true" NodeIndent="10"
		showexpandcollapse="true" expanddepth="0" ontreenodepopulate="tree_TreeNodePopulate">
		<LevelStyles>
		    <asp:TreeNodeStyle CssClass="FileManagerTreeNode" />
		    <asp:TreeNodeStyle CssClass="FileManagerTreeNode" />
		</LevelStyles>
	</asp:treeview>
</div>
<div class="FormFooter">
    <asp:Button ID="btnImport" runat="server" meta:resourcekey="btnImport" CssClass="Button1" Text="Import Resources" OnClientClick="return confirm('Proceed?');" OnClick="btnImport_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" />
</div>