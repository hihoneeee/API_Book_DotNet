using Microsoft.AspNetCore.SignalR;
using TestWebAPI.Repositories.Interfaces;

public class ChatHub : Hub
{
    private readonly IChatHubRepositories _chatHubRepo;

    public ChatHub(IChatHubRepositories chatHubRepo)
    {
        _chatHubRepo = chatHubRepo;
    }
    public async Task JoinConversation(int conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
        var messages = await _chatHubRepo.GetMessagesForConversation(conversationId);

        foreach (var message in messages)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message.userId, message.content, message.createdAt);
        }
    }

    public async Task LeaveConversation(int conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
    }

    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}
