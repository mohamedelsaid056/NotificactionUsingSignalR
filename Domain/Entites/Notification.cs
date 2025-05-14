using Domain.Entites;
  
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.NotificationType;


namespace Domain.Entites
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public NotificationType Type { get; set; } = NotificationType.Info;

      
        public Guid UserId { get; set; }

       
        public User User { get; set; }
    }

}
