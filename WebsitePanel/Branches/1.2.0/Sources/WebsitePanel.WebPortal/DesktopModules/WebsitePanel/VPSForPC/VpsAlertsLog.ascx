<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsAlertsLog.ascx.cs" Inherits="WebsitePanel.Portal.VPSForPC.VpsAlertsLog" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>

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
				    <asp:Image ID="imgIcon" SkinID="AlertLog48" runat="server" />
                    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Alerts Log" />
			    </div>
			    <div class="FormBody">
                    <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_alerts_log" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="Alarms Log is empty." AllowPaging="true" DataSourceID="odsLogEntries" PageSize="20"
                        CssSelectorClass="NormalGridView" EnableViewState="false">
                        <Columns>                        
                            <asp:BoundField DataField="Severity" HeaderText="gvEntriesSeverity" />
                            <asp:BoundField DataField="ResolutionState" HeaderText="gvEntriesResolutionState" />
                            <asp:BoundField DataField="Name" HeaderText="gvEntriesName" />
                            <asp:BoundField DataField="Description" HeaderText="gvEntriesDescription" />
                            <asp:BoundField DataField="Source" HeaderText="gvEntriesSource" Visible="false" />
                            <asp:BoundField DataField="Created" HeaderText="gvEntriesCreated" />
                        </Columns>
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsLogEntries" runat="server"
                            SelectMethod="GetMonitoringAlerts"
                            TypeName="WebsitePanel.Portal.VirtualMachinesForPCHelper">
                    </asp:ObjectDataSource>
			    </div>
            </div>
	        <div class="Right">
		        <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	        </div>
        </div>    	
    </div>
</div>