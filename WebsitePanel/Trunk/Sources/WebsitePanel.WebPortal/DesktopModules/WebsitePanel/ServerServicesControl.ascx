<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerServicesControl.ascx.cs" Inherits="WebsitePanel.Portal.ServerServicesControl" %>
<asp:datalist id="dlServiceGroups" CellPadding="0" CellSpacing="5" Width="100%" RepeatDirection="Vertical"
	Runat="server" DataKeyField="GroupID">
	<ItemTemplate>
		<table width="100%" class="BorderFillBox">
			<tr>
				<td class="NormalBold" valign="top" width="140" nowrap>
					<%# GetSharedLocalizedString("ResourceGroup." + (string)Eval("GroupName")) %>
				</td>

				<td align="right" valign="top" class="Normal">
				    <asp:hyperlink id="lnkAddService" runat="server"
				        NavigateUrl='<%# EditServiceUrl("GroupID", Eval("GroupID").ToString(), "add_service") %>'
				        Text="Add" meta:resourcekey="lnkAddService">
					</asp:hyperlink>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:DataList ID="dlServices" Runat="server" RepeatDirection="Vertical"
					    DataSource='<%# GetGroupServices((int)Eval("GroupID")) %>'
					    CellPadding="4" CellSpacing="1" Width="100%">
						<ItemStyle CssClass="Brick" HorizontalAlign="Left"></ItemStyle>
						<ItemTemplate>
							<b>
								<asp:hyperlink id="lnkEditService" runat="server" NavigateUrl='<%# EditServiceUrl("ServiceID", Eval("ServiceID").ToString(), "edit_service") %>' Width=100% Height=100% ToolTip='<%# Eval("Comments") %>'>
									<%# Eval("ServiceName") %>
								</asp:hyperlink>
							</b>
							<%# Eval("ProviderName") %>
						</ItemTemplate>
					</asp:DataList>
				</td>
			</tr>
		</table>
	</ItemTemplate>
</asp:datalist>
