<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomersServicesViewService.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.CustomersServicesViewService" %>
<div class="FormBody">
	<asp:PlaceHolder runat="server" ID="pnlViewSvcDetails" />
	<br />
	<asp:CheckBox runat="server" ID="chkSendMail" meta:resourcekey="chkSendMail" />
</div>
<div class="FormFooter">
	<asp:Button runat="server" ID="btnReturn" meta:resourcekey="btnReturn" CssClass="Button1" 
		OnClick="btnReturn_Click" />
	<asp:Button runat="server" ID="btnSvcActivate" CssClass="Button1" meta:resourcekey="btnSvcActivate" 
		OnClick="btnSvcActivate_Click" />
	<asp:Button runat="server" ID="btnSvcSuspend" CssClass="Button1" meta:resourcekey="btnSvcSuspend" 
		OnClick="btnSvcSuspend_Click" />
	<asp:Button runat="server" ID="btnSvcCancel" CssClass="Button1" meta:resourcekey="btnSvcCancel" 
		OnClick="btnSvcCancel_Click" />
	<asp:Button runat="server" ID="btnSvcDelete" meta:resourcekey="btnSvcDelete" CssClass="Button1" 
		OnClick="btnSvcDelete_Click" />
</div>