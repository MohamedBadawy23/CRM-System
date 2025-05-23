namespace ASP.Authentication.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
        public string? Category { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int UserId { get; set; }
    }
} 