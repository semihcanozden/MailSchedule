using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSend_Class
{
    public interface IMailSend
    {
        void Send(MailModel mailModel);
    }
}
