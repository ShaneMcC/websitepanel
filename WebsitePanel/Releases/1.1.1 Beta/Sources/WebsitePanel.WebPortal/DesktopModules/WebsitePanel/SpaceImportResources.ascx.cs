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
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using WebsitePanel.EnterpriseServer;
using WebsitePanel.Providers;

namespace WebsitePanel.Portal
{
    public partial class SpaceImportResources : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			// enable async tasks
			//EnableAsyncTasksSupport();

            if (!IsPostBack)
            {
                PrepareTree();
            }
        }

        private void PrepareTree()
        {
            // prepare tree
            tree.CollapseImageUrl = PortalUtils.GetThemedImage("min.gif");
            tree.ExpandImageUrl = PortalUtils.GetThemedImage("max.gif");
            tree.NoExpandImageUrl = PortalUtils.GetThemedImage("empty.gif");
            tree.Nodes.Clear();

            TreeNode rootNode = new TreeNode();
            rootNode.ImageUrl = PortalUtils.GetThemedImage("folder.png");
            rootNode.Text = GetLocalizedString("Text.Resources");
            rootNode.Value = "Root";
            rootNode.Expanded = true;
            tree.Nodes.Add(rootNode);

            // populate root node
            ServiceProviderItemType[] types = ES.Services.Import.GetImportableItemTypes(PanelSecurity.PackageId);
            foreach (ServiceProviderItemType type in types)
            {
                TreeNode node = new TreeNode();
                node.Value = "-" + type.ItemTypeId.ToString();
                node.Text = GetSharedLocalizedString("ServiceItemType." + type.DisplayName);
                node.PopulateOnDemand = true;
                node.ImageUrl = PortalUtils.GetThemedImage("folder.png");
                rootNode.ChildNodes.Add(node);
            }
        }

        protected void tree_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {
            if (e.Node.Value.StartsWith("-"))
            {
                int itemTypeId = Utils.ParseInt(e.Node.Value.Substring(1), 0);
                string[] items = ES.Services.Import.GetImportableItems(PanelSecurity.PackageId, itemTypeId);

                foreach (string item in items)
                {
                    TreeNode node = new TreeNode();
                    node.Text = item;
                    node.Value = itemTypeId.ToString() + "|" + item;
                    node.ShowCheckBox = true;
                    e.Node.ChildNodes.Add(node);
                }
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            // collect data
            List<string> items = new List<string>();
            CollectNodesData(items, tree.Nodes);

            // import
            int result = ES.Services.Import.ImportItems(true, TaskID, PanelSecurity.PackageId, items.ToArray());

			if (result < 0)
			{
				ShowResultMessage(result);
				return;
			}

            // reset tree
            PrepareTree();

			// show progress dialog
			AsyncTaskID = TaskID;
			AsyncTaskTitle = GetLocalizedString("Text.ImportItems");
        }

        private void CollectNodesData(List<string> items, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                    items.Add(node.Value);

                // process children
                if(node.ChildNodes.Count > 0)
                    CollectNodesData(items, node.ChildNodes);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            RedirectSpaceHomePage();
        }
    }
}