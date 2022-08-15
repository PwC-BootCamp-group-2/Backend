namespace spacemeet.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
