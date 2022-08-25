using System.ComponentModel.DataAnnotations.Schema;

namespace spacemeet.Models
{
    public class Space
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int MerchantId { get; set; }
        public string? Type { get; set; }
        public int Capacity { get; set; }
        public List<AssetsModel>? Assets {get; set;}
        public List<ImagesModel>?Imgs { get; set; }
        public string? Location { get; set; }
        public string? Price { get; set; }
    }
}
