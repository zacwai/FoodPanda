using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Core
{
    public class Member
    {
        public int Id { get; set; }
        public int PromoterId { get; set; }
        public int PromoCodeId { get; set; } = 0;
        public DateTime TimeCreate { get; set; } = DateTime.Now;
        public DateTime TimeUpdate { get; set; } = DateTime.Now;
        public string Contact { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string State { get; set; } = "";
        public string VerifySecret { get; set; } = Guid.NewGuid().ToString("n").ToLower().Substring(0, 8);



        [ForeignKey("PromoterId")]
        public Promoter Promoter { get; set; }
    }
}
