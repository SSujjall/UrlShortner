
using UrlShortner.Data.Models;

namespace UrlShortner.Data.Repositories.User
{
    public interface IUserRepository
    {
        Task<ApiUser> GetUserByEmailAsync(string email);
        Task AddUserAsync(ApiUser user);
        Task<bool> UpdateUserAsync(ApiUser user);
    }
}
