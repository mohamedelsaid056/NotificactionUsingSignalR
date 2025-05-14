using Domain.Entites;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(string message, Guid userId, NotificationType type = NotificationType.Info);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId);
        Task MarkAsReadAsync(Guid notificationId);
        Task MarkAllAsReadAsync(Guid userId);
        Task DeleteNotificationAsync(Guid notificationId);
    }

}
