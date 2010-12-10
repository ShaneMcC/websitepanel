<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsDetailsAuditLog.ascx.cs" Inherits="WebsitePanel.Portal.VPS.VpsDetailsAuditLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/AuditLogControl.ascx" TagName="AuditLogControl" TagPrefix="wsp" %>

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
				    <asp:Image ID="imgIcon" SkinID="AuditLog48" runat="server" />
				    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Audit Log" />
			    </div>
			    <div class="FormBody">
			        <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_audit_log" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />
                    
				    <wsp:AuditLogControl id="auditLog" runat="server" LogSource="VPS" />
				    
			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>