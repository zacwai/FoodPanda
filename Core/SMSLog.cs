using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Core
{
    public class SMSLog
    {
        public int Id { get; set; }
        public bool OK { get; set; } = false;
        public string To { get; set; } = "";
        public string Text { get; set; } = "";
        public string MsgId { get; set; } = "";
        public string Msg { get; set; } = "";
        public string ErrMsg { get; set; } = "";
        public string RawResp { get; set; } = "";
        public double Balance { get; set; } = 0;
        public DateTime TimeCreate { get; set; } = DateTime.Now;
    }
}
