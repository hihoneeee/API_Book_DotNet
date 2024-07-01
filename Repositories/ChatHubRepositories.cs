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

        public ChatHubRepositories(ApplicationDbContext context, IHubContext<ChatHub> hubContext)
        {
            _context = context;
        }

        public async Task<Conversation> CheckkConversation(int userId1, int userId2)
        {
            return await _context.Conversations.FirstOrDefaultAsync(c =>
                (c.userId1 == userId1 && c.userId2 == userId2) ||
                (c.userId1 == userId2 && c.userId2 == userId1));
        }
        public async Task<Conversation> CreateConversation(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<Message> SendMessage(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>> GetMessagesForConversation(int conversationId)
        {
            return await _context.Messages
                                 .Where(m => m.conversationId == conversationId)
                                 .OrderBy(m => m.createdAt)
                                 .ToListAsync();
        }
    }
}
