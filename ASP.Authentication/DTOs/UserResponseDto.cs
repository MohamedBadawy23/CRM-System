using System.Text.Json.Serialization;

namespace ASP.Authentication.DTOs;

public class UserResponseDto
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? Role { get; set; }
} 