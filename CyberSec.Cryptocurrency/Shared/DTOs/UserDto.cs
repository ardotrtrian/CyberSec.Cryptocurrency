namespace CyberSec.Cryptocurrency.Shared.DTOs;

public record UserDto(
    string Id,
    string FirtName,
    string LastName,
    string Email,
    int Role);