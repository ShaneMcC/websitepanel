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
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace WebsitePanel.EnterpriseServer
{
    public class CheckWebSiteTask : SchedulerTask
    {
        private class WebSiteResponse
        {
            public int Status;
            public string Text;
        }

        public override void DoWork()
        {
            // Input parameters:
            //  - URL
            //  - USERNAME
            //  - PASSWORD
            //  - RESPONSE_STATUS
            //  - RESPONSE_CONTAIN
            //  - RESPONSE_DOESNT_CONTAIN
            //  - MAIL_FROM
            //  - MAIL_TO
            //  - MAIL_SUBJECT
            //  - MAIL_BODY

            // get input parameters
            string url = (string)TaskManager.TaskParameters["URL"];
            string username = (string)TaskManager.TaskParameters["USERNAME"];
            string password = (string)TaskManager.TaskParameters["PASSWORD"];
            string strResponseStatus = (string)TaskManager.TaskParameters["RESPONSE_STATUS"];
            string responseContains = (string)TaskManager.TaskParameters["RESPONSE_CONTAIN"];
            string responseNotContains = (string)TaskManager.TaskParameters["RESPONSE_DOESNT_CONTAIN"];

			bool useResponseStatus = Convert.ToBoolean(TaskManager.TaskParameters["USE_RESPONSE_STATUS"]);
			bool useResponseContains = Convert.ToBoolean(TaskManager.TaskParameters["USE_RESPONSE_CONTAIN"]);
			bool useResponseDoesntContain = Convert.ToBoolean(TaskManager.TaskParameters["USE_RESPONSE_DOESNT_CONTAIN"]);

            // check input parameters
            if (String.IsNullOrEmpty(url))
            {
                TaskManager.WriteWarning("Specify 'Web Site URL' task parameter.");
                return;
            }

            if ((String.IsNullOrEmpty(strResponseStatus) || !useResponseStatus)
                && (String.IsNullOrEmpty(responseContains) || !useResponseContains)
                && (String.IsNullOrEmpty(responseNotContains) || !useResponseDoesntContain))
            {
                TaskManager.WriteWarning("Specify one of 'Response Status', 'Response Contain' or 'Response Doesn't Contain' parameters.");
                return;
            }

            int responseStatus = Utils.ParseInt(strResponseStatus, -1);
            if (!String.IsNullOrEmpty(strResponseStatus) && responseStatus == -1)
            {
                TaskManager.WriteWarning("Specify correct response HTTP status, e.g. 404, 500, 503, etc.");
                return;
            }

            // load web site
            WebSiteResponse resp = GetWebDocument(url, username, password);

            // check if there was a generic error
            if (resp.Status == -1)
            {
                SendMailMessage(url, resp.Text, "");
            }

            bool sendMessage = false;

            // check status
            if (responseStatus != -1)
            {
            	sendMessage |= ((resp.Status == responseStatus) && useResponseStatus);
            }

            // check "contains"
            if (!String.IsNullOrEmpty(responseContains))
            {
            	sendMessage |= ((resp.Text.ToLower().IndexOf(responseContains.ToLower()) != -1) && useResponseContains);
            }

            // check "not contains"
            if (!String.IsNullOrEmpty(responseNotContains))
            {
            	sendMessage |= ((resp.Text.ToLower().IndexOf(responseNotContains.ToLower()) == -1) && useResponseDoesntContain);
            }

            if (sendMessage)
                SendMailMessage(url, "", resp.Text);
        }

        private void SendMailMessage(string url, string message, string content)
        {
            // input parameters
            string mailFrom = (string)TaskManager.TaskParameters["MAIL_FROM"];
            string mailTo = (string)TaskManager.TaskParameters["MAIL_TO"];
            string mailSubject = (string)TaskManager.TaskParameters["MAIL_SUBJECT"];
            string mailBody = (string)TaskManager.TaskParameters["MAIL_BODY"];

            if (String.IsNullOrEmpty(mailTo))
            {
                TaskManager.WriteWarning("The e-mail message has not been sent because 'Mail To' is empty.");
            }
            else
            {
                if (String.IsNullOrEmpty(mailFrom))
                    mailFrom = "automatic@localhost";

                if (!String.IsNullOrEmpty(mailSubject))
                {
                    mailSubject = Utils.ReplaceStringVariable(mailSubject, "url", url);
                }

                if (!String.IsNullOrEmpty(mailBody))
                {
                    mailBody = Utils.ReplaceStringVariable(mailBody, "url", url);
                    mailBody = Utils.ReplaceStringVariable(mailBody, "message", message);
                    mailBody = Utils.ReplaceStringVariable(mailBody, "content", content);
                }
                else
                {
                    mailBody = message;
                }

                // send mail message
                MailHelper.SendMessage(mailFrom, mailTo, mailSubject, mailBody, false);
            }
        }

        private WebSiteResponse GetWebDocument(string url, string username, string password)
        {
            WebSiteResponse result = new WebSiteResponse();
            HttpWebResponse resp = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                WebRequest req = WebRequest.Create(url);

                // set site credentials if required
                if (!String.IsNullOrEmpty(username))
                {
                    req.Credentials = new NetworkCredential(username, password);
                }

                resp = (HttpWebResponse)req.GetResponse();
                Stream respStream = resp.GetResponseStream();
                string charSet = !String.IsNullOrEmpty(resp.CharacterSet) ? resp.CharacterSet : "utf-8";
                Encoding encode = System.Text.Encoding.GetEncoding(charSet);

                StreamReader sr = new StreamReader(respStream, encode);

                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);

                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    sb.Append(str);
                    count = sr.Read(read, 0, 256);
                }

                result.Status = (int)resp.StatusCode;
                result.Text = sb.ToString();
            }
            catch (WebException ex)
            {
                result.Status = (int)((HttpWebResponse)ex.Response).StatusCode;
                result.Text = ex.ToString();
                TaskManager.WriteError(ex.ToString());
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Text = ex.ToString();
                TaskManager.WriteError(ex.ToString());
            }
            finally
            {
                if (resp != null)
                {
                    resp.Close();
                }
            }

            return result;
        }
    }
}
