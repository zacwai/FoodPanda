using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Models
{
    public class PublicViewOutput
    {
        public string ErrMsg { get; set; } = "";
        public string Id { get; set; }
        public object PublicObject { get; set; }
        public string URL { get; set; } = ZC.Cfg.GetCfg("Domain");
        public PublicViewOutput(string id)
        {
            Id = id;
        }
    }

    public class VerificationOutput
    {
        public string PromoCode { get; set; } = "";
        public string ExpireDate { get; set; } = "";
    }
}
