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
using System.Data;

using ES = WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Ecommerce.EnterpriseServer.TriggerSystem
{
    public class TriggerController
    {
        public static void AddSystemTrigger(string referenceId, string triggerStatus, Type triggerType)
        {
            //
            ITriggerHandler trigger = (ITriggerHandler)Activator.CreateInstance(triggerType);
            //
            EcommerceProvider.AddSystemTrigger(ES.SecurityContext.User.UserId, ES.SecurityContext.User.UserId,
                triggerType.AssemblyQualifiedName, referenceId, trigger.TriggerNamespace, triggerStatus);
        }

        public static void ExecuteSystemTrigger(string referenceId, string triggerNamespace, TriggerEventArgs eventArgs)
        {
            //
            IDataReader dr = null;
            try
            {
                ES.TaskManager.StartTask(SystemTasks.SOURCE_ECOMMERCE, SystemTasks.TASK_EXEC_SYSTEM_TRIGGER);

                List<ITriggerHandler> triggers = new List<ITriggerHandler>();
                dr = EcommerceProvider.GetSystemTrigger(ES.SecurityContext.User.UserId, referenceId, triggerNamespace);
                //
                while(dr.Read())
                {
                    int ownerId = Convert.ToInt32(dr["OwnerID"]);
                    string triggerId = Convert.ToString(dr["TriggerID"]);
                    string triggerHandler = Convert.ToString(dr["TriggerHandler"]);
                    string triggerStatus = (dr["Status"] == DBNull.Value) ? null : Convert.ToString(dr["Status"]);
                    // Instantiate trigger handler
                    ITriggerHandler trigger = (ITriggerHandler)Activator.CreateInstance(Type.GetType(triggerHandler));
                    //
                    trigger.TriggerId = triggerId;
                    trigger.OwnerId = ownerId;
                    trigger.TriggerStatus = triggerStatus;
                    trigger.ReferenceId = referenceId;
                    //
                    triggers.Add(trigger);
                    //
                }
                dr.Close();
                //
                foreach (ITriggerHandler trigger in triggers)
                {
                    try
                    {
                        trigger.ExecuteTrigger(eventArgs);
                    }
                    catch (Exception ex)
                    {
                        ES.TaskManager.WriteError(ex);
                        continue;
                    }
                    //
                    DeleteSystemTrigger(trigger.TriggerId);
                }
            }
            catch (Exception ex)
            {
                throw ES.TaskManager.WriteError(ex);
            }
            finally
            {
                if (dr != null)
                    dr.Close();
                //
                ES.TaskManager.CompleteTask();
            }
        }

        private static void DeleteSystemTrigger(string triggerId)
        {
            EcommerceProvider.DeleteSystemTrigger(ES.SecurityContext.User.UserId, triggerId);
        }
    }
}