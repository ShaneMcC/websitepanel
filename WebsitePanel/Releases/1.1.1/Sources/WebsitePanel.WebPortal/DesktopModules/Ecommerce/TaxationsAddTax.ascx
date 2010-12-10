<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TaxationsAddTax.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.TaxationsAddTax" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>

<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclDescription" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtDescription" CssClass="NormalTextBox" Width="250px" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtDescription" Display="Dynamic"
					Text="*" meta:resourcekey="RequiredFieldValidator" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclTaxStatus" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblTaxStatus" RepeatDirection="Horizontal">
					<asp:ListItem Value="True" Selected="True" meta:resourcekey="TaxActiveStatus" />
					<asp:ListItem Value="False" meta:resourcekey="TaxInactiveStatus" />
				</asp:RadioButtonList>
				<asp:RequiredFieldValidator runat="server" ControlToValidate="rblTaxStatus" Display="Dynamic"
					Text="*" meta:resourcekey="RequiredFieldValidator" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclCountry" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlCountries" CssClass="NormalTextBox" AppendDataBoundItems="true" 
					OnSelectedIndexChanged="ddlCountries_SelectedIndexChanged" AutoPostBack="true">
					<asp:ListItem Value="*" meta:resourcekey="lclAllCountries" />
				</asp:DropDownList>
				<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCountries" Display="Dynamic"
					Text="*" meta:resourcekey="RequiredFieldValidator" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclStateProvince" /></td>
			<td>
			    <asp:PlaceHolder runat="server" ID="plStateProvince">
				    <asp:DropDownList runat="server" ID="ddlStates" CssClass="NormalTextBox" AppendDataBoundItems="true">
					    <asp:ListItem Value="*" meta:resourcekey="liAllStates" />
				    </asp:DropDownList>
					<asp:TextBox runat="server" ID="txtStateProvince" CssClass="NormalTextBox" Width="150px" />&nbsp;
				    <asp:RequiredFieldValidator runat="server" ID="reqStateProvince" Display="Dynamic"
						Text="*" meta:resourcekey="RequiredFieldValidator" />&nbsp;</asp:PlaceHolder>
					<asp:CheckBox runat="server" ID="chkAllStates" meta:resourcekey="chkAllStates" TextAlign="Right" 
						AutoPostBack="true" CausesValidation="false" OnCheckedChanged="chkAllStates_CheckedChanged" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclTaxType" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlTaxType" CssClass="NormalTextBox" 
					OnSelectedIndexChanged="ddlTaxType_SelectedIndexChanged">
					<asp:ListItem Value="2" Selected="True" meta:resourcekey="liPercentage" />
					<asp:ListItem Value="1" meta:resourcekey="liFixed" />
					<asp:ListItem Value="3" meta:resourcekey="liTaxIncluded" />
				</asp:DropDownList>
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclTaxAmount" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtTaxAmount" CssClass="NormalTextBox" Width="150px" />
				<asp:RequiredFieldValidator runat="server" ID="TaxAmountNotEmptyValidator" meta:resourcekey="RequiredFieldValidator"
					ControlToValidate="txtTaxAmount" Display="Dynamic" Text="*" />
				<asp:CompareValidator runat="server" ID="FixedAmountCompareValidator" meta:resourcekey="FixedAmountCompareValidator" 
					ControlToValidate="txtTaxAmount" Display="Dynamic" Operator="DataTypeCheck" Type="Currency" 
					Text="*" />
				<asp:CompareValidator runat="server" ID="PercentageAmountCompareValidator" meta:resourcekey="PercentageAmountCompareValidator"
					ControlToValidate="txtTaxAmount" Display="Dynamic" Operator="DataTypeCheck" Type="Double"
					Text="*" />
			</td>
		</tr>
	</table>
</div>

<div class="FormFooter">
	<asp:Button runat="server" ID="btnAddTax" meta:resourcekey="btnAddTax" CssClass="Button1" 
		OnClick="btnAddTax_Click" />&nbsp;
	<asp:Button runat="server" ID="btnCancel" meta:resourcekey="btnCancel" CssClass="Button1" 
		OnClick="btnCancel_Click" CausesValidation="false" />
</div>