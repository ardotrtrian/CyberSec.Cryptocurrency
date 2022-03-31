using CyberSec.Cryptocurrency.Service.Models;
using CyberSec.Cryptocurrency.Shared.DTOs;

namespace CyberSec.Cryptocurrency.Service.Interfaces;

public interface IUserService
{
    Task<AuthenticateDto> AuthenticateAsync(AuthenticateRequest request);
    Task RegisterAsync(RegisterRequest request, string orgin);
    Task VerifyEmailAsync(string token);
    Task<UserDto> GetAsync(string Id);
}