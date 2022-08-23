using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spacemeet.Models
{
    public class Resource 
    {
        [Key]
        public int Id { get; }
        [ForeignKey("Space")]
        public string? SpaceId {get; set;}
        public string? Name { get; set;}
        public int Amount { get; set;}
        public string? Description { get; set;}
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
