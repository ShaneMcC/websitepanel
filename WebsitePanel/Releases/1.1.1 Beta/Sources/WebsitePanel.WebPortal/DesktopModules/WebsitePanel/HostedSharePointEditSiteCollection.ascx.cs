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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.DNS;
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.Providers.SharePoint;

namespace WebsitePanel.Portal
{
	public partial class HostedSharePointEditSiteCollection : WebsitePanelModuleBase
	{
		SharePointSiteCollection item = null;

		private int OrganizationId
		{
			get
			{
				return PanelRequest.GetInt("ItemID");
			}
		}

		private int SiteCollectionId
		{
			get
			{
				return PanelRequest.GetInt("SiteCollectionID");
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            warningStorage.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");
            editWarningStorage.UnlimitedText = GetLocalizedString("WarningUnlimitedValue");
            
            bool newItem = (this.SiteCollectionId == 0);

			tblEditItem.Visible = newItem;
			tblViewItem.Visible = !newItem;

			//btnUpdate.Visible = newItem;
			btnDelete.Visible = !newItem;
			btnUpdate.Text = newItem ? GetLocalizedString("Text.Add") : GetLocalizedString("Text.Update");
            btnUpdate.OnClientClick = newItem ? GetLocalizedString("btnCreate.OnClientClick") : GetLocalizedString("btnUpdate.OnClientClick");

			btnBackup.Enabled = btnRestore.Enabled = !newItem;

			// bind item
			BindItem();            

			//this.RegisterOwnerSelector();
           
		}

		private void BindItem()
		{
			try
			{
				if (!IsPostBack)
				{
					if (!this.IsDnsServiceAvailable())
					{
						localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_DNS");
					}

					// load item if required
					if (this.SiteCollectionId > 0)
					{
						// existing item
						item = ES.Services.HostedSharePointServers.GetSiteCollection(this.SiteCollectionId);
						if (item != null)
						{
							// save package info
							ViewState["PackageId"] = item.PackageId;
						}
						else
							RedirectToBrowsePage();
					}
					else
					{
						// new item
						ViewState["PackageId"] = PanelSecurity.PackageId;
					}

					//this.gvUsers.DataBind();

					List<CultureInfo> cultures = new List<CultureInfo>();
					foreach (int localeId in ES.Services.HostedSharePointServers.GetSupportedLanguages(PanelSecurity.PackageId))
					{
						cultures.Add(new CultureInfo(localeId, false));
					}

					this.ddlLocaleID.DataSource = cultures;
					this.ddlLocaleID.DataBind();
				}

				if (!IsPostBack)
				{
					// bind item to controls
					if (item != null)
					{
						// bind item to controls
						lnkUrl.Text = item.PhysicalAddress;
                        lnkUrl.NavigateUrl = item.PhysicalAddress;
						litSiteCollectionOwner.Text = String.Format("{0} ({1})", item.OwnerName, item.OwnerEmail);
						litLocaleID.Text = new CultureInfo(item.LocaleId, false).DisplayName;
						litTitle.Text = item.Title;
						litDescription.Text = item.Description;
					    editWarningStorage.QuotaValue = (int)item.WarningStorage;
					    editMaxStorage.QuotaValue = (int)item.MaxSiteStorage;
					}
                    
                    Organization org = ES.Services.Organizations.GetOrganization(OrganizationId);
                        if (org != null)
                        {
                            maxStorage.ParentQuotaValue = org.MaxSharePointStorage;
                            maxStorage.QuotaValue = org.MaxSharePointStorage;

                            editMaxStorage.ParentQuotaValue = org.MaxSharePointStorage;
                            


                            warningStorage.ParentQuotaValue = org.WarningSharePointStorage;
                            warningStorage.QuotaValue = org.WarningSharePointStorage;
                            editWarningStorage.ParentQuotaValue = org.WarningSharePointStorage;
                        }
					
				}
				OrganizationDomainName[] domains = ES.Services.Organizations.GetOrganizationDomains(PanelRequest.ItemID);

				if (domains.Length == 0)
				{
					localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_DOMAINS");
					DisableFormControls(this, btnCancel);
					return;
				}
				//if (this.gvUsers.Rows.Count == 0)
				//{
				//    localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_USERS");
				//    DisableFormControls(this, btnCancel);
				//    return;
				//}
			}
			catch
			{
				                
                localMessageBox.ShowWarningMessage("INIT_SERVICE_ITEM_FORM");
                
			     DisableFormControls(this, btnCancel);
				return;
			}
		}

		private void SaveItem()
		{
			if (!Page.IsValid)
			{
				return;
			}

			
			if (this.SiteCollectionId == 0)
			{
                if (this.userSelector.GetAccount() == null)
                {
                    localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_NO_USERS");
                    return;
                }

                
                // new item
				try
				{
					SharePointSiteCollectionListPaged existentSiteCollections = ES.Services.HostedSharePointServers.GetSiteCollectionsPaged(PanelSecurity.PackageId, this.OrganizationId, "ItemName", String.Format("%{0}", this.domain.DomainName), String.Empty, 0, Int32.MaxValue);
					foreach (SharePointSiteCollection existentSiteCollection in existentSiteCollections.SiteCollections)
					{
						Uri existentSiteCollectionUri = new Uri(existentSiteCollection.Name);
						if (existentSiteCollection.Name == String.Format("{0}://{1}", existentSiteCollectionUri.Scheme, this.domain.DomainName))
						{
							localMessageBox.ShowWarningMessage("HOSTEDSHAREPOINT_DOMAIN_IN_USE");
							return;				
						}
					}

					// get form data
					item = new SharePointSiteCollection();
					item.OrganizationId = this.OrganizationId;
					item.Id = this.SiteCollectionId;
					item.PackageId = PanelSecurity.PackageId;
					item.Name = this.domain.DomainName;
					item.LocaleId = Int32.Parse(this.ddlLocaleID.SelectedValue);
					item.OwnerLogin = this.userSelector.GetAccount();
					item.OwnerEmail = this.userSelector.GetPrimaryEmailAddress();
					item.OwnerName = this.userSelector.GetDisplayName();
					item.Title = txtTitle.Text;
					item.Description = txtDescription.Text;
				    
                    
                    item.MaxSiteStorage = maxStorage.QuotaValue;
				    item.WarningStorage = warningStorage.QuotaValue;

					int result = ES.Services.HostedSharePointServers.AddSiteCollection(item);
					if (result < 0)
					{
						localMessageBox.ShowResultMessage(result);
						return;
					}
				}
				catch (Exception ex)
				{
					localMessageBox.ShowErrorMessage("HOSTEDSHAREPOINT_ADD_SITECOLLECTION", ex);
					return;
				}
			}
            else
			{
                ES.Services.HostedSharePointServers.UpdateQuota(PanelRequest.ItemID, SiteCollectionId, editMaxStorage.QuotaValue, editWarningStorage.QuotaValue);                
			}

			// return
			RedirectToSiteCollectionsList();
		}

		private void AddDnsRecord(int domainId, string recordName, string recordData)
		{
			int result = ES.Services.Servers.AddDnsZoneRecord(domainId, recordName, DnsRecordType.A, recordData, 0);
			if (result < 0)
			{
				ShowResultMessage(result);
			}
		}

		private bool IsDnsServiceAvailable()
		{
			ProviderInfo dnsProvider = ES.Services.Servers.GetPackageServiceProvider(PanelSecurity.PackageId, ResourceGroups.Dns);
			return dnsProvider != null;
		}

		private void DeleteItem()
		{
			// delete
			try
			{
				int result = ES.Services.HostedSharePointServers.DeleteSiteCollection(this.SiteCollectionId);
				if (result < 0)
				{
					ShowResultMessage(result);
					return;
				}
			}
			catch (Exception ex)
			{
				localMessageBox.ShowErrorMessage("HOSTEDSHAREPOINT_DELETE_SITECOLLECTION", ex);
				return;
			}

			// return
			RedirectToSiteCollectionsList();
		}

		protected void odsAccountsPaged_Selected(object sender, ObjectDataSourceStatusEventArgs e)
		{
			if (e.Exception != null)
			{
				localMessageBox.ShowErrorMessage("ORGANIZATION_GET_USERS", e.Exception);
				e.ExceptionHandled = true;
			}
		}


		protected void btnCancel_Click(object sender, EventArgs e)
		{
			// return
			RedirectToSiteCollectionsList();
		}

		protected void btnDelete_Click(object sender, EventArgs e)
		{
			DeleteItem();	
		}

		protected void btnUpdate_Click(object sender, EventArgs e)
		{
			SaveItem();
		}
		protected void btnBackup_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_backup_sitecollection", "SiteCollectionID=" + this.SiteCollectionId,"ItemID=" + PanelRequest.ItemID.ToString()));
		}

		protected void btnRestore_Click(object sender, EventArgs e)
		{
			Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_restore_sitecollection", "SiteCollectionID=" + this.SiteCollectionId, "ItemID=" + PanelRequest.ItemID.ToString()));
		}


		private void RedirectToSiteCollectionsList()
		{
			Response.Redirect(EditUrl("SpaceID", PanelSecurity.PackageId.ToString(), "sharepoint_sitecollections", "ItemID=" + PanelRequest.ItemID.ToString()));
		}

		//private void RegisterOwnerSelector()
		//{
		//    // Define the name and type of the client scripts on the page.
		//    String csname = "OwnerSelectorScript";
		//    Type cstype = this.GetType();

		//    // Get a ClientScriptManager reference from the Page class.
		//    ClientScriptManager cs = Page.ClientScript;

		//    // Check to see if the client script is already registered.
		//    if (!cs.IsClientScriptBlockRegistered(cstype, csname))
		//    {
		//        StringBuilder ownerSelector = new StringBuilder();
		//        ownerSelector.Append("<script type=text/javascript> function DoSelectOwner(ownerId, ownerDisplayName, email) {");
		//        ownerSelector.AppendFormat("{0}.{1}.value=ownerId;", this.Page.Form.ID, this.hdnSiteCollectionOwner.ClientID);
		//        ownerSelector.AppendFormat("{0}.{1}.value=ownerDisplayName;", this.Page.Form.ID, this.txtSiteCollectionOwner.ClientID);
		//        ownerSelector.AppendFormat("{0}.{1}.value=email;", this.Page.Form.ID, this.hdnSiteCollectionOwnerEmail.ClientID);
		//        ownerSelector.Append("} </script>");
		//        cs.RegisterClientScriptBlock(cstype, csname, ownerSelector.ToString(), false);
		//    }

		//}

		//private StringDictionary ConvertArrayToDictionary(string[] settings)
		//{
		//    StringDictionary r = new StringDictionary();
		//    foreach (string setting in settings)
		//    {
		//        int idx = setting.IndexOf('=');
		//        r.Add(setting.Substring(0, idx), setting.Substring(idx + 1));
		//    }
		//    return r;
		//}
	}
}