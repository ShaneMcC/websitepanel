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

namespace WebsitePanel.Portal
{
    public partial class QuotaEditor : WebsitePanelControlBase
    {
        public int QuotaId
        {
            get { return (int)ViewState["QuotaId"]; }
            set { ViewState["QuotaId"] = value; }
        }

        public int QuotaTypeId
        {
            get { return (int)ViewState["QuotaTypeId"]; }
            set
            {
                ViewState["QuotaTypeId"] = value;

                // toggle controls
                txtQuotaValue.Visible = (value > 1);
                chkQuotaUnlimited.Visible = (value > 1);
                chkQuotaEnabled.Visible = (value == 1);
            }
        }

        public string UnlimitedText
        {
            get { return chkQuotaUnlimited.Text; }
            set { chkQuotaUnlimited.Text = value; }

        }

        public int QuotaValue
        {
            get
            {
                if (QuotaTypeId == 1)
                    // bool quota
                    return chkQuotaEnabled.Checked ? 1 : 0;
                else
                {
                    if (ParentQuotaValue == -1)
                    {
                        // numeric quota
                        return chkQuotaUnlimited.Checked ? -1 : Utils.ParseInt(txtQuotaValue.Text, 0);
                    }
                    else
                    {
                        return
                            chkQuotaUnlimited.Checked
                                ? ParentQuotaValue
                                : Math.Min(Utils.ParseInt(txtQuotaValue.Text, 0), ParentQuotaValue);
                    }
                }                
            }
            set
            {
                txtQuotaValue.Text = value.ToString();
                chkQuotaEnabled.Checked = (value > 0);
                chkQuotaUnlimited.Checked = (value == -1);
            }
        }

        public int ParentQuotaValue
        {
            set
            {
                ViewState["ParentQuotaValue"] = value;
                if (value == 0)
                {
                    txtQuotaValue.Enabled = false;
                    chkQuotaEnabled.Enabled = false;
                    chkQuotaUnlimited.Visible = false;
                    chkQuotaEnabled.Checked = false;
                }

                if (value != -1)
                    chkQuotaUnlimited.Visible = false;
            }
            get
            {
                return ViewState["ParentQuotaValue"] != null ? (int) ViewState["ParentQuotaValue"] : 0;
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            WriteScriptBlock();
        }

        protected override void OnPreRender(EventArgs e)
        {
            // set textbox attributes
            txtQuotaValue.Style["display"] = (txtQuotaValue.Text == "-1") ? "none" : "inline";

            
            
            chkQuotaUnlimited.Attributes["onclick"] = String.Format("ToggleQuota('{0}', '{1}');",
                txtQuotaValue.ClientID, chkQuotaUnlimited.ClientID);

            
            // call base handler
            base.OnPreRender(e);
        }

        private void WriteScriptBlock()
        {
            string scriptKey = "QuataScript";            
            if (!Page.ClientScript.IsClientScriptBlockRegistered(scriptKey))
            {
                Page.ClientScript.RegisterClientScriptBlock(GetType(), scriptKey, @"<script language='javascript' type='text/javascript'>
                        function ToggleQuota(txtId, chkId)
                        {   
                            var unlimited = document.getElementById(chkId).checked;
                            document.getElementById(txtId).style.display = unlimited ? 'none' : 'inline';
                            document.getElementById(txtId).value = unlimited ? '-1' : '0';
                        }
                        </script>");
            }
            
        }
    }
}