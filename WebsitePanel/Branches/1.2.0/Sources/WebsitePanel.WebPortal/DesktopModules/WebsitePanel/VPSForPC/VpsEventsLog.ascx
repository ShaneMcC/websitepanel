<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsEventsLog.ascx.cs" Inherits="WebsitePanel.Portal.VPSForPC.VpsEventsLog" %>
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
				    <asp:Image ID="imgIcon" SkinID="EventLog48" runat="server" />
                    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Events Log" />
			    </div>
			    <div class="FormBody">
                    <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_events_log" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />

                    <asp:GridView ID="gvEntries" runat="server" AutoGenerateColumns="False"
                        EmptyDataText="Event Log is empty." AllowPaging="true" DataSourceID="odsLogEntries" PageSize="20"
                        CssSelectorClass="NormalGridView" EnableViewState="false">
                        <Columns>
                            <asp:BoundField DataField="Number" HeaderText="gvEntriesNumber" />
                            <asp:BoundField DataField="Level" HeaderText="gvEntriesLevel" Visible="false" />
                            <asp:BoundField DataField="Category" HeaderText="gvEntriesCategory" Visible="false" />
                            <asp:BoundField DataField="Decription" HeaderText="gvEntriesDecription"/>
                            <asp:BoundField DataField="EventData" HeaderText="gvEntriesEventData" Visible="false" />
                            <asp:BoundField DataField="TimeGenerated" HeaderText="gvEntriesTimeGenerated" />
                        </Columns>
                    </asp:GridView>
        
                    <asp:ObjectDataSource ID="odsLogEntries" runat="server"
                            SelectMethod="GetMonitoredObjectEvents"
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