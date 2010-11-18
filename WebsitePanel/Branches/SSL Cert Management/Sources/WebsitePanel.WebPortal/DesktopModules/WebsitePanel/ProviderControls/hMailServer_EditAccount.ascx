<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="hMailServer_EditAccount.ascx.cs" Inherits="WebsitePanel.Portal.ProviderControls.hMailServer_EditAccount" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="../UserControls/CollapsiblePanel.ascx" %>
<wsp:CollapsiblePanel id="secAutoresponder" runat="server"
    TargetControlID="AutoresponderPanel" meta:resourcekey="secAutoresponder" Text="Autoresponder">
</wsp:CollapsiblePanel>
<asp:Panel ID="AutoresponderPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
		    <td class="SubHead" width="200" nowrap><asp:Label ID="lblResponderEnabled" runat="server" meta:resourcekey="lblResponderEnabled" Text="Enable autoresponder:"></asp:Label></td>
		    <td class="normal" width="100%">
			    <asp:CheckBox ID="chkResponderEnabled" Runat="server" meta:resourcekey="chkResponderEnabled" Text="Yes"></asp:CheckBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead"><asp:Label ID="lblSubject" runat="server" meta:resourcekey="lblSubject" Text="Subject:"></asp:Label></td>
		    <td class="normal" vAlign="top">
			    <asp:TextBox id="txtSubject" runat="server" Width="400px" CssClass="NormalTextBox"></asp:TextBox>
		    </td>
	    </tr>
	    <tr>
		    <td class="SubHead" vAlign="top"><asp:Label ID="lblMessage" runat="server" meta:resourcekey="lblMessage" Text="Message:"></asp:Label></td>
		    <td class="normal">
			    <asp:TextBox id="txtMessage" runat="server" Width="400px" TextMode="MultiLine" Rows="5" CssClass="NormalTextBox"></asp:TextBox>
		    </td>
	    </tr>
    </table>
</asp:Panel>

<wsp:CollapsiblePanel id="secForwarding" runat="server"
    TargetControlID="ForwardingPanel" meta:resourcekey="secForwarding" Text="Mail Forwarding">
</wsp:CollapsiblePanel>
<asp:Panel ID="ForwardingPanel" runat="server" Height="0" style="overflow:hidden;">
    <table width="100%">
	    <tr>
		    <td class="SubHead" width="200" nowrap><asp:Label ID="lblForwardTo" runat="server" meta:resourcekey="lblForwardTo" Text="Forward mail to address:"></asp:Label></td>
		    <td class="normal" width="100%" valign="top">
			    <asp:TextBox id="txtForward" runat="server" Width="200px" CssClass="NormalTextBox"></asp:TextBox>
		    </td>
	    </tr>
    </table>
</asp:Panel>