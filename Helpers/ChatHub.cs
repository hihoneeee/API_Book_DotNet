using Microsoft.AspNetCore.SignalR;
public class ChatHub : Hub
{
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
    public async Task SendNotification(int userId, string content)
    {
        await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", userId, content);
    }
}
