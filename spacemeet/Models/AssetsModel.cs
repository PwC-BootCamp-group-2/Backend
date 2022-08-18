using System.ComponentModel.DataAnnotations;

namespace spacemeet.Models
{
    public class AssetsModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string?Name { get; set; }
        public string?Description { get; set; }
        public string? ImgUrl { get; set; }
    }
}
