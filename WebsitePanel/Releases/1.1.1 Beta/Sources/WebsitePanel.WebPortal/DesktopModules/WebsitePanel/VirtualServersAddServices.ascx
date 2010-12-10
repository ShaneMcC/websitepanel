<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VirtualServersAddServices.ascx.cs" Inherits="WebsitePanel.Portal.VirtualServersAddServices" %>
<div class="FormButtonsBar">
    <asp:Button ID="btnAdd" runat="server" meta:resourcekey="btnAdd" CssClass="Button2" Text="Add" OnClick="btnAdd_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" Text="Cancel" OnClick="btnCancel_Click" />
</div>
<div class="FormBody">
    <asp:DataList ID="dlServers" Runat="server" RepeatColumns="3" RepeatDirection="Horizontal" CellSpacing="15">
	    <ItemStyle Height="120px" CssClass="BorderFillBox" VerticalAlign="Top"></ItemStyle>
	    <ItemTemplate>
		    <table width="160" cellpadding="3">
			    <tr>
				    <td class="Big">
					    <%# Eval("ServerName") %>
				    </td>
			    </tr>
			    <tr>
				    <td class="Normal">
					    <%# Eval("Comments") %>
				    </td>
			    </tr>
			    <tr style="border-top:1px solid #d0d0d0;">
				    <td align="center">
					    <asp:DataList ID="dlServices" Runat="server" DataSource='<%# GetServerServices((int)Eval("ServerID")) %>'
					    CellPadding="1" CellSpacing="1" Width="50%" DataKeyField="ServiceID">
						    <ItemStyle CssClass="Brick" HorizontalAlign="Left" Wrap="false"></ItemStyle>
						    <ItemTemplate>
			                     <asp:CheckBox ID="chkSelected" runat="server" Text='<%# Eval("ServiceName") %>' CssClass="NormalBold" Width="100%" />
						    </ItemTemplate>
					    </asp:DataList>
				    </td>
			    </tr>
		    </table>
	    </ItemTemplate>
    </asp:DataList>
</div>