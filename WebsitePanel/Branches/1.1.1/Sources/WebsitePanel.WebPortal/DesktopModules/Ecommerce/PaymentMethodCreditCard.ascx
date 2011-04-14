<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentMethodCreditCard.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.PaymentMethodCreditCard" %>
<div class="FormBody">
	<table>
		<tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclDisplayName" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtDisplayName" CssClass="NormalTextBox" MaxLength="50" 
					Width="150px" Text="Credit Card" />&nbsp;
				<asp:RequiredFieldValidator runat="server" Display="Dynamic" ErrorMessage="*" ControlToValidate="txtDisplayName" />
			</td>
		</tr>
		<tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclAcceptPayments" /></td>
			<td><asp:DropDownList 
				runat="server" ID="ddlAcceptPlugins" AutoPostBack="true" DataValueField="PluginName" 
				DataTextField="DisplayName" OnSelectedIndexChanged="ddlAcceptPlugins_SelectedIndexChanged"
				 AppendDataBoundItems="true" CssClass="NormalTexBox">
					<asp:ListItem Value="" meta:resourcekey="lblAcceptPlugins" />	
				</asp:DropDownList>&nbsp;
				<asp:RequiredFieldValidator runat="server" ControlToValidate="ddlAcceptPlugins" 
					Display="Dynamic" ErrorMessage="*" />
			</td>
		</tr>
	</table>
	&nbsp;
	<br />
	<asp:PlaceHolder runat="server" ID="cntCcProvSettings" />
</div>

<div class="FormFooter">
	<asp:Button runat="server" ID="btnSaveSettings" meta:resourcekey="btnSaveSettings" CssClass="Button1" 
		OnClick="btnSaveSettings_Click" />&nbsp;
	<asp:Button runat="server" ID="btnDisable" CausesValidation="false" meta:resourcekey="btnDisable" CssClass="Button1" 
		OnClick="btnDisable_Click" />&nbsp;
	<asp:Button runat="server" ID="btnCancel" CausesValidation="false" meta:resourcekey="btnCancel" CssClass="Button1" 
		OnClick="btnCancel_Click" />
</div>