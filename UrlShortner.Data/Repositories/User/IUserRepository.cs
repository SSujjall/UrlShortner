using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortner.Data.Repositories.User
{
    public interface IUserRepository
    {
        Task<Models.ApiUser> GetUserByEmailAsync(string email);
        Task AddUserAsync(Models.ApiUser user);
    }
}
