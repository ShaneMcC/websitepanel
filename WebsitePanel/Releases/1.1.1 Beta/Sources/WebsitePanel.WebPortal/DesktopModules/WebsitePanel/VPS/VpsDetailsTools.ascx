<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsTools.ascx.cs" Inherits="WebsitePanel.Portal.VPS.VpsDetailsTools" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>

<div id="VpsContainer">
    <div class="Module">

	    <div class="Header">
		    <wsp:Breadcrumb id="breadcrumb" runat="server" />
	    </div>
    	
	    <div class="Left">
		    <wsp:Menu id="menu" runat="server" SelectedItem="" />
	    </div>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="Title">
				    <asp:Image ID="imgIcon" SkinID="Server48" runat="server" />
				    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Tools" />
			    </div>
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_tools" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    <table cellspacing="15">
				        <%-- <tr>
				            <td>
				                <asp:Button ID="btnReinstall" runat="server" CssClass="Button1" Width="100"
				                    Text="Re-install" meta:resourcekey="btnReinstall" CausesValidation="false" 
                                    onclick="btnReinstall_Click" />
				            </td>
				            <td>
				                <asp:Localize ID="locReinstall" runat="server" meta:resourcekey="locReinstall" Text="Performs..."></asp:Localize>
				            </td>
				        </tr>--%>
				        <tr>
				            <td>
				                <asp:Button ID="btnDelete" runat="server" CssClass="Button1" Width="100"
				                    Text="Delete" meta:resourcekey="btnDelete" CausesValidation="false" 
                                    onclick="btnDelete_Click" />
				            </td>
				            <td>
				                <asp:Localize ID="locDelete" runat="server" meta:resourcekey="locDelete" Text="Performs..."></asp:Localize>
				            </td>
				        </tr>
				    </table>
			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>