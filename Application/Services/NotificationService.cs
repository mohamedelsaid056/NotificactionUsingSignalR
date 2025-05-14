using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Application.DTOs;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Entites;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationHub _notificationHub;
        private readonly IMapper _mapper;

        public NotificationService(
            INotificationRepository notificationRepository,
            IUserRepository userRepository,
            INotificationHub notificationHub,
            IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _notificationHub = notificationHub;
            _mapper = mapper;
        }

        public async Task<Notification> CreateNotificationAsync(string message, Guid userId, NotificationType type = NotificationType.Info)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Message = message,
                UserId = userId,
                Type = type,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdNotification = await _notificationRepository.AddAsync(notification);

            // Send real-time notification
            var notificationDto = _mapper.Map<NotificationDto>(createdNotification);
            await _notificationHub.SendNotificationToUserAsync(userId, notificationDto);

            return createdNotification;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(Guid userId)
        {
            return await _notificationRepository.GetUnreadByUserIdAsync(userId);
        }

        public async Task MarkAsReadAsync(Guid notificationId)
        {
            await _notificationRepository.MarkAsReadAsync(notificationId);
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            await _notificationRepository.MarkAllAsReadForUserAsync(userId);
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            await _notificationRepository.DeleteAsync(notificationId);
        }


    }

}
