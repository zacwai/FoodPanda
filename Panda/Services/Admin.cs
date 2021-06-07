using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Services
{
    public class Admin
    {
        private string Domain = ZC.Cfg.GetCfg("Domain");
        public XLWorkbook DownloadMember(DateTime timeStart)
        {
            var workbook = new XLWorkbook();

            List<Core.Member> members = new List<Core.Member>();
            Dictionary<int,string> codes = new Dictionary<int, string>();

            using (var db = new Core.Schema())
            {
                members = db.members.Where(c => c.TimeUpdate >= timeStart && c.TimeUpdate < timeStart.AddMonths(1)).Include(c => c.Promoter).ToList();
                codes = db.promocodes.Where(c => c.TimeRedeem >= timeStart && c.TimeRedeem < timeStart.AddMonths(1)).ToDictionary(c=>c.Id, c=>c.Code);
            }
            members = members.OrderBy(c => c.TimeUpdate).ToList();


            #region worksheet
            var worksheet = workbook.Worksheets.Add(timeStart.ToString("MMM yy"));
            var currentRow = 1;

            string _column = "No,Time Create,Time Update,Promoter,Full Name,Contact,Email,State,Promo Code Link,Promo Code";
            var column = _column.Split(',').ToList();

            for (int i = 0; i < column.Count; i++)
                worksheet.Cell(currentRow, (i + 1)).Style.NumberFormat.Format = "@";

            for (int i = 0; i < column.Count; i++)
                worksheet.Cell(currentRow, (i + 1)).Value = column[i];

            worksheet.Range("A1:J1").Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            foreach (var item in members)
            {
                currentRow++;
                for (int i = 0; i < column.Count; i++)
                    worksheet.Cell(currentRow, (i + 1)).Style.NumberFormat.Format = "@";

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = item.TimeCreate;
                worksheet.Cell(currentRow, 3).Value = item.TimeUpdate;
                worksheet.Cell(currentRow, 4).Value = item.Promoter.Username;
                worksheet.Cell(currentRow, 5).Value = item.FullName;
                worksheet.Cell(currentRow, 6).Value = item.Contact;
                worksheet.Cell(currentRow, 7).Value = item.Email;
                worksheet.Cell(currentRow, 8).Value = item.State;
                worksheet.Cell(currentRow, 9).Value = Domain+"/promo/"+item.VerifySecret;
                worksheet.Cell(currentRow, 10).Value = item.PromoCodeId != 0 ? codes[item.PromoCodeId] : "";
            }

            worksheet.Columns().AdjustToContents();
            #endregion

            return workbook;
        }
        public XLWorkbook DownloadPromoter()
        {
            var workbook = new XLWorkbook();

            List<Core.Promoter> promoters = new List<Core.Promoter>();
            using (var db = new Core.Schema())
                promoters = db.promoters.ToList();
            promoters = promoters.OrderBy(c => c.TimeCreate).ToList();


            #region worksheet
            var worksheet = workbook.Worksheets.Add("Promoter");
            var currentRow = 1;

            string _column = "No,Time Create,Username,QR Link,Register Link,Registered,Verified";
            var column = _column.Split(',').ToList();

            for (int i = 0; i < column.Count; i++)
                worksheet.Cell(currentRow, (i + 1)).Style.NumberFormat.Format = "@";

            for (int i = 0; i < column.Count; i++)
                worksheet.Cell(currentRow, (i + 1)).Value = column[i];

            worksheet.Range("A1:G1").Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            foreach (var item in promoters)
            {
                currentRow++;
                for (int i = 0; i < column.Count; i++)
                    worksheet.Cell(currentRow, (i + 1)).Style.NumberFormat.Format = "@";

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = item.TimeCreate;
                worksheet.Cell(currentRow, 3).Value = item.Username;
                worksheet.Cell(currentRow, 4).Value = Domain + "/qr/" + item.Username;
                worksheet.Cell(currentRow, 5).Value = Domain + "/register/" + item.Username;
                worksheet.Cell(currentRow, 6).Value = item.NumRegistered;
                worksheet.Cell(currentRow, 7).Value = item.NumVerified;
            }

            worksheet.Columns().AdjustToContents();
            #endregion

            return workbook;
        }
        public XLWorkbook DownloadPromoCode(DateTime timeStart)
        {
            var workbook = new XLWorkbook();

            List<Core.PromoCode> promoterCodes = new List<Core.PromoCode>();
            using (var db = new Core.Schema())
                promoterCodes = db.promocodes.Where(c=>c.ExpiryDate >= timeStart && c.ExpiryDate < timeStart.AddMonths(1)).ToList();
            promoterCodes = promoterCodes.OrderBy(c => c.Id).OrderBy(c => c.ExpiryDate).ToList();


            #region worksheet
            var worksheet = workbook.Worksheets.Add(timeStart.ToString("MMM yy"));
            var currentRow = 1;

            string _column = "No,Time Create,Expiry Date,Code,Used";
            var column = _column.Split(',').ToList();

            for (int i = 0; i < column.Count; i++)
                worksheet.Cell(currentRow, (i + 1)).Style.NumberFormat.Format = "@";

            for (int i = 0; i < column.Count; i++)
                worksheet.Cell(currentRow, (i + 1)).Value = column[i];

            worksheet.Range("A1:E1").Style.Border.BottomBorder = XLBorderStyleValues.Thick;

            foreach (var item in promoterCodes)
            {
                currentRow++;
                for (int i = 0; i < column.Count; i++)
                    worksheet.Cell(currentRow, (i + 1)).Style.NumberFormat.Format = "@";

                worksheet.Cell(currentRow, 1).Value = currentRow - 1;
                worksheet.Cell(currentRow, 2).Value = item.TimeCreate;
                worksheet.Cell(currentRow, 3).Value = item.ExpiryDate;
                worksheet.Cell(currentRow, 4).Value = item.Code;
                worksheet.Cell(currentRow, 5).Value = item.IsUsed;
            }

            worksheet.Columns().AdjustToContents();
            #endregion

            return workbook;
        }
    }
}
