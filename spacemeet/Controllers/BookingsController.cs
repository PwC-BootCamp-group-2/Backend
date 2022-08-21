using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using spacemeet.Data;
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
                if(MerchWallet != null && booking.Status == "Active")
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
    
            Wallet? userWallet = await _context.Wallets.FindAsync(UserId);
            if(userWallet?.Balance >= booking.Amount)
            {
                //Space_Hubs comission
                double commission = booking.Amount * 20 / 100;
                //Adding booking
                _context.Booking.Add(booking);
                
                userWallet.WithDrawFunds(booking.Amount);
                
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetBooking", new { id = booking.Id }, booking);
            }
            
            
            return BadRequest("Bad Request");

            
        }

        

        private bool BookingExists(int id)
        {
            return (_context.Booking?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
