<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManualPaymentAdd.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.UserControls.ManualPaymentAdd" %>
<%@ Register TagPrefix="wsp" Namespace="WebsitePanel.Ecommerce.Portal" Assembly="WebsitePanel.Portal.Ecommerce.Modules" %>
<table>
	<tr>
		<td valign="top" style="width: 200px;">
			<div>
				<asp:Localize runat="server" meta:resourcekey="lclTransactionId" />
				<br />
				<asp:TextBox runat="server" ID="txtTransactionId" Width="100px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtTransactionId" 
					Display="Dynamic" ErrorMessage="*" />
			</div>
			<div>
				<asp:Localize runat="server" meta:resourcekey="lclCurrency" />
				<br />
				<asp:TextBox runat="server" ID="txtCurrency" MaxLength="3" Width="100px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCurrency" Display="Dynamic"
					Text="*" meta:resourcekey="RequireFieldValidator" />
			</div>
			<div>
				<asp:Localize runat="server" meta:resourcekey="lclTotalAmount" />
				<br />
				<asp:TextBox runat="server" ID="txtTotalAmount" Width="100px" CssClass="NormalTextBox" />
				<asp:RequiredFieldValidator runat="server" ControlToValidate="txtTotalAmount" Display="Dynamic"
					Text="*" meta:resourcekey="RequireFieldValidator" />
				<asp:CompareValidator runat="server" meta:resourcekey="CurrencyCompareValidator"
					Operator="DataTypeCheck" Type="Currency" Display="Dynamic" ControlToValidate="txtTotalAmount"
					Text="*" />
			</div>
			<div>
				<asp:Localize runat="server" meta:resourcekey="lclTransactionStatus" />
				<br />
				<asp:DropDownList runat="server" ID="ddlTranStatus" CssClass="NormalTextBox">
					<asp:ListItem Value="Pending" meta:resourcekey="lclPendingStatus" />
					<asp:ListItem Value="Approved" meta:resourcekey="lclApprovedStatus" />
				</asp:DropDownList>
			</div>
		</td>
		<td valign="top">
			<asp:Localize runat="server" meta:resourcekey="lclPaymentMethod" />
			<asp:Repeater runat="server" ID="rptPaymentMethods">
				<HeaderTemplate>
					<table cellpadding="0" cellspacing="0" class="QuickOutlineTbl">
						<tr>
							<td><wsp:RadioGroupValidator runat="server" ID="reqPaymentMethod" RadioButtonsGroup="QuickPayMethods" 
								CssClass="QuickLabel" Display="Dynamic" meta:resourcekey="reqPaymentMethod" /></td>
						</tr>
				</HeaderTemplate>
				<ItemTemplate>
					<tr>
						<td class="QuickLabel"><wsp:GroupRadioButton runat="server" GroupName="QuickPayMethods" ID="rbChecked" Text='<%# Eval("DisplayName") %>' 
							ControlValue='<%# Eval("MethodName") %>' CssClass="Normal" /></td>
					</tr>
				</ItemTemplate>
				<FooterTemplate>
					</table>
				</FooterTemplate>
			</asp:Repeater>
		</td>
	</tr>
</table>