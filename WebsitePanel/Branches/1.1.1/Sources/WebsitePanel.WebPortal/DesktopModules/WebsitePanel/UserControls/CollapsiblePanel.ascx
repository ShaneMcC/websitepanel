<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CollapsiblePanel.ascx.cs" Inherits="WebsitePanel.Portal.CollapsiblePanel" %>
<asp:Panel ID="HeaderPanel" runat="server" style="cursor: pointer;">
    <table class="Shevron" cellpadding="0" cellspacing="0"
		onmouseout="this.className='Shevron';" onmouseover="this.className='ShevronActive';">
		<tr>
			<td style="white-space: nowrap;padding-right:5px;"><asp:Label ID="lblTitle" runat="server"></asp:Label></td>
			<td class="ShevronLine"></td>
			<td style="padding-left:5px;"><asp:Image ID="ToggleImage" runat="server" ImageUrl="~/images/collapse.jpg" /></td>
		</tr>
    </table>
</asp:Panel>
<ajaxToolkit:CollapsiblePanelExtender ID="cpe" runat="Server" OnResolveControlID="cpe_ResolveControlID"
        TargetControlID="CpeContentPanel"
        ExpandControlID="HeaderPanel"
        CollapseControlID="HeaderPanel"
        Collapsed="False"        
        ExpandDirection="Vertical"
        ImageControlID="ToggleImage"
        ExpandedImage="~/images/collapse.jpg"
        ExpandedText="Collapse"
        CollapsedImage="~/images/expand.jpg"
        CollapsedText="Expand"
        SuppressPostBack="true" /> 