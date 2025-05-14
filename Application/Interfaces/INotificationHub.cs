using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationHub
    {
        Task SendNotificationToUserAsync(Guid userId, NotificationDto notification);
        Task SendNotificationToAllAsync(NotificationDto notification);
    }

}
