<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TermsAndConditionsEdit.ascx.cs" Inherits="WebsitePanel.Ecommerce.Portal.TermsAndConditionsEdit" %>

<div class="FormBody">
	<div class="FormRow"><asp:Localize runat="server" meta:resourcekey="lclHtmlTemplate" /></div>
	<asp:TextBox runat="server" ID="txtTermsAndConds" TextMode="MultiLine" Width="690px" Rows="25" />
</div>

<div class="FormFooter">
	<asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnSave" 
		OnClick="btnSave_Click" />&nbsp;
	<asp:Button runat="server" CssClass="Button1" meta:resourcekey="btnCancel" 
		OnClick="btnCancel_Click" />
</div>