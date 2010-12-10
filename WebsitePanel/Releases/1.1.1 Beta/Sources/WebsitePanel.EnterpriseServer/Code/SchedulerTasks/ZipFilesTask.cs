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

namespace WebsitePanel.EnterpriseServer
{
    public class ZipFilesTask : SchedulerTask
    {
        public override void DoWork()
        {
            // Input parameters:
            //  - FOLDER
            //  - ZIP_FILE

            // get input parameters
            string filesList = (string)TaskManager.TaskParameters["FOLDER"];
            string zipFile = (string)TaskManager.TaskParameters["ZIP_FILE"];

            // check input parameters
            if (String.IsNullOrEmpty(filesList))
            {
                TaskManager.WriteWarning("Specify 'Files List' task parameter");
                return;
            }

            if (String.IsNullOrEmpty(zipFile))
            {
                TaskManager.WriteWarning("Specify 'Zip File' task parameter");
                return;
            }

            // substitute parameters
            DateTime d = DateTime.Now;
            string date = d.ToString("yyyyMMdd");
            string time = d.ToString("HHmm");

            filesList = Utils.ReplaceStringVariable(filesList, "date", date);
            filesList = Utils.ReplaceStringVariable(filesList, "time", time);
            zipFile = Utils.ReplaceStringVariable(zipFile, "date", date);
            zipFile = Utils.ReplaceStringVariable(zipFile, "time", time);

            // zip files and folders
            FilesController.ZipFiles(TaskManager.PackageId, new string[] { filesList }, zipFile);
        }
    }
}
