namespace CyberSec.Cryptocurrency.Shared.DTOs;

public record AuthenticateDto(
    UserDto User,
    string JwtToken);