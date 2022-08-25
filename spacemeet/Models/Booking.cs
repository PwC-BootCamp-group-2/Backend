
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace spacemeet.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public int SpaceId { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int ResourceId { get; set; }
        public int NoR { get; set; }
        public List<BookedDateModel>? BookedDates { get; set; }
        public int Amount { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Used { get; set; }

    //To Enable Cancellation Of Booked Space by both Merchant and Individuals
    public void CancelBooking() { }

       /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookedDateModel>()
                .HasNoKey();
        }*/
    }
}
