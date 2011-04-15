<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpaceDetails.ascx.cs" Inherits="WebsitePanel.Portal.SpaceDetails" %>
<%@ Register Src="UserControls/ServerDetails.ascx" TagName="ServerDetails" TagPrefix="wsp" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>
<div class="FormRightIcon">
    <asp:Image ID="Image1" runat="server" SkinID="Space128" />
</div>
<div class="RightBlockTitle">
    <asp:Literal ID="litSpaceName" runat="server"></asp:Literal>
</div>
<table class="RightBlockTable">
    <tr>
        <td class="SubHead"><asp:Label ID="lblCreated" runat="server" meta:resourcekey="lblCreated" Text="Created:"></asp:Label></td>
        <td class="Normal"><asp:Literal ID="litCreated" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblHostingPlan" runat="server" meta:resourcekey="lblHostingPlan" Text="Hosting Plan:"></asp:Label></td>
        <td class="Normal"><asp:Literal ID="litHostingPlan" runat="server"></asp:Literal></td>
    </tr>
    <tr>
        <td class="SubHead"><asp:Label ID="lblServer" runat="server" meta:resourcekey="lblServer" Text="Server:"></asp:Label></td>
        <td class="Normal"><wsp:ServerDetails ID="serverDetails" runat="server" /></td>
    </tr>
</table>
<div class="Normal">
    <div class="ToolLink">
        <asp:HyperLink ID="lnkSummaryLetter" runat="server" meta:resourcekey="lnkSummaryLetter" Text="View Summary Letter"></asp:HyperLink>
    </div>
    <div class="ToolLink" runat="server" id="OverusageReport">
        <asp:HyperLink ID="lnkOverusageReport" runat="server" meta:resourcekey="lnkOverusageReport" Text="Overusage Report"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkEditSpaceDetails" runat="server" meta:resourcekey="lnkEditSpaceDetails" Text="Edit Details"></asp:HyperLink>
    </div>
    <div class="ToolLink">
        <asp:HyperLink ID="lnkDelete" runat="server" meta:resourcekey="lnkDelete" Text="Delete"></asp:HyperLink>
    </div>
</div>
<br />
<wsp:CollapsiblePanel id="StatusHeader" runat="server"
    TargetControlID="StatusPanel" meta:resourcekey="StatusHeader" Text="Space Status">
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