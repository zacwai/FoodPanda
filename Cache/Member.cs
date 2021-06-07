using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Cache
{
    public class Member
    {
        private static object Locker = new object();
        private static Dictionary<string, Core.Member> MemberList = new Dictionary<string, Core.Member>();
        private static Dictionary<string, string> VerifcationCodeList = new Dictionary<string, string>();

        public static bool ContactExsist(string Contact)
        {
            CheckExist();
            bool IsExsist = false;
            lock (Locker)
            {
                IsExsist = MemberList.ContainsKey(Contact);
            }
            return IsExsist;
        }
        public static Core.Member GetMemberByVericationCode(string Code)
        {
            CheckExist();
            Core.Member result = null;
            lock (Locker)
            {
                if (VerifcationCodeList.ContainsKey(Code))
                    result = MemberList[VerifcationCodeList[Code]];
            }
            return result;
        }
        public static Core.Member UsePromoCode(string Code, int PromoCodeId)
        {
            CheckExist();
            Core.Member result = null;
            lock (Locker)
            {
                if (VerifcationCodeList.ContainsKey(Code))
                {
                    var contact = VerifcationCodeList[Code];
                    result = MemberList[contact];

                    result.TimeUpdate = DateTime.Now;
                    result.PromoCodeId = PromoCodeId;
                    using (var db = new Core.Schema())
                    {
                        db.members.Update(result);
                        db.SaveChanges();
                    }
                }
            }
            return result;
        }
        public static void AddMember(Core.Member member)
        {
            CheckExist();
            lock (Locker)
            {
                if (!MemberList.ContainsKey(member.Contact))
                {
                    while (VerifcationCodeList.ContainsKey(member.VerifySecret))
                        member.VerifySecret = Guid.NewGuid().ToString("n").ToLower().Substring(0, 8);
                    using (var db = new Core.Schema())
                    {
                        db.members.Add(member);
                        db.SaveChanges();
                    }
                    VerifcationCodeList[member.VerifySecret] = member.Contact;
                    MemberList[member.Contact] = member;
                }
            }
        }
        private static void CheckExist()
        {
            if (MemberList.Count > 0) return;

            lock (Locker)
            {
                MemberList = new Dictionary<string, Core.Member>();
                using (var db = new Core.Schema())
                {
                    var members = db.members.ToList();
                    MemberList = members.ToDictionary(c => c.Contact, c => c);

                    VerifcationCodeList = (from c in members select c).ToDictionary(c => c.VerifySecret, c => c.Contact);
                }
            }
        }

    }
}
