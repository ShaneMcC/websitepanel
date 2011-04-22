using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.Virtualization;
using WebsitePanel.EnterpriseServer;


namespace WebsitePanel.Portal.VPSForPC
{
    public partial class VpsCheckPoints : WebsitePanelModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCheckPoints();
            }
        }

        protected void LoadCheckPoints() 
        {
            try
            {
                VirtualMachineSnapshot[] checkPoints = ES.Services.VPSPC.GetVirtualMachineSnapshots(PanelRequest.ItemID);

                treeCheckPoints.Nodes.Clear();
                foreach (VirtualMachineSnapshot curr in checkPoints)
                {
                    treeCheckPoints.Nodes.Add(new TreeNode(curr.Name, curr.Id));
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("Load Snapshot error.", ex);
                btnCreateCheckPoint.Enabled = false;
                btnRestoreCheckPoint.Enabled = false;
            }
        }

        protected void btnCreateCheckPoint_Click(object sender, EventArgs e)
        {
            ResultObject ret = ES.Services.VPSPC.CreateSnapshot(PanelRequest.ItemID);

            if (ret.IsSuccess)
            {
                LoadCheckPoints();
                messageBox.ShowSuccessMessage("CreateCheckPointSuccess");
            }
            else
            {
                messageBox.ShowErrorMessage("CreateCheckPointError");
            }
        }

        protected void btnRestoreCheckPoint_Click(object sender, EventArgs e)
        {
            if (treeCheckPoints.SelectedNode != null)
            {
                ResultObject ret = ES.Services.VPSPC.ApplySnapshot(PanelRequest.ItemID, treeCheckPoints.SelectedNode.Value);


                if (ret.IsSuccess)
                {
                    LoadCheckPoints();
                    messageBox.ShowSuccessMessage("RestoreSnapshotSuccess");
                }
                else
                {
                    messageBox.ShowErrorMessage("RestoreSheckPointError");
                }
            }
            else
            {
                messageBox.ShowErrorMessage("RestoreSnapshotNotSelNode");
            }
        }
    }
}