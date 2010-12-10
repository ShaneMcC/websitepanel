<%@ Control AutoEventWireup="true" %>
<%@ Register TagPrefix="wsp" TagName="SiteFooter" Src="~/DesktopModules/WebsitePanel/SkinControls/SiteFooter.ascx" %>
<%@ Register TagPrefix="wsp" TagName="TopMenu" Src="~/DesktopModules/WebsitePanel/SkinControls/TopMenu.ascx" %>
<%@ Register  TagPrefix="wsp" TagName="Logo" Src="~/DesktopModules/WebsitePanel/SkinControls/Logo.ascx" %>
<%@ Register  TagPrefix="wsp" TagName="SignedInUser" Src="~/DesktopModules/WebsitePanel/SkinControls/SignedInUser.ascx" %>

<asp:ScriptManager ID="scriptManager" runat="server" EnablePartialRendering="true" EnableScriptGlobalization="true" EnableScriptLocalization="true">
	<Services>
		<asp:ServiceReference Path="~/DesktopModules/WebsitePanel/TaskManager.asmx" />
	</Services>
</asp:ScriptManager>

<div id="SkinOutline">
    <div id="SkinContent">
        <div id="Header">
            <table class="Container" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td class="Logo" rowspan="2">
                        <wsp:Logo ID="logo" runat="server" />
                    </td>
                    <td>&nbsp;</td>
                    <td class="Account">
                        <wsp:SignedInUser ID="signedInUser" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="TopMenu">
            <wsp:TopMenu ID="menu" runat="server" />
        </div>
        
        <div id="ContentOneColumn">
			<asp:PlaceHolder ID="ContentPane" runat="server"></asp:PlaceHolder>
		</div>
    </div>
    <div id="Footer">
        <wsp:SiteFooter ID="footer" runat="server" />
    </div>
</div>