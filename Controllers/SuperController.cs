using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Panda.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Controllers
{
    public class SuperController : Controller
    {
        private string secret = "klickpanda64";
        private string password = "xoftcoder";
        private Services.Super service = new Services.Super();
        public IActionResult Index(string id = "", string e = "")
        {
            ViewOutput result = new ViewOutput("Super Admin");
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
        public string Upload(FileUpload input)
        {
            string errMsg ="";

            if (input.Id == password)
            {
                List<string> lines = new List<string>();
                using (var reader = new StreamReader(input.File.OpenReadStream()))
                {
                    while (reader.Peek() >= 0)
                        lines.Add(reader.ReadLine());
                }

                if (input.Type == "Promoter")
                    errMsg = service.UploadPromoter(lines);
                else
                    errMsg = service.UploadPromoCode(lines);
            }
            else
                errMsg = "Invalid credential";

            return errMsg;
        }

        //public async Task<Output> _UpdateImage(UploadFile input)
        //{
        //    var auth = new InitModel(User, ModelState);

        //    ZC.DigitalOceanResult uploadResult = null;
        //    if (input.file != null && input.file.Length > 0)
        //    {
        //        uploadResult = await ZC.DigitalOcean.UploadFile(10, input.file);
        //        await service.UploadImage(auth, input.Id, uploadResult.Link);
        //        var src = uploadResult.Link;
        //        auth.SetResult(new { src });
        //    }

        //    return auth.Result;
        //}
        //public IActionResult UploadPromoter(FileUpload input)
        //{
        //    if (input.Id == secret)
        //    {
        //        var _dt = input.Date.Split(' ');
        //        var date = new DateTime(int.Parse(_dt[1]), int.Parse(_dt[0]), 1);
        //        ClosedXML.Excel.XLWorkbook workbook = null;
        //        if (input.Type == "Member")
        //            workbook = service.DownloadMember(date);
        //        else if (input.Type == "PromoCode")
        //            workbook = service.DownloadPromoCode(date);
        //        else if (input.Type == "Promoter")
        //            workbook = service.DownloadPromoter();

        //        if (workbook != null)
        //            using (var stream = new MemoryStream())
        //            {
        //                workbook.SaveAs(stream);
        //                var content = stream.ToArray();

        //                return File(
        //                    content,
        //                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                    input.Type + ".xlsx");
        //            }
        //    }

        //    return RedirectToAction("Index", new { e = "1" });
        //}
    }
}
