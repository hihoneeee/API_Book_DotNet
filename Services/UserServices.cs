﻿using AutoMapper;
using TestWebAPI.DTOs.User;
using TestWebAPI.Helpers;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;
using static TestWebAPI.Response.HttpStatus;

namespace TestWebAPI.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMapper _mapper;
        private readonly IUserRepositories _userRepo;

        public UserServices(IUserRepositories userRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<UserDTO>> GetCurrentAsync(int id)
        {
            var serviceResponse = new ServiceResponse<UserDTO>();
            try
            {
                var existingUser = await _userRepo.GetCurrentAsync(id);
                if (existingUser == null)
                {
                    serviceResponse.SetNotFound("User");
                    return serviceResponse;
                }
                serviceResponse.data = _mapper.Map<UserDTO>(existingUser);
                serviceResponse.SetSuccess("Get current user successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.success = false;
                serviceResponse.message = $"An unexpected error occurred: {ex.Message}";
                serviceResponse.statusCode = EHttpType.InternalError;
            }
            return serviceResponse;
        }
    }
}
