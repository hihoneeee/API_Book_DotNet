﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TestWebAPI.Data;
using TestWebAPI.Models;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Repositories
{
    public class NotificationRepositories : INotificationRepositories
    {
        private readonly ApplicationDbContext _context;

        public NotificationRepositories(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<Notification> CreateNoficationsAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }
        public async Task<User> FindUserByPropertyIdAsync(int propertyId)
        {
            return await _context.Users
                                 .Where(u => u.PropertyHasDetails.Any(p => p.propertyId == propertyId))
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<Notification>> GetNotificationsForUser(int userId)
        {
            return await _context.Notifications
                                 .Where(n => n.userId == userId)
                                 .Include(n => n.user)
                                 .OrderByDescending(n => n.createdAt)
                                 .ToListAsync();
        }

        public async Task MarkNotificationsAsRead(int userId)
        {
            var notifications = await _context.Notifications.Where(n => n.userId == userId && !n.IsRead).ToListAsync();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }
            await _context.SaveChangesAsync();
        }
    }
}
