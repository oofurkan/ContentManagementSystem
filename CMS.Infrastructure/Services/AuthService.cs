using CMS.Application.DTOs;
using CMS.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CMS.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var isValid = await ValidateUserAsync(loginDto.Username, loginDto.Password);
            if (!isValid)
            {
                throw new InvalidOperationException("Invalid username or password");
            }

            var user = await _userService.GetUserByUsernameAsync(loginDto.Username);
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var token = GenerateJwtToken(user.Username, user.FullName);

            return new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                FullName = user.FullName
            };
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            return await _userService.ValidateCredentialsAsync(username, password);
        }

        public string GenerateJwtToken(string username, string fullName)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "YourSuperSecretKeyHere12345678901234567890");
            var issuer = jwtSettings["Issuer"] ?? "CMS_API";
            var audience = jwtSettings["Audience"] ?? "CMS_Users";
            var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim("FullName", fullName)
                }),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }
    }
}
