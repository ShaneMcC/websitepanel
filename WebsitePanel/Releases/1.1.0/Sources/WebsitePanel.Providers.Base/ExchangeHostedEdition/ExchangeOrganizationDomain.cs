using System;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Providers.ExchangeHostedEdition
{
    public class ExchangeOrganizationDomain
    {
        public string Identity { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public bool IsTemp { get; set; }
    }
}
