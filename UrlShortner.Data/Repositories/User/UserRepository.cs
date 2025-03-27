using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;
        public UserRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(ApiUser user)
        {
            await _dbContext.ApiUsers.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ApiUser> GetUserByEmailAsync(string email)
        {
            return await _dbContext.ApiUsers.Include(u => u.ApiKey).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateUserAsync(ApiUser user)
        {
            var result = _dbContext.Update(user);

            if (result != null)
            {
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
