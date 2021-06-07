using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.ZC
{
    public class Util
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string DateTimePlaceFormat(DateTime dt)
        {
            var day = dt.Day;
            var mlist = new[] { 11, 12, 13 };
            if (mlist.Contains(dt.Day)) return "th";
            else if (day % 10 == 1) return "st";
            else if (day % 10 == 2) return "nd";
            else if (day % 10 == 3) return "rd";
            else return "th";
        }
    }
}
