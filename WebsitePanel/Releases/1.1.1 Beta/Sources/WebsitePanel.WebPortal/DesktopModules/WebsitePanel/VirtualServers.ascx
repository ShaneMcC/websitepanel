<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServers.ascx.cs" Inherits="WebsitePanel.Portal.VirtualServers" %>
<div class="FormButtonsBar">
	<asp:Button ID="btnAddItem" runat="server" meta:resourcekey="btnAddItem" Text="Add Server" CssClass="Button3" OnClick="btnAddItem_Click" />
</div>

<asp:DataList ID="dlServers" Runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="20" HorizontalAlign="Center">
	<ItemStyle CssClass="BorderFillBox" Height="50px" VerticalAlign="Top"></ItemStyle>
	<ItemTemplate>
		<table width="160" cellpadding="3">
			<tr>
				<td class="Big">
					<asp:hyperlink id=lnkEdit CssClass="Black" runat="server" NavigateUrl='<%# EditUrl("ServerID", Eval("ServerID").ToString(), "edit_server") %>'
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
				<td class="Normal" align="center" style="border-top:1px solid #e0e0e0;"><%# Eval("ServicesNumber") %>
					<asp:Localize ID="locServices" runat="server" meta:resourcekey="locServices" Text="services" /></td>
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