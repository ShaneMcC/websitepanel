<%@ Control Language="C#" AutoEventWireup="true" Codebehind="OrderComplete.ascx.cs"
	Inherits="WebsitePanel.Ecommerce.Portal.DesktopModules.Ecommerce.OrderComplete" %>
<div class="FormButtonsBar">
	<div class="FormSectionHeader">
		<asp:Localize runat="server" ID="locPaymentNotes" meta:resourcekey="locPaymentNotes" /></div>
</div>
<div class="FormBody">
	<asp:PlaceHolder runat="server" ID="phMessageArea" EnableViewState="false">
		<p>
			<asp:Localize runat="server" meta:resourcekey="locFinalizeMessage" /></p>
	</asp:PlaceHolder>
</div>
<div class="FormFooter">
	&nbsp;
</div>
