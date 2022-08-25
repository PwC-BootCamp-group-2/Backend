using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MimeKit.Text;
using QRCoder;
using spacemeet.Data;
using spacemeet.Dtos.Booking;
using spacemeet.Models;

namespace spacemeet.Controllers
{
  [Route("api/[controller]/[action]")]
  [ApiController]
  public class BookingsController : ControllerBase
  {
    private readonly spacemeetContext _context;

    public BookingsController(spacemeetContext context)
    {
      _context = context;
    }

    // GET: api/Bookings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBooking()
    {
      if (_context.Booking == null)
      {
        return NotFound();
      }
      return await _context.Booking.ToListAsync();
    }

    // GET: api/Bookings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
      if (_context.Booking == null)
      {
        return NotFound();
      }
      var booking = await _context.Booking.FindAsync(id);

      if (booking == null)
      {
        return NotFound();
      }

      return booking;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetUserBookings(int id)
    {
      if (_context.Booking == null)
      {
        return NotFound();
      }
      var booking = await _context.Booking.Where(e => e.UserId == id).ToListAsync();

      if (booking == null)
      {
        return NotFound();
      }

      return booking;
    }

    // Patch: api/Bookings/AcceptBooking/4
    // Change Status to accepted


    [HttpPatch("{id}")]
    public async Task<IActionResult> AcceptBooking(int id)
    {


      try
      {
        //Accepting the booking
        Booking? booking = await _context.Booking.FindAsync(id);

        int MerchId = booking.MerchantId;
        Wallet? MerchWallet = await _context.Wallets.FirstAsync(e => e.UserId == MerchId);
        if (MerchWallet != null)
        {
          double commission = booking.Amount - (booking.Amount * 20 / 100);
          MerchWallet.FulfilBalance(Convert.ToInt32(commission));
        }
        booking.Status = "Active";
        booking.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!BookingExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    //Reject Bookings
    [HttpPatch("{id}")]
    public async Task<IActionResult> RejectBooking(int id)
    {

      try
      {
        //Rejecting the booking
        Booking? booking = await _context.Booking.FindAsync(id);
        int UserId = booking.UserId;
        Wallet? userWallet = await _context.Wallets.FirstAsync(e => e.UserId == UserId);
        booking.Status = "Rejected";
        booking.UpdatedAt = DateTime.Now;
        userWallet.UpdatedAt = DateTime.Now;
        userWallet.FundWallet(booking.Amount);
        await _context.SaveChangesAsync();
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!BookingExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }
    [HttpPatch]
    public async Task<ServiceResponse<string>> VerifyBooking(int token)
    {
      var booking = await _context.Booking.FirstOrDefaultAsync(u => u.Id == token);
      ServiceResponse<string> response = new ServiceResponse<string>();
      if (booking == null || booking.Used == true)
      {
        response.Success = false;
        response.Message = "User has Checked in";
        return response;
      }
      booking.Used = true;
      await _context.SaveChangesAsync();

      response.Message = "Booking Verifed";
      return response;
    }

    //Cancel Booking
    [HttpPatch("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {




      try
      {
        //cancelling the booking
        Booking? booking = await _context.Booking.FindAsync(id);
        int UserId = booking.UserId;
        int MerchId = booking.MerchantId;
        Wallet? userWallet = await _context.Wallets.FirstAsync(e => e.UserId == UserId);
        Wallet? MerchWallet = await _context.Wallets.FirstAsync(e => e.UserId == MerchId);
        if (MerchWallet != null && booking.Status == "Active")
        {
          double commission = booking.Amount - (booking.Amount * 20 / 100);
          MerchWallet.PendingBalance -= commission;
          MerchWallet.UpdatedAt = DateTime.Now;
        }
        booking.Status = "Cancelled";
        booking.UpdatedAt = DateTime.Now;
        userWallet.UpdatedAt = DateTime.Now;
        userWallet.FundWallet(booking.Amount);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!BookingExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Bookings
    // To protect from overposting attacks
    [HttpPost]
    public async Task<ActionResult<Booking>> MakeBooking(Booking booking)
    {
      if (_context.Booking == null)
      {
        return Problem("Entity set 'spacemeetContext.Booking'  is null.");
      }
      int UserId = booking.UserId;
      User? user = await _context.Users.FindAsync(UserId);

      Wallet? userWallet = await _context.Wallets.FindAsync(UserId);
      if (userWallet?.Balance >= booking.Amount)
      {
        //Space_Hubs comission
        double commission = booking.Amount * 20 / 100;
        //Adding booking
        booking.Used = false;
        _context.Booking.Add(booking);

        userWallet.WithDrawFunds(booking.Amount);
        BookingEmailDto emailRequest = new BookingEmailDto();
        emailRequest.BookingId = booking.Id;
        emailRequest.email = user.email;
        SendBookingEmail(emailRequest);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
      }


      return BadRequest("Bad Request");


    }
    private bool BookingExists(int id)
    {
      return (_context.Booking?.Any(e => e.Id == id)).GetValueOrDefault();
    }
    private void SendBookingEmail(BookingEmailDto request)
    {
      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse("ayoolaanibabs0@gmail.com"));
      email.To.Add(MailboxAddress.Parse(request.email));
      email.Subject = "Space Meet - Your Space Booking";
      string qrText = $"http://localhost:3000/checkinsuccessful/{request.BookingId}";
      QRCodeGenerator qrGenerator = new QRCodeGenerator();
      QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
      QRCodeGenerator.ECCLevel.Q);
      QRCode qrCode = new QRCode(qrCodeData);
      Bitmap qrCodeImage = qrCode.GetGraphic(20);
      byte[] byteImage = qrCodeImage.BitmapToByteArray();
      string imageData = "data:image/png;base64," + Convert.ToBase64String(byteImage);
      email.Body = new TextPart(TextFormat.Html)
      {
        Text = string.Format("<html><body> <p>Dear Cusomer,</p> <p>Please refer below QR Code:</p> <p><img src='" + imageData + "'</p> </body></html>")
      };

      using var smtp = new SmtpClient();
      smtp.Connect("smtp.gmail.com", 465, true);
      smtp.Authenticate("ayoolaanibabs0@gmail.com", "jxwtarvkivovjomz");
      smtp.Send(email);
      smtp.Disconnect(true);
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
