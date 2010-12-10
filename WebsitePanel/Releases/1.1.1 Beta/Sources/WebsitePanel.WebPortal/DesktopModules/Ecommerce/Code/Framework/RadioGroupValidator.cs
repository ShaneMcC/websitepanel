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
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace WebsitePanel.Ecommerce.Portal
{
	public class RadioGroupValidator : CustomValidator
	{
		private List<RadioButton> components;
		private bool buttonsCollected;

		public string RadioButtonsGroup
		{
			get { return (string)ViewState["__RBGroup"]; }
			set { ViewState["__RBGroup"] = value; }
		}

		public string InitialValue
		{
			get
			{
				object obj2 = this.ViewState["InitialValue"];
				if (obj2 != null)
				{
					return (string)obj2;
				}
				return string.Empty;
			}
			set
			{
				this.ViewState["InitialValue"] = value;
			}
		}

		public RadioGroupValidator()
		{
			components = new List<RadioButton>();
		}

		protected override void OnLoad(EventArgs e)
		{
			//
			base.OnLoad(e);
			//
			ClientValidationFunction = "RadioCheckedEvaluateIsValid";
			//
			if (!Page.ClientScript.IsClientScriptIncludeRegistered("EcommerceUtils"))
			{
				Page.ClientScript.RegisterClientScriptInclude("EcommerceUtils",
					Page.ClientScript.GetWebResourceUrl(typeof(RadioGroupValidator), 
						"WebsitePanel.Ecommerce.Portal.Scripts.EcommerceUtils.js"));
			}
		}

		protected override bool ControlPropertiesValid()
		{
			//
			if (String.IsNullOrEmpty(RadioButtonsGroup))
				throw new Exception("'RadioButtonsGroup' property is empty or null");

			return true;
		}

		private void RecursiveRadioButtonsLookup(Control parent, string groupName)
		{
			foreach (Control ctl in parent.Controls)
			{
				if (ctl is RadioButton && ((RadioButton)ctl).GroupName == groupName)
				{
					//
					components.Add((RadioButton)ctl);
				}
				else if (ctl.HasControls())
				{
					RecursiveRadioButtonsLookup(ctl, groupName);
				}
			}
		}

		private void CollectRadioButtons()
		{
			//
			if (!buttonsCollected)
			{
				// workaround for repeater header or footer
				if (Parent is RepeaterItem)
					RecursiveRadioButtonsLookup(Parent.Parent, RadioButtonsGroup);
				else
					RecursiveRadioButtonsLookup(Parent, RadioButtonsGroup);
				//
				buttonsCollected = true;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			//
			CollectRadioButtons();
			//
			string array_values = "";
			//
			foreach (RadioButton rbutton in components)
			{
				if (array_values.Length == 0)
					array_values += String.Format("'{0}'", rbutton.ClientID);
				else
					array_values += String.Format(",'{0}'", rbutton.ClientID);
			}
			//
			Page.ClientScript.RegisterArrayDeclaration(RadioButtonsGroup, array_values);
		}

		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			base.AddAttributesToRender(writer);
			//
			if (RenderUplevel)
			{
				//
				string controlId = this.ClientID;
				//
				AddExpandoAttribute(writer, controlId, "radiogroup", RadioButtonsGroup, false);
			}
		}

		protected override bool EvaluateIsValid()
		{
			//
			CollectRadioButtons();
			//
			foreach (RadioButton rbutton in components)
			{
				//
				PropertyDescriptor descriptor = RequiredFieldValidator.GetValidationProperty(rbutton);
				//
				bool controlChecked = (bool)descriptor.GetValue(rbutton);
				//
				if (controlChecked)
					return true;
			}
			//
			return false;
		}

		protected void AddExpandoAttribute(HtmlTextWriter writer, string controlId, string attributeName, string attributeValue, bool encode)
		{
			if (writer != null)
			{
				writer.AddAttribute(attributeName, attributeValue, encode);
			}
			else
			{
				this.Page.ClientScript.RegisterExpandoAttribute(controlId, attributeName, attributeValue, encode);
			}
		}
	}
}
