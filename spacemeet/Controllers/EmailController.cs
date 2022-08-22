using System.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using QRCoder;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Windows;
using System.Drawing.Imaging;

namespace spacemeet.Controllers
{
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
        [HttpPost]
        public IActionResult SendEmail(string body)
        {
      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse("ayoolaanibabs0@gmail.com"));
      email.To.Add(MailboxAddress.Parse("hakeemanibaba@yahoo.com"));
      email.Subject = "Test email Subject";
      StringBuilder htmlBody = new StringBuilder();
            string qrText = "hi";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] byteImage = qrCodeImage.BitmapToByteArray();
            string imageData = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            //string imageData = "ay";
      htmlBody.Append("<html><body>");
        htmlBody.Append("<p>Dear Cusomer,</p>");
        htmlBody.Append("<p>Please refer below QR Code:</p>");
        htmlBody.Append("<p><img src='" + imageData + "'</p>");
        htmlBody.Append("</body></html>");
      email.Body = new TextPart(TextFormat.Html);

      // {
      //   Text = body
      // };

      using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 465, true);
        smtp.Authenticate("ayoolaanibabs0@gmail.com", "jxwtarvkivovjomz");
        smtp.Send(email);
        smtp.Disconnect(true);
      return Ok("imat");
    }
        
    }
    public static class BitmapExtension
    {
        public static byte[] BitmapToByteArray(this Bitmap bitmap)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}