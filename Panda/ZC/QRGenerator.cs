using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Panda.ZC
{
    public class QRGenerator
    {

        public static string GenerateBase64Image(string qrcode)
        {
            string result = "";
            if (!string.IsNullOrEmpty(qrcode))
            {
                try
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData _qrCodeData = qrGenerator.CreateQrCode(qrcode, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(_qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        qrCodeImage.Save(ms, ImageFormat.Png);
                        result = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                    }
                }
                catch (Exception de)
                {

                }
            }
            return result;
        }
    }
}
