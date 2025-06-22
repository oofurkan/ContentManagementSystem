using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.DTOs;

namespace CMS.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(UserDto dto);
        Task<IEnumerable<ContentDto>> GetUserContentsAsync(Guid userId);
    }
}
