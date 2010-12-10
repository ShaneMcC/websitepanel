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
    public class BackgroundTaskLogRecord
    {
        private DateTime date = DateTime.Now;
        private string text;
        private int severity; /* 0 - Info, 1 - Warning, 2 - Error */
        private string[] textParameters;
        private int textIdent = 0;
        private bool innerTaskStart;
        private string exceptionStackTrace;

        public System.DateTime Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public int Severity
        {
            get { return this.severity; }
            set { this.severity = value; }
        }

        public string[] TextParameters
        {
            get { return this.textParameters; }
            set { this.textParameters = value; }
        }

        public int TextIdent
        {
            get { return this.textIdent; }
            set { this.textIdent = value; }
        }

        public bool InnerTaskStart
        {
            get { return this.innerTaskStart; }
            set { this.innerTaskStart = value; }
        }

        public string ExceptionStackTrace
        {
            get { return this.exceptionStackTrace; }
            set { this.exceptionStackTrace = value; }
        }
    }
}
