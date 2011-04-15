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
using System.Web;
using System.Globalization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebsitePanel.Ecommerce.Portal
{
	[ValidationProperty("Checked")]
	public class GroupRadioButton : RadioButton, IPostBackDataHandler
	{
		public GroupRadioButton()
			: base()
		{
		}

		#region Properties

		public object ControlValue
		{
			get { return ViewState["ControlValue"]; }
			set { ViewState["ControlValue"] = value; }
		}

		private string Value
		{
			get
			{
				string val = Attributes["value"];
				if (val == null)
					val = UniqueID;
				else
					val = UniqueID + "_" + val;
				return val;
			}
		}

		#endregion

		#region Rendering

		protected override void Render(HtmlTextWriter output)
		{
			RenderInputTag(output);
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
		}

		private void RenderInputTag(HtmlTextWriter htw)
		{
			htw.AddAttribute(HtmlTextWriterAttribute.Id, ClientID);
			htw.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
			htw.AddAttribute(HtmlTextWriterAttribute.Name, GroupName);
			htw.AddAttribute(HtmlTextWriterAttribute.Value, Value);
			if (Checked)
				htw.AddAttribute(HtmlTextWriterAttribute.Checked, "checked");
			if (!Enabled)
				htw.AddAttribute(HtmlTextWriterAttribute.Disabled, "disabled");

			string onClick = Attributes["onclick"];
			if (AutoPostBack)
			{
				if (onClick != null)
					onClick = String.Empty;
				onClick += Page.ClientScript.GetPostBackEventReference(this, String.Empty);
				htw.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
				htw.AddAttribute("language", "javascript");
			}
			else
			{
				if (onClick != null)
					htw.AddAttribute(HtmlTextWriterAttribute.Onclick, onClick);
			}

			if (AccessKey.Length > 0)
				htw.AddAttribute(HtmlTextWriterAttribute.Accesskey, AccessKey);
			if (TabIndex != 0)
				htw.AddAttribute(HtmlTextWriterAttribute.Tabindex,
					TabIndex.ToString(NumberFormatInfo.InvariantInfo));
			htw.RenderBeginTag(HtmlTextWriterTag.Input);
			htw.RenderEndTag();

			// add text to the render
			if (!String.IsNullOrEmpty(Text))
				RenderLabel(htw, Text, ClientID);
		}

		protected virtual void RenderLabel(HtmlTextWriter writer, string text, string clientID)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.For, clientID);
			if ((LabelAttributes != null) && (LabelAttributes.Count != 0))
			{
				LabelAttributes.AddAttributes(writer);
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Label);
			writer.Write(text);
			writer.RenderEndTag();
		}

		#endregion

		#region IPostBackDataHandler Members

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			OnCheckedChanged(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey,
			System.Collections.Specialized.NameValueCollection postCollection)
		{
			bool result = false;
			string value = postCollection[GroupName];
			if ((value != null) && (value == Value))
			{
				if (!Checked)
				{
					Checked = true;
					result = true;
				}
			}
			else
			{
				if (Checked)
					Checked = false;
			}
			return result;
		}

		#endregion
	}
}
