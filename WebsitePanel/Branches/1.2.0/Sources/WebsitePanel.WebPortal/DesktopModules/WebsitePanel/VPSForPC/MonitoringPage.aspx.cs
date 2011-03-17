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
    public partial class MonitoringPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartPeriod.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                txtEndPeriod.Text = DateTime.Now.ToShortDateString();
            }

            DateTime StartP = Convert.ToDateTime(txtStartPeriod.Text);
            DateTime EndP = Convert.ToDateTime(txtEndPeriod.Text);

            EndP = (EndP.CompareTo(DateTime.Now.Date) == 0 ? DateTime.Now : EndP);

            PerformanceType pt = PerformanceType.Processor;

            string charType = Page.Request.QueryString["chartType"];

            InitControls(charType, StartP, EndP);

            switch (charType)
            { 
                case "Processor":
                    pt = PerformanceType.Processor;
                    break;
                case "Network" :
                    pt = PerformanceType.Network;
                    break;
                case "Memory" :
                    pt = PerformanceType.Memory;
                    break;
            }

            LoadChartData(ChartCounter, pt, StartP, EndP);
        }

        private void LoadChartData(Chart control, PerformanceType perfType, DateTime startPeriod, DateTime endPeriod)
        {
            PerformanceDataValue[] perfValues = ES.Services.VPSPC.GetPerfomanceValue(PanelRequest.ItemID, perfType, startPeriod, endPeriod);

            if (perfValues != null)
            {
                foreach (PerformanceDataValue item in perfValues)
                {
                    control.Series["series"].Points.AddY(item.SampleValue);
                }
            }
        }

        private void InitControls(string charType, DateTime startPeriod, DateTime endPeriod)
        {
            ChartCounter.Titles.Add(charType);
            ChartCounter.Series["series"].ChartType = (charType.Equals("Processor") ? SeriesChartType.Line : SeriesChartType.SplineArea);
            ChartCounter.Series["series"].IsValueShownAsLabel = true;
            ChartCounter.Series["series"].Color = (!charType.Equals("Memory") ? System.Drawing.Color.FromArgb(220, 65, 140, 240) : ChartCounter.Series["series"].Color);
            ChartCounter.Series["series"]["ShowMarkerLines"] = "True";

            ChartCounter.ChartAreas["chartArea"].AxisX.IsMarginVisible = true;
        }
    }
}