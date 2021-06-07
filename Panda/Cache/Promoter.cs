using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Cache
{
    public class Promoter
    {
        public static int MainPromoterId = 0;
        private static bool UploadLocker = false;
        private static DateTime UploadLockerTime = new DateTime();

        private static object Locker = new object();
        private static Dictionary<string, Core.Promoter> DictPromoter = new Dictionary<string, Core.Promoter>();

        public static void CheckDb()
        {
            CheckExist();
        }
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
        public static void BuilkUpload(List<string>namelist)
        {
            List<Core.Promoter> addlist = new List<Core.Promoter>();
            bool ok = false;
            if (namelist.Count > 0)
            {
                lock (Locker)
                {
                    try
                    {
                        using (var db = new Core.Schema())
                        {
                            foreach (var name in namelist)
                            {
                                var item = new Core.Promoter() { Username = name };
                                addlist.Add(item);
                                db.promoters.Add(item);
                            }
                            db.SaveChanges();
                            ok = true;
                        }
                    }
                    catch (Exception de)
                    {

                    }

                    if (ok)
                        foreach (var p in addlist)
                            DictPromoter[p.Username] = p;
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
        public static bool CheckExsit(string username)
        {
            CheckExist();
            bool result = false;
            lock (Locker)
            {
                result = DictPromoter.ContainsKey(username);
            }
            return result;
        }
        public static Core.Promoter RegisterGetPromoter(string username)
        {
            username = username.ToLower().Trim();

            CheckExist();
            Core.Promoter promoter = null;
            lock (Locker)
            {
                if (DictPromoter.ContainsKey(username))
                {
                    promoter = DictPromoter[username];
                    promoter.NumRegistered++;
                    promoter.TimeUpdate = DateTime.Now;

                    using (var db = new Core.Schema())
                    {
                        db.promoters.Update(promoter);
                        db.SaveChanges();
                    }
                }
            }
            return promoter;
        }
        public static void VerifyGetPromoter(int PromoterId)
        {
            CheckExist();
            lock (Locker)
            {
                var promoter = (from c in DictPromoter where c.Value.Id == PromoterId select c.Value).FirstOrDefault();
                if (promoter!=null)
                {
                    promoter.NumVerified++;
                    promoter.TimeUpdate = DateTime.Now;

                    using (var db = new Core.Schema())
                    {
                        db.promoters.Update(promoter);
                        db.SaveChanges();
                    }
                }
            }
        }
        public static void Add(Core.Promoter promoter)
        {
            CheckExist();
            lock (Locker)
            {
                using (var db = new Core.Schema())
                {
                    db.promoters.Add(promoter);
                    db.SaveChanges();
                }

                DictPromoter[promoter.Username] = promoter;                
            }
        }
        private static void CheckExist()
        {
            if (DictPromoter.Count > 0) return;

            lock (Locker)
            {
                using (var db = new Core.Schema())
                {
                    Core.Promoter MainPromoter = db.promoters.Where(c => c.Username == "panda").FirstOrDefault();
                    List<string> used = new List<string>();
                    if (MainPromoter == null)
                    {
                        MainPromoter = new Core.Promoter() { Username = "panda" };
                        db.promoters.Add(MainPromoter);

                        for (int i = 0; i < 100; i++)
                        {
                            var username = Guid.NewGuid().ToString("n").ToLower().Substring(13, 6);
                            while (used.Contains(username))
                                username = Guid.NewGuid().ToString("n").ToLower().Substring(13, 6);
                            used.Add(username);


                            db.promoters.Add(new Core.Promoter() { Username = username });
                        }
                        db.SaveChanges();
                    }
                    DictPromoter = db.promoters.ToDictionary(c => c.Username, c => c);

                    MainPromoterId = MainPromoter.Id;
                }
            }
        }
    }
}
