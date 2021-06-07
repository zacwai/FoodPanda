using Microsoft.AspNetCore.Mvc;
using Panda.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class AdminController : Controller
    {
        private string secret = "3712240";
        private string password = "stareventer";

        private Services.Admin service = new Services.Admin();
        public IActionResult Index(string id = "", string e = "")
        {
            ViewOutput result = new ViewOutput("admin");
            if (!string.IsNullOrEmpty(e) || (string.IsNullOrEmpty(e) && id != secret))
            {
                result.SetError("Invalid credential");
            }
            return View(result);
        }
        public string _pwd(string i = "")
        {
            if (i == password)
                return "";
            else
                return "Invalid credential";
        }
        public IActionResult Download(DownloadInput input)
        {
            if (input.Password == password)
            {
                var _dt = input.Date.Split(' ');
                var date = new DateTime(int.Parse(_dt[1]), int.Parse(_dt[0]), 1);
                ClosedXML.Excel.XLWorkbook workbook = null;
                if (input.Type == "Member")
                    workbook = service.DownloadMember(date);
                else if (input.Type == "PromoCode")
                    workbook = service.DownloadPromoCode(date);
                else if (input.Type == "Promoter")
                    workbook = service.DownloadPromoter();

                if (workbook != null)
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();

                        return File(
                            content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            input.Type+".xlsx");
                    }
            }

            return RedirectToAction("Index", new { e = "1" });
        }
    }
}
