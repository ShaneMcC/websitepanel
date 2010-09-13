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

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	[Serializable]
	public class Category
	{
		private int categoryId;
		private string categoryName;
		private string categorySku;
		private int parentId;
		private int level;
		private string shortDescription;
		private string fullDescription;
		private DateTime created;
		private DateTime modified;
		private int creatorId;
		private int modifierId;
		private int itemOrder;

		public int CategoryId
		{
			get { return this.categoryId; }
			set { this.categoryId = value; }
		}

		public string CategoryName
		{
			get { return this.categoryName; }
			set { this.categoryName = value; }
		}

		public string CategorySku
		{
			get { return this.categorySku; }
			set { this.categorySku = value; }
		}

		public int ParentId
		{
			get { return this.parentId; }
			set { this.parentId = value; }
		}

		public string ShortDescription
		{
			get { return this.shortDescription; }
			set { this.shortDescription = value; }
		}

		public string FullDescription
		{
			get { return this.fullDescription; }
			set { this.fullDescription = value; }
		}

		public System.DateTime Created
		{
			get { return this.created; }
			set { this.created = value; }
		}

		public System.DateTime Modified
		{
			get { return this.modified; }
			set { this.modified = value; }
		}

		public int CreatorId
		{
			get { return this.creatorId; }
			set { this.creatorId = value; }
		}

		public int ModifierId
		{
			get { return this.modifierId; }
			set { this.modifierId = value; }
		}

		public int Level
		{
			get { return this.level; }
			set { this.level = value; }
		}

		public int ItemOrder
		{
			get { return this.itemOrder; }
			set { this.itemOrder = value; }
		}
	}
}