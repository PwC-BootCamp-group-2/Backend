namespace spacemeet.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int SpaceId { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int ResourceId { get; set; }
        public int NoR { get; set; }
        public DateTime[]? BookedDates { get; set; }
        public string? Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //To Enable Cancellation Of Booked Space by both Merchant and Individuals
        public void CancelBooking() { }
    }
}
