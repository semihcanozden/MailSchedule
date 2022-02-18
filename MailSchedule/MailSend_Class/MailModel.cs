using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSend_Class
{
  public class MailModel
  {
    public List<string> To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public List<string> Attachments { get; set; }
    public List<string> Cc { get; set; }
    public List<string> Bcc { get; set; }
    public bool IsBodyHtml { get; set; }
    public MailModel()
    {
      To = new List<string>();
      Attachments = new List<string>();
      Cc = new List<string>();
      Bcc = new List<string>();
      IsBodyHtml = true;
    }
  }
}
