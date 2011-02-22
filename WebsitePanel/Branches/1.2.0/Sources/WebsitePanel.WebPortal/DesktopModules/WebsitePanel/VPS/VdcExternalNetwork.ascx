<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VdcExternalNetwork.ascx.cs" Inherits="WebsitePanel.Portal.VPS.VdcExternalNetwork" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/Quota.ascx" TagName="Quota" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/CollapsiblePanel.ascx" TagName="CollapsiblePanel" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/PackageIPAddresses.ascx" TagName="PackageIPAddresses" TagPrefix="wsp" %>


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
				    <asp:Image ID="Image1" SkinID="Network48" runat="server" />
				    <asp:Localize ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="External Network"></asp:Localize>
			    </div>
			    <div class="FormBody">

                    
                    <wsp:PackageIPAddresses id="packageAddresses" runat="server"
                            Pool="VpsExternalNetwork"
                            EditItemControl="vps_general"
                            SpaceHomeControl="vdc_external_network"
                            AllocateAddressesControl="vdc_allocate_external_ip" />

    				
    				<br />
				    <wsp:CollapsiblePanel id="secQuotas" runat="server"
                        TargetControlID="QuotasPanel" meta:resourcekey="secQuotas" Text="Quotas">
                    </wsp:CollapsiblePanel>
                    <asp:Panel ID="QuotasPanel" runat="server" Height="0" style="overflow:hidden;">
                    
                        <table cellspacing="6">
                            <tr>
                                <td><asp:Localize ID="locIPQuota" runat="server" meta:resourcekey="locIPQuota" Text="Number of IP Addresses:"></asp:Localize></td>
                                <td><wsp:Quota ID="addressesQuota" runat="server" QuotaName="VPS.ExternalIPAddressesNumber" /></td>
                            </tr>
                            <tr>
                                <td><asp:Localize ID="locBandwidthQuota" runat="server" meta:resourcekey="locBandwidthQuota" Text="Bandwidth, GB:"></asp:Localize></td>
                                <td><wsp:Quota ID="bandwidthQuota" runat="server" QuotaName="VPS.Bandwidth" /></td>
                            </tr>
                        </table>
                    
                    
                    </asp:Panel>

			    </div>
		    </div>
		    <div class="Right">
			    <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
		    </div>
	    </div>
    	
    </div>
</div>