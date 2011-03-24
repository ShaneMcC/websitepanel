<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DomainNamesEditDomain.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.DomainNamesEditDomain" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>
<%@ Register TagPrefix="wsp" TagName="DomainNameCycles" Src="UserControls/DomainNameBillingCycles.ascx" %>
<%@ Register TagPrefix="wsp" TagName="AssignedProducts" Src="UserControls/AddonProducts.ascx" %>

<div class="FormBody">
	<table cellspacing="0" cellpadding="3">
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclDomainTLD" meta:resourcekey="lclDomainTLD" /></td>
			<td>
				<asp:Literal runat="server" ID="txtDomainTLD" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclTLDRegistrar" meta:resourcekey="lclTLDRegistrar" /></td>
			<td>
				<asp:DropDownList runat="server" ID="ddlTLDRegistrar" 
					DataValueField="PluginId" DataTextField="DisplayName" />
				<asp:RequiredFieldValidator runat="server" ErrorMessage="*" Display="Dynamic" 
					ControlToValidate="ddlTLDRegistrar" />
			</td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclProductSku" meta:resourcekey="lclProductSku" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtProductSku" CssClass="NormalTextBox" Width="150px" />
				<asp:RequiredFieldValidator runat="server" ErrorMessage="*" Display="Dynamic" 
					ControlToValidate="txtProductSku" />
			</td>
		</tr>
		<tr runat="server" visible="false">
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclTaxInclusive" /></td>
			<td>
				<asp:CheckBox runat="server" id="chkTaxInclusive" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" meta:resourcekey="lclWhoisEnabled" /></td>
			<td>
				<asp:CheckBox runat="server" ID="chkWhoisEnabled" Checked="true" /></td>
		</tr>
		<tr>
			<td>
				<asp:Localize runat="server" ID="lclTLDStatus" meta:resourcekey="lclTLDStatus" /></td>
			<td>
				<asp:RadioButtonList runat="server" ID="rblTLDStatus" RepeatDirection="Horizontal">
					<asp:ListItem Value="True" Text="Active" Selected="True" />
					<asp:ListItem Value="False" Text="Disabled" />
				</asp:RadioButtonList>
			</td>
		</tr>
	</table>
</div>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclBillingCycles" /></div>
</div>
<div class="FormBody">
	<wsp:ManualContextValidator runat="server" ID="ctxValBillingCycles" CssClass="QuickLabel" Display="Dynamic" EnableViewState="false"
		OnEvaluatingContext="ctxValBillingCycles_EvaluatingContext" EnableClientScript="false" meta:resourcekey="ctxValBillingCycles" />
	<wsp:DomainNameCycles runat="server" ID="ctlBillingCycles"  />
</div>

<div class="FormFooter">
	<asp:Button id="btnSaveTLD" runat="server" meta:resourcekey="btnSaveTLD" 
		CssClass="Button1" OnClick="btnSaveTLD_Click" />&nbsp;
	<asp:Button id="btnDeleteTLD" runat="server" meta:resourcekey="btnDeleteTLD" 
		CssClass="Button1" OnClick="btnDeleteTLD_Click" />&nbsp;
	<asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" 
		CssClass="Button1" CausesValidation="False" OnClick="btnCancel_Click" />
</div>