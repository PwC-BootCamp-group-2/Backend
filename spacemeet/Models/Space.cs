namespace spacemeet.Models
{
    public class Space
    {
        public int Id { get; set; }
        public int MerchantId { get; set; }
        public string? Type { get; set; }
        public int Capacity { get; set; }
        public object? Assets { get; set; }
        public string[]? Images { get; set; }
        public string? Location { get; set; }
        public string? Price { get; set; }
    }
}
