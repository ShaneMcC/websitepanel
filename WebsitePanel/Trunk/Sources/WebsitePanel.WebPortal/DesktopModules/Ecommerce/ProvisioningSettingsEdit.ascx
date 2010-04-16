<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProvisioningSettingsEdit.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.ProvisioningSettingsEdit" %>
<div class="FormButtonsBar">
	<div class="FormSectionHeader"><asp:Localize runat="server" meta:resourcekey="lclProvSettings" /></div>
</div>
<div class="FormBody">
	<table cellspacing="3" cellpadding="0" border="0">
		<tr>
	        <td><asp:Localize runat="server" meta:resourcekey="lclBaseCurrency" /></td>
	        <td>
	           <asp:TextBox runat="server" ID="txtBaseCurrency" CssClass="NormalTextBox" MaxLength="3" Width="80px" />
	           <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBaseCurrency" Display="Dynamic" ErrorMessage="*" /></td>
	    </tr>
	    <tr>
	        <td><asp:Localize runat="server" meta:resourcekey="lclSecurePayments" /></td>
	        <td>
				<asp:RadioButtonList runat="server" ID="rblUseSSL" RepeatDirection="Horizontal">
					<asp:ListItem Value="True" Selected="True" meta:resourcekey="lclUseSSL" />
					<asp:ListItem Value="False" meta:resourcekey="lclNoSSL" />
				</asp:RadioButtonList></td>
	    </tr>
		<tr>
	        <td><asp:Localize runat="server" meta:resourcekey="lclInvoiceNumFormat" /></td>
	        <td>
	           <asp:TextBox runat="server" ID="txtInvoiceNumFormat" CssClass="NormalTextBox" Width="250px" /></td>
	    </tr>
	    <tr>
	        <td><asp:Localize runat="server" meta:resourcekey="lclInvoiceGracePeriod" /></td>
	        <td>
	            <asp:TextBox runat="server" ID="txtGracePeriod" CssClass="NormalTextBox" Width="80px" />
	            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtGracePeriod" 
					Display="Dynamic" ErrorMessage="*" /></td>
	    </tr>
	    <tr>
	        <td><asp:Localize runat="server" meta:resourcekey="lclSvcInvoiceThreshold" /></td>
	        <td>
				<asp:TextBox runat="server" ID="txtSvcsInvoiceThreshold" Width="80px" 
					CssClass="NormalTextBox" Value="4" />
	            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSvcsInvoiceThreshold" 
					Display="dynamic" ErrorMessage="*" />
	        </td>
	    </tr>
	    <tr>
			<td><asp:Localize runat="server" meta:resourcekey="lclSvcCancelThreshold" /></td>
			<td>
				<asp:TextBox runat="server" ID="txtSvcsCancelThreshold" CssClass="NormalTextBox" 
					Width="80px" Value="7" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtSvcsCancelThreshold" 
					Display="dynamic" ErrorMessage="*" />
			</td>
	    </tr>
	</table>
</div>

<div class="FormFooter">
	<asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnSave" 
		OnClick="btnSave_Click" />&nbsp;
	<asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnCancel" 
		OnClick="btnCancel_Click" CausesValidation="false" />
</div>