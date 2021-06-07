using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Core
{
    public class Promoter
    {
        public int Id { get; set; }
        public DateTime TimeCreate { get; set; } = DateTime.Now;
        public DateTime TimeUpdate { get; set; } = DateTime.Now;
        public string Username { get; set; } = "";
        public int NumRegistered { get; set; } = 0;
        public int NumVerified { get; set; } = 0;
    }
}
