using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace spacemeet.Models
{
    public class Resource : Space
    {
        [Key]
        public new int Id { get; set; }
        [ForeignKey("Space")]
        public string? SpaceId {get; set;}
    }
}
