<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesSecuredFoldersControl.ascx.cs" Inherits="WebsitePanel.Portal.WebSitesSecuredFoldersControl" %>
<%@ Import Namespace="WebsitePanel.Portal" %>

<div class="FormRow">
    <asp:Button id="btnToggleSecuredFolders" runat="server" meta:resourcekey="btnToggleSecuredFolders"
            Text="Enable Secured Folders" CssClass="Button2" CausesValidation="false"
            OnClick="btnToggleSecuredFolders_Click"/>
</div>

<asp:Panel ID="SecuredFoldersPanel" runat="server">
    <div class="FormButtonsBar">
        <asp:Button id="btnAddFolder" runat="server" meta:resourcekey="btnAddFolder"
            Text="Add Folder" CssClass="Button2" CausesValidation="false"
            OnClick="btnAddFolder_Click"/>
    </div>
    
    <asp:GridView id="gvFolders" Runat="server" EnableViewState="True" AutoGenerateColumns="false"
        ShowHeader="true"
        CssSelectorClass="NormalGridView" EmptyDataText="gvFolders"
        DataKeyNames="Path" OnRowDeleting="gvFolders_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="gvFoldersName" ItemStyle-Width="100%">
	            <ItemStyle CssClass="NormalBold"></ItemStyle>
	            <ItemTemplate>
		            <asp:hyperlink ID="lnkEditFolder" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_webfolder", Eval("Path").ToString()) %>'>
			            <%# Eval("Path")%>
		            </asp:hyperlink>
	            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <itemtemplate>
                    <asp:ImageButton ID="cmdDeleteFolder" runat="server" SkinID="DeleteSmall"
                        CommandName="delete" CausesValidation="false" meta:resourcekey="cmdDeleteFolder"
                        OnClientClick="return confirm('Delete?');"></asp:ImageButton>
                </itemtemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <br />
    <div class="FormButtonsBar">
        <asp:Button id="btnAddUser" runat="server" meta:resourcekey="btnAddUser"
            Text="Add User" CssClass="Button2" CausesValidation="false"
            OnClick="btnAddUser_Click"/>
    </div>
    <asp:GridView id="gvUsers" Runat="server" EnableViewState="True" AutoGenerateColumns="false"
        ShowHeader="true"
        CssSelectorClass="NormalGridView" EmptyDataText="gvUsers"
        DataKeyNames="Name" OnRowDeleting="gvUsers_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="gvUsersName" ItemStyle-Width="100%">
	            <ItemStyle CssClass="NormalBold"></ItemStyle>
	            <ItemTemplate>
		            <asp:hyperlink ID="lnkEditUser" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_webuser", Eval("Name").ToString()) %>'>
			            <%# Eval("Name") %>
		            </asp:hyperlink>
	            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <itemtemplate>
                    <asp:ImageButton ID="cmdDeleteUser" runat="server" SkinID="DeleteSmall"
                        CommandName="delete" CausesValidation="false" meta:resourcekey="cmdDeleteUser"
                        OnClientClick="return confirm('Delete?');"></asp:ImageButton>
                </itemtemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <br />
    <div class="FormButtonsBar">
        <asp:Button id="btnAddGroup" runat="server" meta:resourcekey="btnAddGroup"
            Text="Add Group" CssClass="Button2" CausesValidation="false"
            OnClick="btnAddGroup_Click"/>
    </div>
    <asp:GridView id="gvGroups" Runat="server" EnableViewState="True" AutoGenerateColumns="false"
        ShowHeader="true"
        EmptyDataText="gvGroups" CssSelectorClass="NormalGridView"
        DataKeyNames="Name" OnRowDeleting="gvGroups_RowDeleting">
        <Columns>
            <asp:TemplateField HeaderText="gvGroupsName" ItemStyle-Width="100%">
	            <ItemStyle CssClass="NormalBold"></ItemStyle>
	            <ItemTemplate>
		            <asp:hyperlink ID="lnkEditGroup" runat="server" NavigateUrl='<%# GetEditControlUrl("edit_webgroup", Eval("Name").ToString()) %>'>
			            <%# Eval("Name") %>
		            </asp:hyperlink>
	            </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <itemtemplate>
                    <asp:ImageButton ID="cmdDeleteGroup" runat="server" SkinID="DeleteSmall"
                        CommandName="delete" CausesValidation="false" meta:resourcekey="cmdDeleteGroup"
                        OnClientClick="return confirm('Delete?');"></asp:ImageButton>
                </itemtemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Panel>