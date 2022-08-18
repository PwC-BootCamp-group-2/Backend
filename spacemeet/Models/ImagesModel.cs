using System.ComponentModel.DataAnnotations;

namespace spacemeet.Models
{
    public class ImagesModel
    {
        public string?Name { get; set; }
        [Key]
        public string?ImgUrl { get; set; }
    }
}
