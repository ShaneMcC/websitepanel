using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using WebsitePanel.Providers.Virtualization;

namespace WebsitePanel.Portal.VPSForPC
{
    public partial class VpsMonitoring : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hItemId.Value = PanelRequest.ItemID.ToString();
            }

            DateTime StartP = DateTime.Now.AddDays(-1);
            DateTime EndP = DateTime.Now;

            EndP = (EndP.CompareTo(DateTime.Now.Date) == 0 ? DateTime.Now : EndP);

            InitControls(StartP, EndP);

            LoadChartData(ChartProc, PerformanceType.Processor, StartP, EndP);
            LoadChartData(ChartNetwork, PerformanceType.Network,  StartP, EndP);
            LoadChartData(ChartMemory, PerformanceType.Memory,  StartP, EndP);
        }

        private void LoadChartData(Chart control, PerformanceType perfType, DateTime startPeriod, DateTime endPeriod)
        {
            PerformanceDataValue[] perfValues = ES.Services.VPSPC.GetPerfomanceValue(PanelRequest.ItemID, perfType, startPeriod, endPeriod);

            if (perfValues != null)
            {
                foreach (PerformanceDataValue item in perfValues)
                {
                    control.Series["series"].Points.AddXY(item.TimeSampled.ToString(), item.SampleValue);
                }
            }
        }

        private void InitControls(DateTime startPeriod, DateTime endPeriod)
        {
            ChartProc.Titles.Add("Processor");
            ChartProc.Series["series"].ChartType = SeriesChartType.Line;
            ChartProc.Series["series"]["ShowMarkerLines"] = "True";
            ChartProc.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;

            ChartNetwork.Titles.Add("Network");
            ChartNetwork.Series["series"].ChartType = SeriesChartType.SplineArea;
            ChartNetwork.Series["series"]["ShowMarkerLines"] = "True";
            ChartNetwork.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;

            ChartMemory.Titles.Add("Memory");
            ChartMemory.Series["series"].ChartType = SeriesChartType.SplineArea;
            ChartMemory.Series["series"]["ShowMarkerLines"] = "True";
            ChartMemory.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;

            //ChartDisc.Titles.Add("Disk I/O");
            //ChartDisc.Series["series"].ChartType = SeriesChartType.Line;
            //ChartDisc.Series["series"].IsValueShownAsLabel = true;
            //ChartDisc.Series["series"]["ShowMarkerLines"] = "True";
            //ChartDisc.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;

        }
    }
}