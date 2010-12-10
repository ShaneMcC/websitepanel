<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Servers.ascx.cs" Inherits="WebsitePanel.Portal.Servers" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Add Server" CssClass="Button3" OnClick="btnAddItem_Click" /></td>
</div>

<asp:DataList ID="dlServers" Runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="10" HorizontalAlign="Center">
	<ItemStyle Height="170px" CssClass="BorderFillBox" VerticalAlign="Top"></ItemStyle>
	<ItemTemplate>
		<table width="160" cellpadding="3">
			<tr>
				<td class="Big">
					<asp:hyperlink id=lnkEdit runat="server" CssClass="Black" NavigateUrl='<%# EditUrl("ServerID", Eval("ServerID").ToString(), "edit_server") %>'
						Width=100% Height=100%>
						<%# Eval("ServerName") %>
					</asp:hyperlink>
				</td>
			</tr>
			<tr>
				<td class="Normal">
					<%# Eval("Comments") %>
				</td>
			</tr>
			<tr>
				<td align="center">
					<asp:DataList ID="dlServices" Runat="server" DataSource='<%# GetServerServices((int)Eval("ServerID")) %>' 
 CellPadding=4 CellSpacing=1 Width=70%>
						<ItemStyle CssClass="Brick" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<b>
								<asp:hyperlink id="lnkEditService" runat="server" NavigateUrl='<%# EditUrl("ServiceID", Eval("ServiceID").ToString(), "edit_service", "ServerID=" + Eval("ServerID").ToString()) %>' 
 Width=100% Height=100% ToolTip='<%# Eval("Comments") %>'>
									<%# Eval("ServiceName") %>
								</asp:hyperlink>
							</b>
						</ItemTemplate>
					</asp:DataList>
				</td>
			</tr>
		</table>
	</ItemTemplate>
</asp:DataList>

<table id="tblEmptyList" runat="server" cellpadding="10" cellspacing="0" width="100%">
    <tr>
        <td class="Normal" align="center">
            <asp:Label ID="lblEmptyList" runat="server" meta:resourcekey="lblEmptyList" Text="Empty list..."></asp:Label>
        </td>
    </tr>
</table>