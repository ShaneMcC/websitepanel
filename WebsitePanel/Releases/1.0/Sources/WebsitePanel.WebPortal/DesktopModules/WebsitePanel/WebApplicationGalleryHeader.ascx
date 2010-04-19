<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebApplicationGalleryHeader.ascx.cs" Inherits="WebsitePanel.Portal.WebApplicationGalleryHeader" %>
<%@ Register Src="UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox"
    TagPrefix="uc1" %>
    
<uc1:SimpleMessageBox ID="messageBox" runat="server" />

<table width="100%" width="100%">
	    <tr>
	        <td colspan="2" width="100%" class="Huge">
	            <asp:Label runat="server" ID="lblTitle" />
	        </td>	        
	    </tr>
	    <tr>
	        <td><asp:Image runat="server" ID="imgLogo" />&nbsp;&nbsp;</td>
	        <td width="100%">
	            <table width="100%">
	                <tr>
	                    <td nowrap><asp:Literal runat="server" ID="litVersion" meta:resourcekey="litVersion" /></td>
	                    <td width="100%"><asp:Label runat="server" ID="lblVersion" /></td>
	                </tr>
	                <tr>
	                    <td nowrap><asp:Literal runat="server" ID="litAuthor" meta:resourcekey="litAuthor"/></td>
	                    <td><asp:HyperLink runat="server" id="hlAuthor" Target="_blank" /></td>
	                </tr>
	                <tr>
	                    <td nowrap><asp:Literal runat="server" ID="litSize" meta:resourcekey="litSize"/></td>
	                    <td><asp:Label runat="server" ID="lblSize" /></td>
	                </tr>
	            </table>
	        </td>
	    </tr>
	    <tr>
	        <td colspan="2">
	            <asp:Label runat="server" ID="lblDescription" />
	        </td>	        
	    </tr>
	</table>
    
