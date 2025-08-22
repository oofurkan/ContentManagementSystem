using CMS.Application.DTOs;

namespace CMS.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<bool> ValidateUserAsync(string username, string password);
        string GenerateJwtToken(string username, string fullName);
    }
}
