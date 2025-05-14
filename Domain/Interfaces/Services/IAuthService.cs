using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Token, Guid UserId)> RegisterAsync(string email, string password);
        Task<(bool Success, string Token, Guid UserId)> LoginAsync(string email, string password);
        Task<bool> ValidateUserAsync(string email, string password);
    }
}
