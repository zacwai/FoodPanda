using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Cache
{
    public class PromoCode
    {
        private static bool UploadLocker = false;
        private static DateTime UploadLockerTime = new DateTime();
        private static object Locker = new object();
        private static Dictionary<int, Core.PromoCode> UnusedPromoCode = new Dictionary<int, Core.PromoCode>();
        private static Dictionary<int, Core.PromoCode> UsedPromoCode = new Dictionary<int, Core.PromoCode>();
        private static List<string> PromoCodes = new List<string>();

        public static bool CanUpload(out DateTime LockTime)
        {
            bool result = !UploadLocker;
            LockTime = UploadLockerTime;
            lock (Locker)
            {
                if (!UploadLocker)
                {
                    UploadLocker = true;
                    UploadLockerTime = DateTime.Now;
                }
            }
            return result;
        }
        public static void BuilkUpload(Dictionary<string, DateTime> namelist)
        {
            List<Core.PromoCode> addlist = new List<Core.PromoCode>();
            if (namelist.Count > 0)
            {
                bool ok = false;
                lock (Locker)
                {
                    try
                    {
                        using (var db = new Core.Schema())
                        {
                            foreach (var name in namelist)
                            {
                                var item = new Core.PromoCode() { Code = name.Key, ExpiryDate = name.Value };
                                addlist.Add(item);
                                db.promocodes.Add(item);
                            }
                            db.SaveChanges();
                            ok = true;
                        }
                    }
                    catch (Exception de)
                    {

                    }

                    if (ok)
                    {
                        foreach (var p in addlist)
                        {
                            UnusedPromoCode[p.Id] = p;
                            PromoCodes.Add(p.Code);
                        }
                    }
                }
            }
        }
        public static void ReleaseUpload()
        {
            lock (Locker)
            {
                    UploadLocker = false;
            }
        }
        public static bool CheckExsit(string code)
        {
            CheckExist();
            bool result = false;
            lock (Locker)
            {
                result = PromoCodes.IndexOf(code) >= 0;
            }
            return result;
        }
        public static Core.PromoCode GetPromoCode(int PromoId)
        {
            CheckExist();
            Core.PromoCode result = null;

            lock (Locker)
            {
                if (UsedPromoCode.ContainsKey(PromoId))
                {
                    result = UsedPromoCode[PromoId];
                }
            }
            return result;
        }
        public static Core.PromoCode AssignPromoCode()
        {
            CheckExist();
            Core.PromoCode result = null;

            lock (Locker)
            {
                if (UnusedPromoCode.Count > 0)
                {
                    //result = UnusedPromoCode.First().Value;
                    result = (from c in UnusedPromoCode.Values where c.ExpiryDate > DateTime.Now select c).FirstOrDefault();

                    UnusedPromoCode.Remove(result.Id);
                    UsedPromoCode[result.Id] = result;

                    using (var db = new Core.Schema())
                    {
                        var pc = db.promocodes.Where(c => c.Id == result.Id).FirstOrDefault();
                        if (pc != null)
                        {
                            pc.IsUsed = true;
                            pc.TimeRedeem = DateTime.Now;
                            db.promocodes.Update(pc);
                            db.SaveChanges();
                        }
                    }
                }
            }
            return result;
        }

        private static void CheckExist()
        {
            if (UnusedPromoCode.Count > 0) return;

            lock (Locker)
            {
                using (var db = new Core.Schema())
                {
                    var codes = db.promocodes.ToList();
                    UnusedPromoCode = (from c in codes where !c.IsUsed && c.ExpiryDate > DateTime.Now select c).OrderBy(c => c.ExpiryDate).ToDictionary(c => c.Id, c => c);
                    UsedPromoCode = (from c in codes where c.IsUsed && c.ExpiryDate <= DateTime.Now select c).ToDictionary(c => c.Id, c => c);
                    PromoCodes = (from c in codes select c.Code).ToList();
                }
            }
        }
    }
}
