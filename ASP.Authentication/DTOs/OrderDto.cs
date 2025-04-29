namespace ASP.Authentication.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? OrderNumber { get; set; } 
        public decimal Price { get; set; }
        public string? Product { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; } 
        public int UserId { get; set; }
    }
}
