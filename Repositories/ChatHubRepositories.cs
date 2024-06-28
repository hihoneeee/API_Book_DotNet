using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class ChatHubRepositories : IChatHubRepositories
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatHubRepositories(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }


        public async Task CreateConversation(string conversationName)
        {
            var conversation = new Conversation
            {
                name = conversationName            
            };

            _context.conversations.Add(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task SendMessage(int conversationId, int userId, string messageContent)
        {
            // Tạo mới một message
            var message = new Message
            {
                conversationId = conversationId,
                userId = userId,
                content = messageContent,
                createdAt = DateTime.UtcNow
            };

            _context.messages.Add(message);
            await _context.SaveChangesAsync();

            // tạo sự kiện realtime
            await _hubContext.Clients.Group(conversationId.ToString()).SendAsync("ReceiveMessage", userId, messageContent, message.createdAt);
        }

        public async Task<List<Message>> GetMessagesForConversation(int conversationId)
        {
            return await _context.messages
                                 .Where(m => m.conversationId == conversationId)
                                 .OrderBy(m => m.createdAt)
                                 .ToListAsync();
        }

        public async Task JoinConversation(int conversationId, string connectionId)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, conversationId.ToString());
        }

        public async Task LeaveConversation(int conversationId, string connectionId)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, conversationId.ToString());
        }
    }
}
