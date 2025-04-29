namespace ASP.Authentication.DTOs
{
    public class Client
    {

        public int Id { get; set; }
        public string? Name { get; set; } 
        public int? Phone { get; set; } 
        public string? Email { get; set; } 
        public string?Address { get; set; } 
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
