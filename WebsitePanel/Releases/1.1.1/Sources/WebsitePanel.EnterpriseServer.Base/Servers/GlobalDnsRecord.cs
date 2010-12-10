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
    [Serializable]
    public class GlobalDnsRecord
    {
        private int recordId;

        public int RecordId
        {
            get { return recordId; }
            set { recordId = value; }
        }

        private int recordOrder;

        public int RecordOrder
        {
            get { return recordOrder; }
            set { recordOrder = value; }
        }
        private int groupId;

        public int GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }
        private int serviceId;

        public int ServiceId
        {
            get { return serviceId; }
            set { serviceId = value; }
        }
        private int serverId;

        public int ServerId
        {
            get { return serverId; }
            set { serverId = value; }
        }
        private int packageId;

        public int PackageId
        {
            get { return packageId; }
            set { packageId = value; }
        }
        private string recordType;

        public string RecordType
        {
            get { return recordType; }
            set { recordType = value; }
        }
        private string recordName;

        public string RecordName
        {
            get { return recordName; }
            set { recordName = value; }
        }
        private string recordData;

        public string RecordData
        {
            get { return recordData; }
            set { recordData = value; }
        }
        private int mxPriority;

        public int MxPriority
        {
            get { return mxPriority; }
            set { mxPriority = value; }
        }

        private int ipAddressId;

        public int IpAddressId
        {
            get { return ipAddressId; }
            set { ipAddressId = value; }
        }

        private string internalIP;
        private string externalIP;

        public GlobalDnsRecord()
        {
        }

        public string InternalIP
        {
            get { return this.internalIP; }
            set { this.internalIP = value; }
        }

        public string ExternalIP
        {
            get { return this.externalIP; }
            set { this.externalIP = value; }
        }
    }
}
