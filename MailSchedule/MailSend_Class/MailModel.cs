using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSend_Class
{
    public class MailModel
    {
        public List<string> Attachments { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        public MailModel()
        {
            Attachments = new List<string>();
            IsBodyHtml = true;
        }
    }
}
