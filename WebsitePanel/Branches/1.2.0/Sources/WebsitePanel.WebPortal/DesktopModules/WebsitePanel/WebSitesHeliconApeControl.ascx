<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesHeliconApeControl.ascx.cs"
	Inherits="WebsitePanel.Portal.WebSitesHeliconApeControl" %>
<%@ Import Namespace="WebsitePanel.Portal" %>

<div class="FormRow">
	<asp:Panel runat="server" ID="panelHeliconApeIsNotInstalledMessage" Visible="false">
		<p>
			<asp:Localize runat="server" meta:resourcekey="ApeModuleNotes" /></p>
	</asp:Panel>
	<asp:Panel runat="server" ID="panelHeliconApeIsNotEnabledMessage" Visible="false">
		<p>
			<asp:Localize runat="server" meta:resourcekey="ApeProductNotes" /></p>
	</asp:Panel>
	<asp:Button ID="btnToggleHeliconApe" runat="server" meta:resourcekey="btnToggleHeliconApe"
		Text="Enable Helicon Ape" CssClass="Button2" CausesValidation="false" OnClick="btnToggleHeliconApe_Click" />
	<div style="float: right;">
		<asp:HyperLink runat="server" Target="_blank" NavigateUrl="http://www.helicontech.com/ape/doc/wsp.htm"
			meta:resourcekey="ModuleHelpLink" />
	</div>
</div>
<asp:Panel ID="HeliconApeFoldersPanel" runat="server">
	<div class="FormButtonsBar">
		<asp:Button ID="btnAddHeliconApeFolder" runat="server" meta:resourcekey="btnAddHeliconApeFolder"
			Text="Add Folder" CssClass="Button2" CausesValidation="false" OnClick="btnAddHeliconApeFolder_Click" />
	</div>
	<asp:GridView ID="gvHeliconApeFolders" runat="server" EnableViewState="True" AutoGenerateColumns="false"
		ShowHeader="true" CssSelectorClass="NormalGridView" EmptyDataText="gvHeliconApeFolders"
		DataKeyNames="Path,ContentPath" OnRowDeleting="gvHeliconApeFolders_RowDeleting">
		<Columns>
			<asp:TemplateField HeaderText="gvHeliconApeFoldersName" ItemStyle-Width="100%">
				<ItemStyle CssClass="NormalBold"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditHeliconApeFolder" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessfolder", Eval("Path").ToString()) %>'>
			            <%# Eval("Path")%>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditHeliconApeFolderAuth" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessfolderauth", Eval("Path").ToString()) %>'
						title="Helicon Ape Folder Security Properties">
			            <image src="/App_Themes/Default/Images/shield.png" style="border: 0;" />
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:ImageButton ID="cmdDeleteHeliconApeFolder" runat="server" SkinID="DeleteSmall"
						CommandName="delete" CausesValidation="false" meta:resourcekey="cmdDeleteHeliconApeFolder"
						OnClientClick="return confirm('Delete?');"></asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<div class="FormButtonsBar">
		<asp:Button ID="btnAddHeliconApeUser" runat="server" meta:resourcekey="btnAddHeliconApeUser"
			Text="Add User" CssClass="Button2" CausesValidation="false" OnClick="btnAddHeliconApeUser_Click" />
	</div>
	<asp:GridView ID="gvHeliconApeUsers" runat="server" EnableViewState="True" AutoGenerateColumns="false"
		ShowHeader="true" CssSelectorClass="NormalGridView" EmptyDataText="gvHeliconApeUsers"
		DataKeyNames="Name" OnRowDeleting="gvHeliconApeUsers_RowDeleting">
		<Columns>
			<asp:TemplateField HeaderText="gvHeliconApeUsersName" ItemStyle-Width="100%">
				<ItemStyle CssClass="NormalBold"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditUser" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessuser", Eval("Name").ToString()) %>'>
			            <%# Eval("Name") %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:ImageButton ID="cmdDeleteUser" runat="server" SkinID="DeleteSmall" CommandName="delete"
						CausesValidation="false" meta:resourcekey="cmdDeleteUser" OnClientClick="return confirm('Delete?');">
					</asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
	<br />
	<div class="FormButtonsBar">
		<asp:Button ID="btnAddHeliconApeGroup" runat="server" meta:resourcekey="btnAddHeliconApeGroup"
			Text="Add Group" CssClass="Button2" CausesValidation="false" OnClick="btnAddHeliconApeGroup_Click" />
	</div>
	<asp:GridView ID="gvHeliconApeGroups" runat="server" EnableViewState="True" AutoGenerateColumns="false"
		ShowHeader="true" EmptyDataText="gvHeliconApeGroups" CssSelectorClass="NormalGridView"
		DataKeyNames="Name" OnRowDeleting="gvHeliconApeGroups_RowDeleting">
		<Columns>
			<asp:TemplateField HeaderText="gvHeliconApeGroupsName" ItemStyle-Width="100%">
				<ItemStyle CssClass="NormalBold"></ItemStyle>
				<ItemTemplate>
					<asp:HyperLink ID="lnkEditGroup" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_htaccessgroup", Eval("Name").ToString()) %>'>
			            <%# Eval("Name") %>
					</asp:HyperLink>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField>
				<ItemTemplate>
					<asp:ImageButton ID="cmdDeleteGroup" runat="server" SkinID="DeleteSmall" CommandName="delete"
						CausesValidation="false" meta:resourcekey="cmdDeleteGroup" OnClientClick="return confirm('Delete?');">
					</asp:ImageButton>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Panel>
