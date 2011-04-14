<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BandwidthReportPackageDetails.ascx.cs" Inherits="WebsitePanel.Portal.BandwidthReportPackageDetails" %>
<%@ Register Src="SpaceDetailsHeaderControl.ascx" TagName="SpaceDetailsHeaderControl" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Gauge.ascx" TagName="Gauge" TagPrefix="uc1" %>
<%@ Register TagPrefix="wsp" TagName="CollapsiblePanel" Src="UserControls/CollapsiblePanel.ascx" %>

<div class="FormBody">
    <wsp:SpaceDetailsHeaderControl ID="spaceDetails" runat="server" />
    
    <wsp:CollapsiblePanel id="secSummary" runat="server"
        TargetControlID="SummaryPanel" meta:resourcekey="secSummary" Text="Bandwidth by Resources">
    </wsp:CollapsiblePanel>
    <asp:Panel ID="SummaryPanel" runat="server" Height="0" style="overflow:hidden;">
        <div class="Big">
            <asp:Literal ID="litPeriod" runat="server"></asp:Literal>
        </div>
        <br />
        <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="False"
            EmptyDataText="gvSummary" CssSelectorClass="NormalGridView">
            <Columns>
	            <asp:TemplateField HeaderText="gvSummaryGroupName" ItemStyle-Width="150">
		            <ItemTemplate>
			            <span class="Big"><%# GetSharedLocalizedString("ReportResourceGroup." + Eval("GroupName")) %></span>
		            </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="gvSummaryBandwidthTotal">
		            <ItemTemplate>
                        <uc1:Gauge ID="gauge" runat="server" OneColour="true"
                            Progress='<%# Convert.ToInt32(Eval("MegaBytesTotal")) %>'
                            Total='<%# BandwidthTotal %>' DisplayText="false" />
                         <span class="Medium" title='<%# Eval("BytesTotal") + " " + GetLocalizedString("lblBytes.Text") %>'><%# Eval("MegaBytesTotal")%> <asp:Label ID="lblMB2" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label></span>
		            </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="gvSummaryBandwidthSent">
	                <ItemStyle CssClass="Normal" HorizontalAlign="Center" />
		            <ItemTemplate>
		                <span title='<%# Eval("BytesSent") + " " + GetLocalizedString("lblBytes.Text") %>'><%# Eval("MegaBytesSent")%> <asp:Label ID="lblMB2" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label></span>
		            </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="gvSummaryBandwidthReceived">
	                <ItemStyle CssClass="Normal" HorizontalAlign="Center" />
		            <ItemTemplate>
		                <span title='<%# Eval("BytesReceived") + " " + GetLocalizedString("lblBytes.Text") %>'><%# Eval("MegaBytesReceived")%> <asp:Label ID="lblMB2" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label></span>
		            </ItemTemplate>
	            </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridFooter">
            <div class="Medium">
                <asp:Label ID="lblTotal" runat="server" meta:resourcekey="lblTotal" Text="Total:"></asp:Label>
                <asp:Literal ID="litTotal" runat="server"></asp:Literal> <asp:Label ID="lblMB1" runat="server" meta:resourcekey="lblMB" Text="MB"></asp:Label>
            </div>
        </div>
    </asp:Panel>
</div>
<div class="FormFooter">
    <asp:Button ID="btnCancel" runat="server" CssClass="Button1" meta:resourcekey="btnCancel" CausesValidation="false" 
        Text="Cancel" OnClick="btnCancel_Click" />
</div>