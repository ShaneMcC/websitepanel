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
using System.Text;

using WebsitePanel.Server;

namespace WebsitePanel.EnterpriseServer
{
    public class RunSystemCommandTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - SERVER_NAME
            //  - EXECUTABLE_PATH

            // get input parameters
            string serverName = (string)TaskManager.TaskParameters["SERVER_NAME"];
            string execPath = (string)TaskManager.TaskParameters["EXECUTABLE_PATH"];
            string execParams = (string)TaskManager.TaskParameters["EXECUTABLE_PARAMS"];

            if (execParams == null)
                execParams = "";

            // check input parameters
            if (String.IsNullOrEmpty(serverName))
            {
                TaskManager.WriteWarning("Specify 'Server Name' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(execPath))
            {
                TaskManager.WriteWarning("Specify 'Executable Path' task parameter");
                return;
            }

            // find server by name
            ServerInfo server = ServerController.GetServerByName(serverName);
            if (server == null)
            {
                TaskManager.WriteWarning(String.Format("Server with the name '{0}' was not found", serverName));
                return;
            }

            // execute system command
            WindowsServer winServer = new WindowsServer();
            ServiceProviderProxy.ServerInit(winServer, server.ServerId);
            TaskManager.Write(winServer.ExecuteSystemCommand(execPath, execParams));
        }
    }
}
