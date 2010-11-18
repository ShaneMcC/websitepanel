<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SignedInUser.ascx.cs" Inherits="WebsitePanel.Portal.SkinControls.SignedInUser" %>
<asp:Panel ID="AnonymousPanel" runat="server">
	<asp:HyperLink ID="lnkSignIn" runat="server" meta:resourcekey="lnkSignIn">Sign In</asp:HyperLink>
</asp:Panel>
<asp:Panel ID="LoggedPanel" runat="server">
	<asp:Localize runat="server" meta:resourcekey="locWelcome"/> <strong><asp:Literal ID="litUsername" runat="server"></asp:Literal></strong>&nbsp;&nbsp;&nbsp;
	<asp:HyperLink ID="lnkEditUserDetails" runat="server" meta:resourcekey="lnkEditUserDetails">My Account</asp:HyperLink>
	| <asp:LinkButton ID="cmdSignOut" runat="server" Text="Sign Out" meta:resourcekey="cmdSignOut"
		CausesValidation="false" OnClick="cmdSignOut_Click"></asp:LinkButton>
</asp:Panel>