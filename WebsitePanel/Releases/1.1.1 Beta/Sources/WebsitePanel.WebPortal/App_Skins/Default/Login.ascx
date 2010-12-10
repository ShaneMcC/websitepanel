<%@ Control AutoEventWireup="true" %>
<%@ Register TagPrefix="wsp" TagName="SiteFooter" Src="~/DesktopModules/WebsitePanel/SkinControls/SiteFooter.ascx" %>
<%@ Register  TagPrefix="wsp" TagName="Logo" Src="~/DesktopModules/WebsitePanel/SkinControls/Logo.ascx" %>

<div id="LoginSkinOutline">
    <div id="LoginSkinContent">
		<div id="HeaderLogin">
			<a href='<%= Page.ResolveUrl("~/") %>'><asp:Image runat="server" SkinID="Logo" /></a>
		</div>
        <div id="ContentLogin">
            <asp:PlaceHolder ID="ContentPane" runat="server"></asp:PlaceHolder>
        </div>
    </div>
    <div id="Footer">
        <wsp:SiteFooter ID="SiteFooter1" runat="server" />
    </div>
</div>