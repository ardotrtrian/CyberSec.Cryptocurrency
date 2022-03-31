namespace CyberSec.Cryptocurrency.Service.Models;

public record AuthenticateRequest(
    string Email, 
    string Password);