using CyberSec.Cryptocurrency.Service.Entities;

namespace CyberSec.Cryptocurrency.Service.Interfaces;

public interface IJwtUtils
{
    string GenerateJwtToken(User user);
    string ValidateJwtToken(string token);
    public RefreshToken GenerateRefreshToken(string ipAddress);
}