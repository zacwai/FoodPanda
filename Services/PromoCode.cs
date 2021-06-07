using Panda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class PromoCode
    {
        public PublicViewOutput Verify(string id)
        {
            PublicViewOutput result = new PublicViewOutput(id);
            var member = Cache.Member.GetMemberByVericationCode(id);

            if (member == null)
                result.ErrMsg = "This link is no longer valid";

            if (string.IsNullOrEmpty(result.ErrMsg))
            {
                Core.PromoCode promoCode = null;
                if (member.PromoCodeId == 0)
                {
                    promoCode = Cache.PromoCode.AssignPromoCode();
                    if (promoCode != null)
                    {
                        Cache.Member.UsePromoCode(id, promoCode.Id);
                        Cache.Promoter.VerifyGetPromoter(member.PromoterId);
                    }
                }
                else
                    promoCode = Cache.PromoCode.GetPromoCode(member.PromoCodeId);

                if (promoCode == null)
                    result.ErrMsg = "Promo Code is temporarily unavailable";
                if (string.IsNullOrEmpty(result.ErrMsg))
                {
                    VerificationOutput mainResult = new VerificationOutput()
                    {
                        ExpireDate = promoCode.ExpiryDate.Day + ZC.Util.DateTimePlaceFormat(promoCode.ExpiryDate) + " " + promoCode.ExpiryDate.ToString("MMMMM yyyy"),
                        PromoCode = promoCode.Code
                    };
                    result.PublicObject = mainResult;
                }
            }

            return result;
        }
    }
}
