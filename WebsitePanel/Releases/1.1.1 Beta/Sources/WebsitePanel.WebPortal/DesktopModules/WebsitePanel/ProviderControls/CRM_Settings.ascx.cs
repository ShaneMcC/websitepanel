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
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.Common;

namespace WebsitePanel.Portal.ProviderControls
{
    public partial class CRM_Settings : WebsitePanelControlBase, IHostingServiceProviderSettings
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

     

        public void BindSettings(System.Collections.Specialized.StringDictionary settings)
        {
            txtReportingService.Text = settings[Constants.ReportingServer];
            txtSqlServer.Text = settings[Constants.SqlServer];
            txtDomainName.Text = settings[Constants.IFDWebApplicationRootDomain];
            txtPort.Text = settings[Constants.Port];
            txtAppRootDomain.Text = settings[Constants.AppRootDomain];
            int selectedAddressid = FindAddressByText(settings[Constants.CRMWebsiteIP]);
            ddlCrmIpAddress.AddressId = (selectedAddressid > 0) ? selectedAddressid : 0; 
            
            ddlSchema.SelectedValue = settings[Constants.UrlSchema];
            
        }

        public void SaveSettings(System.Collections.Specialized.StringDictionary settings)
        {
            settings[Constants.ReportingServer] = txtReportingService.Text;
            settings[Constants.SqlServer] = txtSqlServer.Text;
            settings[Constants.IFDWebApplicationRootDomain] = txtDomainName.Text;
            settings[Constants.Port] = txtPort.Text;
            settings[Constants.AppRootDomain] = txtAppRootDomain.Text;
            if (ddlCrmIpAddress.AddressId > 0)
			{
				IPAddressInfo address = ES.Services.Servers.GetIPAddress(ddlCrmIpAddress.AddressId);
                if (String.IsNullOrEmpty(address.ExternalIP))
				{
                    settings[Constants.CRMWebsiteIP] = address.InternalIP;
				}
				else
				{
                    settings[Constants.CRMWebsiteIP] = address.ExternalIP;
				}
			}
			else
			{
                settings[Constants.CRMWebsiteIP] = String.Empty;
			}
             
            settings[Constants.UrlSchema] = ddlSchema.SelectedValue;
        }

        private static int FindAddressByText(string address)
        {
            foreach (IPAddressInfo addressInfo in ES.Services.Servers.GetIPAddresses(IPAddressPool.General, PanelRequest.ServerId))
            {
                if (addressInfo.InternalIP == address || addressInfo.ExternalIP == address)
                {
                    return addressInfo.AddressId;
                }
            }
            return 0;
        }

     
    }
}