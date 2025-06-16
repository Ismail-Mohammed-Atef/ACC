using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository.RepositoryInterfaces
{
    public interface IIssueNotificationRepository : IGenericRepository<IssueNotification>
    {
        
        Task<List<IssueNotification>> GetUnreadByIssueAndUserAsync(int issueId, string userId);

    }
}
