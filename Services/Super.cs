using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class Super
    {
        public string UploadPromoter(List<string>input)
        {

            List<string> errMsg = new List<string>();
            DateTime dt = new DateTime();
            if (!Cache.Promoter.CanUpload(out dt))
                errMsg.Add("Someone is uploading. please try again later (upload since:" + dt + ")");
            else
            {
                Regex r = new Regex("^[a-zA-Z0-9]*$");
                List<string> passUser = new List<string>();

                for (int i = 0; i < input.Count; i++)
                {
                    var username = input[i].ToLower();
                    if (r.IsMatch(username) && !Cache.Promoter.CheckExsit(username))
                        passUser.Add(username);
                    else
                        errMsg.Add(i + " (" + username + ")");
                }
                Cache.Promoter.BuilkUpload(passUser);
                Cache.Promoter.ReleaseUpload();
            }
            return errMsg.Count>0?" Fail line: "+ string.Join(", ", errMsg):"";
        }
        public string UploadPromoCode(List<string> input)
        {

            List<string> errMsg = new List<string>();
            DateTime dt = new DateTime();
            if (!Cache.PromoCode.CanUpload(out dt))
                errMsg.Add("Someone is uploading. please try again later (upload since:" + dt + ")");
            else
            {
                Regex r = new Regex("^[a-zA-Z0-9]*$");
                Dictionary<string, DateTime> passCase = new Dictionary<string, DateTime>();

                for (int i = 0; i < input.Count; i++)
                {
                    var arr = input[i].Split(',');
                    var code = arr[0];
                    var date = new DateTime();

                    if (r.IsMatch(code) && !Cache.PromoCode.CheckExsit(code) && arr.Length == 2 && _convertDt(arr[1], out date) && date > DateTime.Now)
                        passCase.Add(code, date);
                    else
                        errMsg.Add(i + " (" + input[i] + ")");
                }
                Cache.PromoCode.BuilkUpload(passCase);
                Cache.PromoCode.ReleaseUpload();
            }
            return errMsg.Count > 0 ? " Fail line: " + string.Join(", ", errMsg) : "";
        }

        private bool _convertDt(string input, out DateTime dt)
        {
            var _dt = input.Split('-');
            dt = new DateTime();
            try
            {
                dt = new DateTime(int.Parse(_dt[0]), int.Parse(_dt[1]), int.Parse(_dt[2]));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
