using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsitePanel.Providers.Mail.SM7.Extensions
{
	public static class MailAccountExtensions
	{
		/// <summary>
		/// Prepares all the necessary parameters to call SetUserSettings web method.
		/// </summary>
		/// <param name="mailbox"></param>
		/// <returns></returns>
		public static string[] PrepareSetRequestedUserSettingsWebMethodParams(this MailAccount mailbox)
		{
			return new string[] {
                        "isenabled=" + mailbox.Enabled.ToString(),
						// Fix for incorrect mailbox size
                        "maxsize=" + (mailbox.UnlimitedSize ? "0" : mailbox.MaxMailboxSize.ToString()),
                        "lockpassword=" + mailbox.PasswordLocked.ToString(),
						"passwordlocked" + mailbox.PasswordLocked.ToString(),
                        "replytoaddress=" + (mailbox.ReplyTo != null ? mailbox.ReplyTo : ""),
                        "signature=" + (mailbox.Signature != null ? mailbox.Signature : ""),
						"spamforwardoption=none"
			};
		}
	}
}
