<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteFooter.ascx.cs" Inherits="WebsitePanel.Portal.SkinControls.SiteFooter" %>
<%@ Register TagPrefix="wsp" TagName="ProductVersion" Src="ProductVersion.ascx" %>
<table class="Container" cellpadding="0" cellspacing="0">
    <tr>
        <td class="Copyright">
			<asp:Localize ID="locPoweredBy" runat="server" meta:resourcekey="locPoweredBy" />
        </td>
        <td class="Version">
            <asp:Localize ID="locVersion" runat="server" meta:resourcekey="locVersion" /> <wsp:ProductVersion id="wspVersion" runat="server" AssemblyName="WebsitePanel.Portal.Modules"/>
        </td>
    </tr>
</table>