<%@ Control Language="C#" EnableViewState="false" AutoEventWireup="true" CodeBehind="UserAccountDetails.ascx.cs" Inherits="WebsitePanel.Portal.UserAccountDetails" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="FormRightIcon">
    <asp:Image ID="imgUser" runat="server" SkinID="User128" />
    <asp:Image ID="imgAdmin" runat="server" SkinID="Admin128" />
    <asp:Image ID="imgReseller" runat="server" SkinID="Reseller128" />
</div>
<div class="RightBlockTitle">
    <asp:Literal ID="litUsername" runat="server"></asp:Literal>
</div>
<table class="RightBlockTable">
    <tr>
        <td class="SubHead" style="white-space:nowrap;"><asp:Localize ID="locFullName" runat="server" meta:resourcekey="locFullName" Text="Name:"/></td>
        <td class="Normal"><asp:Literal ID="litFullName" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Localize ID="locEmail" runat="server" meta:resourcekey="locEmail" Text="E-mail:"/></td>
        <td class="Normal"><asp:HyperLink ID="lnkEmail" runat="server"></asp:HyperLink></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Localize ID="locRole" runat="server" meta:resourcekey="locRole" Text="Role:"/></td>
        <td class="Normal"><asp:Literal ID="litRole" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Localize ID="locCreated" runat="server" meta:resourcekey="locCreated" Text="Created:"/></td>
        <td class="Normal"><asp:Literal ID="litCreated" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Localize ID="locUpdated" runat="server" meta:resourcekey="locUpdated" Text="Updated:"/></td>
        <td class="Normal"><asp:Literal ID="litUpdated" runat="server"></asp:Literal></td>
    </tr>
</table>
<div class="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkSummaryLetter" runat="server" meta:resourcekey="lnkSummaryLetter" Text="View Summary Letter"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkEditAccountDetails" runat="server" meta:resourcekey="lnkEditAccountDetails" Text="Edit Details"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkChangePassword" runat="server" meta:resourcekey="lnkChangePassword" Text="Change Password"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkDelete" runat="server" meta:resourcekey="lnkDelete" Text="Delete"></asp:HyperLink>
    </div>
</div>
<br />
<wsp:CollapsiblePanel id="StatusHeader" runat="server"
    TargetControlID="StatusPanel" meta:resourcekey="StatusHeader" Text="Account Status">
</wsp:CollapsiblePanel>
<asp:Panel ID="StatusPanel" runat="server">
	<table cellpadding="5" style="width:100%;">
		<tr>
			<td align="center">
				<div class="MediumBold" style="padding:5px;">
					<asp:Literal ID="litStatus" runat="server"></asp:Literal>
				</div>
				<div id="StatusBlock" runat="server" style="padding:5px;text-align:center;">
					<asp:ImageButton ID="cmdActive" runat="server" SkinID="Start" meta:resourcekey="cmdActive" CommandName="Active" OnClick="statusButton_Click" />
					<asp:ImageButton ID="cmdSuspend" runat="server" SkinID="Pause" meta:resourcekey="cmdSuspend" CommandName="Suspended" OnClick="statusButton_Click" />
					<asp:ImageButton ID="cmdCancel" runat="server" SkinID="Stop" meta:resourcekey="cmdCancel" CommandName="Cancelled" OnClick="statusButton_Click" />
				</div>
			</td>
		</tr>
	</table>
</asp:Panel>