using Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Repositories
{
    public interface INotificationRepository
    {
        Task<Notification> GetByIdAsync(Guid id);
        Task<IEnumerable<Notification>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Notification>> GetUnreadByUserIdAsync(Guid userId);
        Task<Notification> AddAsync(Notification notification);
        Task UpdateAsync(Notification notification);
        Task MarkAsReadAsync(Guid id);
        Task MarkAllAsReadForUserAsync(Guid userId);
        Task DeleteAsync(Guid id);
    }

}
