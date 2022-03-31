using CyberSec.Cryptocurrency.Service.Enums;

namespace CyberSec.Cryptocurrency.Service.Entities;

public class User
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public Role Role { get; set; }
    public string VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public bool IsVerified => VerifiedAt.HasValue;
}
