using BusinessLogic.Repository.RepositoryInterfaces;
using DataLayer.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Repository.RepositoryClasses
{
    public class IssueNotificationRepository : GenericRepository<IssueNotification>, IIssueNotificationRepository
    {
        private readonly AppDbContext _context;

        public IssueNotificationRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

      
        public async Task<List<IssueNotification>> GetUnreadByIssueAndUserAsync(int issueId, string userId)
        {
            return await _context.IssueNotifications
                .Where(n => n.IssueId == issueId && n.ReceiverId == userId && !n.IsRead)
                .ToListAsync();
        }

    }
}
