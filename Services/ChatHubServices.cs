using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using TestWebAPI.DTOs.Category;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Helpers;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Response;
using TestWebAPI.Services.Interfaces;

namespace TestWebAPI.Services
{
    public class ChatHubServices : IChatHubServices
    {
        private readonly IChatHubRepositories _chatHubRepo;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly IMapper _mapper;

        public ChatHubServices(IChatHubRepositories chatHubRepo, IMapper mapper, IHubContext<ChatHub> hubContext) {
            _chatHubRepo = chatHubRepo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<ServiceResponse<ConversationDTO>> GetOrCreateConversation(ConversationDTO conversationDTO)
        {
            var serviceResponse = new ServiceResponse<ConversationDTO>();
            try
            {
                var checkConversation = await _chatHubRepo.CheckkConversation(conversationDTO.userId1, conversationDTO.userId2);
                if (checkConversation != null)
                {
                    var messages = await _chatHubRepo.GetMessagesForConversation(checkConversation.id);
                    var conversationWithMessages = new ConversationDTO
                    {
                        userId1 = checkConversation.userId1,
                        userId2 = checkConversation.userId2,
                        Messages = messages.Select(m => new GetMessageDTO
                        {
                            ConversationId = m.conversationId,
                            UserId = m.userId,
                            Content = m.content,
                            CreatedAt = DateTime.UtcNow,
                        }).ToList()
                    };
                    serviceResponse.data = conversationWithMessages;
                    serviceResponse.SetSuccess("Conversation exists, returning messages.");
                }
                else
                {
                    var conversation = _mapper.Map<Conversation>(conversationDTO);
                    var createConversation = await _chatHubRepo.CreateConversation(conversation);
                    serviceResponse.SetSuccess("Create successfully!");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<MessageDTO>> SendMessage(MessageDTO messageDTO)
        {
            var serviceResponse = new ServiceResponse<MessageDTO>();
            try
            {
                var message = _mapper.Map<Message>(messageDTO);
                var sendMessage = await _chatHubRepo.SendMessage(message);
                serviceResponse.SetSuccess("Message sent successfully!");

                await _hubContext.Clients.Group(messageDTO.conversationId.ToString()).SendAsync("ReceiveMessage", messageDTO.userId, messageDTO.content, messageDTO.createdAt);

            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }


        public async Task JoinConversation(int conversationId, string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, conversationId.ToString());
        }

        public async Task LeaveConversation(int conversationId, string connectionId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, conversationId.ToString());
        }

        public async Task<ServiceResponse<List<GetMessageDTO>>> GetMessagesForConversation(int conversationId)
        {
            var serviceResponse = new ServiceResponse<List<GetMessageDTO>>();
            try
            {
                var messages = await _chatHubRepo.GetMessagesForConversation(conversationId);
                serviceResponse.data = _mapper.Map<List<GetMessageDTO>>(messages);
                serviceResponse.SetSuccess("Get successfully!");
            }
            catch (Exception ex)
            {
                serviceResponse.SetError(ex.Message);
            }
            return serviceResponse;
        }

    }
}
