// Copyright (c) 2010, SMB SAAS Systems Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
//
// - Redistributions of source code must  retain  the  above copyright notice, this
//   list of conditions and the following disclaimer.
//
// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
//   this list of conditions  and  the  following  disclaimer in  the documentation
//   and/or other materials provided with the distribution.
//
// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
//   contributors may be used to endorse or  promote  products  derived  from  this
//   software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using WebsitePanel.Providers;
using WebsitePanel.Providers.DNS;
using WebsitePanel.Server.Utils;
using Microsoft.Web.Services3;

namespace WebsitePanel.Server
{
    /// <summary>
    /// Summary description for DNSServer
    /// </summary>
    [WebService(Namespace = "http://smbsaas/websitepanel/server/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [Policy("ServerPolicy")]
    [ToolboxItem(false)]
    public class DNSServer : HostingServiceProviderWebService, IDnsServer
    {
        private IDnsServer DnsProvider
        {
            get { return (IDnsServer)Provider; }
        }

        #region Zones
        [WebMethod, SoapHeader("settings")]
        public bool ZoneExists(string zoneName)
        {
            try
            {
                Log.WriteStart("'{0}' ZoneExists", ProviderSettings.ProviderName);
                bool result = DnsProvider.ZoneExists(zoneName);
                Log.WriteEnd("'{0}' ZoneExists", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' ZoneExists", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public string[] GetZones()
        {
            try
            {
                Log.WriteStart("'{0}' GetZones", ProviderSettings.ProviderName);
                string[] result = DnsProvider.GetZones();
                Log.WriteEnd("'{0}' GetZones", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetZones", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddPrimaryZone(string zoneName, string[] secondaryServers)
        {
            try
            {
                Log.WriteStart("'{0}' AddPrimaryZone", ProviderSettings.ProviderName);
                DnsProvider.AddPrimaryZone(zoneName, secondaryServers);
                Log.WriteEnd("'{0}' AddPrimaryZone", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddPrimaryZone", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddSecondaryZone(string zoneName, string[] masterServers)
        {
            try
            {
                Log.WriteStart("'{0}' AddSecondaryZone", ProviderSettings.ProviderName);
                DnsProvider.AddSecondaryZone(zoneName, masterServers);
                Log.WriteEnd("'{0}' AddSecondaryZone", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddSecondaryZone", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteZone(string zoneName)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteZone", ProviderSettings.ProviderName);
                DnsProvider.DeleteZone(zoneName);
                Log.WriteEnd("'{0}' DeleteZone", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteZone", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void UpdateSoaRecord(string zoneName, string host, string primaryNsServer, string primaryPerson)
        {
            try
            {
                Log.WriteStart("'{0}' UpdateSoaRecord", ProviderSettings.ProviderName);
                DnsProvider.UpdateSoaRecord(zoneName, host, primaryNsServer, primaryPerson);
                Log.WriteEnd("'{0}' UpdateSoaRecord", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' UpdateSoaRecord", ProviderSettings.ProviderName), ex);
                throw;
            }
        }
        #endregion

        #region Records
        [WebMethod, SoapHeader("settings")]
        public DnsRecord[] GetZoneRecords(string zoneName)
        {
            try
            {
                Log.WriteStart("'{0}' GetZoneRecords", ProviderSettings.ProviderName);
                DnsRecord[] result = DnsProvider.GetZoneRecords(zoneName);
                Log.WriteEnd("'{0}' GetZoneRecords", ProviderSettings.ProviderName);
                return result;
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' GetZoneRecords", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                Log.WriteStart("'{0}' AddZoneRecord", ProviderSettings.ProviderName);
                DnsProvider.AddZoneRecord(zoneName, record);
                Log.WriteEnd("'{0}' AddZoneRecord", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddZoneRecord", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteZoneRecord(string zoneName, DnsRecord record)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteZoneRecord", ProviderSettings.ProviderName);
                DnsProvider.DeleteZoneRecord(zoneName, record);
                Log.WriteEnd("'{0}' DeleteZoneRecord", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteZoneRecord", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void AddZoneRecords(string zoneName, DnsRecord[] records)
        {
            try
            {
                Log.WriteStart("'{0}' AddZoneRecords", ProviderSettings.ProviderName);
                DnsProvider.AddZoneRecords(zoneName, records);
                Log.WriteEnd("'{0}' AddZoneRecords", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' AddZoneRecords", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        [WebMethod, SoapHeader("settings")]
        public void DeleteZoneRecords(string zoneName, DnsRecord[] records)
        {
            try
            {
                Log.WriteStart("'{0}' DeleteZoneRecords", ProviderSettings.ProviderName);
                DnsProvider.DeleteZoneRecords(zoneName, records);
                Log.WriteEnd("'{0}' DeleteZoneRecords", ProviderSettings.ProviderName);
            }
            catch (Exception ex)
            {
                Log.WriteError(String.Format("'{0}' DeleteZoneRecords", ProviderSettings.ProviderName), ex);
                throw;
            }
        }

        #endregion
    }
}
