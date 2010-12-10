<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StatisticsEditStatistics.ascx.cs" Inherits="WebsitePanel.Portal.StatisticsEditStatistics" %>
<div class="FormBody">
<table cellSpacing="0" cellPadding="5" width="100%">
	<tr id="newRow" runat="server">
		<td class="SubHead" nowrap width=200><asp:Label ID="lblWebSite" runat="server" meta:resourcekey="lblWebSite" Text="Web Site:"></asp:Label></td>
		<td width="100%" class="NormalBold">
			
			<asp:Label id="lblDomainName" runat="server" CssClass="Huge"></asp:Label>
            <asp:DropDownList ID="ddlWebSites" runat="server" CssClass="NormalTextBox"
                DataTextField="Name" DataValueField="Name">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="valRequireWebSite" runat="server" ErrorMessage="*" ControlToValidate="ddlWebSites"></asp:RequiredFieldValidator></td>
	</tr>
</table>

<asp:PlaceHolder ID="providerControl" runat="server"></asp:PlaceHolder>
</div>

<div class="FormFooter">
    <asp:Button ID="btnUpdate" runat="server" CssClass="Button1"
        Text="Update" OnClick="btnUpdate_Click" />
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
    <asp:Button ID="btnDelete" runat="server" meta:resourcekey="btnDelete" CssClass="Button1" CausesValidation="false" 
        Text="Delete" OnClientClick="return confirm('Delete Site?');" OnClick="btnDelete_Click" />
</div>