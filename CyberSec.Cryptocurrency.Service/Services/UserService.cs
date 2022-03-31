using CyberSec.Cryptocurrency.Service.Exceptions;
using CyberSec.Cryptocurrency.Service.Persistence;
using CyberSec.Cryptocurrency.Service.Interfaces;
using CyberSec.Cryptocurrency.Service.Models;
using CyberSec.Cryptocurrency.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using CyberSec.Cryptocurrency.Service.Helpers;
using System.Security.Cryptography;
using CyberSec.Cryptocurrency.Service.Enums;

namespace CyberSec.Cryptocurrency.Service.Services;

public class UserService : IUserService
{
    private readonly CryptocurrencyContext _context;
    private readonly IJwtUtils _jwtUtils;
    private readonly IDate _date;
    private readonly IEmailService _emailService;

    public UserService(
        CryptocurrencyContext context,
        IJwtUtils jwtUtils,
        IDate date,
        IEmailService emailService)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _date = date;
        _emailService = emailService;
    }

    public async Task<AuthenticateDto> AuthenticateAsync(AuthenticateRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UserException("Username or password is incorrect");

        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new(
            new(user.Id, user.FirstName, user.LastName, user.Email, (int)user.Role),
            jwtToken);
    }

    public async Task VerifyEmailAsync(string token)
    {
        var user = _context.Users.SingleOrDefault(x => x.VerificationToken == token);

        if (user is null)
            throw new UserException("User verification failed");

        user.VerifiedAt = _date.Now;
        user.VerificationToken = null!;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserDto> GetAsync(string Id)
    {
        var user = await _context.Users.FindAsync(Id);
        if (user is null)
            throw new UserException("User not found");

        return new(user.Id, user.FirstName, user.LastName, user.Email, (int)user.Role);
    }

    public async Task RegisterAsync(RegisterRequest request, string origin)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (userExists)
            throw new UserException("User already exists");

        var verificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        await _context.Users.AddAsync(new()
        {
            Id = Guider.Generate(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = Role.Admin,
            VerificationToken = verificationToken,
            VerifiedAt = null
        });

        await _context.SaveChangesAsync();

        await SendVerificationEmailAsync(origin, request.Email, verificationToken);
    }

    private async Task SendVerificationEmailAsync(string origin, string email, string token)
    {
        string message;
        if (!string.IsNullOrEmpty(origin))
        {
            // origin exists if request sent from browser single page app (e.g. Angular or React)
            // so send link to verify via single page app
            var verifyUrl = $"{origin}/account/verify-email?token={token}";
            message = $@"<p>Please click the below link to verify your email address:</p>
                            <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
        }
        else
        {
            // origin missing if request sent directly to api (e.g. from Postman)
            // so send instructions to verify directly with api
            message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                            <p><code>{token}</code></p>";
        }

        await _emailService.SendAsync(
            to: email,
            subject: "Sign-up Verification API - Verify Email",
            html: $@"<h4>Verify Email</h4>
                        <p>Thanks for registering!</p>
                        {message}"
        );
    }
}