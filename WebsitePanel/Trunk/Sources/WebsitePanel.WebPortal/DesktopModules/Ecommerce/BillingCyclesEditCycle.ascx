<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillingCyclesEditCycle.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.BillingCyclesEditCycle" %>
<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclCycleName" meta:resourcekey="lclCycleName" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtCycleName" CssClass="NormalTextBox" Width="250px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCycleName" Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclBillingPeriod" meta:resourcekey="lclBillingPeriod" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtPeriodLength" CssClass="NormalTextBox" Width="100px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtPeriodLength" 
					Display="Dynamic" ErrorMessage="*" />
				<asp:RegularExpressionValidator runat="server" ControlToValidate="txtPeriodLength" ValidationExpression="\d+" 
					Display="Dynamic" ErrorMessage="*" />
				&nbsp;
				<asp:DropDownList runat="server" ID="ddlBillingPeriod">
					<asp:ListItem Value="day" Text="Day(s)" meta:resourcekey="itmDays"/>
					<asp:ListItem Value="month" Text="Month(s)" meta:resourcekey="itmMonths"/>
					<asp:ListItem Value="year" Text="Year(s)" meta:resourcekey="itmYears"/>
				</asp:DropDownList>
			</td>
		</tr>
	</table>
</div>

<div class="FormFooter">
	<asp:Button id="btnSaveCycle" runat="server" meta:resourcekey="btnSaveCycle" 
		CssClass="Button1" OnClick="btnSaveCycle_Click" />&nbsp;
	<asp:Button id="btnDeleteCycle" runat="server" meta:resourcekey="btnDeleteCycle" 
		CssClass="Button1" CausesValidation="false" OnClick="btnDeleteCycle_Click" />&nbsp;
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" 
		CssClass="Button1" CausesValidation="False" OnClick="btnCancel_Click" />
</div>