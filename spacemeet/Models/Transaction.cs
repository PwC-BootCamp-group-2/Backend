using System.ComponentModel.DataAnnotations;

namespace spacemeet.Models
{
    public class Transaction
    {
        [Key]
        public string? Id { get; set; }
        public int UserId { get; set; }
        public string? Type { get; set; }
        public string? Purpose { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
