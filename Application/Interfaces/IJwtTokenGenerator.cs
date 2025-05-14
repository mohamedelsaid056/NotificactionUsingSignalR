using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entites;

namespace Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Domain.Entites.User user);
        Guid? ValidateToken(string token);
    }

}
