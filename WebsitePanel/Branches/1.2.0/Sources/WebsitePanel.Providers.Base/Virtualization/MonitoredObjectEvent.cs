using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers.Virtualization
{
    public class MonitoredObjectEvent
    {
        public string Category { get; set; }

        public string Decription { get; set; }

        public string EventData { get; set; }

        public string Level { get; set; }

        public int Number { get; set; }

        public DateTime TimeGenerated { get; set; }
    }
}
