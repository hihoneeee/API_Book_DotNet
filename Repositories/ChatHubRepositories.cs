using Humanizer;
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
            return await _context.Conversations.Include(c => c.Messages).ThenInclude(m=>m.User)
                .FirstOrDefaultAsync(c =>
                (c.userId1 == userId1 && c.userId2 == userId2) ||
                (c.userId1 == userId2 && c.userId2 == userId1));
        }
         
        public async Task<List<Conversation>> GetConversationByUserId(int userId)
        {
            return await _context.Conversations
                .OrderByDescending(c => c.updatedAt)
                .Where(c => c.userId1 == userId || c.userId2 == userId)
                .ToListAsync();
        }
        
        public async Task<Conversation> UpdateConversationAsync(Conversation oldConversation)
        {
            oldConversation.updatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return oldConversation;
        }

        public async Task<Conversation> CreateConversation(Conversation conversation)
        {
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<Conversation> GetConversationById(int id)
        {
            return await _context.Conversations.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Message> SendMessage(Message message)
        {
            // Thêm thông báo vào ngữ cảnh dữ liệu
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            // Truy vấn lại thông báo cùng với dữ liệu người dùng liên quan
            var messageWithUser = await _context.Messages
                .Include(m => m.User)
                .FirstOrDefaultAsync(m => m.id == message.id);

            return messageWithUser;
        }

        public async Task<List<Message>> GetMessagesForConversation(int conversationId)
        {
            return await _context.Messages
                                 .Include(m=>m.User)
                                 .Where(m => m.conversationId == conversationId)
                                 .OrderBy(m => m.createdAt)
                                 .ToListAsync();
        }

        public async Task<bool> CheckUserSendMessageAsync(int userId)
        {
            return await _context.Conversations.AnyAsync(c => c.userId1 == userId || c.userId2 == userId);
        }

    }
}
