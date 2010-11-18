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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;
using System.Xml;
using System.Collections.Generic;

namespace WebsitePanel.Portal
{
    public partial class UserSpaces : WebsitePanelModuleBase
    {
        XmlNodeList xmlIcons = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            // check for user
            bool isUser = PanelSecurity.SelectedUser.Role == UserRole.User;

            // load icons data
            xmlIcons = this.Module.SelectNodes("Icon");

            if (isUser && xmlIcons != null)
            {
                // USER
                UserPackagesPanel.Visible = true;
                PackagesList.DataSource = new PackagesHelper().GetMyPackages();
                PackagesList.DataBind();

                if (PackagesList.Items.Count == 0)
                {
                    litEmptyList.Text = GetLocalizedString("gvPackages.Empty");
                    EmptyPackagesList.Visible = true;
                }
            }
            else
            {
                // ADMINS, RESELLERS
                ResellerPackagesPanel.Visible = true;
                gvPackages.PageSize = UsersHelper.GetDisplayItemsPerPage();
                gvPackages.Columns[1].Visible = (PanelSecurity.EffectiveUser.Role == UserRole.Administrator);
            }

            // toggle button
            ButtonsPanel.Visible = (PanelSecurity.SelectedUserId != PanelSecurity.EffectiveUserId);
        }

        public string GetSpaceHomePageUrl(int spaceId)
        {
            return PortalUtils.GetSpaceHomePageUrl(spaceId);
        }

        protected void odsPackages_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                ProcessException(e.Exception);
                this.DisableControls = true;
                e.ExceptionHandled = true;
            }
        }

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(EditUrl(PortalUtils.USER_ID_PARAM, PanelSecurity.SelectedUserId.ToString(),
                "create_space"));
        }

        public MenuItemCollection GetIconsDataSource(int packageId)
        {
            // load package context
            PackageContext cntx = PackagesHelper.GetCachedPackageContext(packageId);

            // init collection
            MenuItemCollection items = new MenuItemCollection();

            // get icons list
            foreach (XmlNode xmlNode in xmlIcons)
            {
                // create icon item
                MenuItem iconItem = CreateMenuItem(cntx, xmlNode);
                if (iconItem == null)
                    continue;

                // add into list
                items.Add(iconItem);
            }

            return items;
        }

        public MenuItemCollection GetIconMenuItems(object menuItems)
        {
            return (MenuItemCollection)menuItems;
        }

        public bool IsIconMenuVisible(object menuItems)
        {
            return ((MenuItemCollection)menuItems).Count > 0;
        }

        private MenuItem CreateMenuItem(PackageContext cntx, XmlNode xmlNode)
        {
            string pageId = GetXmlAttribute(xmlNode, "pageID");

            if (!PortalUtils.PageExists(pageId))
                return null;

            string url = GetXmlAttribute(xmlNode, "url");
            string title = GetXmlAttribute(xmlNode, "title");
            string imageUrl = GetXmlAttribute(xmlNode, "imageUrl");
            string target = GetXmlAttribute(xmlNode, "target");
            string resourceGroup = GetXmlAttribute(xmlNode, "resourceGroup");
            string quota = GetXmlAttribute(xmlNode, "quota");
            bool disabled = Utils.ParseBool(GetXmlAttribute(xmlNode, "disabled"), false);

            // get custom page parameters
            XmlNodeList xmlParameters = xmlNode.SelectNodes("Parameters/Add");
            List<string> parameters = new List<string>();
            foreach (XmlNode xmlParameter in xmlParameters)
            {
                parameters.Add(xmlParameter.Attributes["name"].Value
                    + "=" + xmlParameter.Attributes["value"].Value);
            }

            // add menu item
            string pageUrl = !String.IsNullOrEmpty(url) ? url : PortalUtils.NavigatePageURL(
                pageId, PortalUtils.SPACE_ID_PARAM, cntx.Package.PackageId.ToString(), parameters.ToArray());
            string pageName = !String.IsNullOrEmpty(title) ? title : PortalUtils.GetLocalizedPageName(pageId);
            MenuItem item = new MenuItem(pageName, "", "", disabled ? null : pageUrl);
            item.ImageUrl = PortalUtils.GetThemedIcon(imageUrl);

            if (!String.IsNullOrEmpty(target))
                item.Target = target;
            item.Selectable = !disabled;

            // check groups/quotas
            bool display = true;
            if (cntx != null)
            {
                display = (String.IsNullOrEmpty(resourceGroup)
                    || cntx.Groups.ContainsKey(resourceGroup)) &&
                    (String.IsNullOrEmpty(quota)
                    || (cntx.Quotas.ContainsKey(quota) &&
                        cntx.Quotas[quota].QuotaAllocatedValue != 0));
            }

            // process nested menu items
            XmlNodeList xmlMenuNodes = xmlNode.SelectNodes("MenuItems/MenuItem");
            foreach (XmlNode xmlMenuNode in xmlMenuNodes)
            {
                MenuItem menuItem = CreateMenuItem(cntx, xmlMenuNode);
                if (menuItem != null)
                    item.ChildItems.Add(menuItem);
            }

            if (display && !(disabled && item.ChildItems.Count == 0))
                return item;

            return null;
        }

        private string GetXmlAttribute(XmlNode node, string name)
        {
            return node.Attributes[name] != null ? node.Attributes[name].Value : null;
        }
    }
}