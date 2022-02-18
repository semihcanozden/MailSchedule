using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_Class
{
  public class LogQueueModel
  {
    public string Log { get; set; }
    public DateTime Date { get; set; }
    public LogQueueModel()
    { }
    public LogQueueModel(string log, DateTime date)
    {
      Log = log;
      Date = date;
    }
  }
}
