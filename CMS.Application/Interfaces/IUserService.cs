using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.DTOs;
using CMS.Domain.Entities;

namespace CMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto dto);
        Task<UserDto> CreateUserWithAuthAsync(UserCreateDto dto);
        Task<IEnumerable<ContentDto>> GetUserContentsAsync(Guid userId);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> ValidateCredentialsAsync(string username, string password);
    }
}
