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
using spacemeet.Dtos.Booking;

namespace spacemeet.Controllers
{
    [Route("[controller]")]
    public class EmailController : ControllerBase
    {
      [HttpPost]
        public IActionResult SendEmail(BookingEmailDto request)
        {
      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse("ayoolaanibabs0@gmail.com"));
      email.To.Add(MailboxAddress.Parse(request.email));
      email.Subject = "Test email Subject";
            string qrText = request.BookingId;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            byte[] byteImage = qrCodeImage.BitmapToByteArray();
            string imageData = "data:image/png;base64," + Convert.ToBase64String(byteImage);
            string image = "https://blog.hubspot.com/hubfs/image8-2.jpg";
            email.Body = new TextPart(TextFormat.Html)
      {
        Text = string.Format("<html><body> <p>Dear Cusomer,</p> <p>Please refer below QR Code:</p> <p><img src='" + image + "'</p> </body></html>")
      };

      using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 465, true);
        smtp.Authenticate("ayoolaanibabs0@gmail.com", "jxwtarvkivovjomz");
        smtp.Send(email);
        smtp.Disconnect(true);
      return Ok();
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