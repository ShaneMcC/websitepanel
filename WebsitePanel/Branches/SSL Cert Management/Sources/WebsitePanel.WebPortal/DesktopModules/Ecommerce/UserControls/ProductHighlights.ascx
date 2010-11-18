<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductHighlights.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.ProductHighlights" %>
<table cellspacing="0" cellpadding="3">
	<tr>
		<td><asp:Localize runat="server" meta:resourcekey="lclHighlightText" /></td>
		<td><asp:TextBox runat="server" ID="txtHighlightText" CssClass="NormalTextBox" /></td>
		<td><asp:Button runat="server" ID="btnAddHighlight" CausesValidation="false" meta:resourcekey="btnAddHighlight" 
			OnClick="btnAddHighlight_Click" CssClass="Button1" /></td>
	</tr>
</table>
<ul style="margin-left: 0px;">
<asp:GridView ID="gvProductHightlights" runat="server" meta:resourcekey="gvProductHightlights" 
	AutoGenerateColumns="False" CssSelectorClass="NormalGridView">
	<Columns>
		<asp:TemplateField meta:resourcekey="gvPlanHighlight">
			<ItemStyle Width="40%"></ItemStyle>
			<ItemTemplate>
				<li><%# Container.DataItem %></li>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemStyle Wrap="False" />
			<ItemTemplate>
				<asp:Button runat="server" ID="btnMoveUp" meta:resourcekey="btnMoveUp" CommandArgument='<%# Container.DataItemIndex %>' 
					CausesValidation="false" CommandName="ITEM_MOVEUP" CssClass="Button1" />&nbsp;
				<asp:Button runat="server" ID="btnMoveDown" meta:resourcekey="btnMoveDown"  CommandArgument='<%# Container.DataItemIndex %>' 
					CausesValidation="false" CommandName="ITEM_MOVEDOWN" CssClass="Button1" />&nbsp;
				<asp:Button runat="server" ID="btnDeleteCycle" meta:resourcekey="btnDeleteCycle"  CommandArgument='<%# Container.DataItemIndex %>' 
					CausesValidation="false" CommandName="ITEM_DELETE" CssClass="Button1" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
	<HeaderStyle CssClass="GridHeader" HorizontalAlign="Left" />
	<RowStyle CssClass="Normal" />
	<PagerStyle CssClass="GridPager" />
	<EmptyDataRowStyle CssClass="Normal" />
	<PagerSettings Mode="NumericFirstLast" />
</asp:GridView>
</ul>