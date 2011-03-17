using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers.Virtualization
{
    public partial class VirtualNetworkInfo
    {        
        public bool BoundToVMHost {get;set;}

        public string DNSServers { get; set; }

        public string DefaultGatewayAddress { get; set; }

        public string Description { get; set; }

        public string EnablingIPAddress { get; set; }

        public bool HighlyAvailable { get; set; }

        public ushort HostBoundVlanId { get; set; }

        public System.Guid Id { get; set; }

        public string Name { get; set; }

        public string NetworkAddress { get; set; }

        public string NetworkMask { get; set; }

        public string Tag { get; set; }

        public string VMHost { get; set; }

        public System.Guid VMHostId { get; set; }

        public string WINServers { get; set; }        
    }

}
