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
using System.Collections.Generic;
using System.Web.UI.WebControls;
using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers.ResultObjects;

namespace WebsitePanel.Portal
{
    public partial class WebApplicationGallery : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			// Maintains appearance settings corresponding to user's display preferences
			gvApplications.PageSize = UsersHelper.GetDisplayItemsPerPage();

            try
            {
				GalleryCategoriesResult result = ES.Services.WebApplicationGallery.GetGalleryCategories(PanelSecurity.PackageId);
				//
				if (!result.IsSuccess)
				{
					ddlCategory.Visible = false;
					messageBox.ShowMessage(result, "WAG_NOT_AVAILABLE", "ModuleWAG");
					return;
				}

                if (!IsPostBack)
                {
                    BindCategories();
					BindApplications();
                }
            }
            catch(Exception ex)
            {
                ShowErrorMessage("GET_WEB_GALLERY_CATEGORIES", ex);             
            }
        }

		protected void gvApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
		{
			gvApplications.PageIndex = e.NewPageIndex;
			//
			BindApplications();
		}

        private void BindCategories()
        {
			GalleryCategoriesResult result = ES.Services.WebApplicationGallery.GetGalleryCategories(PanelSecurity.PackageId);
			//
			ddlCategory.DataSource = result.Value;
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataValueField = "Id";
            ddlCategory.DataBind();

            // add empty
            ddlCategory.Items.Insert(0, new ListItem(GetLocalizedString("SelectCategory.Text"), ""));
        }

		private void BindApplications()
		{
			WebAppGalleryHelpers helper = new WebAppGalleryHelpers();
			//
			GalleryApplicationsResult result = helper.GetGalleryApplications(ddlCategory.SelectedValue, PanelSecurity.PackageId);
			//
			gvApplications.DataSource = result.Value;
			gvApplications.DataBind();
		}

		protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			gvApplications.PageIndex = 0;
			//
			BindApplications();
		}

        protected void odsApplications_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {            
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Install")
                Response.Redirect(EditUrl("ApplicationID", e.CommandArgument.ToString(), "edit",
                    "SpaceID=" + PanelSecurity.PackageId.ToString()));
        }
    }
}