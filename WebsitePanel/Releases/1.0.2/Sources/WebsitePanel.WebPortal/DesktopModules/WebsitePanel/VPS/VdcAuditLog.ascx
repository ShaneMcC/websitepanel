<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAuditLog.ascx.cs" Inherits="WebsitePanel.Portal.VPS.VdcAuditLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PopupHeader.ascx" TagName="PopupHeader" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/AuditLogControl.ascx" TagName="AuditLogControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>

<div id="VpsContainer">
    <div class="Module">

	    <div class="Header">
		    <wsp:Breadcrumb id="breadcrumb" runat="server" />
	    </div>
    	
	    <div class="Left">
		    <wsp:Menu id="menu" runat="server" SelectedItem="vdc_audit_log" />
	    </div>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="Title">
				    <asp:Image ID="imgIcon" SkinID="AuditLog48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Audit Log"></asp:Localize>
			    </div>
			    <div class="FormBody">

                    <wsp:AuditLogControl id="auditLog" runat="server" LogSource="VPS" />

			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>