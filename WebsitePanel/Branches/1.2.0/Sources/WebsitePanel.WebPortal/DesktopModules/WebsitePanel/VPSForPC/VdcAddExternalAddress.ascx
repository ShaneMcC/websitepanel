<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcAddExternalAddress.ascx.cs" Inherits="WebsitePanel.Portal.VPSForPC.VdcAddExternalAddress" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/AllocatePackageIPAddresses.ascx" TagName="AllocatePackageIPAddresses" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>

<div id="VpsContainer">
    <div class="Module">

	    <div class="Header">
		    <wsp:Breadcrumb id="breadcrumb" runat="server" />
	    </div>
    	
	    <div class="Left">
		    <wsp:Menu id="menu" runat="server" SelectedItem="vdc_external_network" />
	    </div>
    	
	    <div class="Content">
		    <div class="Center">
			    <div class="Title">
				    <asp:Image ID="imgIcon" SkinID="Network48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Allocate IP Addresses"></asp:Localize>
			    </div>
			    <div class="FormBody">

                <wsp:AllocatePackageIPAddresses id="allocateAddresses" runat="server"
                        Pool="VpsExternalNetwork"
                        ResourceGroup="VPS"
                        ListAddressesControl="vdc_external_network" />
				    
			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>

