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

namespace WebsitePanel.Providers.HostedSolution
{
    public class OrganizationStatistics    
	{
		private int allocatedUsers;
		private int createdUsers;

		private int allocatedDomains;
		private int createdDomains;

        private int allocatedMailboxes;
        private int createdMailboxes;

        private int allocatedContacts;
        private int createdContacts;

        private int allocatedDistributionLists;
        private int createdDistributionLists;

        private int allocatedPublicFolders;
        private int createdPublicFolders;

        private int allocatedDiskSpace;
        private int usedDiskSpace;

		private int allocatedSharePointSiteCollections;
		private int createdSharePointSiteCollections;

        private int createdCRMUsers;
        private int allocatedCRMUsers;
		
		
        public int CreatedCRMUsers
        {
            get { return createdCRMUsers; }
            set { createdCRMUsers = value; }
        }

        public int AllocatedCRMUsers
        {
            get { return allocatedCRMUsers; }
            set { allocatedCRMUsers = value; }
        }
        
        public int AllocatedUsers
		{
			get { return allocatedUsers; }
			set { allocatedUsers = value; }
		}

		public int CreatedUsers
		{
			get { return createdUsers; }
			set { createdUsers = value; }
		}
		
        public int AllocatedMailboxes
        {
            get { return allocatedMailboxes; }
            set { allocatedMailboxes = value; }
        }

        public int CreatedMailboxes
        {
            get { return createdMailboxes; }
            set { createdMailboxes = value; }
        }

        public int AllocatedContacts
        {
            get { return allocatedContacts; }
            set { allocatedContacts = value; }
        }

        public int CreatedContacts
        {
            get { return createdContacts; }
            set { createdContacts = value; }
        }

        public int AllocatedDistributionLists
        {
            get { return allocatedDistributionLists; }
            set { allocatedDistributionLists = value; }
        }

        public int CreatedDistributionLists
        {
            get { return createdDistributionLists; }
            set { createdDistributionLists = value; }
        }

        public int AllocatedPublicFolders
        {
            get { return allocatedPublicFolders; }
            set { allocatedPublicFolders = value; }
        }

        public int CreatedPublicFolders
        {
            get { return createdPublicFolders; }
            set { createdPublicFolders = value; }
        }

        public int AllocatedDomains
        {
            get { return allocatedDomains; }
            set { allocatedDomains = value; }
        }

        public int CreatedDomains
        {
            get { return createdDomains; }
            set { createdDomains = value; }
        }

        public int AllocatedDiskSpace
        {
            get { return allocatedDiskSpace; }
            set { allocatedDiskSpace = value; }
        }

        public int UsedDiskSpace
        {
            get { return usedDiskSpace; }
            set { usedDiskSpace = value; }
        }

		public int AllocatedSharePointSiteCollections
		{
			get { return allocatedSharePointSiteCollections; }
			set { allocatedSharePointSiteCollections = value; }
		}

		public int CreatedSharePointSiteCollections
		{
			get { return createdSharePointSiteCollections; }
			set { createdSharePointSiteCollections = value; }
		}

        public int CreatedBlackBerryUsers { get; set; }
        public int AllocatedBlackBerryUsers { get; set; }


        public int CreatedOCSUsers { get; set; }
        public int AllocatedOCSUsers { get; set; }

	}
}

