using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Core
{
    public class PromoCode
    {
        public int Id { get; set; }
        public DateTime TimeCreate { get; set; } = DateTime.Now;
        public DateTime TimeRedeem { get; set; } = new DateTime();
        public string Code { get; set; } = "c" + DateTime.Now.ToString("mm") + Guid.NewGuid().ToString("n").ToLower().Substring(6, 11) + DateTime.Now.ToString("dd");
        public DateTime ExpiryDate { get; set; } = new DateTime(2021, 12, 31);
        public bool IsUsed { get; set; } = false;
    }
}
