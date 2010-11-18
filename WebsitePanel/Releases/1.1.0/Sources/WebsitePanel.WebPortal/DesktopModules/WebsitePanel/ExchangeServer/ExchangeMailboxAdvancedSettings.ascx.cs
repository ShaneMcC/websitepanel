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
using WebsitePanel.Providers.HostedSolution;
using WebsitePanel.EnterpriseServer;

namespace WebsitePanel.Portal.ExchangeServer
{
	public partial class ExchangeMailboxAdvancedSettings : WebsitePanelModuleBase
	{
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindSettings();
            }
        }

        private void BindSettings()
        {
            try
            {
                // get settings
                ExchangeMailbox mailbox = ES.Services.ExchangeServer.GetMailboxAdvancedSettings(
                    PanelRequest.ItemID, PanelRequest.AccountID);

				// title
				litDisplayName.Text = mailbox.DisplayName;

				// load space context
				PackageContext cntx = PackagesHelper.GetCachedPackageContext(PanelSecurity.PackageId);

				chkPOP3.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_POP3ALLOWED) && !cntx.Quotas[Quotas.EXCHANGE2007_POP3ALLOWED].QuotaExhausted;
				chkIMAP.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_IMAPALLOWED) && !cntx.Quotas[Quotas.EXCHANGE2007_IMAPALLOWED].QuotaExhausted;
				chkOWA.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_OWAALLOWED) && !cntx.Quotas[Quotas.EXCHANGE2007_OWAALLOWED].QuotaExhausted;
				chkMAPI.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_MAPIALLOWED) && !cntx.Quotas[Quotas.EXCHANGE2007_MAPIALLOWED].QuotaExhausted;
				chkActiveSync.Visible = cntx.Quotas.ContainsKey(Quotas.EXCHANGE2007_ACTIVESYNCALLOWED) && !cntx.Quotas[Quotas.EXCHANGE2007_ACTIVESYNCALLOWED].QuotaExhausted;

                // bind form
                chkPOP3.Checked = mailbox.EnablePOP;
                chkIMAP.Checked = mailbox.EnableIMAP;
                chkOWA.Checked = mailbox.EnableOWA;
                chkMAPI.Checked = mailbox.EnableMAPI;
                chkActiveSync.Checked = mailbox.EnableActiveSync;

				lblTotalItems.Text = mailbox.TotalItems.ToString();
				lblTotalSize.Text = mailbox.TotalSizeMB.ToString();
				lblLastLogon.Text = Utils.FormatDateTime(mailbox.LastLogon);
				lblLastLogoff.Text = Utils.FormatDateTime(mailbox.LastLogoff);

                sizeIssueWarning.ValueKB = mailbox.IssueWarningKB;
				sizeProhibitSend.ValueKB = mailbox.ProhibitSendKB;
				sizeProhibitSendReceive.ValueKB = mailbox.ProhibitSendReceiveKB;

                daysKeepDeletedItems.ValueDays = mailbox.KeepDeletedItemsDays;

                txtAccountName.Text = mailbox.Domain + "\\" + mailbox.AccountName;

                // get account meta
                ExchangeAccount account = ES.Services.ExchangeServer.GetAccount(PanelRequest.ItemID, PanelRequest.AccountID);
                chkPmmAllowed.Checked = (account.MailboxManagerActions & MailboxManagerActions.AdvancedSettings) > 0;
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_GET_MAILBOX_ADVANCED", ex);
            }
        }

        private void SaveSettings()
        {
            if (!Page.IsValid)
                return;

            try
            {
                if (((sizeIssueWarning.ValueKB <= sizeProhibitSend.ValueKB && sizeIssueWarning.ValueKB != -1) || sizeProhibitSend.ValueKB == -1) 
                    && ((sizeProhibitSend.ValueKB <= sizeProhibitSendReceive.ValueKB && sizeProhibitSend.ValueKB != -1) || sizeProhibitSendReceive.ValueKB == -1))
                {
                    int result = ES.Services.ExchangeServer.SetMailboxAdvancedSettings(
                        PanelRequest.ItemID, PanelRequest.AccountID,
                        chkPOP3.Checked,
                        chkIMAP.Checked,
                        chkOWA.Checked,
                        chkMAPI.Checked,
                        chkActiveSync.Checked,

                        sizeIssueWarning.ValueKB,
                        sizeProhibitSend.ValueKB,
                        sizeProhibitSendReceive.ValueKB,

                        daysKeepDeletedItems.ValueDays);

                    if (result < 0)
                    {
                        messageBox.ShowResultMessage(result);
                        return;
                    }

                    messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILBOX_ADVANCED");
                }
                else
                {
                    messageBox.ShowErrorMessage("EXCHANGE_SET_ORG_LIMITS_VALIDATION");
                }
            }
            catch (Exception ex)
            {
				messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILBOX_ADVANCED", ex);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        protected void chkPmmAllowed_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int result = ES.Services.ExchangeServer.SetMailboxManagerSettings(PanelRequest.ItemID, PanelRequest.AccountID,
                chkPmmAllowed.Checked, MailboxManagerActions.AdvancedSettings);

                if (result < 0)
                {
                    messageBox.ShowResultMessage(result);
                    return;
                }

                messageBox.ShowSuccessMessage("EXCHANGE_UPDATE_MAILMANAGER");
            }
            catch (Exception ex)
            {
                messageBox.ShowErrorMessage("EXCHANGE_UPDATE_MAILMANAGER", ex);
            }
        }
	}
}