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

﻿using System;
using WebsitePanel.Providers.WebAppGallery;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.Portal
{
    public partial class WebApplicationGalleryInstall : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindApplicationDetails();
            }
        }

        private void BindApplicationDetails()
        {
            try
            {
                GalleryApplicationResult appResult = ES.Services.WebApplicationGallery.GetGalleryApplicationDetails(PanelSecurity.PackageId,
                                                                                    PanelRequest.ApplicationID);
                // check for errors
                if (!appResult.IsSuccess)
                {
                    messageBox.ShowMessage(appResult, "WAG_NOT_AVAILABLE", "WebAppGallery");
                    return;
                }

                // bind details
                if (appResult.Value != null)
                    appHeader.BindApplicationDetails(appResult.Value);

                // check for warnings
                if (appResult.ErrorCodes.Count > 0)
                {
                    // app does not meet requirements
                    messageBox.ShowMessage(appResult, "WAG_CANNOT_INSTALL_APPLICATION", "WebAppGallery");
                    btnInstall.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("GET_GALLERY_APPLIACTION_DETAILS", ex);
            }
        }

        protected void btnInstall_Click(object sender, EventArgs e)
        {
			// Ensure server-side validation has taken its part in the play
			if (!Page.IsValid)
				return;

            bool isSuccess = true;
            try
            {
                GalleryWebAppStatus status;
                do
                {
                    status =
                        ES.Services.WebApplicationGallery.GetGalleryApplicationStatus(PanelSecurity.PackageId,
                                                                                      PanelRequest.ApplicationID);
                }
				while (status == GalleryWebAppStatus.Downloading);
				//
				switch(status)
				{
					case GalleryWebAppStatus.Failed:
					case GalleryWebAppStatus.NotDownloaded:
						ShowErrorMessage("GALLERY_APP_DOWNLOAD_FAILED");
						isSuccess = false;
						break;
				}
            }
            catch(Exception ex)
            {
                isSuccess = false;
                ShowErrorMessage("GET_GALLERY_APPLICATION_STATUS", ex);                
            }

            if (isSuccess)
            {
                Response.Redirect(EditUrl("ApplicationID", PanelRequest.ApplicationID, "editParams",
                                          "SpaceID=" + PanelSecurity.PackageId));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }

    }
}