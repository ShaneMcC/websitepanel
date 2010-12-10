<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainNameBillingCycles.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.DomainNameBillingCycles" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<asp:PlaceHolder runat="server" ID="phBillingCycles">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td><asp:Localize runat="server" ID="lclSelectCycle" meta:resourcekey="lclSelectCycle" /></td>
			<td><asp:DropDownList runat="server" ID="ddlBillingCycles" DataTextField="CycleName" DataValueField="CycleID" /></td>
			<td><asp:Button runat="server" ID="btnAddCycle" CssClass="Button1"
				CausesValidation="false" meta:resourcekey="btnAddCycle" OnClick="btnAddCycle_Click" /></td>
		</tr>
	</table>
</asp:PlaceHolder>

<asp:GridView ID="gvDomainNameCycles" runat="server" meta:resourcekey="gvDomainNameCycles" 
	AutoGenerateColumns="False" CssSelectorClass="NormalGridView" DataKeyNames="CycleID">
	<Columns>
		<asp:TemplateField meta:resourcekey="gvBillingCycle">
			<ItemStyle Width="40%" CssClass="Centered" />
			<ItemTemplate>
				<%# Eval("CycleName")%>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField meta:resourcekey="gvBillingPeriod">
			<ItemStyle Wrap="False" CssClass="Centered" />
			<ItemTemplate>
				<%# Eval("PeriodLength") %>&nbsp;<%# Eval("BillingPeriod", "{0}(s)") %>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField meta:resourcekey="gvSetupFee">
			<ItemStyle Wrap="False" CssClass="Centered" />
			<ItemTemplate>
				<asp:TextBox runat="server" ID="txtSetupFee" CssClass="NormalTextBox Centered" Width="60px" Text='<%# Eval("SetupFee", "{0:C}") %>' />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSetupFee" Display="Dynamic"
					Text="*" meta:resourcekey="RequiredFieldValidator" />
				<asp:CompareValidator runat="server" ControlToValidate="txtSetupFee" Operator="DataTypeCheck"
					Type="Currency" Text="*" Display="Dynamic" meta:resourcekey="CurrencyCompareValidator" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField meta:resourcekey="gvRecurringFee">
			<ItemStyle Wrap="False" CssClass="Centered" />
			<ItemTemplate>
				<asp:TextBox runat="server" ID="txtRecurringFee" CssClass="NormalTextBox Centered"  Width="60px" Text='<%# Eval("RecurringFee", "{0:C}") %>' />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecurringFee" Display="Dynamic"
					Text="*" meta:resourcekey="RequiredFieldValidator" />
				<asp:CompareValidator runat="server" ControlToValidate="txtRecurringFee" Operator="DataTypeCheck"
					Type="Currency" Text="*" Display="Dynamic" meta:resourcekey="CurrencyCompareValidator" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField meta:resourcekey="gvTransferFee">
			<ItemStyle Wrap="False" CssClass="Centered" />
			<ItemTemplate>
				<asp:TextBox runat="server" ID="txtTransferFee" CssClass="NormalTextBox Centered"  Width="60px" Text='<%# Eval("TransferFee", "{0:C}") %>' />
				<asp:CompareValidator runat="server" ControlToValidate="txtTransferFee" Operator="DataTypeCheck"
					Type="Currency" Text="*" Display="Dynamic" meta:resourcekey="CurrencyCompareValidator" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemStyle Wrap="False" />
			<ItemTemplate>
				<asp:Button runat="server" ID="btnMoveUp" meta:resourcekey="btnMoveUp" CommandArgument='<%# Container.DataItemIndex %>' 
					CausesValidation="false" CommandName="CYCLE_MOVEUP" CssClass="Button1" />&nbsp;
				<asp:Button runat="server" ID="btnMoveDown" meta:resourcekey="btnMoveDown"  CommandArgument='<%# Container.DataItemIndex %>' 
					CausesValidation="false" CommandName="CYCLE_MOVEDOWN" CssClass="Button1" />&nbsp;
				<asp:Button runat="server" ID="btnDeleteCycle" meta:resourcekey="btnDeleteCycle"  CommandArgument='<%# Container.DataItemIndex %>' 
					CausesValidation="false" CommandName="CYCLE_DELETE" CssClass="Button1" />
			</ItemTemplate>
		</asp:TemplateField>
	</Columns>
	<HeaderStyle CssClass="GridHeader" HorizontalAlign="Left" />
	<RowStyle CssClass="Normal" />
	<PagerStyle CssClass="GridPager" />
	<EmptyDataRowStyle CssClass="Normal" />
	<PagerSettings Mode="NumericFirstLast" />
</asp:GridView>