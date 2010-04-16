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
    public partial class WebApplicationGalleryHeader :  WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GalleryApplicationResult appResult =
                        ES.Services.WebApplicationGallery.GetGalleryApplicationDetails(PanelSecurity.PackageId,
                                                                                     PanelRequest.ApplicationID);
					//
					if (!appResult.IsSuccess)
					{
						messageBox.ShowMessage(appResult, "WAG_NOT_AVAILABLE", "ModuleWAG");
						return;
					}
					//
					GalleryApplication application = appResult.Value;
                    if (application != null)
                    {
                        lblVersion.Text = application.Version;
                        lblDescription.Text = application.Description;
                        lblTitle.Text = application.Title;
                        lblSize.Text = application.Size;
                        imgLogo.ImageUrl = "~/DesktopModules/WebsitePanel/resizeimage.ashx?url=" + Server.UrlEncode(application.IconUrl) +
                                           "&width=250&height=250";

                        hlAuthor.Text = application.AuthorName;
                        hlAuthor.NavigateUrl = application.AuthorUrl;



                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("GET_GALLERY_APPLIACTION_DETAILS", ex);
            }
        }
    }
}