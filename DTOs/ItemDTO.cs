namespace POS_API.DTOs
{
    public class ItemCreateDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
        public IFormFile Image { get; set; }
    }

}
