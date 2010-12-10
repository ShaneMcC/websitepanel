<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServersEditReboot.ascx.cs" Inherits="WebsitePanel.Portal.ServersEditReboot" %>
<%@ Register Src="ServerHeaderControl.ascx" TagName="ServerHeaderControl" TagPrefix="uc1" %>

<uc1:ServerHeaderControl id="ServerHeaderControl1" runat="server"/>
<div class="FormBody">
    <div class="RedBorderFillBox">
        <asp:Button ID="btnReboot" runat="server" meta:resourcekey="btnReboot" Text="Reboot Server"
			CssClass="Button2" OnClientClick="return confirm('Continue with Server reboot?');" OnClick="btnReboot_Click" />
    </div>
</div>


<div class="FormFooter">
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" Text="Cancel" CssClass="Button1"
        CausesValidation="False" OnClick="btnCancel_Click" />
</div>