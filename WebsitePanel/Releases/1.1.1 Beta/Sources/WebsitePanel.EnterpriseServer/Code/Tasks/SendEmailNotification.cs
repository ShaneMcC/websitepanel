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

namespace WebsitePanel.EnterpriseServer.Tasks
{
    public class SendEmailNotification : TaskEventHandler
    {
        public override void OnStart()
        {
            // nothing to-do
        }

        public override void OnComplete()
        {
            if (!TaskManager.HasErrors)
            {
                // Send user add notification
                if (TaskManager.TaskSource == "USER" &&
                    TaskManager.TaskName == "ADD" && TaskManager.ItemId > 0)
                {
                    SendAddUserNotification();
                }
                // Send hosting package add notification
                if (TaskManager.TaskSource == "HOSTING_SPACE"
                    && TaskManager.TaskName == "ADD" && TaskManager.ItemId > 0)
                {
                    SendAddPackageNotification();
                }
                // Send hosting package add notification
                if (TaskManager.TaskSource == "HOSTING_SPACE_WR"
                    && TaskManager.TaskName == "ADD" && TaskManager.ItemId > 0)
                {
                    SendAddPackageWithResourcesNotification();
                }
            }
        }

        private void CheckSmtpResult(int resultCode)
        {
            if (resultCode != 0)
            {
                TaskManager.WriteWarning("Unable to send an e-mail notification");
                TaskManager.WriteParameter("SMTP Result", resultCode);
            }
        }

        protected void SendAddPackageWithResourcesNotification()
        {
            try
            {
                bool sendLetter = (bool)TaskManager.TaskParameters["SendLetter"];
                if (sendLetter)
                {
                    int sendResult = PackageController.SendPackageSummaryLetter(TaskManager.ItemId, null, null, true);
                    CheckSmtpResult(sendResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }

        protected void SendAddPackageNotification()
        {
            try
            {
                int userId = (int)TaskManager.TaskParameters["UserId"];
                bool sendLetter = (bool)TaskManager.TaskParameters["SendLetter"];
                bool signup = (bool)TaskManager.TaskParameters["Signup"];
                // send space letter if enabled
                UserSettings settings = UserController.GetUserSettings(userId, UserSettings.PACKAGE_SUMMARY_LETTER);
                if (sendLetter
                    && !String.IsNullOrEmpty(settings["EnableLetter"])
                    && Utils.ParseBool(settings["EnableLetter"], false))
                {
                    // send letter
                    int smtpResult = PackageController.SendPackageSummaryLetter(TaskManager.ItemId, null, null, signup);
                    CheckSmtpResult(smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }

        protected void SendAddUserNotification()
        {
            try
            {
                bool sendLetter = (bool)TaskManager.TaskParameters["SendLetter"];
                int userId = TaskManager.ItemId;
                // send account letter if enabled
                UserSettings settings = UserController.GetUserSettings(userId, UserSettings.ACCOUNT_SUMMARY_LETTER);
                if (sendLetter
                    && !String.IsNullOrEmpty(settings["EnableLetter"])
                    && Utils.ParseBool(settings["EnableLetter"], false))
                {
                    // send letter
                    int smtpResult = PackageController.SendAccountSummaryLetter(userId, null, null, true);
                    CheckSmtpResult(smtpResult);
                }
            }
            catch (Exception ex)
            {
                TaskManager.WriteWarning(ex.StackTrace);
            }
        }
    }
}