<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebSitesAllocateIPAddresses.ascx.cs" Inherits="WebsitePanel.Portal.WebSitesAllocateIPAddresses" %>
<%@ Register Src="UserControls/AllocatePackageIPAddresses.ascx" TagName="AllocatePackageIPAddresses" TagPrefix="wsp" %>

<div class="FormBody">

    <wsp:AllocatePackageIPAddresses id="allocateAddresses" runat="server"
            Pool="WebSites"
            ResourceGroup="Web"
            ListAddressesControl="" />
            
</div>