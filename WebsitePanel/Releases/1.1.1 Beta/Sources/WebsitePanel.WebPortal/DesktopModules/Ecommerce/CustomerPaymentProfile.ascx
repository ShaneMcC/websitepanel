<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerPaymentProfile.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomerPaymentProfile" %>
<%@ Register TagPrefix="wsp" TagName="PaymentProfile" Src="PaymentMethods/CreditCard_Payment.ascx" %>

<wsp:PaymentProfile runat="server" id="ctlPaymentProfile" savedetailsenabled="false" />

<div class="FormFooter">
	<asp:Button runat="server" ID="btnCreateProfile" meta:resourcekey="btnCreateProfile" CssClass="Button1" 
		OnClick="btnCreateProfile_Click" />
	<asp:Button runat="server" ID="btnUpdateProfile" meta:resourcekey="btnUpdateProfile" CssClass="Button1" 
		OnClick="btnUpdateProfile_Click" />
	<asp:Button runat="server" ID="btnDeleteProfile" meta:resourcekey="btnDeleteProfile" CssClass="Button1" 
		OnClick="btnDeleteProfile_Click" />
</div>