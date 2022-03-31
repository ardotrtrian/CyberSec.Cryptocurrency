namespace CyberSec.Cryptocurrency.Service.Models;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);