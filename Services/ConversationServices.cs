using AutoMapper;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.DTOs.Notification;
using TestWebAPI.DTOs.Role;
using TestWebAPI.DTOs.User;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class ConversationServices : IConversationServices
    {
        private readonly IMapper _mapper;
        private readonly IChatHubRepositories _chatHubRepo;
        private readonly IUserRepositories _userRepo;

        public ConversationServices(IMapper mapper, IChatHubRepositories chatHubRepo, IUserRepositories userRepo) {
            _mapper = mapper;
            _chatHubRepo = chatHubRepo;
            _userRepo = userRepo;
        }

        public async Task<ServiceResponse<List<GetConversationDTO>>> GetConversationByUserId(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetConversationDTO>>();
            try
            {
                var conversations = await _chatHubRepo.GetConversationByUserId(userId);
                if (conversations == null || !conversations.Any())
                {
                    serviceResponse.SetNotFound("Conversations");
                    return serviceResponse;
                }

                var userIds = conversations.SelectMany(c => new[] { c.userId1, c.userId2 }).Distinct();
                var users = await _userRepo.GetUsersByIds(userIds);

                var conversationDTOs = _mapper.Map<List<GetConversationDTO>>(conversations);

                foreach (var conversationDTO in conversationDTOs)
                {
                    var otherUserId = conversationDTO.userId1 == userId ? conversationDTO.userId2 : conversationDTO.userId1;
                    var otherUser = users.FirstOrDefault(u => u.id == otherUserId);

                    conversationDTO.dataUser = otherUser != null ? new GetUserDTO
                    {
                        id = otherUser.id,
                        first_name = otherUser.first_name,
                        last_name = otherUser.last_name,
                        avatar = otherUser.avatar,
                    } : null;
                }

                serviceResponse.data = conversationDTOs;
                serviceResponse.SetSuccess("Conversations retrieved successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }
    }
}
