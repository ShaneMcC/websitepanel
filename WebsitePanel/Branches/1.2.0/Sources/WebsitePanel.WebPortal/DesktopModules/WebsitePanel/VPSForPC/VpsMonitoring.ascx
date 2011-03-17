<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VpsMonitoring.ascx.cs" Inherits="WebsitePanel.Portal.VPSForPC.VpsMonitoring" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Src="../UserControls/SimpleMessageBox.ascx" TagName="SimpleMessageBox" TagPrefix="wsp" %>
<%@ Register Src="UserControls/ServerTabs.ascx" TagName="ServerTabs" TagPrefix="wsp" %>
<%@ Register Src="UserControls/FormTitle.ascx" TagName="FormTitle" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Menu.ascx" TagName="Menu" TagPrefix="wsp" %>
<%@ Register Src="UserControls/Breadcrumb.ascx" TagName="Breadcrumb" TagPrefix="wsp" %>
<%@ Register Src="../UserControls/EnableAsyncTasksSupport.ascx" TagName="EnableAsyncTasksSupport" TagPrefix="wsp" %>

<wsp:EnableAsyncTasksSupport id="asyncTasks" runat="server"/>
<asp:Timer runat="server" Interval="180000" ID="operationTimer" />

<link type="text/css" href="/App_Themes/Default/Styles/jquery-ui-1.8.9.css" rel="stylesheet" />	
<link type="text/css" href="/App_Themes/Default/Styles/jquery.window.css" rel="stylesheet" />	

<script src="JavaScript/jquery-1.4.4.min.js" type="text/javascript"></script>
<script src="JavaScript/jquery-ui-1.8.9.min.js" type="text/javascript"></script>
<script src="JavaScript/jquery.window.js" type="text/javascript"></script>

<div runat="server" id="divWrapper">
<script type="text/javascript">
    function ShowASPanel(chartType) {
        var sUrl = "/DesktopModules/WebsitePanel/VPSForPC/MonitoringPage.aspx" + "?ItemID=" + $("#<%= hItemId.ClientID %>").val() + '&chartType=' + chartType;
        var win = $.window({
            title: "Performance Counter: " + chartType,
            url: sUrl,
            width: "590px"
        });

        win.getFrame().height(500);
        win.resize(600, 500);
        return false;
    };

</script>
</div>

<div id="VpsContainer">
    <div class="Module">
        <asp:HiddenField ID="hItemId" runat="server"  />
	    <div class="Header">
		    <wsp:Breadcrumb id="breadcrumb" runat="server" />
	    </div>
	    <div class="Left">
		        <wsp:Menu id="menu" runat="server" SelectedItem="" />
	    </div>
	    <div class="Content">
		    <div class="Center">
			    <div class="Title" id="divTitle">
				    <asp:Image ID="imgIcon" SkinID="Monitoring48" runat="server" />
                    <wsp:FormTitle ID="locTitle" runat="server" meta:resourcekey="locTitle" Text="Monitoring" />
			    </div>
			    <div class="FormBody">
                    <wsp:ServerTabs id="tabs" runat="server" SelectedTab="vps_monitoring" />	
                    <wsp:SimpleMessageBox id="messageBox" runat="server" />

                    <h1>Monitoring</h1>
               <div id="testClass">
                    <div id="monitoringWrapper">
				        <asp:UpdatePanel runat="server" ID="UpdatePanelCharts" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="operationTimer" EventName="Tick" />
                            </Triggers>
                            <ContentTemplate>                
                            <asp:Chart ID="ChartProc" runat="server" Palette="BrightPastel" 
                                ImageType="Png" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="584px" Height="296px" BorderlineDashStyle="Solid" 
                                BorderWidth="1" BorderColor="181, 64, 1" IsSoftShadows="False">
                                <Series>
							        <asp:Series BorderWidth="1" XValueType="DateTime" Name="series" ChartType="Line" MarkerStyle="Circle" ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="0" YValueType="Double" IsValueShownAsLabel="false"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="20" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <div>
                                <asp:Button ID="btnShowProcessorAsPanel"  runat="server" OnClientClick="return ShowASPanel('Processor')" Text="Show Processor in window" CssClass="Button1"/>
			                </div>
                            <asp:Chart ID="ChartNetwork" runat="server" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="584px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" 
                                BorderWidth="1px" BorderColor="#B54001" IsSoftShadows="False">
                                <Series>
							        <asp:Series MarkerSize="8" BorderWidth="1" XValueType="DateTime" Name="series" 
                                        ChartType="Area" MarkerStyle="Circle" ShadowColor="Black" 
                                        BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="0" 
                                        YValueType="Double" IsValueShownAsLabel="false"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <div>
                                <asp:Button ID="btnShowNetworkAsPanel"  runat="server" OnClientClick="return ShowASPanel('Network')" Text="Show Network in window" CssClass="Button1"/>
			                </div>
                            <asp:Chart ID="ChartMemory" runat="server" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="584px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" 
                                BorderWidth="1px" BorderColor="#B54001" IsSoftShadows="False">
                                <Series>
							        <asp:Series MarkerSize="8" BorderWidth="1" XValueType="DateTime" Name="series" 
                                        ChartType="StackedArea" MarkerStyle="Circle" ShadowColor="Black" 
                                        BorderColor="180, 26, 59, 105" Color="#33CC33" ShadowOffset="0" 
                                        YValueType="Double" IsValueShownAsLabel="false"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
                            <div>
                                <asp:Button ID="btnShowMemoryAsPanel"  runat="server" OnClientClick="return ShowASPanel('Memory')" Text="Show Memory in window" CssClass="Button1"/>
			                </div>

<%--                            <asp:Chart ID="ChartDisc" runat="server" Palette="BrightPastel" BackColor="#F3DFC1" 
                                ImageType="Png" ImageLocation="TempImages/ChartPic_#SEQ(300,3)" 
                                Width="280px" Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" 
                                BorderWidth="2" BorderColor="181, 64, 1">
						        <borderskin SkinStyle="Emboss"></borderskin>
                                <Series>
							        <asp:Series MarkerSize="8" BorderWidth="3" XValueType="Double" Name="series" ChartType="Line" MarkerStyle="Circle" ShadowColor="Black" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" ShadowOffset="2" YValueType="Double"></asp:Series>
                                </Series>
                                <ChartAreas>
							        <asp:ChartArea Name="chartArea" BorderColor="64, 64, 64, 64" BorderDashStyle="Solid" BackSecondaryColor="White" BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
								        <area3dstyle Rotation="25" Perspective="9" LightStyle="Realistic" Inclination="40" IsRightAngleAxes="False" WallWidth="3" IsClustered="False" />
								        <axisy LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisy>
								        <axisx LineColor="64, 64, 64, 64">
									        <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
									        <MajorGrid LineColor="64, 64, 64, 64" />
								        </axisx>
							        </asp:ChartArea>
                                </ChartAreas>
                            </asp:Chart>
--%>                        </ContentTemplate>
                     </asp:UpdatePanel>
                    </div>
            </div>
	        <div class="Right">
		        <asp:Localize ID="FormComments" runat="server" meta:resourcekey="FormComments"></asp:Localize>
	        </div>
        </div>    	
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $(".txtDateTimePeriod").datepicker();
    });
</script>
