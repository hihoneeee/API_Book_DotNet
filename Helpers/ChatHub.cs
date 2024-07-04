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
