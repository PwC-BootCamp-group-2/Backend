using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spacemeet.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public int SpaceId { get; set;}
        public string? Description { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
