using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Dtos.Booking
{
    public class BookingEmailDto
    {
        public string email { get; set; } = string.Empty;
        public string BookingId { get; set; } = string.Empty;
    }
}