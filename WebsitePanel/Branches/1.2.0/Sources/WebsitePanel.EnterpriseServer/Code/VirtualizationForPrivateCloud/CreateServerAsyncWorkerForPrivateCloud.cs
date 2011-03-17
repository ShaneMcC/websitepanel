//// Copyright (c) 2010, SMB SAAS Systems Inc.
//// All rights reserved.
////
//// Redistribution and use in source and binary forms, with or without modification,
//// are permitted provided that the following conditions are met:
////
//// - Redistributions of source code must  retain  the  above copyright notice, this
////   list of conditions and the following disclaimer.
////
//// - Redistributions in binary form  must  reproduce the  above  copyright  notice,
////   this list of conditions  and  the  following  disclaimer in  the documentation
////   and/or other materials provided with the distribution.
////
//// - Neither  the  name  of  the  SMB SAAS Systems Inc.  nor   the   names  of  its
////   contributors may be used to endorse or  promote  products  derived  from  this
////   software without specific prior written permission.
////
//// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING,  BUT  NOT  LIMITED TO, THE IMPLIED
//// WARRANTIES  OF  MERCHANTABILITY   AND  FITNESS  FOR  A  PARTICULAR  PURPOSE  ARE
//// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
//// ANY DIRECT, INDIRECT, INCIDENTAL,  SPECIAL,  EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//// (INCLUDING, BUT NOT LIMITED TO,  PROCUREMENT  OF  SUBSTITUTE  GOODS OR SERVICES;
//// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)  HOWEVER  CAUSED AND ON
//// ANY  THEORY  OF  LIABILITY,  WHETHER  IN  CONTRACT,  STRICT  LIABILITY,  OR TORT
//// (INCLUDING NEGLIGENCE OR OTHERWISE)  ARISING  IN  ANY WAY OUT OF THE USE OF THIS
//// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//﻿using System;
//using System.Collections.Generic;
//using System.Web;
//using System.Threading;
//using System.ComponentModel;
//using WebsitePanel.Providers;
//using WebsitePanel.Providers.Common;
//using WebsitePanel.Providers.ResultObjects;
//using WebsitePanel.Providers.Virtualization;
//using WebsitePanel.Providers.VirtualizationForPC;

//namespace WebsitePanel.EnterpriseServer
//{
//    public class CreateServerAsyncWorkerForPrivateCloud
//    {

//        public static void Run(VMInfo vmInfo)
//        {
//            BackgroundWorker createVMBackground = new BackgroundWorker();
//            ResultObject taskInfo = null;

//            createVMBackground.DoWork += (sender, e) => {
//                 VMInfo vm = (VMInfo)e.Argument;

//                 // Add audit log
//                 taskInfo = TaskManager.StartResultTask<ResultObject>("VPSForPC", "CREATE");
//                 TaskManager.ItemId = vm.Id;
//                 TaskManager.ItemName = vm.Name;
//                 TaskManager.PackageId = vm.PackageId;

//                 e.Result = CreateVM(vm); 
//            };
            
//            createVMBackground.RunWorkerCompleted += (sender, e) => {
//                if ((e.Error != null) || !String.IsNullOrEmpty(((VMInfo)e.Result).exMessage))
//                {
//                    if (taskInfo != null)
//                    {
//                        TaskManager.CompleteResultTask(taskInfo, VirtualizationErrorCodes.CREATE_ERROR, new Exception(((VMInfo)e.Result).exMessage));
//                    }

//                    PackageController.DeletePackageItem(((VMInfo)e.Result).Id);
//                }
//                else
//                {
//                    if (taskInfo != null)
//                    {
//                        TaskManager.CompleteTask();
//                    }

//                    PackageController.UpdatePackageItem((VMInfo)e.Result);
//                }
//            };

//            createVMBackground.RunWorkerAsync(vmInfo);
//        }

//        private static VMInfo CreateVM(VMInfo vmInfo)
//        {
//            VirtualizationServerForPC ws = new VirtualizationServerForPC();
//            ServiceProviderProxy.Init(ws, vmInfo.ServiceId);

//            vmInfo = ws.CreateVirtualMachine(vmInfo);

//            Providers.Virtualization.VMComputerSystemStateInfo state = vmInfo.State;

//            vmInfo.CurrentJob = null;

//            while (state == Providers.Virtualization.VMComputerSystemStateInfo.UnderCreation)
//            {
//                System.Threading.Thread.Sleep(30000);
//                VMInfo stateVmInfo = ws.GetVirtualMachine(vmInfo.Name);
//                state = stateVmInfo.State;
//            }

//            if (state == Providers.Virtualization.VMComputerSystemStateInfo.PowerOff)
//            {
//                ws.ConfigureCreatedVMNetworkAdapters(vmInfo);
//            }

//            return vmInfo;
//        }

//    }
//}

﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Threading;
using WebsitePanel.Providers.Virtualization;
using WebsitePanel.Providers.VirtualizationForPC;

namespace WebsitePanel.EnterpriseServer
{
    public class CreateVMAsyncWorker
    {
        public int ThreadUserId { get; set; }
        public VMInfo vmTemplate { get; set; }

        public CreateVMAsyncWorker()
        {
            ThreadUserId = -1; // admin
        }

        public void CreateAsync()
        {
            // start asynchronously
            Thread t = new Thread(new ThreadStart(Create));
            t.Start();
        }

        private void Create()
        {
            // impersonate thread
            if (ThreadUserId != -1)
                SecurityContext.SetThreadPrincipal(ThreadUserId);

            // perform backup
            VirtualizationServerControllerForPrivateCloud.CreateVirtualMachineAsunc(vmTemplate);
        }
    }
}