using Application.Interfaces;
using Domain.Entites;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<(bool Success, string Token, Guid UserId)> RegisterAsync(string email, string password)
        {
            // Check if user already exists
            if (await _userRepository.EmailExistsAsync(email))
            {
                return (false, null, Guid.Empty);
            }

            // Create new user
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                PasswordHash = HashPassword(password)
            };

            await _userRepository.AddAsync(user);

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return (true, token, user.Id);
        }

        public async Task<(bool Success, string Token, Guid UserId)> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return (false, null, Guid.Empty);
            }

            // Verify password
            if (!VerifyPassword(password, user.PasswordHash))
            {
                return (false, null, Guid.Empty);
            }

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return (true, token, user.Id);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            return VerifyPassword(password, user.PasswordHash);
        }

        private string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(null, password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
