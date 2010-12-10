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
	public class DomainNameCycle
	{
		private int cycleId;
		private string cycleName;
		private string billingPeriod;
		private int periodLength;
		private decimal setupFee;
		private decimal recurringFee;
		private decimal transferFee;
		private int sortOrder;

		public int CycleId
		{
			get { return cycleId; }
			set { cycleId = value; }
		}

		public string CycleName
		{
			get { return cycleName; }
			set { cycleName = value; }
		}

		public string BillingPeriod
		{
			get { return billingPeriod; }
			set { billingPeriod = value; }
		}

		public int PeriodLength
		{
			get { return periodLength; }
			set { periodLength = value; }
		}

		public decimal SetupFee
		{
			get { return setupFee; }
			set { setupFee = value; }
		}

		public decimal RecurringFee
		{
			get { return recurringFee; }
			set { recurringFee = value; }
		}

		public decimal TransferFee
		{
			get { return transferFee; }
			set { transferFee = value; }
		}

		public int SortOrder
		{
			get { return sortOrder; }
			set { sortOrder = value; }
		}
	}
}