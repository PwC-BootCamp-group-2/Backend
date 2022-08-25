using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Dtos.Booking
{
    public class BookingEmailDto
    {
        public string email { get; set; } = string.Empty;
        public int BookingId { get; set; }
    }
}