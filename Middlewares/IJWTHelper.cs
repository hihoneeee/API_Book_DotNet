﻿using System.Security.Claims;
using TestWebAPI.DTOs.User;

namespace TestWebAPI.Middlewares
{
    public interface IJWTHelper
    {
        Task<string> GenerateJWTToken(int id, string roleCode, DateTime expire);
        Task<string> GenerateJWTRefreshToken(int id, string roleCode, DateTime expire);
        Task<string> GenerateJWTTokenForEmail(int id, string newEmail, string oldEmail, DateTime expire);
        Task<bool> ValidateRefreshTokenAsync(string refreshToken);
        int GetUserIdFromToken(string token);
        string GetUserRoleFromToken(string token);
        string GetNewEmailFromToken(string token);
        string GetOldEmailFromToken(string token);
    }
}
