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
using WebsitePanel.Providers.Common;
using WebsitePanel.Providers.ResultObjects;
using WebsitePanel.Providers.Virtualization;
using System.Collections.Generic;

namespace WebsitePanel.Portal.VPSForPC
{
    public partial class VpsDetailsGeneral : WebsitePanelModuleBase
    {
        private class ActionButton
        {
            public string Text { get; set; }
            public string Command { get; set; }
            public string Style { get; set; }
            public string OnClientClick { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindGeneralDetails();
        }

        private void BindGeneralDetails()
        {
            VMInfo item = VirtualMachinesForPCHelper.GetCachedVirtualMachineForPC(PanelRequest.ItemID);

            if (!String.IsNullOrEmpty(item.CurrentTaskId))
            {
                DetailsTable.Visible = false;
                return;
            }

            VMInfo vm = null;
            try
            {
                vm = ES.Services.VPSPC.GetVirtualMachineGeneralDetails(PanelRequest.ItemID);
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_GET_VM_DETAILS", ex);
            }

            if (vm != null)
            {
                bool displayRDP = (Request.Browser.Browser == "IE"
                    && Request.Browser.ActiveXControls
                    && Request.Browser.VBScript
                    && vm.State != VMComputerSystemStateInfo.PowerOff
                    && vm.State != VMComputerSystemStateInfo.Paused
                    && vm.State != VMComputerSystemStateInfo.Saved);
                lnkHostname.Text = item.HostName.ToUpper();
                lnkHostname.Visible = displayRDP;

                litHostname.Text = item.HostName.ToUpper();
                litHostname.Visible = !displayRDP;

                litDomain.Text = item.JoinDomain;

                if (!IsPostBack)
                {
                    // set host name change form
                    txtHostname.Text = item.HostName;
                    txtDomain.Text = item.JoinDomain;
                    UpdatePanel1.Attributes.Add("style", "Width:160px; Height:120px;");

                }

                litRdpPageUrl.Text = Page.ResolveUrl("~/DesktopModules/WebsitePanel/VPSForPC/RemoteDesktop/Connect.aspx?ItemID=" + PanelRequest.ItemID + "&Resolution=");

                litUptime.Text = vm.ModifiedTime;
                litStatus.Text = GetLocalizedString("State." + vm.State.ToString());
                litCreated.Text = vm.CreatedDate.ToString();

                // CPU
                vmInfoPerfomence.Visible = (vm.State != VMComputerSystemStateInfo.CreationFailed);
                imgThumbnail.Visible = (vm.State != VMComputerSystemStateInfo.CreationFailed);

                if (vm.State != VMComputerSystemStateInfo.CreationFailed)
                {
                    cpuGauge.Progress = vm.PerfCPUUtilization;
                    litCpuPercentage.Text = String.Format(GetLocalizedString("CpuPercentage.Text"), vm.PerfCPUUtilization);

                    // RAM
                    if (vm.Memory > 0)
                    {
                        int ramPercent = Convert.ToInt32((float)vm.ProcessMemory / (float)vm.Memory * 100);
                        ramGauge.Total = vm.Memory;
                        ramGauge.Progress = vm.ProcessMemory;
                        litRamPercentage.Text = String.Format(GetLocalizedString("MemoryPercentage.Text"), ramPercent);
                        litRamUsage.Text = String.Format(GetLocalizedString("MemoryUsage.Text"), vm.ProcessMemory, vm.Memory);
                    }
                    else
                    {
                        ramGauge.Visible = false;
                        litRamPercentage.Visible = false;
                        litRamUsage.Visible = false;
                        locRam.Visible = false;
                    }

                    // HDD
                    if (vm.HddLogicalDisks != null && vm.HddLogicalDisks.Length > 0)
                    {
                        HddRow.Visible = true;

                        int freeHdd = 0;
                        int sizeHdd = 0;

                        foreach (LogicalDisk disk in vm.HddLogicalDisks)
                        {
                            freeHdd += disk.FreeSpace;
                            sizeHdd += disk.Size;
                        }

                        int usedHdd = sizeHdd - freeHdd;

                        int hddPercent = Convert.ToInt32((float)usedHdd / (float)sizeHdd * 100);
                        hddGauge.Total = sizeHdd;
                        hddGauge.Progress = usedHdd;
                        litHddPercentage.Text = String.Format(GetLocalizedString("HddPercentage.Text"), hddPercent);
                        litHddUsage.Text = String.Format(GetLocalizedString("HddUsage.Text"), freeHdd, sizeHdd, vm.HddLogicalDisks.Length);
                    }

                    // update image
                    imgThumbnail.ImageUrl =
                        String.Format("~/DesktopModules/WebsitePanel/VPSForPC/VirtualMachineImage.ashx?ItemID={0}&rnd={1}",
                        PanelRequest.ItemID, DateTime.Now.Ticks);
                }
                // load virtual machine meta item
                VMInfo vmi = VirtualMachinesForPCHelper.GetCachedVirtualMachineForPC(PanelRequest.ItemID);

                // draw buttons
                List<ActionButton> buttons = new List<ActionButton>();

                vmi.StartTurnOffAllowed = true;
                vmi.RebootAllowed = true;
                vmi.StartTurnOffAllowed = true;
                vmi.PauseResumeAllowed = true;
                vmi.ResetAllowed = true;

                if (vmi.StartTurnOffAllowed
                    && (vm.State == VMComputerSystemStateInfo.PowerOff
                    || vm.State == VMComputerSystemStateInfo.Saved
                    || vm.State == VMComputerSystemStateInfo.Stored))
                    buttons.Add(CreateActionButton("Start", "start.png"));

                if (vm.State == VMComputerSystemStateInfo.Running)
                {
                    if (vmi.RebootAllowed)
                        buttons.Add(CreateActionButton("Reboot", "reboot.png"));

                    if (vmi.StartTurnOffAllowed)
                        buttons.Add(CreateActionButton("ShutDown", "shutdown.png"));
                }

                if (vmi.StartTurnOffAllowed
                    && (vm.State == VMComputerSystemStateInfo.Running
                    || vm.State == VMComputerSystemStateInfo.Paused))
                    buttons.Add(CreateActionButton("TurnOff", "turnoff.png"));

                if (vmi.PauseResumeAllowed
                    && vm.State == VMComputerSystemStateInfo.Running)
                    buttons.Add(CreateActionButton("Pause", "pause.png"));

                if (vmi.PauseResumeAllowed
                    && vm.State == VMComputerSystemStateInfo.Paused)
                    buttons.Add(CreateActionButton("Resume", "start2.png"));

                if (vmi.ResetAllowed
                    && (vm.State == VMComputerSystemStateInfo.Running
                    || vm.State == VMComputerSystemStateInfo.Paused))
                    buttons.Add(CreateActionButton("Reset", "reset2.png"));

                repButtons.DataSource = buttons;
                repButtons.DataBind();

                // other actions
                bool manageAllowed = VirtualMachinesForPCHelper.IsVirtualMachineManagementAllowed(PanelSecurity.PackageId);
                btnChangeHostnamePopup.Visible = manageAllowed;
            }
            else
            {
                DetailsTable.Visible = false;
                messageBox.ShowErrorMessage("VPS_LOAD_VM_ITEM");
            }
        }

        private ActionButton CreateActionButton(string command, string icon)
        {
            ActionButton btn = new ActionButton();
            btn.Command = command;
            btn.Style = String.Format(
                "background: transparent url({0}) left center no-repeat;",
                PortalUtils.GetThemedImage(String.Format("VPS/{0}", icon)));

            string localizedText = GetLocalizedString("Command." + command);
            btn.Text = localizedText != null ? localizedText : command;

            btn.OnClientClick = GetLocalizedString("OnClientClick." + command);

            return btn;
        }

        protected void repButtons_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            try
            {
                ResultObject res = null;

                string command = e.CommandName;
                if (command == "Snapshot")
                {
                    res = ES.Services.VPSPC.CreateSnapshot(PanelRequest.ItemID);
                }
                else
                {
                    // parse command
                    VirtualMachineRequestedState state = (VirtualMachineRequestedState)Enum.Parse(
                        typeof(VirtualMachineRequestedState), command, true);

                    // call services
                    res = ES.Services.VPSPC.ChangeVirtualMachineState(PanelRequest.ItemID, state);
                }

                // check results
                if (res.IsSuccess)
                {
                    if (command == "Snapshot")
                    {
                        // go to snapshots screen
                        Response.Redirect(EditUrl("ItemID", PanelRequest.ItemID.ToString(), "vps_snapshots",
                            "SpaceID=" + PanelSecurity.PackageId.ToString()));
                    }
                    else
                    {
                        // return
                        BindGeneralDetails();
                        return;
                    }
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_ERROR_CHANGE_VM_STATE", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_ERROR_CHANGE_VM_STATE", ex);
            }
        }

        protected void btnChangeHostname_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                string hostname = String.Format("{0}.{1}", txtHostname.Text.Trim(), txtDomain.Text.Trim());

                ResultObject res = ES.Services.VPS.UpdateVirtualMachineHostName(PanelRequest.ItemID,
                    hostname, chkUpdateComputerName.Checked);

                if (res.IsSuccess)
                {
                    // show success message
                    messageBox.ShowSuccessMessage("VPS_CHANGE_VM_HOSTNAME");
                    BindGeneralDetails();

                    // clear fields
                    //txtHostname.Text = "";
                    //txtDomain.Text = "";
                    chkUpdateComputerName.Checked = false;
                }
                else
                {
                    // show error
                    messageBox.ShowMessage(res, "VPS_CHANGE_VM_HOSTNAME", "VPS");
                }
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("VPS_CHANGE_VM_HOSTNAME", ex);
            }
        }
    }
}