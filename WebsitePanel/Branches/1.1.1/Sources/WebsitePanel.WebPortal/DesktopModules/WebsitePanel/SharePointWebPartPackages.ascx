<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SharePointWebPartPackages.ascx.cs" Inherits="WebsitePanel.Portal.SharePointWebPartPackages" %>
<div class="FormBody">
    <table cellspacing="0" cellpadding="3" width="100%">
	    <tr>
		    <td class="Huge" colspan="2"><asp:Literal id="litSiteName" runat="server"></asp:Literal></td>
	    </tr>
	    <tr>
	        <td colspan="2">
	            <br />
                <asp:Button id="btnInstall" runat="server" meta:resourcekey="btnInstall" Text="Install Package" CssClass="Button2" CausesValidation="false" OnClick="btnInstall_Click"/>
	        </td>
	    </tr>
	    <tr>
		    <td>
    		
        		
                <asp:ListBox ID="lbWebPartPackages" runat="server" Rows="10" Width="300px">
                </asp:ListBox>



		    </td>
		    <td valign="top" width="100%">
                <asp:Button id="btnUninstall" runat="server" meta:resourcekey="btnUninstall"
                    Text="Uninstall" CssClass="Button1" CausesValidation="false" OnClientClick="return confirm('Uninstall?');" OnClick="btnUninstall_Click"/>
		    </td>
	    </tr>
    </table>
</div>
<div class="FormFooter">
    <asp:Button ID="btnCancel" runat="server" meta:resourcekey="btnCancel" CssClass="Button1" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
</div>