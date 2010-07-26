<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InstallerInstallApplicationComplete.ascx.cs" Inherits="WebsitePanel.Portal.InstallerInstallApplicationComplete" %>
<%@ Register Src="installerapplicationheader.ascx" TagName="ApplicationHeader" TagPrefix="dnc" %>

<div class="FormBody">
    <dnc:applicationheader id="installerapplicationheader" runat="server"></dnc:applicationheader>
    <br />
    <br />
		<asp:PlaceHolder ID="completePanel" runat="server"></asp:PlaceHolder>
    <br />
    <br />
</div>
<div class="FormFooter">
    <asp:Button ID="btnReturn" runat="server" meta:resourcekey="btnReturn" Text="  OK  " CssClass="Button1" OnClick="btnReturn_Click" />
</div>