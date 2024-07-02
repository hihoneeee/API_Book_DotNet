using Microsoft.AspNetCore.SignalR;
public class ChatHub : Hub
{
    public string GetConnectionId()
    {
        return Context.ConnectionId;
    }
}
