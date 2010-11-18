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
using System.Data;
using System.Configuration;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebsitePanel.Ecommerce.Portal
{
	public class UrlBuilder : UriBuilder
	{
		NameValueCollection queryString = null;

		#region Properties

		public NameValueCollection QueryString
		{
			get
			{
				if (queryString == null)
				{
					queryString = new NameValueCollection();
				}

				return queryString;
			}
		}

		public string PageName
		{
			get
			{
				string path = base.Path;
				return path.Substring(path.LastIndexOf("/") + 1);
			}
			set
			{
				string path = base.Path;
				path = path.Substring(0, path.LastIndexOf("/"));
				base.Path = string.Concat(path, "/", value);
			}
		}

		#endregion

		public static UrlBuilder FromCurrentRequest()
		{
			return new UrlBuilder(
				HttpContext.Current.Request.Url.AbsoluteUri
			);
		}

		public static UrlBuilder FromScratch()
		{
			UrlBuilder builder = FromCurrentRequest();
			builder.QueryString.Clear();

			return builder;
		}

		public static UrlBuilder FromSpecifiedString(string uri)
		{
			return new UrlBuilder(uri);
		}

		public static UrlBuilder FromSpecifiedPage(System.Web.UI.Page page)
		{
			return new UrlBuilder(page.Request.Url.AbsoluteUri);
		}

		#region Constructor overloads

		private UrlBuilder()
		{
		}

		private UrlBuilder(string uri)
			: base(uri)
		{
			PopulateQueryString();
		}

		private UrlBuilder(Uri uri)
			: base(uri)
		{
			PopulateQueryString();
		}

		private UrlBuilder(string schemeName, string hostName)
			: base(schemeName, hostName)
		{
		}

		private UrlBuilder(string scheme, string host, int portNumber)
			: base(scheme, host, portNumber)
		{
		}

		private UrlBuilder(string scheme, string host, int port, string pathValue)
			: base(scheme, host, port, pathValue)
		{
		}

		private UrlBuilder(string scheme, string host, int port, string path, string extraValue)
			: base(scheme, host, port, path, extraValue)
		{
		}

		private UrlBuilder(System.Web.UI.Page page)
			: base(page.Request.Url.AbsoluteUri)
		{
			PopulateQueryString();
		}

		#endregion

		#region Public methods

		public new string ToString()
		{
			GetQueryString();

			if (Port == 80 || Port == 443)
				return Uri.AbsoluteUri.Replace(String.Concat(":", Port), String.Empty);

			return Uri.AbsoluteUri;
		}

		public void Navigate()
		{
			_Navigate(true);
		}

		public void Navigate(bool endResponse)
		{
			_Navigate(endResponse);
		}

		private void _Navigate(bool endResponse)
		{
			string uri = this.ToString();
			
			HttpContext.Current.Response.Redirect(uri, endResponse);
		}

		#endregion

		#region Private methods

		private void PopulateQueryString()
		{
			string query = base.Query;

			if (queryString == null)
				queryString = new NameValueCollection();
			else
				queryString.Clear();

			if (String.IsNullOrEmpty(query))
			{
				/*foreach (string key in HttpContext.Current.Request.Params)
				{
					queryString[key] = HttpContext.Current.Request.Params[key];
				}*/
			}
			else
			{
				query = query.Substring(1); //remove the ?

				string[] pairs = query.Split(new char[] { '&' });
				foreach (string s in pairs)
				{
					string[] pair = s.Split(new char[] { '=' });

					queryString[pair[0]] = (pair.Length > 1) ? pair[1] : string.Empty;
				}
			}
		}

		private void GetQueryString()
		{
			int count = queryString.Count;

			if (count == 0)
			{
				base.Query = string.Empty;
				return;
			}

			string[] pairs = new string[count];

			for (int i = 0; i < count; i++)
			{
				pairs[i] = String.Concat(queryString.AllKeys[i], "=", queryString[i]);
			}

			base.Query = string.Join("&", pairs);
		}

		#endregion
	}
}
