using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortner.Data.Interface.IServices;
using UrlShortner.Data.Models;
using UrlShortner.Data.Persistence;

namespace UrlShortner.Data.Services
{
    public class UrlService : IUrlService
    {
        private readonly AppDbContext _dbContext;

        public UrlService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
