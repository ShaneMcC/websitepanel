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
using System.Collections.Specialized;
using System.Xml.Serialization;

namespace WebsitePanel.Ecommerce.EnterpriseServer
{
	/// <summary>
	/// Stores all necessary details for the checkout operation.
	/// </summary>
	public class CheckoutDetails
	{
		private bool persistent;
		private NameValueCollection hash = null;
		public string[][] DetailsArray;

		[XmlIgnore]
		NameValueCollection Details
		{
			get
			{
				if (hash == null)
				{
					// create new dictionary
					hash = new NameValueCollection();

					// fill dictionary
					if (DetailsArray != null)
					{
						foreach (string[] pair in DetailsArray)
							hash.Add(pair[0], pair[1]);
					}
				}
				return hash;
			}
		}

		[XmlIgnore]
		public string this[string key]
		{
			get
			{
				return Details[key];
			}
			set
			{
				// set details
				Details[key] = value;

				// rebuild array
				DetailsArray = new string[Details.Count][];
				for (int i = 0; i < Details.Count; i++)
				{
					DetailsArray[i] = new string[] { Details.Keys[i], Details[Details.Keys[i]] };
				}
			}
		}

		public bool Persistent
		{
			get { return persistent; }
			set { persistent = value; }
		}

		public string[] GetAllKeys()
		{
			return Details.AllKeys;
		}

		public void Remove(string name)
		{
			Details.Remove(name);
		}

		public int GetInt32(string key)
		{
			return Int32.Parse(Details[key]);
		}

		public long GetInt64(string key)
		{
			return Int64.Parse(Details[key]);
		}

		public bool GetBoolean(string key)
		{
			return Boolean.Parse(Details[key]);
		}
	}
}
