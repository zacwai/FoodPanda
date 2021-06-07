using Panda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class Member
    {
        private ZC.Isentric Isentric = new ZC.Isentric();
        public string Register(RegistrationInput input)
        {
            var errMsg = "";
            Core.Member member = new Core.Member()
            {
                Contact = input.Contact,
                Email = input.Email.ToLower().Trim(),
                FullName = input.FullName.ToUpper().Trim(),
                State = input.State
            };

            var promoter = Cache.Promoter.RegisterGetPromoter(input.Id);
            member.PromoterId = promoter == null ? Cache.Promoter.MainPromoterId : promoter.Id;

            if (string.IsNullOrEmpty(errMsg))
            {
                Cache.Member.AddMember(member);

                new Thread(() =>
                {
                    Isentric.Send("6" + input.Contact, "KLICKER: Here is your FoodPanda promo code. " + ZC.Cfg.GetCfg("Domain") + "/voucher/" + member.VerifySecret);
                }).Start();
            }

            return errMsg;
        }
    }
}
