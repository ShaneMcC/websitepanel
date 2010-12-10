<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Peers.ascx.cs" Inherits="WebsitePanel.Portal.Peers" %>
<%@ Import Namespace="WebsitePanel.Portal" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Create Peer Account" CssClass="Button3" OnClick="btnAddItem_Click" />
</div>
<asp:GridView id="usersList" runat="server" AutoGenerateColumns="False"
	AllowPaging="True" AllowSorting="True" DataSourceID="odsUserPeers"
	EnableViewState="False" EmptyDataText="usersList"
	CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField SortExpression="Username" HeaderText="usersListUsername">
			<ItemTemplate>
				<asp:hyperlink id=lnkEdit runat="server" NavigateUrl='<%# EditUrl("PeerID", Eval("UserID").ToString(), "edit_peer", "UserID=" + PanelSecurity.SelectedUserId.ToString()) %>'>
					<%# Eval("Username") %>
				</asp:hyperlink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="FullName" SortExpression="FullName" HeaderText="usersListName">
            <ItemStyle Wrap="False" Width="50%" />
        </asp:BoundField>
		<asp:BoundField DataField="Email" SortExpression="Email" HeaderText="usersListEmail">
		    <HeaderStyle Wrap="False" />
		     <ItemStyle Wrap="False" Width="50%" />
		</asp:BoundField>
	</Columns>
</asp:GridView>
<asp:ObjectDataSource ID="odsUserPeers" runat="server"
    SelectMethod="GetUserPeers" TypeName="WebsitePanel.Portal.UsersHelper" OnSelected="odsUserPeers_Selected" MaximumRowsParameterName="" StartRowIndexParameterName="">
</asp:ObjectDataSource>