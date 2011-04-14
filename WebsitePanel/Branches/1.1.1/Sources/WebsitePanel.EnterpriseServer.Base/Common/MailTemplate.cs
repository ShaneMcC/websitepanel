using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;

namespace WebsitePanel.EnterpriseServer
{
    public class MailTemplate
    {
        public string From { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsHtml { get; set; }
        public MailPriority Priority { get; set; }
    }
}
