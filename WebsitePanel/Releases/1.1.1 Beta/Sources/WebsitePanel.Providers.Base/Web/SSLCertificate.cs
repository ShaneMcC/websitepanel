using System;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.Providers.Web
{
    [Serializable]
    public class SSLCertificate
    {
        public int id { get; set; }
        public string FriendlyName { get; set; }
        public string Hostname { get; set; }
        public string DistinguishedName { get; set; }
        public string CSR { get; set; }
        public int CSRLength { get; set; }
        public int SiteID { get; set; }
        public int UserID { get; set; }        
        public bool Installed { get; set; }
        public string PrivateKey { get; set; }
        public string Certificate { get; set; }
        public byte[] Hash { get; set; }
        public string Organisation { get; set; }
        public string OrganisationUnit { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public DateTime ValidFrom { get; set;}
        public DateTime ExpiryDate { get; set; }
        public string SerialNumber { get; set; }
        public byte[] Pfx { get; set; }
        public string Password { get; set; }
        public bool Success { get; set; }
        public bool IsRenewal { get; set; }
        public int PreviousId { get; set; }

        public SSLCertificate()
        {}
                
    }
}
