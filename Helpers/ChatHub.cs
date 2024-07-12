using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TestWebAPI.DTOs.ChatHub;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;
[Authorize]
public class ChatHub : Hub
{
    private readonly IChatHubRepositories _chatHubRepo;
    private readonly IUserRepositories _userRepo;
    private readonly IMapper _mapper;

    public ChatHub(IMapper mapper,IChatHubRepositories chatHubRepo, IUserRepositories userRepo)
    {
        _chatHubRepo = chatHubRepo;
        _userRepo = userRepo;
        _mapper = mapper;
    }
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
    public async Task SendNotification(int userId, string content)
    {
        await Clients.Group(userId.ToString()).SendAsync("ReceiveNotification", content);
    }

    public async Task SendMessageAysnc(MessageDTO messageDTO)
    {
        try
        {
            var message = _mapper.Map<Message>(messageDTO);
            var sendMessage = await _chatHubRepo.SendMessage(message);
            var conversation = await _chatHubRepo.GetConversationById(messageDTO.conversationId);
            var user1 = await _userRepo.GetCurrentAsync(conversation.userId1);
            var user2 = await _userRepo.GetCurrentAsync(conversation.userId2);
            if (conversation.userId1 != messageDTO.userId)
            {
                var content = $"{user2.first_name}{user2.last_name} has sent you a new message!";
                await Clients.Group(conversation.userId1.ToString()).SendAsync("ReceiveNotification", content);
            }

            if (conversation.userId2 != messageDTO.userId)
            {
                var content = $"{user1.first_name}{user1.last_name} has sent you a new message!";
                await Clients.Group(conversation.userId2.ToString()).SendAsync("ReceiveNotification", content);
            }

            await Clients.Group(messageDTO.conversationId.ToString()).SendAsync("ReceiveMessage", messageDTO.userId, messageDTO.content, messageDTO.createdAt);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while doing something!", ex);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext().Request.Query["userId"].ToString();
        if (!string.IsNullOrEmpty(userId))
        {
            Context.Items["userId"] = userId;
        }

        await base.OnConnectedAsync();
    }
}
